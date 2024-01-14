using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Cinema.Controllers
{
    [Route("api/loayalityPoints")]
    [ApiController]
    public class LoyalityPointyController : BaseController
    {
        public LoyalityPointyController(AppDbContext context) : base(context) { }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserPoints()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userPoints = await _context.LoyalityPoints.FirstOrDefaultAsync(x => x.userId == userId);

            if (userPoints == null)
                return Ok(JsonSerializer.Serialize(new { }));
            return Ok(userPoints);
        }

        [HttpGet]
        [Route("{userId}")]
        [Authorize]
        public async Task<IActionResult> getUserPoints(string userId)
        {
            var userPoints = await _context.LoyalityPoints.FirstOrDefaultAsync(x => x.userId == userId);

            if (userPoints == null)
                return Ok(JsonSerializer.Serialize(new { }));
            return Ok(userPoints);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> addOrUpdatePoints([FromBody] LoyalityPointsDTO request)
        {
            if (request.userId == null)
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "You need to specify userId!"
                }));

            if (!await _context.Users.Where(x => x.Id == request.userId).AnyAsync())
                return BadRequest(JsonSerializer.Serialize(new
                {
                    Error = "User with given ID doesn't exists!"
                }));

            var dbObject = await _context.LoyalityPoints.FirstOrDefaultAsync(x => x.userId == request.userId);

            if (dbObject == null)
            {
                var pointsModel = mapper.Map<LoyalityPointsModel>(request);
                pointsModel.dateAdded = DateTime.UtcNow;
                await _context.LoyalityPoints.AddAsync(pointsModel);
                await _context.SaveChangesAsync();
                return Ok(pointsModel);
            }

            _context.Entry(dbObject).CurrentValues.SetValues(request);
            await _context.SaveChangesAsync();

            return Ok(dbObject);
        }

        [HttpDelete]
        [Route("{userId}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> deleteUserPoints(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest();


            if (!await _context.LoyalityPoints.AnyAsync(x => x.userId == userId))
            {
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Object doesn't exists"
                }));
            }
            await _context.LoyalityPoints.Where(x => x.userId == userId).ExecuteDeleteAsync();

            return Ok();
        }



    }
}
