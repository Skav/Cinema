using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Xml.Serialization;

namespace Cinema.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MovieController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> getMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> getMovieById(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var movie = await _context.Movies.FirstOrDefaultAsync(x => x.id == id);

            if (movie == null)
                return Ok("[]");

            return Ok(movie);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> addMovie(string title, int duration, string genre)
        {
            var movie = new MoviesModel(title, duration, genre, true);
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }
    }
}
