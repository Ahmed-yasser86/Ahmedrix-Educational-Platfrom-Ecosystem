using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Areas.Admin.Data;
using OnlineCoursesPlatform.Areas.Admin.Models;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Areas.Admin.Controllers
{


    [Area("Admin")] 
    public class UsersToCategoryController : Controller
    {

        readonly private ApplicationDbContext _Contxt;
        private readonly IDataFunctions _dataFunctions;
        public UsersToCategoryController(ApplicationDbContext context, IDataFunctions dataFunctions)
        {
            _Contxt = context;
            _dataFunctions = dataFunctions;
        }



        [HttpGet]
        public async Task<IActionResult> GetUsersForCategory(int categoryId)
        {
            UsersCategoryListModel usersCategoryListModel = new UsersCategoryListModel();

            var allUsers = await GetAllUsers();
            var selectedUsersForCategory = await GetSavedSelectedUsersForCategory(categoryId);

            usersCategoryListModel.Users = allUsers;
            usersCategoryListModel.UserSelected = selectedUsersForCategory;

            return PartialView("_UsersListViewPartial", usersCategoryListModel);

        }


        private async Task<List<UserModel>> GetSavedSelectedUsersForCategory(int categoryId)
        {
            var savedSelectedUsersForCategory = await (from usersToCat in _Contxt.UserCategories
                                                       where usersToCat.CategoryId == categoryId
                                                       select new UserModel
                                                       {
                                                           Id = usersToCat.UserId
                                                       }).ToListAsync();
            return savedSelectedUsersForCategory;
        }

        private async Task<List<UserModel>> GetAllUsers()
        {
            var allUsers = await (from user in _Contxt.Users
                                  select new UserModel
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      FirstName = user.FirstName,
                                      LastName = user.LastName
                                  }
                                  ).ToListAsync();
            return allUsers;
        }



      
        public async Task<IActionResult> Index()
        {
            return View(await _Contxt.Categories.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelectedUsers([Bind("CategoryId, UserSelected")] UsersCategoryListModel usersCategoryListModel)
        {


            ModelState.Remove("Users");
            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                var errors = entry.Value.Errors;

                if (errors.Count > 0)
                {
                    Console.WriteLine($"Key: {key}");
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  - Error: {error.ErrorMessage}");
                        // If you need the exception:
                        // Console.WriteLine($"  - Exception: {error.Exception?.Message}");
                    }
                }
            }

            List<UserCategory> usersSelectedForCategoryToAdd = null;

            if (usersCategoryListModel.UserSelected != null)
            {
                usersSelectedForCategoryToAdd = await GetUsersForCategoryToAdd(usersCategoryListModel);
            }

            var usersSelectedForCategoryToDelete = await GetUsersForCategoryToDelete(usersCategoryListModel.CategoryId);

            await _dataFunctions.UpdateUserCategoryEntityAsync(usersSelectedForCategoryToDelete, usersSelectedForCategoryToAdd);

            usersCategoryListModel.Users = await GetAllUsers();

            return PartialView("_UsersListViewPartial", usersCategoryListModel);

        }

        private async Task<List<UserCategory>> GetUsersForCategoryToAdd(UsersCategoryListModel usersCategoryListModel)
        {
            var usersForCategoryToAdd = (from userCat in usersCategoryListModel.UserSelected
                                         select new UserCategory
                                         {
                                             CategoryId = usersCategoryListModel.CategoryId,
                                             UserId = userCat.Id
                                         }).ToList();

            return await Task.FromResult(usersForCategoryToAdd);

        }
        private async Task<List<UserCategory>> GetUsersForCategoryToDelete(int categoryId)
        {
            var usersForCategoryToDelete = await (from userCat in _Contxt.UserCategories
                                                  where userCat.CategoryId == categoryId
                                                  select new UserCategory
                                                  {
                                                      Id = userCat.Id,
                                                      CategoryId = categoryId,
                                                      UserId = userCat.UserId
                                                  }
                                                  ).ToListAsync();
            return usersForCategoryToDelete;

        }



    }
}
