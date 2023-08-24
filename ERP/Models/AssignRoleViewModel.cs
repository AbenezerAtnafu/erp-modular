using ERP.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace ERP.Models
{
    public class AssignRoleViewModel
    {
        public User User { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public string RoleId { get; set; }
    }
}
