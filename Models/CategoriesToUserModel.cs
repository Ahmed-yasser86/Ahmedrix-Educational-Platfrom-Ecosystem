using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Models
{
    public class CategoriesToUserModel
    {

        public  string UserId { get; set; }

        public ICollection<Category> categories { get; set; }

        public ICollection<Category> CategoriesSelected { get; set; }


    }
}
