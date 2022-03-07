using ArenaBlog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArenaBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly BlogContext _context;

        public ValuesController(BlogContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
        {
            return await _context.BlogPosts.ToListAsync();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
        {
            var blogs = await _context.BlogPosts.FindAsync(id);

            if (blogs == null)
            {
                return NotFound();
            }

            return blogs;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult<BlogPost>> PostBlogPost(BlogPost model)
        {
            _context.BlogPosts.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBlogPosts", new { id = model.BlogPostId }, model);
        }
        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBlogPost(int id, BlogPost model)
        {
            if (id != model.BlogPostId)
            {
                return BadRequest();
            }

            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogPostExists(id))
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

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BlogPost>> DeleteBlogPost(int id)
        {
            var blogs = await _context.BlogPosts.FindAsync(id);
            if (blogs == null)
            {
                return NotFound();
            }

            _context.BlogPosts.Remove(blogs);
            await _context.SaveChangesAsync();

            return blogs;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPost>>> GetProducts(string[] Tags)
        {
            var blogs = _context.BlogPosts.AsQueryable();

            if (Tags != null) // check availability 
            {
                blogs = _context.BlogPosts.Where(i => i.Tags == Tags);
            }

            return await blogs.ToListAsync();
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.BlogPostId == id);
        }
    }
}
