using ERP.Areas.Identity.Data;
using ERP.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace ERP.Controllers.UserManagment
{
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        private readonly ICacheService _cacheService;

        public RoleController(RoleManager<IdentityRole> roleManager,ICacheService cacheService)
        {
            this.roleManager = roleManager;
            _cacheService = cacheService;
        }


        public IActionResult Index(string searchTerm, int? page)
        {
            var roles = roleManager.Roles.ToList();
            var pageSize = 10;
            var pageNumber = page ?? 1;
            var expiryTime = DateTimeOffset.Now.AddMinutes(5);
            var cacheData = _cacheService.GetData("Roles");
            if (!string.IsNullOrEmpty(cacheData))
            {
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<User>>(cacheData);
                var pagedData = new StaticPagedList<User>(deserializedData, pageNumber, pageSize, deserializedData.Count());
                return View(pagedData);
            }


            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Apply search filter based on the searchTerm
                roles = roles.Where(r => r.Name.Contains(searchTerm)).ToList();
            }

          
            var pagedRoles = roles.ToPagedList(pageNumber, pageSize);
            _cacheService.SetData("Roles", pagedRoles, expiryTime);

            return View(pagedRoles);
        }
    }
}
