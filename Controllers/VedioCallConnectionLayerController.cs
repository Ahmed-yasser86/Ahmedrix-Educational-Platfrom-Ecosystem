using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Interfaces;
using OnlineCoursesPlatform.Models;
using System.Security.Claims;

    namespace OnlineCoursesPlatform.Controllers
    {
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class VedioCallConnectionLayerController : Controller
        {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context; 
        public VedioCallConnectionLayerController(UserManager<ApplicationUser> userManager, IEmailService emailService,ApplicationDbContext context)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
        }

        //[HttpGet("StartVedioCall")]
        //    public IActionResult StartVedioCall()
        //    {
        //    var userId = _userManager.GetUserId(User);
        //    return Redirect($"http://localhost:3000?userId={userId}");
        //}




        [HttpPost("save-room")]

        public IActionResult SaveRoomUrl([FromBody] RoomSaveRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RoomUrl))
                return BadRequest(new { error = "Data is missing" });

            string url = request.RoomUrl;
                      if (!string.IsNullOrEmpty(url))
            {
                var appointment = _context.Appointments.FirstOrDefault(a => a.RoomUrl == url);
                var now = DateTime.Now;
                var appointmentDate = appointment.AppointmentDate;

                var timeDifference = now - appointmentDate;

                if (Math.Abs(timeDifference.TotalMinutes) <= 30 && appointment.Status == "Pending")
                {
                    appointment.Status = "attended";
                    _context.SaveChanges();
                }
           

            }
            return Ok(new { message = "Saved successfully" });
        }


        [HttpGet("Details/{id}")]
            public ActionResult Details(int id)
            {
                return View();
            }

            [HttpGet("Delete/{id}")]
            public ActionResult Delete(int id)
            {
                return View();
            }
        }
    }