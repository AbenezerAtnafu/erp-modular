using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;

namespace ERP.Controllers
{
    public class IdDownloadController : Controller
    {
        public async Task<IActionResult> Download()
        {
            // Create a new Playwright instance
            using var playwright = await Playwright.CreateAsync();

            // Launch a browser instance
            await using var browser = await playwright.Webkit.LaunchAsync();

            // Create a new browser context
            await using var context = await browser.NewContextAsync();

            // Create a new page
            var page = await context.NewPageAsync();

            // Navigate to the desired URL
            await page.GotoAsync("https://erp.mols.gov.et/Identity/Account/Login?ReturnUrl=%2FEmployees%2FCreateId%2F2164");
            await page.GetByPlaceholder("Email").ClickAsync();
            await page.GetByPlaceholder("Email").FillAsync("abrhambelete.haile@gmail.com");
            await page.GetByPlaceholder("Password").ClickAsync();
            await page.GetByPlaceholder("Password").FillAsync("Error1@1");
            await page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();

            var front = "#front-face";
            await page.WaitForSelectorAsync(front, new () { Timeout = 1000000 });

            var screenshot = await page.ScreenshotAsync();

            return File(screenshot, "image/png");

        }
    }
}
