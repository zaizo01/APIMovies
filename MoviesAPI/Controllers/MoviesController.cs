using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using MoviesAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.Extensions.Logging;

namespace MoviesAPI.Controllers
{
    [Route("api/Movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileStore fileStore;
        private readonly ILogger<MoviesController> logger;
        private readonly string container = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileStore fileStore, ILogger<MoviesController> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStore = fileStore;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<MoviesIndexDTO>> GetMovies()
        {

            // filters
            var topMovies = 5;
            var today = DateTime.Today;

            var upcomingMovies = await context.Movies
                    .Where(movie => movie.PremiereDate > today)
                    .OrderBy(movie => movie.PremiereDate)
                    .Take(topMovies)
                    .ToListAsync();

            var inTheaters = await context.Movies
                    .Where(movie => movie.InTheathers)
                    .Take(topMovies)
                    .ToListAsync();

            var result = new MoviesIndexDTO();
            result.UpcomingMovies = mapper.Map<List<MovieDTO>>(upcomingMovies);
            result.MoviesInCimenas = mapper.Map<List<MovieDTO>>(inTheaters);
            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] MoviesFilterDTO moviesFilter) {

            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(moviesFilter.Title))
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.Tittle.Contains(moviesFilter.Title));
            }

            if (moviesFilter.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(movie => movie.InTheathers);
            }

            if (moviesFilter.UpcomingMovies)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(movie => movie.PremiereDate > today);
            }

            if (moviesFilter.GenderId != 0)
            {
                moviesQueryable = moviesQueryable
                    .Where(movie => movie.MoviesGenders.Select(gender => gender.GenderId)
                    .Contains(moviesFilter.GenderId));
            }

            if (!string.IsNullOrEmpty(moviesFilter.FieldToSort) && moviesFilter.FieldToSort == "Tittle")
            {
                try
                {
                    //var sortType = moviesFilter.SortAsc == true ? "ASC" : "DESC";
                    //moviesQueryable = moviesQueryable.OrderBy($"{moviesFilter.FieldToSort} {sortType}");
                    if (moviesFilter.SortAsc == true) 
                    {
                        moviesQueryable = moviesQueryable.OrderBy(x => x.Tittle); 
                    }
                    else
                    {
                        moviesQueryable = moviesQueryable.OrderByDescending(x => x.Tittle);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }
            }

            await HttpContext.InsertParameterPagination(moviesQueryable,
                moviesFilter.NumberOfRecordsPerPage);

            var movies = await moviesQueryable.Paginar(moviesFilter.Paginacion).ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id:int}", Name = "GetMovie")]
        public async Task<ActionResult<MoviesDetailsDTO>> GetMovie(int id)
        {
            var movie = await context.Movies
                .Include(ma => ma.MoviesActors).ThenInclude(ma => ma.Actor)
                .Include(mg => mg.MoviesGenders).ThenInclude(mg => mg.Gender)
                .FirstOrDefaultAsync(movie => movie.Id == id);

            if (movie == null) return NotFound();
            movie.MoviesActors = movie.MoviesActors.OrderBy(movie => movie.Order).ToList();

            return mapper.Map<MoviesDetailsDTO>(movie);
        }


        private void AssignOrder(Movie movie)
        {
            if(movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostMovie([FromForm] MoviePostDTO moviePostDTO)
        {

            var movie = mapper.Map<Movie>(moviePostDTO);

            if (moviePostDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await moviePostDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(moviePostDTO.Poster.FileName);
                    movie.Poster = await fileStore.SaveFile(content, extension, container, moviePostDTO.Poster.ContentType);
                }
            }

            AssignOrder(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("GetMovie", new { id = movie.Id }, movieDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutMovie([FromForm] MoviePostDTO moviePostDTO, int id)
        {
            var movie = await context.Movies
                .Include(x => x.MoviesActors)
                .Include(x => x.MoviesGenders)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (movie == null) return NotFound();
            movie = mapper.Map(moviePostDTO, movie);
            if (moviePostDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await moviePostDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(moviePostDTO.Poster.FileName);
                    movie.Poster = await fileStore.EditFile(content, extension, container, movie.Poster, moviePostDTO.Poster.ContentType);
                }
            }
            AssignOrder(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> PatchActor(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {
            if (patchDocument == null) return BadRequest();

            var movie = await context.Movies.FirstOrDefaultAsync(actor => actor.Id == id);
            if (movie == null) return NotFound();

            var movieDTO = mapper.Map<MoviePatchDTO>(movie);
            patchDocument.ApplyTo(movieDTO, ModelState);

            var isValid = TryValidateModel(movieDTO);
            if (!isValid) return BadRequest(ModelState);

            mapper.Map(movieDTO, movie);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var MovieExits = await context.Movies.AnyAsync(movie => movie.Id == id);
            if (!MovieExits) return NotFound();
            context.Remove(new Actor() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
