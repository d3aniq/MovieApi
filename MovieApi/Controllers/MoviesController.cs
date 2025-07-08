using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.DTOs;
using MovieApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;

        public MoviesController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        //{
        //    return await _context.Movies.ToListAsync();
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            var movieDtos = movies.Select(m => new MovieDto
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseYear = m.ReleaseYear
            }).ToList();

            return Ok(movieDtos);
        }


        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<MovieDetailDto>> GetMovie(int id)
        //{
        //    var movie = await _context.Movies
        //        .Include(m => m.MovieDetails)
        //        .Include(m => m.Actors)
        //        .Include(m => m.Reviews)
        //        .FirstOrDefaultAsync(m => m.Id == id);

        //    if (movie == null) return NotFound();

        //    var dto = new MovieDetailDto
        //    {
        //        Id = movie.Id,
        //        Title = movie.Title,
        //        ReleaseYear = movie.ReleaseYear,
        //        Description = movie.MovieDetails?.Description,
        //        Genre = movie.MovieDetails?.Genre,
        //        Actors = movie.Actors.Select(a => new ActorDto
        //        {
        //            Id = a.Id,
        //            Name = a.Name
        //        }).ToList(),
        //        Reviews = movie.Reviews.Select(r => new ReviewDto
        //        {
        //            Id = r.Id,
        //            Text = r.Text,
        //            Rating = r.Rating
        //        }).ToList()
        //    };

        //    return Ok(dto);
        //}


        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
