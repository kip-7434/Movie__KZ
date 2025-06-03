using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie_Recommendation.Extensions;
using Movie_Recommendation.Models;
using Movie_Recommendation.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            var exists = await _userManager.FindByEmailAsync(model.Email);
            if (exists != null)
            {
                TempData.SetData(AlertLevel.Warning, "Account exists", "An account with the given email exists. Proceed to login");
                return View(model);
            }
          
            if (ModelState.IsValid)
            {
                
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Names = model.Names,

                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var msg = $"Your account has been created";
                    TempData.SetData(AlertLevel.Success, "Registered successfully", msg);
                    return View();
                }

                AddErrors(result);
            }

            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && !user.EmailConfirmed && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                TempData.SetData(AlertLevel.Error, "User error", "Your email is not confirmed!");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                TempData.SetData(AlertLevel.Success, "Welcome", "Login successful!");
                return RedirectToAction("Dashboard");
            }
            TempData.SetData(AlertLevel.Error, "User error", "Invalid Username or Password!");
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.SignOutAsync();
            TempData.SetData(AlertLevel.Success, "Logout", "You have logged out");
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
