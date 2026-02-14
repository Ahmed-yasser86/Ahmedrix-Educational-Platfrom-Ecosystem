using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Entities;

namespace OnlineCoursesPlatform.Controllers
{
    public class ContentController : Controller
    {
        
        
        private readonly ApplicationDbContext _context;

        public ContentController( ApplicationDbContext applicationDbContext)
        {
                        _context = applicationDbContext;

        }
        public IActionResult Index(int CategoryItemId)
        {

            var Content = (from  c in _context.Contents
                          where c.CatItemId == CategoryItemId
                          select new Content
                          {
                              Id = c.Id,
                              Title = c.Title,
                              HTMLContent = c.HTMLContent,
                              VideoLink = c.VideoLink,
                              CatItemId = c.CatItemId,
                              CategoryId = c.CategoryItem.CategoryId
                          }).FirstOrDefault();

            return View(Content);
        }
    }
}
