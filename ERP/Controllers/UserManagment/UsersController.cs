using ERP.Areas.Identity.Data;
using ERP.Interface;
using ERP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace ERP.Controllers.AccountManagment
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly employee_context _context;
        private readonly ICacheService _cacheService;
        public UsersController(UserManager<User> userManager, employee_context context, ICacheService cacheService)
        {
            _userManager = userManager;
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var pageSize = 10;
            var pageNumber = page ?? 1;

        var allUsersExceptCurrentUserQuery = _userManager.Users.Where(u => u.Id != currentUser.Id);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                allUsersExceptCurrentUserQuery = allUsersExceptCurrentUserQuery.Where(u =>
                    u.Email.ToLower().Contains(searchTerm) || // Search by email (case-insensitive)
                    u.UserName.ToLower().Contains(searchTerm) // Search by username (case-insensitive)
                );
            }

            var totalCount = await allUsersExceptCurrentUserQuery.CountAsync();

            var allUsersExceptCurrentUser = await allUsersExceptCurrentUserQuery
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedUsers = new StaticPagedList<User>(allUsersExceptCurrentUser, pageNumber, pageSize, totalCount);

            return View(pagedUsers);
        }


        public async Task<IActionResult> ActiveUsers(string searchTerm, int? page)
        {
            
            var currentUser = await _userManager.GetUserAsync(User);
            var pageSize = 10;
            var pageNumber = page ?? 1;
            _cacheService.RemoveData("Users");

            var cacheData = _cacheService.GetData("Users");
            if (!string.IsNullOrEmpty(cacheData))
            {
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<User>>(cacheData);
                var pagedData = new StaticPagedList<User>(deserializedData, pageNumber, pageSize, deserializedData.Count());
                return View(pagedData);
            }

            var usersQuery = _context.Users.Where(u => u.Id != currentUser.Id && u.is_active);
            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                    searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase
                    usersQuery = usersQuery.Where(u =>
                    u.Email.ToLower().Contains(searchTerm) || // Search by email (case-insensitive)
                    u.UserName.ToLower().Contains(searchTerm) // Search by username (case-insensitive)
                );
            }

            var totalCount = await usersQuery.CountAsync();

            var users = await usersQuery
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedUsers = new StaticPagedList<User>(users, pageNumber, pageSize, totalCount);
            _cacheService.SetData("Users", pagedUsers, expiryTime);

            return View(pagedUsers);
        }




        [Route("Users/DeactivatedUsres")]
        public async Task<IActionResult> InActiveUsers(string searchTerm, int? page)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var pageSize = 10;


      
            var pageNumber = page ?? 1;
            _cacheService.RemoveData("Users");


            var cacheData = _cacheService.GetData("Users");
            if (!string.IsNullOrEmpty(cacheData))
            {
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<User>>(cacheData);
                var pagedData = new StaticPagedList<User>(deserializedData, pageNumber, pageSize, deserializedData.Count());
                return View(pagedData);
            }

            var usersQuery = _context.Users.Where(u => u.Id != currentUser.Id && !u.is_active);


            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                usersQuery = usersQuery.Where(u =>
                    u.Email.ToLower().Contains(searchTerm) || // Search by email (case-insensitive)
                    u.UserName.ToLower().Contains(searchTerm) // Search by username (case-insensitive)
                );
            }

            var totalCount = await usersQuery.CountAsync();
            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            var users = await usersQuery
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedUsers = new StaticPagedList<User>(users, pageNumber, pageSize, totalCount);
            _cacheService.SetData("Users", pagedUsers, expiryTime);

            return View(pagedUsers);
        }


        public async Task<IActionResult> DeactivateUser()
        {
            var userId = Convert.ToString(HttpContext.Request.Form["inactiveid"]);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Warning"] = "User not found.";
               return RedirectToAction(nameof(ActiveUsers));
            }

            else
            {
                user.is_active = false;
                _userManager.UpdateAsync(user).Wait();

                TempData["Success"] = "User is InActive.";
                _cacheService.RemoveData("Users");
                return RedirectToAction(nameof(ActiveUsers));
            }
        }
        
        public async Task<IActionResult> ActivateUser()
        {
            var userId = Convert.ToString(HttpContext.Request.Form["activeid"]);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Warning"] = "User not found. You can find it in inactive users.";
                return RedirectToAction(nameof(InActiveUsers));
            }
            else
            {
                user.is_active = true;
                _userManager.UpdateAsync(user).Wait();

                TempData["Success"] = "User is Active. You can find it in active users.";
                _cacheService.RemoveData("Users");
                return RedirectToAction(nameof(InActiveUsers));
            }
        }
    }
}
