using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;

namespace Cinema.Controllers
{
    [Route("api/movieShows")]
    [ApiController]
    public class MovieShowController : BaseController
    {
        private TimeZoneInfo polandTime = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        //private TimeZoneInfo polandTime = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw"); -- unix
        public MovieShowController(AppDbContext context) : base(context) { }

        [HttpGet]
        public async Task<IActionResult> getMovieShows()
        {
            var moviesShow = await _context.MovieShow.Where(x => x.date >= DateTime.UtcNow).ToListAsync();

            if (moviesShow == null || moviesShow.Count() == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(moviesShow);
        }

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> getAllMovieShows()
        {
            var moviesShow = await _context.MovieShow.ToListAsync();

            if (moviesShow == null || moviesShow.Count() == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(moviesShow);
        }

        [HttpPost]
        [Authorize(Roles = ("Admin,Staff"))]
        public async Task<IActionResult> addMovieShow([FromBody] MovieShowDTO request)
        {
            if (request == null || request.roomId == null || request.movieId == null || request.date == null || request.hour == null)
                return BadRequest();

            if(!await _context.Rooms.Where(x => x.id == request.roomId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "Room with given ID doesn't exists!"
                }));

            if (!await _context.Movies.Where(x => x.id == request.movieId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "Movie with given ID doesn't exists!"
                }));

            var movieShow = mapper.Map<MovieShowModel>(request);
            request.date = DateTime.SpecifyKind(request.date, DateTimeKind.Local);
            request.date = request.date.ToUniversalTime();
            movieShow.date = request.date;
            movieShow.dateAdded = DateTime.UtcNow;
            
            await _context.MovieShow.AddAsync(movieShow);
            await _context.SaveChangesAsync();

            return Created("", movieShow);
        }

        [HttpPut]
        [Route("{movieShowId:int}/edit")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> editMovieShow(int movieShowId, [FromBody] MovieShowDTO request)
        {
            if (movieShowId == null)
                return BadRequest();

            if (request.roomId != null && !await _context.Rooms.Where(x => x.id == request.roomId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "Room with given ID doesn't exists!"
                }));

            if (request.movieId != null && !await _context.Movies.Where(x => x.id == request.movieId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "Movie with given ID doesn't exists!"
                }));

            var movieShow = _context.MovieShow.FirstOrDefault(x => x.id == movieShowId);

            if(movieShow == null)
                return NotFound(JsonSerializer.Serialize(new
                {
                    Error = "Movie with given id not exists!"
                }));

            request.date = DateTime.SpecifyKind(request.date, DateTimeKind.Local);
            request.date = request.date.ToUniversalTime();
            movieShow.date = request.date;
            _context.Entry(movieShow).CurrentValues.SetValues(request);
            await _context.SaveChangesAsync();

            return Ok(movieShow);
        }

        [HttpDelete]
        [Route("{movieShowId:int}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> deleteMovieShow(int movieShowId)
        {
            if (movieShowId == 0)
                return BadRequest();

            if (!await _context.MovieShow.AnyAsync(x => x.id == movieShowId))
            {
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Object doesn't exists"
                }));
            }
            await _context.MovieShow.Where(x => x.id == movieShowId).ExecuteDeleteAsync();

            return Ok();
        }

        [HttpGet]
        [Route("{movieShowId:int}/getSeats")]
        public async Task<IActionResult> getSeats(int movieShowId)
        {
            if (movieShowId == null)
                return BadRequest();

            var query = from movieShow in _context.MovieShow
                        join room in _context.Rooms on movieShow.roomId equals room.id
                        where movieShow.id == movieShowId
                        select new
                        {
                            movieShow.id,
                            movieShow.movieId,
                            movieShow.hour,
                            movieShow.date,
                            room.rows,
                            room.columns,
                            room.roomNo
                        };

            var movieShowResponse = await query.FirstOrDefaultAsync();

            if (movieShowResponse == null)
                return NotFound();

            var reservedSeatsQuery = await _context.Reservations
                .Where(x => x.movieShowId == movieShowId)
                .Where(x => x.status == "Confirmed" || x.status == "in_progress")
                .ToListAsync();

            Dictionary<int, List<int>> reservedSeats = new Dictionary<int, List<int>>();
            if(reservedSeatsQuery.Count() != 0)
            {
                foreach (var item in  reservedSeatsQuery)
                {
                    if(item.status == "in_progress")
                    {
                        if (DateTime.UtcNow > item.dateAdded.AddMinutes(15))
                        {
                            _context.Entry(item).CurrentValues.SetValues(new { status = "Cancelled" });
                            await _context.SaveChangesAsync();
                        }
                    }
                }



                foreach (var item in reservedSeatsQuery)
                {
                    if (item.status == "Cancelled")
                        continue;
                    if (reservedSeats.ContainsKey(item.seatRow))
                        reservedSeats[item.seatRow].Add(item.seatColumn);
                    else
                        reservedSeats.Add(item.seatRow, new List<int> { item.seatColumn });
                }
            }

            return Ok(JsonSerializer.Serialize(new
            {
                movieShowId = movieShowId,
                roomNo = movieShowResponse.roomNo,
                rows = movieShowResponse.rows,
                columns = movieShowResponse.columns,
                reservedSeats = reservedSeats
            }));

        }
    }
}
