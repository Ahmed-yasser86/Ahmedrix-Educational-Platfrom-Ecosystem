using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsletterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Email is required!" });
            }

            var exists = _context.Newsletters.Any(n => n.Email == email);
            if (exists)
            {
                return Json(new { success = false, message = "You are already subscribed!" });
            }

            var newsletter = new Newsletter { Email = email };
            _context.Newsletters.Add(newsletter);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Subscribed successfully! Thank you." });
        }
    }
}