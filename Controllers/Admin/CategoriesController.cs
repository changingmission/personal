using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personal.Data;
using Personal.Entities;
using Personal.Helpers;

namespace Personal.Controllers.Admin
{
    public class CategoriesController:Controller
    {
        private readonly AppDbContext context;
        public CategoriesController(AppDbContext context)
        {
            this.context = context;

        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await context.Categories.ToListAsync();
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(category.Title);
            while (await context.Tags.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            category.Slug = slug;
            if (!ModelState.IsValid)
                return View();
            await context.AddAsync(category);
            await context.SaveChangesAsync();
            TempData["message.success"] = "Saved!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories.FindAsync(id);
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            var rand = new Random();
            var slug = SlugHelper.GenerateSlug(category.Title);
            while (await context.Tags.AnyAsync(t => t.Slug == slug))
            {
                slug += rand.Next(1000, 9999);
            }
            category.Slug = slug;
            if (!ModelState.IsValid)
                return View();
            context.Update(category);
            await context.SaveChangesAsync();
            TempData["message.success"] = "Edited!";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var tag = await context.Categories.FindAsync(id);
            if (tag == null)
            {
                TempData["message.info"] = "Category not found";
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirm([FromForm] int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["message.info"] = "Category not found";
                return RedirectToAction(nameof(Index));
            }

            context.Remove(category);
            await context.SaveChangesAsync();

            TempData["message.success"] = "Deleted!";
            return RedirectToAction(nameof(Index));

        }
    }
}