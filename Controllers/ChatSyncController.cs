using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Data; 
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Models;

namespace OnlineCoursesPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatSyncController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ChatSyncController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncMessages([FromBody] List<ChatMessageDto> messages)
        {
            if (messages == null || !messages.Any())
                return BadRequest("No messages to sync.");

            var entities = messages.Select(m => new ChatMessage
            {
                UserId = m.SenderId,
                CategoryId = m.RoomId,
                Content = m.Content,
                Timestamp = m.Timestamp
            }).ToList();

            await _context.ChatMessages.AddRangeAsync(entities);
            await _context.SaveChangesAsync();

            return Ok(new { count = entities.Count, message = "Synced successfully" });
        }
    }
}