using Microsoft.AspNetCore.Mvc;

namespace MOLS.Bootstrapper.Controllers;

public class HomeController : Controller
{
    IHostEnvironment env;
    public HomeController(IHostEnvironment env)
    {
        this.env = env;
    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewData["Message"] = "Your application description page.";
        
        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";

        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
