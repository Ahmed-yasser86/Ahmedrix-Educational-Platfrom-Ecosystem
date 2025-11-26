using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCoursesPlatform.Areas_Admin_Controllers
{


    [Area("Admin")]

    public class CategoryItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CategoryItems
        public async Task<IActionResult> Index(int Categoryid)
        {
            List<CategoryItem> CatItems = await (from Item in _context.CategoryItems
                                                 join content in _context.Contents on Item.Id equals content.CategoryItem.Id into contentGroup
                                                 from subcontent in contentGroup.DefaultIfEmpty()
                                                 where Item.CategoryId == Categoryid
                                                 select new CategoryItem
                                                 {
                                                     Title = Item.Title,
                                                     Description = Item.Description,
                                                     DateTimeItemAdded = Item.DateTimeItemAdded,
                                                     MediaTypeId = Item.MediaTypeId,
                                                     Id = Item.Id,
                                                     CategoryId = Item.CategoryId,
                                                     contentId = subcontent != null ? subcontent.Id : 0
                                                 }).ToListAsync();
            ViewData["Categoryid"]= Categoryid;    
            return View(CatItems);
        }

        // GET: CategoryItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryItem = await _context.CategoryItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            return View(categoryItem);
        }

        // GET: CategoryItems/Create
        public async Task<IActionResult> Create(int categoryId)
        {

            List<Entities.MediaType> items = await _context.MediaTypes.ToListAsync();

            ViewData["Categoryid"]= categoryId;

            CategoryItem categoryItem = new CategoryItem
            {

                CategoryId = categoryId,
                MediaTypes = items.ConvertToSelectedListItems(0)

            };


            


            return View(categoryItem);
        }

        // POST: CategoryItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,DateTimeItemAdded,CategoryId,MediaTypeId")] CategoryItem categoryItem)
        {


            if (ModelState.IsValid)
            {
                _context.Add(categoryItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryItem);
        }

        // GET: CategoryItems/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var categoryItem = await _context.CategoryItems.FindAsync(id);
            List<Entities.MediaType> items = await _context.MediaTypes.ToListAsync();

            categoryItem.MediaTypes = items.ConvertToSelectedListItems(0);

            if (categoryItem == null)
            {
                return NotFound();
            }
            return View(categoryItem);
        }

        // POST: CategoryItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DateTimeItemAdded,CategoryId,MediaTypeId")] CategoryItem categoryItem)
        {


            ModelState.Remove("MediaTypes");

            if (id != categoryItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryItemExists(categoryItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Field: {state.Key} - Error: {error.ErrorMessage}");
                }
            }

            return View(categoryItem);
        }

        // GET: CategoryItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoryItem = await _context.CategoryItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            return View(categoryItem);
        }

        // POST: CategoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryItem = await _context.CategoryItems.FindAsync(id);
            if (categoryItem != null)
            {
                _context.CategoryItems.Remove(categoryItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryItemExists(int id)
        {
            return _context.CategoryItems.Any(e => e.Id == id);
        }
    }
}
