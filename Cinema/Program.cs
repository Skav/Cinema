using Cinema.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cinema
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database
            builder.Services.AddEntityFrameworkNpgsql()
                .AddDbContext<AppDbContext>(opt => opt.UseNpgsql(
                builder.Configuration.GetConnectionString("PostgresConnectionString")));

            // Add authentication with JWT Bearer Token
            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("OURSECRETKEY")), // Replace with your secret key
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // Set issuer and audience if you want to validate them
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddIdentityCore<UsersModel>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders(); // Add this to enable token generation

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Add this to enable authentication
            app.UseAuthorization();

            app.MapControllers();

            // Add roles
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Staff", "Customer" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Add basic admin
            using (var scope = app.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UsersModel>>();

                string username = "Admin";
                string email = "admin@admin.com";
                string password = "Test12#";

                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new UsersModel();
                    user.UserName = username;
                    user.Email = email;
                    user.EmailConfirmed = true;

                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.Run();
        }
    }
}
