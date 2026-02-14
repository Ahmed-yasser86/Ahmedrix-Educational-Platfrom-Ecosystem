using OnlineCoursesPlatform.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnlineCoursesPlatform.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Description { get; set; }

        public string ProfileImagePath { get; set; } = string.Empty;
        public ICollection<Category> Courses { get; set; } = new List<Category>();
    }
}