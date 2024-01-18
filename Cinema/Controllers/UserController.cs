using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace Cinema.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        public UserController(AppDbContext context): base(context) { }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserInfo()
        {
            string userRole;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            try
            {
                userRole = User.FindFirst(ClaimTypes.Role).Value;
            }
            catch (NullReferenceException)
            {
                userRole = "Customer";
            }

            var query = from AspNetUsers in _context.Users
                        where AspNetUsers.Id == userId
                        select new
                        {
                            AspNetUsers.UserName, 
                            AspNetUsers.Email, 
                        };

            var userData = await query.FirstOrDefaultAsync();

            if (userData == null)
                return NotFound();

            return Ok(JsonSerializer.Serialize(new
            {
                username = userData.UserName,
                email = userData.Email,
                role = userRole
            }));
        }

    }
}
 