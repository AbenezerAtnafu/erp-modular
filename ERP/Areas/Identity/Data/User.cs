using Microsoft.AspNetCore.Identity;

namespace ERP.Areas.Identity.Data;

public class User : IdentityUser
{
    public bool is_active { get; set; }
    public bool IsFirstLogin { get; set; }
    public int NotificationCount { get; set; }

}

