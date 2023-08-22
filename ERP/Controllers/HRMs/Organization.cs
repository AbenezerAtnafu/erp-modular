using Microsoft.AspNetCore.Mvc;

namespace ERP.Controllers
{
    public class Organization : Controller
    {
        internal int id;

        public IActionResult Index()
        {
            return View();
        }
    }
}
