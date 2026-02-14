using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Areas.Admin.Models;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Notifications;
using System.Text.Json;

namespace OnlineCoursesPlatform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StreamsController : Controller
    {

        private readonly MediaService _mediaService;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        public StreamsController(MediaService mediaService, ApplicationDbContext context, IMediator mediator)
        {
            _mediaService = mediaService;
            _context = context;
            _mediator = mediator;
            
        }

        public async Task<IActionResult> Index()
        {
            var sessions = await _context.LiveSessions
                .Include(s => s.Instructor)
                .Include(s => s.Category)
                .Where(s => !s.IsDeleted)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            var viewModel = new AdminStreamDashboardViewModel
            {
                ActiveSessions = sessions.Where(s => s.IsActive).ToList(),
                PastSessions = sessions.Where(s => !s.IsActive).ToList()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new StartStreamViewModel
            {
                AvailableInstructors = await _context.Instructors
                    .Select(i => new SelectListItem { Value = i.Id.ToString(), Text = i.Name })
                    .ToListAsync(),
                AvailableCategories = new List<SelectListItem>()
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetCategoriesByInstructor(int instructorId)
        {
            var categories = await _context.Categories
                .Where(c => c.InstructorId == instructorId)
                .Select(c => new { value = c.Id, text = c.Title })
                .ToListAsync();
            return Json(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StartStreamViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueKey = $"live_{model.SelectedInstructorId}_{Guid.NewGuid().ToString().Substring(0, 6)}";

                var session = new LiveSession
                {
                    Title = model.Title,
                    CategoryId = model.SelectedCategoryId,
                    InstructorId = model.SelectedInstructorId,
                    StreamKey = uniqueKey,
                    StartTime = DateTime.Now,
                    IsActive = true
                };

                _context.LiveSessions.Add(session);
                await _context.SaveChangesAsync();

                var category = await _context.Categories.FindAsync(model.SelectedCategoryId);
                var instructor = await _context.Instructors.FindAsync(model.SelectedInstructorId);

                await _mediator.Publish(new LiveStreamStartedNotification
                {
                    CategoryId = model.SelectedCategoryId,
                    CategoryTitle = category?.Title ?? "Course",
                    StreamTitle = model.Title,
                    InstructorName = instructor?.Name ?? "Instructor"
                });

                model.GeneratedStreamKey = uniqueKey;
                return View("StreamReady", model);
            }
            return View(model);
        }

        [HttpPost]
        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> EndStream(string streamKey)
        {
            var session = await _context.LiveSessions
                .FirstOrDefaultAsync(s => s.StreamKey == streamKey && !s.IsDeleted);

            if (session != null)
            {
                session.IsActive = false;
                session.EndTime = DateTime.Now;

                
                await _context.SaveChangesAsync();
                TempData["Success"] = "Stream Has terminated";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}