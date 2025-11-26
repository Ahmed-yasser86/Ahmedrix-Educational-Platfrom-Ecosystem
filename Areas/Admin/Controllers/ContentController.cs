using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Content
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Contents.ToListAsync());
        //}

     
        // GET: Content/Create
        public IActionResult Create(int categoryId, int categoryItemId)
        {
            Content newContent = new Content
            {
                CategoryId = categoryId,
                CatItemId = categoryItemId
            };
            return View(newContent);
        }

        // POST: Content/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,HTMLContent,VideoLink,CatItemId,CategoryId")] Content content)
        {
            var category = _context.CategoryItems.Find(content.CatItemId); // get existing category
            content.CategoryItem = category; // assign it
            ModelState.Remove("CategoryItem"); // ignore validation

            if (ModelState.IsValid)
            {
                _context.Add(content);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Category", new { Categoryid = category.CategoryId });
            }
            return View(content);
        }

        // GET: Content/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _context.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        // POST: Content/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,HTMLContent,VideoLink,CatItemId,CategoryId")] Content content)
        {
            if (id != content.Id)
            {
                return NotFound();
            }

            // Load the existing content WITH its navigation property
            var existingContent = await _context.Contents
                .Include(c => c.CategoryItem)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingContent == null)
            {
                return NotFound();
            }

            // Update the properties
            existingContent.Title = content.Title;
            existingContent.HTMLContent = content.HTMLContent;
            existingContent.VideoLink = content.VideoLink;

            ModelState.Remove("CategoryItem");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(existingContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                // Now use the CategoryId from the existing CategoryItem
                return RedirectToAction(nameof(Index), "CategoryItems",
                    new { area = "Admin", Categoryid = existingContent.CategoryItem.CategoryId });
            }

            return View(content);
        }



        private bool ContentExists(int id)
        {
            return _context.Contents.Any(e => e.Id == id);
        }
    }
}
