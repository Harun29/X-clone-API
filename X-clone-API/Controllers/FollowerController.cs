﻿using Microsoft.AspNetCore.Http;
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

        [HttpPost("add-follower")]
        public async Task<IActionResult> AddFollower(int userId, int followingId)
        {
            var follower = new Follower
            {
                UserFollowing = followingId,
                UserFollowed = userId
            };

            await _context.AddAsync(follower);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("remove-follower")]
        public async Task<IActionResult> RemoveFollower(int followerId)
        {
            var follower = _context.Followers.Find(followerId);
            if(follower == null)
            {
                return BadRequest();
            }

            return Ok();
        }

    }
}
