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

        //GET POST BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost([FromHeader] int postID)
        {
            var post = _context.Posts.Find(postID);
            if(post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        //GET POSTS BY CURRENT USER
        [HttpGet("usersPosts")]
        public async Task<IActionResult> GetUsersPost([FromBody] string username)
        {
            var user = await _context.Users.FindAsync(username);
            if(user == null)
            {
                return NotFound();
            }
            var posts = user.Posts.ToList();
            return Ok(posts);
        }


        //GET POSTS BY USERS THAT CURRENT USER IS FOLLOWING
        [HttpGet("Following/{username}")]
        public async Task<IActionResult> GetPostsByFollowing(string username)
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


    }
}
