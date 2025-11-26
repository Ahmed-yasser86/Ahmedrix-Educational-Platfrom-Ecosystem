using OnlineCoursesPlatform.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCoursesPlatform.Entities
{
    public class UserCategory
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CategoryId { get; set; }

        public ApplicationUser User { get; set; }

        //[ForeignKey("CategoryId")]
        //public Category Category { get; set; }
    }
}
