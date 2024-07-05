using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X_clone_API.Repository;
using X_clone_API.Repository.Models;

namespace X_clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepostController : ControllerBase
    {
        private readonly XCloneDbContext _context;

        public RepostController(XCloneDbContext context)
        {
                _context = context;
        }

        [HttpPost("AddRepost")]
        public async Task<IActionResult> PostRepost(int postReposted,[FromBody] int userReposted)
        {
            var repost = new Repost
            {
                PostReposted = postReposted,
                UserReposted = userReposted
            };

            var checkExisting = _context.Reposts.FirstOrDefault(r => r.PostReposted == postReposted && r.UserReposted == userReposted );
            if (checkExisting != null)
            {
                return BadRequest();
            }

            var post = await _context.Posts.FindAsync(postReposted);
            if(post == null)
            {
                return NotFound();
            }
            post.NoReposts += 1;

            _context.Posts.Update(post);
            await _context.Reposts.AddAsync(repost);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("DeleteRepost")]
        public async Task<IActionResult> DeleteRepost([FromBody] int repostId)
        {
            var repost = _context.Reposts.Find(repostId);
            if(repost == null) 
            {
                return BadRequest();
            }

            var postReposted = repost.PostReposted;

            var post = await _context.Posts.FindAsync(postReposted);
            if (post == null)
            {
                return NotFound();
            }
            post.NoReposts -= 1;

            _context.Posts.Update(post);
            _context.Reposts.Remove(repost);
            await _context.SaveChangesAsync();

            return Ok("Repost deleted!");
        }
    }
}
