using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using X_clone_API.Repository.Models;
using X_clone_API.Repository;
using System.Globalization;

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
        public async Task<IActionResult> AddUser(string email, string name, string username, string birthday, string? bio = null, IFormFile? profilePicture = null, IFormFile? coverPicture = null)
        {

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(birthday))
            {
                return BadRequest();
            }

            if (!DateOnly.TryParseExact(birthday, "yyyy-MM-dd", null, DateTimeStyles.None, out DateOnly parsedBirthday))
            {
                return BadRequest("Invalid date format for birthday. Use 'yyyy-MM-dd'.");
            }

            var user = new User {
                Email = email,
                Name = name,
                Username = username,
                Birthday = parsedBirthday,
                Bio = bio ?? string.Empty,
                NoFollowers = 0,
                NoFollowing = 0,
            };

            if (profilePicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await profilePicture.CopyToAsync(memoryStream);
                    user.ProfilePicture = memoryStream.ToArray();
                }
            }
            if (coverPicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await coverPicture.CopyToAsync(memoryStream);
                    user.CoverPicture = memoryStream.ToArray();
                }
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // get user
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser([FromRoute] string username) 
        {   
            var user = _context.Users.Find(username);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
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
                user.ProfilePicture = memoryStream.ToArray();
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

            user.ProfilePicture = null;
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
