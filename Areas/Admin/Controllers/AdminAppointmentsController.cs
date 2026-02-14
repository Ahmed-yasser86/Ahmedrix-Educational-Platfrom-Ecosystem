using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Interfaces;
using System.Security.Claims;

namespace OnlineCoursesPlatform.Areas.Admin.Controllers
{
    [Area("Admin")] 
 //  [Route("Admin/[controller]/[action]")]
    [Route("Admin/[controller]/[action]/{id?}")] 
    public class AdminAppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AdminAppointmentsController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> AdminIndex()
        {
            var allAppointments = await _context.Appointments
                .Include(a => a.Instructor)
                .Include(a => a.Student)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

           
            return View("AdminAppointments", allAppointments);
        }

        public async Task<IActionResult> AdminEdit(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Instructor)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View("AdminEditAppointment", appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminEdit(int id, Appointment model)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = model.Status;
            appointment.Notes = model.Notes;
            appointment.RoomUrl = model.RoomUrl;
            appointment.AppointmentDate = model.AppointmentDate;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AdminIndex));
        }

        public async Task<IActionResult> AdminDelete(int id)
        {
            var appointment = await _context.Appointments
                .Include(a => a.Instructor)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View("AdminDeleteAppointment", appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminDeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(AdminIndex));
        }
    }
}