using ERP.Areas.Identity.Data;
using ERP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ERP.Controllers.UserManagment
{
    public class UserRoleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly employee_context _context;
        public UserRoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, employee_context context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var users = await _userManager.Users.Where(u => u.Id != currentUser.Id).ToListAsync();
            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (User user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                List<string> userRoles = await GetUserRoles(user);

                if (userRoles.Count > 0)
                {
                    string role = userRoles[0];
                    thisViewModel.RoleId = role;
                }
                else
                {
                    thisViewModel.RoleId = "No Role";
                }

                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;

                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(User user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }


        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;


            var user = await _userManager.FindByIdAsync(userId);
          
            var userrole = _context.Users.FirstOrDefault(a => a.Id == userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,



                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    /* if (role.Name == "Director")
                     {
                         userRolesViewModel.DepartmentId = userEmp.DepartmentId;

                     }*/

                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }


                model.Add(userRolesViewModel);

            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            var userrole = _context.Users.FirstOrDefault(a => a.Id == userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }


            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            var userAsp = _context.UserRoles.FirstOrDefault(a => a.UserId == user.Id);

  



            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }


            //_context.Users.Update(userrole);
            //await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}