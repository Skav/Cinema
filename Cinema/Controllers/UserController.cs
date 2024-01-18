using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Security.Claims;
using System.Text.Json;

namespace Cinema.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly UserManager<UsersModel> _usersManager;
        public UserController(AppDbContext context, UserManager<UsersModel> userManager): base(context) {
            _usersManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getUserInfo()
        {
            string userRole;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

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


            try
            {
                userRole = User.FindFirst(ClaimTypes.Role).Value;
            }
            catch (NullReferenceException)
            {
                userRole = "Customer";
                var userDb = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                var user = await _usersManager.FindByIdAsync(userDb.Id);


                if (user == null) 
                    return Conflict();

                await _usersManager.AddToRoleAsync(user, userRole);
                _context.SaveChanges();
            }

           

            return Ok(JsonSerializer.Serialize(new
            {
                username = userData.UserName,
                email = userData.Email,
                role = userRole
            }));
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        [Route("create")]
        public async Task<IActionResult> addStaffUser(StaffDTO request)
        {
            if (await _usersManager.FindByEmailAsync(request.email) != null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User exists!"
                }));

            if (await _context.Users.Where(x => x.UserName == request.username).FirstOrDefaultAsync() != null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User exists!"
                }));


            if (request.role != "Admin" && request.role != "Staff")
                return BadRequest(JsonSerializer.Serialize(new
                {
                    error = "Wrong role"
                }));

            var user = new UsersModel();
            user.Email = request.email;
            user.UserName = request.username;
            user.EmailConfirmed = true;
            await _usersManager.CreateAsync(user, request.password);
            await _usersManager.AddToRoleAsync(user, request.role);
            _context.SaveChanges();

            return Created("", JsonSerializer.Serialize(new
            {
                username = user.UserName,
                email = user.Email
            }));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> registerUser(UserDTO request)
        {
            if (await _usersManager.FindByEmailAsync(request.email) != null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User exists!"
                }));

            if (await _context.Users.Where(x => x.UserName == request.username).FirstOrDefaultAsync() != null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User exists!"
                }));

            var user = new UsersModel();
            user.Email = request.email;
            user.UserName = request.username;
            user.EmailConfirmed = true;
            await _usersManager.CreateAsync(user, request.password);
            await _usersManager.AddToRoleAsync(user, "Customer");
            _context.SaveChanges();

            return Created("", JsonSerializer.Serialize(new
            {
                username = user.UserName,
                email = user.Email
            }));
        }

    }


}
 