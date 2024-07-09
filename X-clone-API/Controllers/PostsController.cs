using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using X_clone_API.Repository;
using X_clone_API.Repository.Models;

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
        [HttpGet("post/{postID}")]
        public IActionResult GetPost([FromRoute] int postID)
        {
            var post = _context.Posts
                                .Include(p => p.UserPostedNavigation)
                                .FirstOrDefault(p => p.PostId == postID);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        //GET POSTS BY CURRENT USER
        [HttpGet("posts-by-user")]
        public async Task<IActionResult> GetUsersPosts(string username)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.Reposts)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            var allPosts = user.Posts.ToList();

            foreach(Repost re in user.Reposts)
            {
                var post = _context.Posts
                                    .Include(p => p.UserPostedNavigation)
                                    .FirstOrDefault(p => p.PostId == re.PostReposted);
                if(post == null)
                {
                    return BadRequest();
                }
                post.Reposts = [];
                allPosts.Add(post);
            }

            var orderedPosts = allPosts.OrderByDescending(p => p.PostId).ToList();

            return Ok(orderedPosts);
        }



        //GET POSTS BY USERS THAT CURRENT USER IS FOLLOWING
        [HttpGet("followingPosts/{username}")]
        public async Task<IActionResult> GetPostsByFollowing([FromRoute] string username)
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
                .Include(p => p.UserPostedNavigation)
                .ToListAsync();

            var reposts = await _context.Reposts
                .Where(p => followingUserIds.Contains(p.UserReposted))
                .Include (p => p.PostRepostedNavigation)
                .ToListAsync();

            foreach (Repost re in reposts)
            {
                var post = re.PostRepostedNavigation;
                post.Reposts = [];
                posts.Add(post);
            }

            var orderedPosts = posts.OrderByDescending(p => p.PostId).ToList();

            return Ok(orderedPosts);
        }

        //CREATE POST

        [HttpPost("create/{userId}")]
        public async Task<IActionResult> CreatePost([FromRoute] int userId,[FromBody] string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest();
            }
            var post = new Post
            {
                UserPosted = userId,
                PostContent = content
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //DELETE POST

        [HttpDelete("deletePost/{postID}")]
        public async Task<IActionResult> DeletePost([FromRoute] int postID)
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
        public async Task<IActionResult> UpdatePost([FromRoute] int postID, int usersId,[FromBody] string newContent) 
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
