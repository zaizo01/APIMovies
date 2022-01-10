using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("api/Movies/{movieId:int}/Reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ReviewController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ReviewDTO>>> GetReviewsMovies(int movieId, [FromQuery] PaginacionDTO paginationDTO)
        {
            var movieExist = await context.Movies.AnyAsync(movie => movie.Id == movieId);
            if (!movieExist) return NotFound();

            var queryable = context.Reviews.Include(review => review.User).AsQueryable();
            queryable = queryable.Where(review => review.MovieId == movieId);
            await HttpContext.InsertParameterPagination(queryable, paginationDTO.NumberOfRecordsPerPage);
            var review = await queryable.Paginar(paginationDTO).ToListAsync();
            return mapper.Map<List<ReviewDTO>>(review);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostReview(int movieId, [FromBody] ReviewPostDTO reviewPostDTO)
        {
            var movieExist = await context.Movies.AnyAsync(movie => movie.Id == movieId);
            if (!movieExist) NotFound();

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            
            var reviewExist = await context.Reviews
                .AnyAsync(movie => movie.MovieId == movieId && movie.UserId == userId);

            if (reviewExist) return BadRequest("The User has written a comment on the movie.");

            var review = mapper.Map<Review>(reviewPostDTO);
            review.MovieId = movieId;
            review.UserId = userId;


            context.Add(review);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{reviewId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PutReview(int movieId, int reviewId, [FromBody] ReviewPostDTO reviewPostDTO)
        {
            var movieExist = await context.Movies.AnyAsync(movie => movie.Id == movieId);
            if (!movieExist) return NotFound();

            var reviewDB = await context.Reviews.FirstOrDefaultAsync(review => review.Id == reviewId);
            if (reviewDB == null) return NotFound();

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
           
            if (reviewDB.UserId != userId) return BadRequest("Don't have permission for edit this comment.");

            reviewDB = mapper.Map(reviewPostDTO, reviewDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{reviewId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> DeleteReview(int reviewId)
        {
            var reviewDB = await context.Reviews.FirstOrDefaultAsync(review => review.Id == reviewId);
            if (reviewDB == null) return NotFound();

            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
           
            if (reviewDB.UserId != userId) return Forbid();

            context.Remove(reviewDB);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
