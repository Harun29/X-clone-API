using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X_clone_API.Repository;

namespace X_clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        public readonly XCloneDbContext _context;

        public PostController(XCloneDbContext context)
        {
            _context = context;
        }

        //GET POSTS

        //GET POST BY ID
        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetPost([FromHeader] int postID)
        {
            var post = _context.Posts.Find(postID);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        //GET POSTS BY CURRENT USER
        [HttpGet("posts/{username}")]
        public async Task<IActionResult> GetUsersPost([FromHeader] string username)
        {
            var user = await _context.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            var posts = user.Posts.ToList();
            return Ok(posts);
        }


        //GET POSTS BY USERS THAT CURRENT USER IS FOLLOWING
        [HttpGet("followingPosts/{username}")]
        public async Task<IActionResult> GetPostsByFollowing([FromHeader] string username)
        {
            // Get the current user ID from the username
            var user = await _context.Users
                .Include(u => u.FollowerUserFollowingNavigations)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Get the user IDs of the users the current user is following
            var followingUserIds = user.FollowerUserFollowingNavigations
                .Select(f => f.UserFollowed)
                .ToList();

            // Get the posts by the users the current user is following
            var posts = await _context.Posts
                .Where(p => followingUserIds.Contains(p.UserPosted))
                .ToListAsync();

            return Ok(posts);
        }

        //CREATE POST

        //DELETE POST

        [HttpDelete("deletePost/{postID}")]
        public async Task<IActionResult> DeletePost([FromHeader] int postID)
        {
            var post = _context.Posts.Find(postID);
            if(post == null)
            {
                return BadRequest();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok($"Post with the id of {postID} was deleted!");
        }

        //EDIT POST

        [HttpPut("update/{postID}")]
        public async Task<IActionResult> UpdatePost([FromHeader] int postID, [FromBody] int usersId, string newContent) 
        {
            var post = _context.Posts.Find(postID);
            if(post == null)
            {
                return BadRequest();
            }
            if(usersId != post.UserPosted) 
            {
                return BadRequest();
            }

            post.PostContent = newContent;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return Ok($"Updated post with the id of {postID}!");
        }

    }
}
