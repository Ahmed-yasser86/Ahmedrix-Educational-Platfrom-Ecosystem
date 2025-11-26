using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace OnlineCoursesPlatform.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // For testing, just print to console
            Console.WriteLine($"Sending email to {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}
