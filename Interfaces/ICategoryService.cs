using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesThatHasContent();
        Task<List<Category>> GetCategoriesCurrentlySavedForUser(string userId);

         Task<List<UserCategory>> GetCategoriesForUserToDelete(string userId);

     
            List<UserCategory> GetCategoriesToAdd(string[] categoriesSelected, string userId);
    }
}
