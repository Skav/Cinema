using AutoMapper;
using Cinema.Models;
using Cinema.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Cinema.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : BaseController
    {
        public RoomController(AppDbContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> getRooms()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return Ok(rooms);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> getRoomById(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var room = await _context.Rooms.FirstOrDefaultAsync(x => x.id == id);

            if (room == null)
                return Ok(JsonSerializer.Serialize(new { }));

            return Ok(room);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> addRoom([FromBody] RoomDTO request)
        {
            if (request == null || request.seatsInRow <= 0 || request.rows <= 0 || request.seatsInRow <= 0 || request.roomNo <= 0)
                return BadRequest();

            var room = mapper.Map<RoomsModel>(request);
            room.dateAdded = DateTime.UtcNow;

            if (await _context.Rooms.AnyAsync(x => x.roomNo == room.roomNo))
            {
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Object already exists"
                }));
            }
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();

            return Ok(room);
        }

        [HttpDelete]
        [Route("{id:int}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> deleteRoom(int id)
        {
            if (!await _context.Rooms.AnyAsync(x => x.id == id))
            {
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Object doesn't exists"
                }));
            }
            await _context.Rooms.Where(x => x.id == id).ExecuteDeleteAsync();

            return Ok();
        }

        [HttpPut]
        [Route("{id:int}/edit")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> updateRoom(int id, [FromBody] RoomDTO request)
        {
            if (request == null)
                return BadRequest();

            var dbObject = await _context.Rooms.FindAsync(id);

            if (dbObject == null)
                return NotFound();

            _context.Entry(dbObject).CurrentValues.SetValues(request);
            await _context.SaveChangesAsync();

            return Ok(dbObject);
        }
    }
}
