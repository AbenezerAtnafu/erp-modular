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
    public class ZonesController : Controller
    {
        private readonly employee_context _context;

        public ZonesController(employee_context context)
        {
            _context = context;
        }

        // GET: Zones
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Zones.Include(z => z.Region);
            return View(await employee_context.ToListAsync());
        }

        // GET: Zones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zones == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones
                .Include(z => z.Region)
                .FirstOrDefaultAsync(m => m.id == id);
            if (zone == null)
            {
                return NotFound();
            }

            return View(zone);
        }

        // GET: Zones/Create
        public IActionResult Create()
        {
            ViewData["region_id"] = new SelectList(_context.Regions, "id", "name");
            return View();
        }

        // POST: Zones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date,region_id")] Zone zone)
        {
            
                zone.updated_date = DateTime.Now;
                zone.created_date = DateTime.Now;
                _context.Add(zone);
                ViewData["region_id"] = new SelectList(_context.Regions, "id", "name", zone.region_id);
              await _context.SaveChangesAsync();
                TempData["Success"] = "You have created successfully.";
                return RedirectToAction(nameof(Index));
          
        }

        // GET: Zones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Zones == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones.FindAsync(id);
            if (zone == null)
            {
                return NotFound();
            }
            ViewData["region_id"] = new SelectList(_context.Regions, "id", "name", zone.region_id);
            return View(zone);
        }

        // POST: Zones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date,region_id")] Zone zone)
        {
            if (id != zone.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    zone.updated_date = DateTime.Now;
                    _context.Update(zone);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZoneExists(zone.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Success"] = "You have Updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["region_id"] = new SelectList(_context.Regions, "id", "name", zone.region_id);
            return View(zone);
        }

        // GET: Zones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Zones == null)
            {
                return NotFound();
            }

            var zone = await _context.Zones
                .Include(z => z.Region)
                .FirstOrDefaultAsync(m => m.id == id);
            if (zone == null)
            {
                return NotFound();
            }

            return View(zone);
        }

        // POST: Zones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Zones == null)
            {
                return Problem("Entity set 'employee_context.Zones'  is null.");
            }
            var zone = await _context.Zones.FindAsync(id);
            if (zone != null)
            {
                _context.Zones.Remove(zone);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "You have deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool ZoneExists(int id)
        {
          return (_context.Zones?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
