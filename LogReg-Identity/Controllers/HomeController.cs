using LogReg_Identity.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;

namespace LogReg_Identity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LogReg_Identity.Services.IUserService _userService;

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, LogReg_Identity.Services.IUserService userService)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {

                var usersWithRoles = await _userService.GetUsersWithRolesAsync();
                ViewBag.Users = usersWithRoles;

            }
            _logger.LogInformation("Home page accessed at {Time}", DateTime.UtcNow);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string? id)
        {
            // Fetch the user details using the user ID
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                // Handle case where the user is not found
                return NotFound();
            }

            var roles = await _userService.GetAllRoleNamesAsync();
            var userRoles = await _userService.GetUserRolesAsync(user);

            // Optionally, pass the user details to the view
            ViewBag.Roles = roles;
            ViewBag.userId = id;
            ViewBag.Name = user.FirstName + " " + user.LastName; ;
            ViewBag.UserEmail = user.Email;
            ViewBag.UserPhoneNumber = user.PhoneNumber;
            ViewBag.UserRoles = userRoles;
            ViewBag.IsAdmin = userRoles.Contains("Admin");

            // Return a view with the user details (replace "UserDetails" with your actual view)
            return View(user);
        }




        [HttpGet]
        public IActionResult Edit(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            ViewBag.Id = id;
            return View();
        }




        [HttpPost]

        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                return RedirectToAction("UserDetails", new { id = userId });
            }


            var ok = await _userService.AssignRoleAsync(userId, role);
            return RedirectToAction("UserDetails", new { id = userId });
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
}
