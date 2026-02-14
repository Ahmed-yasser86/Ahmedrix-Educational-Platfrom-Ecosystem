using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Models; // أو المكان اللي فيه موديل الـ Instructor
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCoursesPlatform.Entities
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual ApplicationUser Student { get; set; }

        [Required]
        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public virtual Instructor Instructor { get; set; }

        [Required]
        [Display(Name = "Appointment Date & Time")]
        public DateTime AppointmentDate { get; set; }

        public string? Notes { get; set; }
        public string RoomUrl { get; set; }
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}