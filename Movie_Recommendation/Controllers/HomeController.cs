using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie_Recommendation.Extensions;
using Movie_Recommendation.Models;
using Movie_Recommendation.Services;
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
        private readonly MovieService _movieService;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger, MovieService movieService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _movieService = movieService;
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

        [HttpGet]
        public async Task<IActionResult> AllMovies()
        {
            var userData = await _movieService.GetPopularMoviesAsync();
            ViewData["movieCount"] = userData.Count();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetMovies()
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"].FirstOrDefault();
                var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault(); // asc or desc
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                // Get data from service
                var movies = await _movieService.GetPopularMoviesAsync(); // returns List<Movie>

                // Filter
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    movies = movies.Where(m =>
                        (!string.IsNullOrEmpty(m.Title) && m.Title.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(m.Overview) && m.Overview.Contains(searchValue, StringComparison.OrdinalIgnoreCase)) ||
                        m.ReleaseDate.Contains(searchValue)
                    ).ToList();
                }

                // Sort
                if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
                {
                    movies = sortColumn switch
                    {
                        "title" => sortDirection == "asc" ? movies.OrderBy(m => m.Title).ToList() : movies.OrderByDescending(m => m.Title).ToList(),
                        "releaseDate" => sortDirection == "asc" ? movies.OrderBy(m => m.ReleaseDate).ToList() : movies.OrderByDescending(m => m.ReleaseDate).ToList(),
                        "voteAverage" => sortDirection == "asc" ? movies.OrderBy(m => m.VoteAverage).ToList() : movies.OrderByDescending(m => m.VoteAverage).ToList(),
                        "popularity" => sortDirection == "asc" ? movies.OrderBy(m => m.Popularity).ToList() : movies.OrderByDescending(m => m.Popularity).ToList(),
                        _ => movies
                    };
                }

                var recordsTotal = movies.Count;

                var pagedData = movies.Skip(skip).Take(pageSize).Select(m => new {
                    title = m.Title,
                    releaseDate = m.ReleaseDate,
                    vote_average = m.VoteAverage,
                    popularity = m.Popularity,
                    overview = m.Overview,
                    poster_path = m.PosterPath
                });

                return Json(new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = pagedData
                });
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
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
