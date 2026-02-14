using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Areas.Admin.Models
{
    public class UsersCategoryListModel
    {
       
        public int CategoryId { get; set; }
        public ICollection<UserModel> Users { get; set; }
        public ICollection<UserModel> UserSelected { get; set; }


    }
}
