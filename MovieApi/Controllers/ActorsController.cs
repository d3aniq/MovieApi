using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly MovieContext _context;

        public ActorsController(MovieContext context)
        {
            _context = context;
        }

        // GET /api/actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
        {
            return await _context.Actors.ToListAsync();
        }

        // GET /api/actors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null) return NotFound();
            return actor;
        }

        // POST /api/actors
        [HttpPost]
        public async Task<ActionResult<Actor>> CreateActor(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, actor);
        }

        // PUT /api/actors/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActor(int id, Actor updatedActor)
        {
            if (id != updatedActor.Id) return BadRequest();

            _context.Entry(updatedActor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Actors.Any(a => a.Id == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST /api/movies/{movieId}/actors/{actorId}
        [HttpPost("/api/movies/{movieId}/actors/{actorId}")]
        public async Task<IActionResult> AddActorToMovie(int movieId, int actorId)
        {
            var movie = await _context.Movies.Include(m => m.MovieActors).FirstOrDefaultAsync(m => m.Id == movieId);
            var actor = await _context.Actors.FindAsync(actorId);

            if (movie == null || actor == null) return NotFound();

            movie.MovieActors.Add(new MovieActor
            {
                MovieId = movieId,
                ActorId = actorId,
            });

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
