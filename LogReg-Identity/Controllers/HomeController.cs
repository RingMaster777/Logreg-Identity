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

        public HomeController(ILogger<HomeController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {

                var users = _userManager.Users.Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    Role = _userManager.GetRolesAsync(u).Result.FirstOrDefault()
                }).ToList();
				
				
                ViewBag.Users = users;

            }
            _logger.LogInformation("Home page accessed at {Time}", DateTime.UtcNow);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(string? id)
        {
            // Fetch the user details using the user ID
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                // Handle case where the user is not found
                return NotFound();
            }

            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            // Optionally, pass the user details to the view
            ViewBag.Roles = roles;
            ViewBag.userId = id;
            ViewBag.Name = user.FirstName + " " + user.LastName; ;
            ViewBag.UserEmail = user.Email;
            ViewBag.UserPhoneNumber = user.PhoneNumber;
            ViewBag.UserRoles = userRoles;

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

        public async Task<IActionResult> AssignRole (string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role)) {
                return RedirectToAction("UserDetails", new { id = userId });
            }


            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) {
                return RedirectToAction("UserDetails", new { id = userId });
            }

            // Get current roles
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Remove the user from all roles
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add the new role
            await _userManager.AddToRoleAsync(user, role);

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
