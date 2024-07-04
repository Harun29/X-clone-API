using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X_clone_API.Repository.Models;
using X_clone_API.Repository;

namespace X_clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {

        private readonly XCloneDbContext _context;

        public LikeController(XCloneDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> LikePost(int postId, int userId)
        {
            var liked = new Liked
            {
                PostLiked = postId,
                UserLiked = userId
            };

            _context.Likeds.Add(liked);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLiked(int likedId)
        {
            var liked = _context.Likeds.Find(likedId);
            if (liked == null)
            {
                return BadRequest();
            }

            _context.Likeds.Remove(liked);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
