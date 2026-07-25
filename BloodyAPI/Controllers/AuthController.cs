using BloodyAPI.Data;
using BloodyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloodyAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext db;

        public AuthController(AppDbContext db)
        {
            this.db = db;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            bool exists = await db.Users
                .AnyAsync(x => x.Username == request.Username);

            if (exists)
                return BadRequest("Username already exists");


            var user = new User
            {
                Username = request.Username,

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(request.Password)
            };


            db.Users.Add(user);

            await db.SaveChangesAsync();


            return Ok(new
            {
                message = "Account created"
            });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await db.Users
                .FirstOrDefaultAsync(x =>
                    x.Username == request.Username);


            if (user == null)
                return Unauthorized("Invalid username or password");


            bool valid =
                BCrypt.Net.BCrypt.Verify(
                    request.Password,
                    user.PasswordHash);


            if (!valid)
                return Unauthorized("Invalid username or password");

            return Ok(new
            {
                message = "Login successful",
                username = user.Username,
                id = user.Id
            });
        }

        [HttpPost("checksubscribed")]
        public async Task<IActionResult> CheckSubscribed(int UserID)
        {
            var user = await db.Users
                .FirstOrDefaultAsync(x =>
                    x.Id == UserID);


            if (user == null)
                return Unauthorized("Invalid UserID");

            return Ok(new
            {
                subscribed = user.IsSubscribed,
                id = user.Id
            });
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";
    }



    public class LoginRequest
    {
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";
    }
}
