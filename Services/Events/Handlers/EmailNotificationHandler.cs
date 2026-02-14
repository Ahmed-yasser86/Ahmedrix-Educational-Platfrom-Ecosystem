using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Interfaces;
using OnlineCoursesPlatform.Notifications;

namespace OnlineCoursesPlatform.Notifications.Handlers
{
    public class EmailNotificationHandler : INotificationHandler<LiveStreamStartedNotification>
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;

        public EmailNotificationHandler(IEmailService emailService, ApplicationDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        public async Task Handle(LiveStreamStartedNotification notification, CancellationToken cancellationToken)
        {
            var studentEmails = await _context.UserCategories
                .Where(uc => uc.CategoryId == notification.CategoryId)
                .Select(uc => uc.User.Email)
                .ToListAsync(cancellationToken);

            if (!studentEmails.Any()) return;

            var subject = $"🔴 Now Live: {notification.CategoryTitle}";
            var body = $@"
                <div style='font-family: Arial;'>
                    <h2>Live Stream Alert!</h2>
                    <p>Instructor <b>{notification.InstructorName}</b> has just started a live stream titled:</p>
                    <div style='background: #f9f9f9; padding: 15px; border-left: 5px solid #e74c3c;'>
                        <strong>{notification.StreamTitle}</strong>
                    </div>
                    <p>Log in to the platform now to watch the session.</p>
                </div>";

            foreach (var email in studentEmails)
            {
                if (string.IsNullOrEmpty(email)) continue;

                try
                {
                    await _emailService.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email to {email}: {ex.Message}");
                }
            }
        }
    }
}