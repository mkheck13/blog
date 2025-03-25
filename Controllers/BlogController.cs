using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.Models;
using blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace blog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly BlogServices _blogServices;

        public BlogController(BlogServices blogServices)
        {
            _blogServices = blogServices;
        }

        [HttpGet("GetAllBlogs")]
        public async Task<IActionResult> GetAllBlogs()
        {
            var blogs = await _blogServices.GetBlogsAsync();

            if(blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs Found"});
        }

        [HttpPost("AddBlog")]
        public async Task<IActionResult> AddBlog([FromBody] BlogModel blog)
        {
            var success = await _blogServices.AddBlogAsync(blog);

            if(success) return Ok(new {Success = true});

            return BadRequest(new {Message = "Failed to Add Blog"});
        }

        [HttpPut("EditBlog")]
        public async Task<IActionResult> EditBlog([FromBody] BlogModel blog)
        {
            var success = await _blogServices.EditBlogAsync(blog);

            if(success) return Ok(new {Success = true});

            return BadRequest(new {Message = "No Blog was Found"});
        }

        [HttpDelete("DeleteBlog")]
        public async Task<IActionResult> DeleteBlog([FromBody] BlogModel blog)
        {
            var success = await _blogServices.EditBlogAsync(blog);

            if(success) return Ok(new {Success = true});

            return BadRequest(new {Message = "No Blog was Found"});
        }

        [HttpGet("GetBlogsByUserId/{userId}")]
        public async Task<IActionResult> GetAllBlogsByUserId(int userId)
        {
            var blogs = await _blogServices.GetBlogsByUserIdAsync(userId);

            if(blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs Found"});
        }

        [HttpGet("GetBlogsByCategory/{category}")]
        public async Task<IActionResult> GetAllBlogsByCategory(string category)
        {
            var blogs = await _blogServices.GetBlogsByCategoryAsync(category);

            if(blogs != null) return Ok(blogs);

            return BadRequest(new {Message = "No Blogs Found"});
        }
    }
}