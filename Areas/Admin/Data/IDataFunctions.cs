using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Areas.Admin.Data
{
    public interface IDataFunctions
    {
        Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, List<UserCategory> userCategoryItemsToAdd);
    }
}
