using ERP.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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


        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }
    }
}
