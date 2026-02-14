using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Interfaces;
using OnlineCoursesPlatform.Models;
using OnlineCoursesPlatform.Services;
using System.ComponentModel;
using System.Threading.Tasks;

namespace OnlineCoursesPlatform.Controllers
{
    public class CategoriesToUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IDataFunction _dataFunction;
        private readonly ICategoryService _categoryService; // Add this field


        public CategoriesToUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDataFunction dataFunction, ICategoryService categoryService)
        {

            _context = context;
            _userManager = userManager;
            _dataFunction = dataFunction;
            _categoryService = categoryService; 

        }

        public async Task<IActionResult> Index()
        {

            CategoriesToUserModel CatToUser = new CategoriesToUserModel();


            IEnumerable<Category> CatList = await _categoryService.GetCategoriesThatHasContent();


            var userId = _userManager.GetUserAsync(User).Result?.Id;

            CatToUser.categories = CatList.ToList();

            CatToUser.UserId = userId;

            CatToUser.CategoriesSelected = await _categoryService.GetCategoriesCurrentlySavedForUser(userId);
            return View(CatToUser);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string[] CategoriesSelected)
        {
            var userId = _userManager.GetUserAsync(User).Result?.Id;
            var GetCategoriesForDeletion = await _categoryService.GetCategoriesForUserToDelete(userId);

            var GetCategoriesForAdding = _categoryService.GetCategoriesToAdd(CategoriesSelected, userId);


            await _dataFunction.UpdateUserCategoryEntityAsyc(GetCategoriesForDeletion, GetCategoriesForAdding);
            return RedirectToAction("Index", "Home");

        }


        // Add these methods to your CategoriesToUserController


        //private async Task<List<Category>> GetCategoriesThatHasContent()
        //{

        //    var CatThatHasContent = await (from Cat in _context.Categories
        //                                   join
        //                    CatItem in _context.CategoryItems on Cat.Id
        //                    equals CatItem.CategoryId
        //                                   join Cont in _context.Contents
        //                    on CatItem.Id equals Cont.CatItemId
        //                                   select new Category
        //                                   {
        //                                       Id = Cat.Id,
        //                                       Title = Cat.Title,
        //                                       Description = Cat.Description
        //                                   }).Distinct().ToListAsync();


        //    return CatThatHasContent;
        //}

        //private async Task<List<Category>> GetCategoriesCurrentlySavedForUser(string UserId)
        //{
        //    //var CategoriesSavedForUser = from Cat in _context.Categories
        //    //                             join UserCat in _context.UserCategories
        //    //                             on Cat.Id equals UserCat.CategoryId
        //    //                             where UserCat.UserId == _userManager.GetUserId(User)
        //    //                             join CatItem in _context.CategoryItems
        //    //                             on Cat.Id equals CatItem.CategoryId
        //    //                             join Cont in _context.Contents
        //    //                             on CatItem.Id equals Cont.CatItemId
        //    //                             select new Category
        //    //                             {
        //    //                                 Id = Cat.Id,
        //    //                                 Title = Cat.Title,
        //    //                                 Description = Cat.Description
        //    //                             };


        //    var CategoriesSavedForUser = await (from userCat in _context.UserCategories
        //                                        where userCat.UserId == UserId
        //                                        select new Category
        //                                        {
        //                                            Id = userCat.CategoryId
        //                                        }).Distinct().ToListAsync();


        //    return CategoriesSavedForUser;
        //}

        //private async Task<List<UserCategory>> GetCategoriesForUserToDelete(string UserId)
        //{
        //    // FIX: Return ALL UserCategory records for this user (not just CategoryId)
        //    var CategoriesForUserToDelete = await (from userCat in _context.UserCategories
        //                                           where userCat.UserId == UserId
        //                                           select new UserCategory
        //                                           {
        //                                               Id = userCat.Id, // FIX: Use UserCategory.Id, not CategoryId
        //                                               CategoryId = userCat.CategoryId,
        //                                               UserId = userCat.UserId
        //                                           }).ToListAsync();

        //    return CategoriesForUserToDelete;
        //}

        //    private List<UserCategory> GetCategoriesToAdd(string[] categoriesSelected, string userId)
        //{
        //    var categoriesToAdd = (from categoryId in categoriesSelected
        //                           select new UserCategory
        //                           {
        //                               UserId = userId,
        //                               CategoryId = int.Parse(categoryId)
        //                           }).ToList();

        //    return categoriesToAdd;

        //}


    }
}

