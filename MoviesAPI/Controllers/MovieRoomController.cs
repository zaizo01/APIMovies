using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("api/MovieRoom")]
    [ApiController]
    public class MovieRoomController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly GeometryFactory geometryFactory;

        public MovieRoomController(ApplicationDbContext context, IMapper mapper, GeometryFactory geometryFactory)
        {
            this.context = context;
            this.mapper = mapper;
            this.geometryFactory = geometryFactory;
        }
        public async Task<ActionResult<List<MovieRoomDTO>>> GetMovieRooms()
        {
            var movieRooms = await context.MovieRooms.ToListAsync();
            return mapper.Map<List<MovieRoomDTO>>(movieRooms);
        }

        [HttpGet("{id:int}", Name = "GetMovieRoomById")]
        public async Task<ActionResult<MovieRoomDTO>> GetMovieRoomById(int id)
        {
            var movieRoom = await context.MovieRooms.FirstOrDefaultAsync(movieRoom => movieRoom.Id == id);
            if (movieRoom == null) return NotFound();
            return mapper.Map<MovieRoomDTO>(movieRoom);
        }

        [HttpGet("NearbyCinemas")]
        public async Task<ActionResult<List<MovieRoomNearDTO>>> NearbyCinemas([FromQuery] MovieRoomNearFilterDTO filter)
        {
            var userLocation = geometryFactory.CreatePoint(new Coordinate(filter.Longitude, filter.Latitude));

            var movieRoom = await context.MovieRooms
                .OrderBy(x => x.Location.Distance(userLocation))
                .Where(x => x.Location.IsWithinDistance(userLocation, filter.DistanceInKms * 1000))
                .Select(x => new MovieRoomNearDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Location.Y,
                    Longitude = x.Location.X,
                    DistanceInMeters = Math.Round(x.Location.Distance(userLocation))
                }).ToListAsync();

            return movieRoom;
        }


        [HttpPost]
        public async Task<ActionResult> PostMovieRoom([FromBody] MovieRoomPostDTO movieRoomDto)
        {
            var movieRoom = mapper.Map<MovieRoom>(movieRoomDto);
            context.Add(movieRoom);
            await context.SaveChangesAsync();
            var movieRoomDTO = mapper.Map<MovieRoomDTO>(movieRoom);
            return new CreatedAtRouteResult("GetMovieRoomById", new { id = movieRoom.Id }, movieRoomDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> PutMovieRoom(int id, [FromBody] MovieRoomPostDTO movieRoomDTO)
        {
            var movieRoom = mapper.Map<MovieRoom>(movieRoomDTO);
            movieRoom.Id = id;
            context.Entry(movieRoom).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMovieRoom(int id)
        {
            var movieRoomExists = await context.MovieRooms.AnyAsync(movieRoom => movieRoom.Id == id);
            if (!movieRoomExists) return NotFound();
            context.Remove(new MovieRoom { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
