using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ThreadService.Data;
using ThreadService.Models;

namespace ThreadService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThreadPostsController : ControllerBase
    {
        private readonly ThreadServiceContext _context;

        public ThreadPostsController(ThreadServiceContext context)
        {
            _context = context;
        }

        // GET: api/ThreadPosts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadPost>>> GetThreadPost()
        {
            return await _context.ThreadPost.ToListAsync();
        }

        // GET: api/ThreadPosts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadPost>> GetThreadPost(long id)
        {
            var threadPost = await _context.ThreadPost.FindAsync(id);

            if (threadPost == null)
            {
                return NotFound();
            }

            return threadPost;
        }

        // PUT: api/ThreadPosts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThreadPost(long id, ThreadPost threadPost)
        {
            if (id != threadPost.Id)
            {
                return BadRequest();
            }

            _context.Entry(threadPost).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadPostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ThreadPosts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ThreadPost>> PostThreadPost(ThreadPost threadPost)
        {
            _context.ThreadPost.Add(threadPost);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetThreadPost", new { id = threadPost.Id }, threadPost);
        }

        // DELETE: api/ThreadPosts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThreadPost(long id)
        {
            var threadPost = await _context.ThreadPost.FindAsync(id);
            if (threadPost == null)
            {
                return NotFound();
            }

            _context.ThreadPost.Remove(threadPost);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ThreadPostExists(long id)
        {
            return _context.ThreadPost.Any(e => e.Id == id);
        }
    }
}
