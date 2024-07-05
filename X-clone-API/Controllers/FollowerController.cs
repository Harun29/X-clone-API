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
        public async Task<IActionResult> AddFollower(int userId, int followedId)
        {
            var follower = new Follower
            {
                UserFollowing = userId,
                UserFollowed = followedId
            };

            var checkExisting = _context.Followers.FirstOrDefault(f => f.UserFollowing == userId && f.UserFollowed == followedId);
            if (checkExisting != null) {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(userId);
            var followed = await _context.Users.FindAsync(followedId);
            if(user == null || followed == null)
            {
                return NotFound();
            }

            user.NoFollowing += 1;
            followed.NoFollowers += 1;

            _context.Users.Update(user);
            _context.Users.Update(followed);

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
            var followedId = follower.UserFollowing;
            var user = await _context.Users.FindAsync(userId);
            var followed = await _context.Users.FindAsync(followedId);
            if (user == null || followed == null)
            {
                return NotFound();
            }

            user.NoFollowing -= 1;
            followed.NoFollowers -= 1;

            _context.Users.Update(user);
            _context.Users.Update(followed);

            _context.Followers.Remove(follower);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
