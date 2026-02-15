using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using OnlineCoursesPlatform.Data;

public class ChatController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);

        var myCoursesids = _context.UserCategories
            .Where(u => u.UserId == userId)
            .Select(u => u.CategoryId)
            .ToList();

        var myCourses = _context.Categories .Where(c => myCoursesids.Contains(c.Id)) .ToList();

        return View(myCourses);
    }


    [Route("Chat/history/{roomId}")] 
                 public async Task<IActionResult> GetHistory(int roomId)
    {
        var messages = await _context.ChatMessages
            .Where(m => m.CategoryId == roomId)
            .OrderByDescending(m => m.Timestamp)
            .Take(50)
            .Join(_context.Users,
                message => message.UserId,
                user => user.Id,
                (message, user) => new {
                    senderId = message.UserId.ToString(),
                    senderName = user.UserName,
                    content = message.Content,
                    timestamp = message.Timestamp
                })
            .OrderBy(m => m.timestamp) 
            .ToListAsync();

        return Ok(messages);
    }

    public IActionResult Room(int id)
    {
        ViewBag.RoomId = id;
        ViewBag.UserId = _userManager.GetUserId(User);
        ViewBag.UserName = _userManager.GetUserName(User);
        return View();
    }
}
