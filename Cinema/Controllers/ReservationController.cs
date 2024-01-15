using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Cinema.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : BaseController
    {
        public ReservationController(AppDbContext context) : base(context) { }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        [Route("adminAll")]
        public async Task<IActionResult> getAllReservations()
        {
            var reservations = await _context.Reservations.ToListAsync();

            if (reservations.Count == 0)
                return NotFound();

            return Ok(reservations);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserReservations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
                return NotFound();

            if (!await _context.Users.Where(x => x.Id == userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            var reservations = await _context.Reservations.Where(x => x.userId == userId).ToArrayAsync();

            if (reservations.Count() == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(reservations);
        }

        [HttpGet]
        [Route("{reservationId:int}")]
        [Authorize]
        public async Task<IActionResult> getUserReservationById(int reservationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
                return NotFound();

            if (!await _context.Users.Where(x => x.Id == userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            var reservations = await _context.Reservations.Where(x => x.userId == userId).Where(x => x.id == reservationId).ToArrayAsync();

            if (reservations.Count() == 0)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(reservations);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> addUserReservation([FromBody] ReservationDTO request)
        {
            request.userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (request.userId == null)
                return Unauthorized();

            var userData = await _context.Users.Where(x => x.Id == request.userId).FirstOrDefaultAsync();

            if (userData == null)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            if (request.movieShowId == null)
                return BadRequest();

            var query = from movieShow in _context.MovieShow
                        join room in _context.Rooms on movieShow.roomId equals room.id
                        where movieShow.id == request.movieShowId
                        select new
                        {
                            movieShow.id,
                            room.rows,
                            room.seatsInRow,
                        };

            var movieShowResponse = await query.FirstOrDefaultAsync();
            if (movieShowResponse == null)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "MovieShow with given ID doesn't exists!"
                }));

            if (request.seatRow <= 0 || request.seatColumn <= 0)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "seatRow and seatColumn must be greater than 0!"
                }));

            if (request.seatRow > movieShowResponse.rows || request.seatColumn > movieShowResponse.seatsInRow)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "seatRow or seatColumn are above room limits"
                }));

            var reservation = mapper.Map<ReservationModel>(request);
            reservation.dateAdded = DateTime.UtcNow;
            reservation.status = "in_progress";
            reservation.isPaid = false;
            reservation.email = userData.Email;
            reservation.fullName = userData.UserName;

            var reservedSeat = await _context.Reservations.Where(x => x.movieShowId == reservation.movieShowId)
                .Where(x => x.seatRow == reservation.seatRow)
                .Where(x => x.seatColumn == reservation.seatColumn)
                .Where(x => x.status == "Confirmed" || x.status == "in_progress")
                .FirstOrDefaultAsync();
                

            if (reservedSeat != null)
            {
                if (reservedSeat.status == "in_progress")
                {
                    if (DateTime.UtcNow > reservedSeat.dateAdded.AddMinutes(15))
                    {
                        _context.Entry(reservedSeat).CurrentValues.SetValues(new { status = "Cancelled" });
                        await _context.SaveChangesAsync();
                    }
                }

                if (reservedSeat.status != "Cancelled")
                    return Conflict(JsonSerializer.Serialize(new
                    {
                        Error = $"Seat is reserved",
                        seatRow = reservation.seatRow,
                        seatColumn = reservation.seatColumn
                    }));
            }

                await _context.Reservations.AddAsync(reservation);
            await _context.SaveChangesAsync();
            return Created("", JsonSerializer.Serialize(new
            {
                id = reservation.id,
                row = reservation.seatRow,
                column = reservation.seatColumn,
                status = reservation.status,
            }));
        }

        [HttpPut]
        [Route("{reservationId:int}/confirmReservation")]
        [Authorize]
        public async Task<IActionResult> confirmReservation(int reservationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
                return Unauthorized();

            if (!await _context.Users.Where(x => x.Id == userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            var reservation = await _context.Reservations.Where(x => x.id == reservationId).FirstOrDefaultAsync();

            if (reservation == null)
                return NotFound();

            _context.Entry(reservation).CurrentValues.SetValues(new { status = "Confirmed" });
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }


        [HttpDelete]
        [Route("{reservationId:int}/cancelReservation")]
        [Authorize]
        public async Task<IActionResult> cancelReservation(int reservationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null)
                return Unauthorized();

            if (!await _context.Users.Where(x => x.Id == userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            var reservation = await _context.Reservations.Where(x => x.id == reservationId).FirstOrDefaultAsync();

            if (reservation == null)
                return NotFound();

            _context.Entry(reservation).CurrentValues.SetValues(new { status = "Canceled" });
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }
    }
}
