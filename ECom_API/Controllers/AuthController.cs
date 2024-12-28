using ECom_API.Data;
using ECom_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECom_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            // Check the email already exists
            if (_context.Users.Any(u => u.Email == user.Email)) 
                return BadRequest("Email is already registerd!");

            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registerd successfully.");
        }

        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
                return Unauthorized("Invalid email or password!");

            return Ok(new { message = "Login successful.", user = existingUser });
        }
    }
}
