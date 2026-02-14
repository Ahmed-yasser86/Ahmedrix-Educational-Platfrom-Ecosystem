using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Interfaces;
namespace OnlineCoursesPlatform.Services
{
    public class CategoryService : ICategoryService
    {

        readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Category>> GetCategoriesThatHasContent()
        {
            var CatThatHasContent = await (from Cat in _context.Categories
                                           join
                            CatItem in _context.CategoryItems on Cat.Id
                            equals CatItem.CategoryId
                                           join Cont in _context.Contents
                            on CatItem.Id equals Cont.CatItemId
                                           select new Category
                                           {
                                               Id = Cat.Id,
                                               Title = Cat.Title,
                                               Description = Cat.Description
                                           }).Distinct().ToListAsync();


            return CatThatHasContent;
        }

        public async Task<List<Category>> GetCategoriesCurrentlySavedForUser(string UserId)
        {
            //var CategoriesSavedForUser = from Cat in _context.Categories
            //                             join UserCat in _context.UserCategories
            //                             on Cat.Id equals UserCat.CategoryId
            //                             where UserCat.UserId == _userManager.GetUserId(User)
            //                             join CatItem in _context.CategoryItems
            //                             on Cat.Id equals CatItem.CategoryId
            //                             join Cont in _context.Contents
            //                             on CatItem.Id equals Cont.CatItemId
            //                             select new Category
            //                             {
            //                                 Id = Cat.Id,
            //                                 Title = Cat.Title,
            //                                 Description = Cat.Description
            //                             };


            var CategoriesSavedForUser = await (from userCat in _context.UserCategories
                                                where userCat.UserId == UserId
                                                select new Category
                                                {
                                                    Id = userCat.CategoryId
                                                }).Distinct().ToListAsync();


            return CategoriesSavedForUser;
        }

       public  async Task<List<UserCategory>> GetCategoriesForUserToDelete(string UserId)
        {
            // FIX: Return ALL UserCategory records for this user (not just CategoryId)
            var CategoriesForUserToDelete = await (from userCat in _context.UserCategories
                                                   where userCat.UserId == UserId
                                                   select new UserCategory
                                                   {
                                                       Id = userCat.Id, // FIX: Use UserCategory.Id, not CategoryId
                                                       CategoryId = userCat.CategoryId,
                                                       UserId = userCat.UserId
                                                   }).ToListAsync();

            return CategoriesForUserToDelete;
        }

      public   List<UserCategory> GetCategoriesToAdd(string[] categoriesSelected, string userId)
        {
            var categoriesToAdd = (from categoryId in categoriesSelected
                                   select new UserCategory
                                   {
                                       UserId = userId,
                                       CategoryId = int.Parse(categoryId)
                                   }).ToList();

            return categoriesToAdd;

        }




    }
}
