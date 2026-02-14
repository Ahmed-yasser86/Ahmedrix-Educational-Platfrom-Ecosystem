using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Data; 
using OnlineCoursesPlatform.Areas.Admin.Models; 
using System.Net.Http;
using Newtonsoft.Json;

namespace OnlineCoursesPlatform.Controllers
{
    public class LiveStudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public LiveStudentController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;

            var user = await _context.Users
                .Include(u => u.UserCategories) 
                .FirstOrDefaultAsync(u => u.Email == userEmail);


            if (user == null) return NotFound();

            var activeSessions = await _context.LiveSessions
                .Where(s => s.IsActive && !s.IsDeleted)
                .ToListAsync();

            var studentLessons = new List<StudentLiveViewModel>();



            if (user.UserCategories != null)
            {
                foreach (var category in user.UserCategories)
                {
                    var session = activeSessions.FirstOrDefault(s => s.CategoryId == category.CategoryId);

                    var viewModel = new StudentLiveViewModel
                    {
                        CategoryId = category.Id,
                        CategoryTitle = _context.Categories.Find(category.CategoryId)?.Title ?? "Unknown Course",
                        StreamKey = session?.StreamKey,
                        IsLiveNow = false
                    };

                    if (session != null)
                    {
                        viewModel.IsLiveNow = await CheckProxyStatus(session.StreamKey);
                    }

                    studentLessons.Add(viewModel);
                }
            }

            return View(studentLessons);
        }
        private async Task<bool> CheckProxyStatus(string streamKey)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiUrl = "http://video-lb:20000";

                var loginData = new { username = "admin", password = "0192023a7bbd73250516f069df18b500" };
                var loginRes = await client.PostAsJsonAsync($"{apiUrl}/api/v1/login", loginData);

                if (!loginRes.IsSuccessStatusCode) return false;

                var loginResult = await loginRes.Content.ReadFromJsonAsync<NmsResponse<LoginData>>();
                var token = loginResult?.data?.token;

                if (string.IsNullOrEmpty(token)) return false;

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var streamsRes = await client.GetAsync($"{apiUrl}/api/v1/streams");

                if (streamsRes.IsSuccessStatusCode)
                {
                    var content = await streamsRes.Content.ReadAsStringAsync();
                    return content.Contains(streamKey);
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public class NmsResponse<T> { public bool success { get; set; } public T data { get; set; } }
        public class LoginData { public string token { get; set; } }
        public IActionResult Watch(string key)
        {
            if (string.IsNullOrEmpty(key)) return RedirectToAction("Index");
            ViewBag.StreamKey = key;
            return View();
        }
    }
}