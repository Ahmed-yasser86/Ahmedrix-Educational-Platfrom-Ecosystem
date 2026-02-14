using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Models;

namespace OnlineCoursesPlatform.Controllers
{
    public class InstructorsController : Controller
    {

        ApplicationDbContext _context;
        public InstructorsController(ApplicationDbContext context) {
            _context = context;
        }
        // GET: InstructorsController
        public ActionResult Index()
        {
            List<Instructor> instructors;

            instructors = _context.Instructors.ToList();

            return View("InstructorView", instructors);
        }


     

        // GET: InstructorsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InstructorsController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InstructorsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InstructorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InstructorsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InstructorsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
