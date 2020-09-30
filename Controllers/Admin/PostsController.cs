using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Data;
using Personal.Entities;
using Personal.Helpers;

namespace Personal.Controllers.Admin
{
    public class PostsController : Controller
    {
        private readonly AppDbContext context;

        public PostsController(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var posts = await context.Posts.Where(p => p.PostStatus != PostStatus.Trash).ToListAsync();
            return View(posts);
        }
        [HttpGet]
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["categories"] = await context.Categories.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Post post, int[] SelectedCategoryIds)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(post.Title);
            while (await context.Posts.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            post.Slug = slug;
            foreach (var selectedCatId in SelectedCategoryIds)
            {
                post.PostCategories.Add(new PostCategory { CategoryId = selectedCatId });
            }
            if (!ModelState.IsValid)
                return View();
            await context.AddAsync(post);
            await context.SaveChangesAsync();

            TempData["message.success"] = "Saved!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["categories"] = await context.Categories.ToListAsync();
            var post = await context.Posts
                .Include(p => p.PostCategories)
                .FirstOrDefaultAsync(p => p.Id == id);
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Post post, int[] SelectedCategoryIds)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(post.Title);
            while (await context.Posts.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            post.Slug = slug;
            
            //remove unselected
            var removedCategories=new List<PostCategory>();
            foreach (var postCategory in context.PostCategories)
            {
                if(!SelectedCategoryIds.Contains(postCategory.CategoryId))
                    removedCategories.Add(postCategory);
            }
            foreach (var postCategory in removedCategories)
            {
                context.PostCategories.Remove(postCategory);
            }

            foreach (var selectedCatId in SelectedCategoryIds.Where(selectedCatId => !context.PostCategories.Any(pc => pc.CategoryId == selectedCatId))
            //add newly selected
            )
            {
                context.PostCategories.Add(new PostCategory { CategoryId = selectedCatId, PostId = post.Id });
            }

            if (!ModelState.IsValid)
                return View();
            context.Update(post);
            await context.SaveChangesAsync();

            TempData["message.success"] = "Edited!";
            return RedirectToAction(nameof(Index));
        }
    }
}