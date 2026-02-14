using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Data
{
    public interface IDataFunction
    {

        public Task UpdateUserCategoryEntityAsyc(List<UserCategory> userCatToDelete, List<UserCategory> userCatToAdd);
     


    }
}
