using OnlineCoursesPlatform.Areas.Admin.Models;

namespace OnlineCoursesPlatform.Comparers
{
    public class Comparer : IEqualityComparer<UserModel>
    {
        public bool Equals(UserModel x, UserModel y)
        {
            return x.Id == y.Id;
        }
        public int GetHashCode(UserModel obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
