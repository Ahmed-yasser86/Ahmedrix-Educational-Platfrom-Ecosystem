using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Interfaces;
using OnlineCoursesPlatform.Models;
[ApiController]
[Route("api/[controller]")]
public class EmailsController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmail(SendEmailRequest request)
    {
        try
        {
            await _emailService.SendEmailAsync(request.To, request.Subject, request.Body);
            return Ok("Email sent successfully!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Could not send email.");
        }
    }
}
