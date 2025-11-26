using OnlineCoursesPlatform.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCoursesPlatform.Entities
{
    public class MediaType : IPrimaryProperty
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Image Path")] 
        public string? ImagePath { get; set; }

        [Required]

        [ForeignKey("MediaTypeId")]
        public virtual ICollection<CategoryItem> CategoryItems { get; set; }
    }
}
