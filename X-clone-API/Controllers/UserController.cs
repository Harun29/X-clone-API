using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using X_clone_API.Repository.Models;
using X_clone_API.Repository;

namespace X_clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly XCloneDbContext _context;

        public UserController(XCloneDbContext context)
        {
            _context = context;
        }


        // ADD NEW USER
        [HttpPost]
        public async Task<IActionResult> AddUser(string email, string name, string username, string birthday)
        {
            if (!DateOnly.TryParse(birthday, out DateOnly parsedBirthday))
            {
                return BadRequest("Invalid date format for birthday.");
            }

            //date format "yyyy-mm-dd"

            var user = new User { Email = email, Name = name, Username = username, Birthday = parsedBirthday };
            if(user == null)
            {
                return BadRequest();
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok();
        }

        // Get Profile Picture
        [HttpGet("{username}/profile-picture")]
        public async Task<IActionResult> GetProfilePicture(string username)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return File(user.ProfilePicute, "image/jpeg"); // Assuming the image is in jpeg format
        }

        // Get Cover Picture
        [HttpGet("{username}/cover-picture")]
        public async Task<IActionResult> GetCoverPicture(string username)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return File(user.CoverPicture, "image/jpeg"); // Assuming the image is in jpeg format
        }

        // Update Profile Picture
        [HttpPut("{username}/profile-picture")]
        public async Task<IActionResult> UpdateProfilePicture(string username, [FromForm] IFormFile profilePicture)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            using (var memoryStream = new MemoryStream())
            {
                await profilePicture.CopyToAsync(memoryStream);
                user.ProfilePicute = memoryStream.ToArray();
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Update Cover Picture
        [HttpPut("{username}/cover-picture")]
        public async Task<IActionResult> UpdateCoverPicture(string username, [FromForm] IFormFile coverPicture)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            using (var memoryStream = new MemoryStream())
            {
                await coverPicture.CopyToAsync(memoryStream);
                user.CoverPicture = memoryStream.ToArray();
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Profile Picture
        [HttpDelete("{username}/profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture(string username)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            user.ProfilePicute = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Cover Picture
        [HttpDelete("{username}/cover-picture")]
        public async Task<IActionResult> DeleteCoverPicture(string username)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            user.CoverPicture = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
