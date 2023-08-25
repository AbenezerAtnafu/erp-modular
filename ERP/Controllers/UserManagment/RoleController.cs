using ERP.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace ERP.Controllers.UserManagment
{
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }


        public IActionResult Index(string searchTerm, int? page)
        {
            var roles = roleManager.Roles.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Apply search filter based on the searchTerm
                roles = roles.Where(r => r.Name.Contains(searchTerm)).ToList();
            }

            const int pageSize = 10;
            var pageNumber = page ?? 1;
            var pagedRoles = roles.ToPagedList(pageNumber, pageSize);

            return View(pagedRoles);
        }
    }
}
