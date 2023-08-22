using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Address;

namespace ERP.Controllers
{
    public class WoredasController : Controller
    {
        private readonly employee_context _context;

        public WoredasController(employee_context context)
        {
            _context = context;
        }

        // GET: Woredas
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Woredas.Include(w => w.Subcity).Include(w => w.Zone);
            return View(await employee_context.ToListAsync());
        }

        // GET: Woredas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Woredas == null)
            {
                return NotFound();
            }

            var woreda = await _context.Woredas
                .Include(w => w.Subcity)
                .Include(w => w.Zone)
                .FirstOrDefaultAsync(m => m.id == id);
            if (woreda == null)
            {
                return NotFound();
            }

            return View(woreda);
        }

        // GET: Woredas/Create
        public IActionResult Create()
        {
            ViewData["subcity_id"] = new SelectList(_context.Subcitys, "id", "name");
            ViewData["zone_id"] = new SelectList(_context.Zones, "id", "name");
            return View();
        }

        // POST: Woredas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date,subcity_id,zone_id")] Woreda woreda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(woreda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["subcity_id"] = new SelectList(_context.Subcitys, "id", "name", woreda.subcity_id);
            ViewData["zone_id"] = new SelectList(_context.Zones, "id", "name", woreda.zone_id);
            return View(woreda);
        }

        // GET: Woredas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Woredas == null)
            {
                return NotFound();
            }

            var woreda = await _context.Woredas.FindAsync(id);
            if (woreda == null)
            {
                return NotFound();
            }
            ViewData["subcity_id"] = new SelectList(_context.Subcitys, "id", "name", woreda.subcity_id);
            ViewData["zone_id"] = new SelectList(_context.Zones, "id", "name", woreda.zone_id);
            return View(woreda);
        }

        // POST: Woredas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date,subcity_id,zone_id")] Woreda woreda)
        {
            if (id != woreda.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(woreda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WoredaExists(woreda.id))
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
            ViewData["subcity_id"] = new SelectList(_context.Subcitys, "id", "name", woreda.subcity_id);
            ViewData["zone_id"] = new SelectList(_context.Zones, "id", "name", woreda.zone_id);
            return View(woreda);
        }

        // GET: Woredas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Woredas == null)
            {
                return NotFound();
            }

            var woreda = await _context.Woredas
                .Include(w => w.Subcity)
                .Include(w => w.Zone)
                .FirstOrDefaultAsync(m => m.id == id);
            if (woreda == null)
            {
                return NotFound();
            }

            return View(woreda);
        }

        // POST: Woredas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Woredas == null)
            {
                return Problem("Entity set 'employee_context.Woredas'  is null.");
            }
            var woreda = await _context.Woredas.FindAsync(id);
            if (woreda != null)
            {
                _context.Woredas.Remove(woreda);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WoredaExists(int id)
        {
          return (_context.Woredas?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
