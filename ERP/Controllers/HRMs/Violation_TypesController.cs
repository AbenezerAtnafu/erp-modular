using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Employee_managments;

namespace ERP.Controllers
{
    public class Violation_TypesController : Controller
    {
        private readonly employee_context _context;

        public Violation_TypesController(employee_context context)
        {
            _context = context;
        }

        // GET: Violation_Types
        public async Task<IActionResult> Index()
        {
              return _context.Violation_Typess != null ? 
                          View(await _context.Violation_Typess.ToListAsync()) :
                          Problem("Entity set 'employee_context.Violation_Typess'  is null.");
        }

        // GET: Violation_Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Violation_Typess == null)
            {
                return NotFound();
            }

            var violation_Types = await _context.Violation_Typess
                .FirstOrDefaultAsync(m => m.id == id);
            if (violation_Types == null)
            {
                return NotFound();
            }

            return View(violation_Types);
        }

        // GET: Violation_Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Violation_Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Violation_Types violation_Types)
        {
            if (ModelState.IsValid)
            {
                _context.Add(violation_Types);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(violation_Types);
        }

        // GET: Violation_Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Violation_Typess == null)
            {
                return NotFound();
            }

            var violation_Types = await _context.Violation_Typess.FindAsync(id);
            if (violation_Types == null)
            {
                return NotFound();
            }
            return View(violation_Types);
        }

        // POST: Violation_Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Violation_Types violation_Types)
        {
            if (id != violation_Types.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(violation_Types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Violation_TypesExists(violation_Types.id))
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
            return View(violation_Types);
        }

        // GET: Violation_Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Violation_Typess == null)
            {
                return NotFound();
            }

            var violation_Types = await _context.Violation_Typess
                .FirstOrDefaultAsync(m => m.id == id);
            if (violation_Types == null)
            {
                return NotFound();
            }

            return View(violation_Types);
        }

        // POST: Violation_Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Violation_Typess == null)
            {
                return Problem("Entity set 'employee_context.Violation_Typess'  is null.");
            }
            var violation_Types = await _context.Violation_Typess.FindAsync(id);
            if (violation_Types != null)
            {
                _context.Violation_Typess.Remove(violation_Types);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Violation_TypesExists(int id)
        {
          return (_context.Violation_Typess?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
