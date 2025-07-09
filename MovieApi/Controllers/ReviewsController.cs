using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
