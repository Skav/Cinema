using Cinema.DTO;
using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

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
            var userRole = User.FindFirst(ClaimTypes.Role).Value;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId == null) 
                return Unauthorized();

            var query = from AspNetUsers in _context.Users
                        where AspNetUsers.Id == userId
                        select new
                        {
                            AspNetUsers.UserName,
                            AspNetUsers.Email,
                        };

            var userData = await query.FirstOrDefaultAsync();

            if (userData == null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User doesnt exists!"
                }));
            

            return Ok(JsonSerializer.Serialize(new
            {
                username = userData.UserName,
                email = userData.Email,
                role = userRole
            }));
        }

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> getAllUserInfo()
        {

            var users = await _context.Users.ToArrayAsync();

            if (users == null)
                return Ok(JsonSerializer.Serialize(new {}));

            var usersList = new List<userTransportDTO>{ };

            foreach (var userData in users)
            {
                var userModel = new UsersModel();
                var userDTO = new userTransportDTO();
                userModel.UserName = userData.UserName;
                userModel.Email = userData.Email;
                userModel.Id = userData.Id;

                var role = await _usersManager.GetRolesAsync(userModel);

                userDTO.email = userModel.Email;
                userDTO.username = userModel.UserName;
                userDTO.id = userModel.Id;
                userDTO.role = role[0];

                usersList.Add(userDTO);
            }

            return Ok(usersList);
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


            if (request.role != "Admin" && request.role != "Staff" && request.role != "Customer")
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Wrong role"
                }));


            if (!checkEmail(request.email))
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Email address is invalid!"
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
                    error = "User with given email exists!"
                }));

            if (await _context.Users.Where(x => x.UserName == request.username).FirstOrDefaultAsync() != null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "User with given username exists!"
                }));

            var validator = new PasswordValidator<UsersModel>();
            var passwordResult = await validator.ValidateAsync(_usersManager, null, request.password);
            if (!passwordResult.Succeeded)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Password is to weak! You need to use 1 digit, 1 special character and 1 capital letter"
                }));


            if (!checkEmail(request.email))
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Email address is invalid!"
                }));

            var user = new UsersModel();
            user.Email = request.email;
            user.UserName = request.username;
            user.EmailConfirmed = true;
            await _usersManager.CreateAsync(user, request.password);
            _context.SaveChanges();
            await _usersManager.AddToRoleAsync(user, "Customer");
            _context.SaveChanges();

            return Created("", JsonSerializer.Serialize(new
            {
                username = user.UserName,
                email = user.Email
            }));
        }

        [HttpDelete]
        [Route("{userId}/delete")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> removeUser(string userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                return Conflict(JsonSerializer.Serialize(new
                {
                    error = "Given user do not exists!"
                }));

            await _usersManager.DeleteAsync(user);
            return Ok();
        }

        private bool checkEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            return match.Success;

        }
    }
}
 