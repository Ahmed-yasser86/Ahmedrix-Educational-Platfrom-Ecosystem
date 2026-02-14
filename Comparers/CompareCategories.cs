using OnlineCoursesPlatform.Areas.Admin.Models;
using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Comparers
{
    public class CompareCategories : IEqualityComparer<Category>
    {

        public bool Equals(Category x, Category y)
        {
            return x.Id == y.Id;
        }
        public int GetHashCode(Category obj)
        {
            return obj.Id.GetHashCode();
        }


    }
}
