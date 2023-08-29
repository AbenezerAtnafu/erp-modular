using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using ERP.Models.HRMS.Employee_managments;

namespace ERP.Controllers.HRMs
{
    public class LanguagesController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;

        public LanguagesController(employee_context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Languages
        public async Task<IActionResult> Index()
        {
              return _context.languages != null ? 
                          View(await _context.languages.ToListAsync()) :
                          Problem("Entity set 'employee_context.languages'  is null.");
        }

        // GET: Languages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.languages == null)
            {
                return NotFound();
            }

            var language = await _context.languages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // GET: Languages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,name,ability_to_listen,ability_to_Speak,ability_to_Read,ability_to_write,employee_id,created_date,updated_date")] Language language)
        {
            if (ModelState.IsValid)
            {
                var users = _userManager.GetUserId(HttpContext.User);
                var employee = _context.Employees.FirstOrDefault(a => a.user_id == users);

                if (employee != null)
                {
                    language.employee_id = employee.id;
                    language.created_date = DateTime.Now;
                    language.updated_date = DateTime.Now;
                    _context.Add(language);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "New Language is added.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Warning"] = "You should First fill in Your detail.";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return View(language);
            }
                
        }

        // GET: Languages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.languages == null)
            {
                return NotFound();
            }

            var language = await _context.languages.FindAsync(id);
            if (language == null)
            {
                TempData["Warning"] = "Language Not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,name,ability_to_listen,ability_to_Speak,ability_to_Read,ability_to_write,employee_id,created_date,updated_date")] Language language)
        {
            if (ModelState.IsValid)
            {
                if (id != language.Id)
                {
                    TempData["Warning"] = "Language Not found.";
                    return RedirectToAction(nameof(Index));
                }

                try
                {
                    language.updated_date = DateTime.Now;
                    _context.Update(language);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Language is Updated.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.Id))
                    {
                        TempData["Warning"] = "Something went wrong.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                return View(language);
            }
        }

        // GET: Languages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.languages == null)
            {
                return NotFound();
            }

            var language = await _context.languages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (language == null)
            {
                return NotFound();
            }

            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.languages == null)
            {
                return Problem("Entity set 'employee_context.languages'  is null.");
            }
            var language = await _context.languages.FindAsync(id);
            if (language != null)
            {
                _context.languages.Remove(language);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LanguageExists(int id)
        {
          return (_context.languages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
