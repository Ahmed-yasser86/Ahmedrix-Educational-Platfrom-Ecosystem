using System.ComponentModel.DataAnnotations;

namespace OnlineCoursesPlatform.Entities
{
    public class Newsletter
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public DateTime SubscribedAt { get; set; } = DateTime.Now;
    }
}