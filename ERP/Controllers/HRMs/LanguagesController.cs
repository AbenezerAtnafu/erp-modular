using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Language;

namespace ERP.Controllers.HRMs
{
    public class LanguagesController : Controller
    {
        private readonly employee_context _context;

        public LanguagesController(employee_context context)
        {
            _context = context;
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
                _context.Add(language);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(language);
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
                return NotFound();
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
            if (id != language.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(language);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LanguageExists(language.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(language);
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
