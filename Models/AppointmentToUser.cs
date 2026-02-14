using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Models;

namespace OnlineCoursesPlatform.Models
{
    public class AppointmentToUserModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }

        public ICollection<Instructor>? AvailableInstructors { get; set; }

        public ICollection<Appointment>? UserAppointments { get; set; }

        public int SelectedInstructorId { get; set; }
        public DateTime SelectedDate { get; set; }
        public string? Notes { get; set; }
    }
}