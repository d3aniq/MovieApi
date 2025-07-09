using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.DTOs;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET /api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        // GET /api/movies/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();
            return movie;
        }

        // GET /api/movies/{id}/details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.Reviews)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var dto = new MovieDetailDto
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,

                MovieDetails = new MovieDetailsDto
                {
                    Id = movie.Id,
                    Genre = "Unknown", 
                    Duration = 120     
                },

                Actors = movie.MovieActors.Select(ma => new ActorDto
                {
                    Id = ma.Actor?.Id ?? 0, // 0 som fallback om Actor är null
                    Name = ma.Actor?.Name ?? "Unknown" // fallback-namn
                }).ToList(),


                Reviews = movie.Reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Text = r.Comment 
                }).ToList()
            };

            return dto;
        }

        // POST /api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }

        // PUT /api/movies/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, Movie updatedMovie)
        {
            if (id != updatedMovie.Id) return BadRequest();

            _context.Entry(updatedMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Movies.Any(m => m.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE /api/movies/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NotFound();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
