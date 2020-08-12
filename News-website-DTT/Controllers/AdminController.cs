using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using News_website_DTT.Models;
using News_website_DTT.ViewModels;

namespace News_website_DTT.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found.";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in await userManager.Users.ToListAsync())
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found.";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
                return View(model);
            }
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var result = await roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("ListRoles");
        }

        [HttpGet]
        public IActionResult AssignUserRole(string roleId)
        {
            var model = new UserRoleViewModel
            {
                RoleId = roleId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignUserRole(UserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = await roleManager.FindByIdAsync(model.RoleId);
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with name = {model.UserName} cannot be found.";
                return View("NotFound");
            }
            else if (await userManager.IsInRoleAsync(user, role.Name))
            {
                ModelState.AddModelError("", "User is already assigned to this role.");
                return View(model);
            }
            else
            {
                var result = await userManager.AddToRoleAsync(user, role.Name);

                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole", new { id = role.Id });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        public async Task<IActionResult> RemoveUserRole(string roleId, string userName)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            var user = await userManager.FindByNameAsync(userName);

            var result = await userManager.RemoveFromRoleAsync(user, role.Name);
            
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            return RedirectToAction("EditRole", new { id = role.Id });
        }
    }
}
