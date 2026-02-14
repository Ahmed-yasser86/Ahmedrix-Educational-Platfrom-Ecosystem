using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Models;

namespace OnlineCoursesPlatform.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ApplicationDbContext _context;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly UserManager<ApplicationUser> _userManager;
    public HomeController(ILogger<HomeController> logger , ApplicationDbContext Context,SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager )
    {
        _logger = logger;
        _context = Context;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {


        IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModel = null;

        IEnumerable<GroupedCategoryItemByCategoryModel> groupedCategoryItemByCategoryModel = null;


        CategoryDetailsModel categoryDetailsModel = new CategoryDetailsModel();


        if (_signInManager.IsSignedIn(User))
        {
            var user = _userManager.GetUserAsync(User);

            if (user != null)
            {
                categoryItemDetailsModel = await CategoryItemsDetailsForUserAsync(user.Result.Id);

                groupedCategoryItemByCategoryModel = await GetGroupedCategoryItemByCategoryModel(categoryItemDetailsModel);


                categoryDetailsModel.CategoryItems = groupedCategoryItemByCategoryModel;

            }

            return View(categoryDetailsModel);

        }

        var allCategories = await GetAllCategories();

        return View (new CategoryDetailsModel
        {
            Categories = allCategories
        } );

    }

    public IActionResult AboutUs()
    {
        return View("AboutUs");
    }
    private async Task<List<Category> >GetAllCategories()
    {


        var k =  await (from category in _context.Categories join 
                        CatItem in _context.CategoryItems on category.Id equals 
                        CatItem.CategoryId join content in _context.Contents on 
                        CatItem.Id equals content.CatItemId select new Category
        {

            Id = category.Id,
            Title = category.Title,
            Description = category.Description,
            ThumbnailImagePath = category.ThumbnailImagePath
        }).Distinct().ToListAsync();

        return k;

    }
    private async Task<IEnumerable<GroupedCategoryItemByCategoryModel>> GetGroupedCategoryItemByCategoryModel(IEnumerable<CategoryItemDetailsModel> categoryItemDetails)
    {

        return  from item in categoryItemDetails
                group item by item.CategoryId into g
                select new GroupedCategoryItemByCategoryModel
                {
                    Id = g.Key,
                    Title = g.Select(i => i.CategoryTitle).FirstOrDefault() ?? string.Empty,
                    Items = g
                };


    }



    private async Task<IEnumerable<CategoryItemDetailsModel>> CategoryItemsDetailsForUserAsync(string userId)
    {
        return await (from catItem in _context.CategoryItems  // OR CategoryItem if singular
                      join category in _context.Categories on catItem.CategoryId equals category.Id
                      join content in _context.Contents on catItem.Id equals content.CatItemId  // Use FK property
                      join userCat in _context.UserCategories on category.Id equals userCat.CategoryId
                      join mediaType in _context.MediaTypes on catItem.MediaTypeId equals mediaType.Id
                      where userCat.UserId == userId
                      select new CategoryItemDetailsModel
                      {
                          CategoryId = category.Id,
                          CategoryTitle = category.Title,
                          CategoryItemId = catItem.Id,
                          CategoryItemTitle = catItem.Title,
                          CategoryItemDescription = catItem.Description,
                          MediaImagePath = mediaType.ImagePath
                      }).ToListAsync();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
