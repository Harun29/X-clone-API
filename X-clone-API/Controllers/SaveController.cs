using Microsoft.AspNetCore.Mvc;
using X_clone_API.Repository;
using X_clone_API.Repository.Models;

namespace X_clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaveController : ControllerBase
    {
        private readonly XCloneDbContext _context;

        public SaveController(XCloneDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SavePost(int postId, int userId)
        {
            var saved = new Saved
            {
                PostSaved = postId,
                UserSaved = userId
            };

            _context.Saveds.Add(saved);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSaved(int savedId)
        {
            var saved = _context.Saveds.Find(savedId);
            if(saved == null)
            {
                return BadRequest();
            }

            _context.Saveds.Remove(saved);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
