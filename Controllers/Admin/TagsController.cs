using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Data;
using Personal.Entities;
using Personal.Helpers;

namespace Personal.Controllers.Admin
{
    public class TagsController : Controller
    {
        private readonly AppDbContext context;
        public TagsController(AppDbContext context)
        {
            this.context = context;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tags = await context.Tags.ToListAsync();
            return View(tags);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Tag tag)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(tag.Title);
            while (await context.Tags.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            tag.Slug = slug;
            if (!ModelState.IsValid)
                return View();
            await context.AddAsync(tag);
            await context.SaveChangesAsync();
            TempData["message.success"] = "Saved!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tag = await context.Tags.FindAsync(id);
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Tag tag)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(tag.Title);
            while (await context.Tags.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            tag.Slug = slug;
            if (!ModelState.IsValid)
                return View();
            context.Update(tag);
            await context.SaveChangesAsync();
            TempData["message.success"] = "Edited!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var tag = await context.Tags.FindAsync(id);
            if (tag == null)
            {
                TempData["message.info"] = "Tag not found";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirm([FromForm] int id)
        {
            var tag = await context.Tags.FindAsync(id);
            if (tag == null)
            {
                TempData["message.info"] = "Tag not found";
                return RedirectToAction(nameof(Index));
            }

            context.Remove(tag);
            await context.SaveChangesAsync();

            TempData["message.success"] = "Deleted!";
            return RedirectToAction(nameof(Index));

        }



    }
}