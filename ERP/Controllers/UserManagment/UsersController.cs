using ERP.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace ERP.Controllers.AccountManagment
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly employee_context _context;
        public UsersController(UserManager<User> userManager, employee_context context)
        {
            _userManager = userManager;
            _context = context;
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


        public async Task<IActionResult> ActiveUsers(string searchTerm, int pageNumber = 1)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var pageSize = 10;

            var usersQuery = _context.Users.Where(u => u.Id != currentUser.Id && u.is_active);

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

            return View(pagedUsers);
        }

        [Route("Users/DeactivatedUsres")]
        public async Task<IActionResult> InActiveUsers(string searchTerm, int pageNumber = 1)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var pageSize = 10;

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

            var users = await usersQuery
                .OrderBy(u => u.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedUsers = new StaticPagedList<User>(users, pageNumber, pageSize, totalCount);

            return View(pagedUsers);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/Activate/{userId}")]
        public async Task<IActionResult> ActivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.is_active = true;
            _userManager.UpdateAsync(user).Wait();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/Deactivate/{userId}")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            user.is_active = false;
            _userManager.UpdateAsync(user).Wait();

            return Ok();
        }




    }
}
