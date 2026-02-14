using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Interfaces;
using OnlineCoursesPlatform.Models;

namespace OnlineCoursesPlatform.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        // Constructor
        public CategoriesController(
            ApplicationDbContext context,
            ICategoryService categoryService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        // Index Action - GET
        public async Task<ActionResult> Index()
        {
            IEnumerable<Category> CatList = await _categoryService.GetCategoriesThatHasContent();
            return View("_BrowesAllCoursesForNonRegistred", CatList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.CategoryItems)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return View("_DisplayCourseCardInfo", category);
        }

        [HttpGet]
        public async Task<IActionResult> IsUserSubscribed(int categoryId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { isSubscribed = false });
            }

            var userId = _userManager.GetUserId(User);
            var isSubscribed = await _context.UserCategories
                .AnyAsync(uc => uc.CategoryId == categoryId && uc.UserId == userId);

            return Json(new { isSubscribed = isSubscribed });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleSubscription(int categoryId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    success = false,
                    message = "Please login to subscribe",
                    requiresLogin = true
                });
            }

            var userId = _userManager.GetUserId(User);

            try
            {
                var existingSubscription = await _context.UserCategories
                    .FirstOrDefaultAsync(uc => uc.CategoryId == categoryId && uc.UserId == userId);

                if (existingSubscription != null)
                {
                    _context.UserCategories.Remove(existingSubscription);
                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        success = true,
                        isSubscribed = false,
                        message = "Unsubscribed successfully"
                    });
                }
                else
                {
                    var userCategory = new UserCategory
                    {
                        CategoryId = categoryId,
                        UserId = userId
                    };

                    _context.UserCategories.Add(userCategory);
                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        success = true,
                        isSubscribed = true,
                        message = "Subscribed successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Error: " + ex.Message
                });
            }
        }
    }
}