using ECom_API.Data;
using ECom_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Register([FromBody] User user)
        {
            // Validate if all fields are provided
            if (string.IsNullOrEmpty(user.FullName) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("All fields are required.");
            }

            // Check if the email already exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest(new { Message = "Email is already registered!" });
            }

            // Hash the password
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            // Get the role of the current user (the one making the request)
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);  // Assuming you are using authentication to identify the current user

            // If the current user is not an admin, set the role to "User"
            if (currentUser == null || currentUser.Role != "Admin")
            {
                user.Role = "User";  // Regular users can only register as "User"
            }

            // If the current user is an admin, use the role passed in the request
            if (currentUser != null && currentUser.Role == "Admin")
            {
                user.Role = user.Role ?? "User";  // Allow admin to specify a role, default to "User" if not provided
            }

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            // Log the incoming data to ensure correct structure
            Console.WriteLine($"Received Request: Email={user?.Email}, Password={user?.Password}");

            // Validate if both email and password are provided
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Email and Password are required.");
            }

            // Check if the user exists in the database
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser == null)
            {
                return Unauthorized(new { Message = "Invalid email or password!" });
            }

            // Check if the password matches
            bool passwordValid = BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password);
            if (!passwordValid)
            {
                return Unauthorized(new { Message = "Invalid email or password!" });
            }

            // Return success message and user data (excluding password)
            return Ok(new
            {
                Message = "Login successful.",
                User = new
                {
                    existingUser.Id,
                    existingUser.Email,
                    existingUser.FullName
                }
            });
        }

    }
}
