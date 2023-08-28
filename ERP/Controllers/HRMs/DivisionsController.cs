using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Office;

namespace ERP.Controllers.HRMs
{
    public class DivisionsController : Controller
    {
        private readonly employee_context _context;

        public DivisionsController(employee_context context)
        {
            _context = context;
        }

        // GET: Divisions
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Divisions.Include(d => d.Organization);
            return View(await employee_context.ToListAsync());
        }

        // GET: Divisions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Divisions == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions
                .Include(d => d.Organization)
                .FirstOrDefaultAsync(m => m.id == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // GET: Divisions/Create
        [HttpGet]
        public IActionResult Create()
        {
         
            ViewData["org_id"] = new SelectList(_context.Organizations, "id", "name");
            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date,org_id")] Division division)
        {
            if (ModelState.IsValid)
            {
                division.created_date = DateTime.Now.Date;
                division.updated_date = DateTime.Now.Date;
                _context.Add(division);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Divisions"); // Corrected redirection
            }

            ViewData["org_id"] = new SelectList(_context.Organizations, "id", "name", division.org_id);
            return View(division);
        }

        // GET: Divisions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Divisions == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }
            ViewData["org_id"] = new SelectList(_context.Organizations, "id", "name", division.org_id);
            return View(division);
        }

        // POST: Divisions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date,org_id")] Division division)
        {
            if (id != division.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(division);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DivisionExists(division.id))
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
            ViewData["org_id"] = new SelectList(_context.Organizations, "id", "name", division.org_id);
            return View(division);
        }

        // GET: Divisions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Divisions == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions
                .Include(d => d.Organization)
                .FirstOrDefaultAsync(m => m.id == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Divisions == null)
            {
                return Problem("Entity set 'employee_context.Divisions'  is null.");
            }
            var division = await _context.Divisions.FindAsync(id);
            if (division != null)
            {
                _context.Divisions.Remove(division);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DivisionExists(int id)
        {
            return (_context.Divisions?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
