using Admin.Dashbord.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Dashbord.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _roleManager.Roles.ToListAsync();
            return View(result);
        }

        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);

                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("Name", "Role already exists");
            }

            return View(nameof(Index), await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                ModelState.AddModelError("Id", "No role exists with this id.");
                return RedirectToAction(nameof(Index));
            }

            UpdateRoleViewModel updateRoleViewModel = new UpdateRoleViewModel
            {
                Id = id,
                Name = role.Name!
            };

            return View(updateRoleViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);

                if (!roleExists)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);

                    if (role is not null)
                    {
                        role.Name = model.Name;
                        await _roleManager.UpdateAsync(role);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role already exists");
                    return View(model);

                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is not null)
                await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }
    }
}
