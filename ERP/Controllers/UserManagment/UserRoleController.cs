using ERP.Areas.Identity.Data;
using ERP.Interface;
using ERP.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using X.PagedList;
using Newtonsoft.Json;

namespace ERP.Controllers.UserManagment
{
    public class UserRoleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly employee_context _context;
        private readonly ICacheService _cacheService;
        public UserRoleController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, employee_context context, ICacheService cacheService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;
            _cacheService.RemoveData("UserRoles");

            var cacheData = _cacheService.GetData("UserRoles");
            if (!string.IsNullOrEmpty(cacheData))
            {
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<UserRolesViewModel>>(cacheData);
                var pagedData = new StaticPagedList<UserRolesViewModel>(deserializedData, pageNumber, pageSize, deserializedData.Count());
                return View(pagedData);
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var users = _userManager.Users.Where(u => u.Id != currentUser.Id && u.is_active).ToList();
            var userRolesViewModel = new List<UserRolesViewModel>();
            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                users = users.Where(u =>
                    u.Email.ToLower().Contains(searchTerm) || // Search by email (case-insensitive)
                    u.UserName.ToLower().Contains(searchTerm) // Search by username (case-insensitive)
                ).ToList();
            }

            foreach (var user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                List<string> userRoles = await GetUserRoles(user);

                if (userRoles.Count > 0)
                {
                    var role = userRoles[0];
                    thisViewModel.RoleId = role;
                }
                else
                {
                    thisViewModel.RoleId = "No Assigned Role";
                }

                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;

                userRolesViewModel.Add(thisViewModel);
            }

          


            var pagedUserRoles = userRolesViewModel.ToPagedList(pageNumber, pageSize);
           

            _cacheService.SetData("UserRoles", pagedUserRoles, expiryTime);

            return View(pagedUserRoles);
        }
        private async Task<List<string>> GetUserRoles(User user)
            {

                List<string> roles = new List<string>();

                using (var context = new UserDbContext(new DbContextOptions<UserDbContext>())) // Pass the DbContextOptions to the constructor
                {
                    roles = new List<string>(await _userManager.GetRolesAsync(user));
                }

                return roles;
            }


        public async Task<IActionResult> Manage(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            var userrole = _context.Users.FirstOrDefault(a => a.Id == userId);
            if (user == null)
            {
                TempData["Warning"] = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewBag.UserName = user.UserName;
            var model = new List<ManageUserRolesViewModel>();
            foreach (var role in _roleManager.Roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Add(new ManageUserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Selected = true
                    });
                }
                else
                {
                    model.Add(new ManageUserRolesViewModel
                    {
                        RoleId = role.Id,
                        RoleName = role.Name,
                        Selected = false
                    });
                }
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
                TempData["Success"] = "Cannot remove user's existing roles.";
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));
            var userAsp = _context.UserRoles.FirstOrDefault(a => a.UserId == user.Id);

            if (!result.Succeeded)
            {
                TempData["Success"] = "Cannot add selected roles to user.";
                return View(model);
            }

            TempData["Success"] = "User is assigned role.";
            return RedirectToAction("Index");
        }
    }
}