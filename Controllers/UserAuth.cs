using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.Data;
using OnlineCoursesPlatform.Models;

namespace OnlineCoursesPlatform.Controllers
{
    public class UserAuth : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public UserAuth(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            model.LoginInValid = "True";
            ModelState.Remove("LoginInValid");
            ModelState.Remove("LoginFailedMessage");

            if (ModelState.IsValid)
            {

                foreach (var state in ModelState.Values)
                {
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage); // or log it
                    }
                }



                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    model.LoginFailedMessage = "Invalid login attempt.";
                    return PartialView("_UserLoginPartial", model);
                }
                else
                {
                    model.LoginInValid = "False";
                    // Redirect to Home page after successful login
                    return RedirectToAction("Index", "Home");
                }
            }

            return PartialView("_UserLoginPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }
    }
}
