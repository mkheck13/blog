using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.Context;
using blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace blog.Services
{
    public class BlogServices
    {
        private readonly DataContext _dataContext;

        public BlogServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<BlogModel>> GetBlogsAsync() => await _dataContext.Blog.ToListAsync();

        public async Task<bool> AddBlogAsync(BlogModel blog)
        {
            await _dataContext.Blog.AddAsync(blog);
            return await _dataContext.SaveChangesAsync() !=0 ;
        }

        public async Task<bool> EditBlogAsync(BlogModel blog)
        {
            var blogToEdit = await GetBlogByIdAsync(blog.Id);

            if(blogToEdit == null) return false;

            blogToEdit.Title = blog.Title;
            blogToEdit.Image = blog.Image;
            blogToEdit.Category = blog.Category;
            blogToEdit.Description = blog.Description;
            blogToEdit.Date = blog.Date;
            blogToEdit.IsDeleted = blog.IsDeleted;
            blogToEdit.IsPublished = blog.IsPublished;
            blogToEdit.PublisherName = blog.PublisherName;

            _dataContext.Blog.Update(blogToEdit);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        // Find Async Searches by the Primary key (aka our ID) we use this over SingleOrDefault because its more efficient
        private async Task<BlogModel> GetBlogByIdAsync(int id) => await _dataContext.Blog.FindAsync(id);

        public async Task<List<BlogModel>> GetBlogsByUserIdAsync(int id) => await _dataContext.Blog.Where(blogs => blogs.UserId == id).ToListAsync();

        public async Task<List<BlogModel>> GetBlogsByCategoryAsync(string category) => await _dataContext.Blog.Where(blogs => blogs.Category == category).ToListAsync();
    }
}