using System;
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
            var posts = await context.Posts.Where(p=>p.PostStatus!=PostStatus.Trash).ToListAsync();
            return View(posts);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(post.Title);
            while (await context.Tags.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            post.Slug = slug;
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
            var post = await context.Posts.FindAsync(id);
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(post.Title);
            while (await context.Tags.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            post.Slug = slug;
            if (!ModelState.IsValid)
                return View();
            context.Update(post);
            await context.SaveChangesAsync();

            TempData["message.success"] = "Edited!";
            return RedirectToAction(nameof(Index));
        }
    }
}