using Admin.Dashbord.Models.Roles;
using Admin.Dashbord.Models.Users;
using ECommerce.Domain.Entities.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Dashbord.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var usersViewModel = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                usersViewModel.Add(new UserViewModel
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Email = user.Email!,
                    Username = user.UserName!,
                    Roles = roles
                });
            }

            return View(usersViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();

            var model = new UserRoleViewModel
            {
                UserId = id,
                Username = user.UserName!,
                Roles = new List<UpdateRoleViewModel>()
            };

            foreach (var role in roles)
            {
                model.Roles.Add(new UpdateRoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                });
            }

            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var rolesForUser = await _userManager.GetRolesAsync(user);

            // Role was granted -> uncheck for the role (Remove this role)
            // Role was not granted -> check for the role (Add this role)

            foreach (var role in model.Roles) // All roles in the system
            {
                if (rolesForUser.Any(r => r == role.Name) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.Name);

                if (!rolesForUser.Any(r => r == role.Name) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.Name);

            }

            return RedirectToAction(nameof(Index));
        }

    }
}
