﻿using Microsoft.AspNetCore.Http;
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

        [HttpPost("add-repost")]
        public async Task<IActionResult> PostRepost(int postReposted,[FromBody] int userReposted)
        {
            var repost = new Repost
            {
                PostReposted = postReposted,
                UserReposted = userReposted
            };

            _context.Reposts.Add(repost);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("delete-repost")]
        public async Task<IActionResult> DeleteRepost([FromBody] int repostId)
        {
            var repost = _context.Reposts.Find(repostId);
            if(repost == null) 
            { 
                return BadRequest()
            }

            _context.Reposts.Remove(repost);
            await _context.SaveChangesAsync();

            return Ok("Repost deleted!");
        }
    }
}
