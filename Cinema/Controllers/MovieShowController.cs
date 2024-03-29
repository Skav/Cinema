﻿using Cinema.DTO;
using Cinema.Models;
using Humanizer;
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
        [Route("{movieShowId:int}")]
        public async Task<IActionResult> getMovieShowsById(int movieShowId)
        {
            var moviesShow = await _context.MovieShow.Where(x => x.id == movieShowId).FirstOrDefaultAsync();

            if (moviesShow == null)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(moviesShow);
        }

        [HttpGet]
        [Route("movie/{movieId:int}")]
        public async Task<IActionResult> getMovieShows(int movieId)
        {
            var moviesShow = await _context.MovieShow.Where(x => x.movieId == movieId).Where(x => x.date >= DateTime.UtcNow).ToListAsync();

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
            if(!await _context.Rooms.Where(x => x.id == request.roomId).AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Room with given ID doesn't exists!"
                }));

            if (!await _context.Movies.Where(x => x.id == request.movieId).AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Movie with given ID doesn't exists!"
                }));

            var movieShow = mapper.Map<MovieShowModel>(request);
            request.date = DateTime.SpecifyKind(request.date, DateTimeKind.Utc);
            request.date = request.date.ToUniversalTime();

            var movieShowDate = request.date.AtMidnight();
            var now = DateTime.UtcNow.AtMidnight();

            if (movieShowDate < now)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Date must be higher or equals today's date"
                }));

            movieShow.date = request.date;
            movieShow.dateAdded = DateTime.UtcNow;

            if (await _context.MovieShow
                .Where(x => x.movieId == request.movieId)
                .Where(x => x.roomId == request.roomId)
                .Where(x => x.date == request.date)
                .Where(x => x.hour == request.hour)
                .AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Movie show with given data already exists!"
                }));

           
            
            await _context.MovieShow.AddAsync(movieShow);
            await _context.SaveChangesAsync();

            return Created("", movieShow);
        }

        [HttpPut]
        [Route("{movieShowId:int}/edit")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> editMovieShow(int movieShowId, [FromBody] MovieShowDTO request)
        {
            if (request.roomId != null && !await _context.Rooms.Where(x => x.id == request.roomId).AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Room with given ID doesn't exists!"
                }));

            if (request.movieId != null && !await _context.Movies.Where(x => x.id == request.movieId).AnyAsync())
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Movie with given ID doesn't exists!"
                }));

            var movieShow = _context.MovieShow.FirstOrDefault(x => x.id == movieShowId);

            if(movieShow == null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Movie with given id not exists!"
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
                            room.seatsInRow,
                            room.roomNo
                        };

            var movieShowResponse = await query.FirstOrDefaultAsync();

            if (movieShowResponse == null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Movie show doesnt exists!"
                }));

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
                        var timeAdded = item.dateAdded;
                        if (DateTime.UtcNow > timeAdded.AddMinutes(15))
                        {
                            item.dateUpdate = DateTime.UtcNow;
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
                movieId = movieShowResponse.id,
                movieHour = movieShowResponse.hour,
                movieDate = movieShowResponse.date,
                roomNo = movieShowResponse.roomNo,
                rows = movieShowResponse.rows,
                seatInRow = movieShowResponse.seatsInRow,
                reservedSeats = reservedSeats
            })) ;

        }
    }
}
