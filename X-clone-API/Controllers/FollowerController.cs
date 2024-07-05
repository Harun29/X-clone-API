using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X_clone_API.Repository;
using X_clone_API.Repository.Models;

namespace X_clone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowerController : ControllerBase
    {
        private readonly XCloneDbContext _context;

        public FollowerController (XCloneDbContext context)
        {
            _context = context;
        }

        [HttpPost("Follow")]
        public async Task<IActionResult> AddFollower(int userId, int followingId)
        {
            var follower = new Follower
            {
                UserFollowing = followingId,
                UserFollowed = userId
            };
            var user = await _context.Users.FindAsync(userId);
            var following = await _context.Users.FindAsync(followingId);
            if(user == null || following == null)
            {
                return NotFound();
            }

            user.NoFollowing += 1;
            following.NoFollowers += 1;

            _context.Users.Update(user);
            _context.Users.Update(following);

            await _context.Followers.AddAsync(follower);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("Unfollow")]
        public async Task<IActionResult> RemoveFollower(int followerId)
        {
            var follower = _context.Followers.Find(followerId);
            if(follower == null)
            {
                return BadRequest();
            }

            var userId = follower.UserFollowed;
            var followingId = follower.UserFollowing;
            var user = await _context.Users.FindAsync(userId);
            var following = await _context.Users.FindAsync(followingId);
            if (user == null || following == null)
            {
                return NotFound();
            }

            user.NoFollowing -= 1;
            following.NoFollowers -= 1;

            _context.Users.Update(user);
            _context.Users.Update(following);

            _context.Followers.Remove(follower);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
