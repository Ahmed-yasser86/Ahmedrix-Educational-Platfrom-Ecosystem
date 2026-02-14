using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineCoursesPlatform.Areas.Admin.Models
{
    public class StartStreamViewModel
    {
        public string Title { get; set; }

        public int SelectedInstructorId { get; set; }
        public IEnumerable<SelectListItem>? AvailableInstructors { get; set; }

        public int SelectedCategoryId { get; set; }
        public IEnumerable<SelectListItem>? AvailableCategories { get; set; }

        public string? GeneratedStreamKey { get; set; }
    }
}