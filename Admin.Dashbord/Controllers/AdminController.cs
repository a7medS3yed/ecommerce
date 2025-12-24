using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Shared.Dtos.Identitys;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace Admin.Dashbord.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);

            if (user is null)
            {
                ModelState.AddModelError("", "Invalid login attempt");
                return View(userLogin);
            }

            var result = await _signInManager.PasswordSignInAsync(user, userLogin.Password, false, false);

            if (!result.Succeeded || (!await _userManager.IsInRoleAsync(user, "Admin") && (!await _userManager.IsInRoleAsync(user, "Super Admin"))))
            {
                ModelState.AddModelError("", "You are not authorized");
                return View(userLogin);
            }

            return RedirectToAction(nameof(Index), "Home");

        }

        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
           return RedirectToAction("Login");
        }
    }
}
