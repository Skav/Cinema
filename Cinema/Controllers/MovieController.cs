using AutoMapper;
using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Xml.Serialization;

namespace Cinema.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : BaseController
    {
        public MovieController(AppDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> getMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            if (movies == null || movies.Count() == 0)
                return Ok(JsonSerializer.Serialize(new {}));
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
                return Ok(JsonSerializer.Serialize(new {}));

            return Ok(movie);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> addMovie([FromBody] MovieDTO request)
        {
            if(request == null || request.title == null || request.duration <= 0 || request.genre == null)
                return BadRequest();

            var movie = mapper.Map<MoviesModel>(request);
            movie.dateAdded = DateTime.UtcNow;

            if(await _context.Movies.AnyAsync(x => x.title == movie.title))
            {
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Object already exists"
                }));
            }
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return Created("", movie);
        }

        [HttpDelete]
        [Route("{id:int}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> deleteMovie(int id)
        {
            if (!await _context.Movies.AnyAsync(x => x.id == id))
            {
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Object doesn't exists"
                }));
            }
            await _context.Movies.Where(x => x.id == id).ExecuteDeleteAsync();

            return Ok();
        }

        [HttpPut]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> updateMovie(int id, [FromBody] MovieDTO request)
        {
            if (request == null)
                return BadRequest();

            var dbObject = await _context.Movies.FindAsync(id);

            if(dbObject == null)
                return NotFound();

            _context.Entry(dbObject).CurrentValues.SetValues(request);
            var result = await _context.SaveChangesAsync();

            return Ok(dbObject);
        }
    }
}
