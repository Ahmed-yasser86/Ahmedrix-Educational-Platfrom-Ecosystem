using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Models
{
    public class CategoryDetailsModel
    {
        public IEnumerable<GroupedCategoryItemByCategoryModel> CategoryItems { get; set; }

        public IEnumerable<Category> Categories { get; set; }

    }
}
