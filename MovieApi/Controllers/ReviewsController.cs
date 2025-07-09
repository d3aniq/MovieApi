using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId}/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieContext _context;

        public ReviewsController(MovieContext context)
        {
            _context = context;
        }

        // GET /api/movies/{movieId}/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForMovie(int movieId)
        {
            var movie = await _context.Movies.Include(m => m.Reviews)
                                             .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null) return NotFound();

            return Ok(movie.Reviews);
        }

        [HttpPost]
        public async Task<ActionResult<Review>> AddReviewToMovie(int movieId, ReviewDto reviewDto)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null) return NotFound();

            var review = new Review
            {
                Comment = reviewDto.Text,
                Rating = reviewDto.Rating,
                MovieId = movieId
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReviewsForMovie), new { movieId = movieId }, review);
        }


    }
}
