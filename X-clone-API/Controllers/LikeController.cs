using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X_clone_API.Repository.Models;
using X_clone_API.Repository;
using Microsoft.Extensions.Hosting;

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

            var checkExisting = _context.Likeds.FirstOrDefault(f => f.UserLiked == userId && f.PostLiked == postId);
            if (checkExisting != null)
            {
                return BadRequest();
            }

            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            post.NoLikes += 1;

            _context.Likeds.Add(liked);
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLiked(int userId, int postId)
        {
            var liked = _context.Likeds.FirstOrDefault(l => l.UserLiked == userId && l.PostLiked == postId);
            if (liked == null)
            {
                return BadRequest();
            }

            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            post.NoLikes -= 1;

            _context.Posts.Update(post);
            _context.Likeds.Remove(liked);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
