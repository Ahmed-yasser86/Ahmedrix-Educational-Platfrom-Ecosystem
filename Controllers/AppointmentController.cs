using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Interfaces;
using OnlineCoursesPlatform.Models;
using System.Security.Claims;

namespace OnlineCoursesPlatform.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        public AppointmentController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = new AppointmentToUserModel
            {
                UserId = userId,
                AvailableInstructors = await _context.Instructors.ToListAsync(),
                UserAppointments = await _context.Appointments
                    .Include(a => a.Instructor)
                    .Where(a => a.StudentId == userId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync()
            };

            return View("~/Views/Appoinments/Appoinments.cshtml", model);
        }


 


  


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAppointment(AppointmentToUserModel model)
        {
            if (ModelState.IsValid)
            {
                string roomId = new Random().Next(1000000, 9999999).ToString();
                string meetingUrl = $"http://localhost:8448/#{roomId}";
                var appointment = new Appointment
                {
                    StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    InstructorId = model.SelectedInstructorId,
                    AppointmentDate = model.SelectedDate,
                    Notes = model.Notes,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    RoomUrl = meetingUrl
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                var studentEmail = User.FindFirstValue(ClaimTypes.Email);

                string subject = "Class Confirmed - Your Meeting Link";
                string body = $@"
            <h3>Booking Confirmed!</h3>
            <p>Meeting Date: {model.SelectedDate.ToString("f")}</p>
            <p>Link: <a href='{meetingUrl}'>{meetingUrl}</a></p>";

                try
                {
                    await _emailService.SendEmailAsync(studentEmail, subject, body);
                    TempData["Success"] = "Appointment booked and email sent!";
                }
                catch
                {
                    TempData["Warning"] = "Booked, but email failed.";
                }

                return RedirectToAction(nameof(Index));

            }
            model.AvailableInstructors = await _context.Instructors.ToListAsync();
            model.UserAppointments = await _context.Appointments
                .Include(a => a.Instructor)
                .Where(a => a.StudentId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ToListAsync();

            return View("~/Views/Appoinments/Appoinments.cshtml", model);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateAppointment(AppointmentToUserModel model)
        //{
        //    if (model.SelectedInstructorId > 0 && model.SelectedDate > DateTime.Now)
        //    {
        //        var appointment = new Appointment
        //        {
        //            StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier), 
        //            InstructorId = model.SelectedInstructorId,
        //            AppointmentDate = model.SelectedDate,
        //            Notes = model.Notes,
        //            Status = "Pending",
        //            CreatedAt = DateTime.Now
        //        };

        //        _context.Appointments.Add(appointment);
        //        await _context.SaveChangesAsync();

        //        TempData["Success"] = "Appointment booked successfully!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ModelState.AddModelError("", "Please select a valid instructor and future date.");
        //    return RedirectToAction(nameof(Index));
        //}

        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (appointment != null && appointment.StudentId == userId)
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Complete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Completed";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}