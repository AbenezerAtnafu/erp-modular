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
    public class SubcitiesController : Controller
    {
        private readonly employee_context _context;

        public SubcitiesController(employee_context context)
        {
            _context = context;
        }

        // GET: Subcities
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Subcitys.Include(s => s.Region);
            return View(await employee_context.ToListAsync());
        }

        // GET: Subcities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subcitys == null)
            {
                return NotFound();
            }

            var subcity = await _context.Subcitys
                .Include(s => s.Region)
                .FirstOrDefaultAsync(m => m.id == id);
            if (subcity == null)
            {
                return NotFound();
            }

            return View(subcity);
        }

        // GET: Subcities/Create
        public IActionResult Create()
        {
            ViewData["region_id"] = new SelectList(_context.Regions, "id", "name");
            return View();
        }

        // POST: Subcities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date,region_id")] Subcity subcity)
        {
           
                subcity.created_date = DateTime.Now;
                subcity.updated_date = DateTime.Now;
                _context.Add(subcity);
            ViewData["region_id"] = new SelectList(_context.Regions, "id", "name", subcity.region_id);
            await _context.SaveChangesAsync();
                TempData["Success"] = "You have created successfully.";
                return RedirectToAction(nameof(Index));
            
   
        
        }

        // GET: Subcities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Subcitys == null)
            {
                return NotFound();
            }

            var subcity = await _context.Subcitys.FindAsync(id);
            if (subcity == null)
            {
                return NotFound();
            }
            ViewData["region_id"] = new SelectList(_context.Regions, "id", "name", subcity.region_id);
            return View(subcity);
        }

        // POST: Subcities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date,region_id")] Subcity subcity)
        {
            if (id != subcity.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    subcity.updated_date = DateTime.Now;
                    _context.Update(subcity);
                    TempData["Success"] = "You have Updated successfully.";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubcityExists(subcity.id))
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
            ViewData["region_id"] = new SelectList(_context.Regions, "id", "name", subcity.region_id);
            return View(subcity);
        }

        // GET: Subcities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Subcitys == null)
            {
                return NotFound();
            }

            var subcity = await _context.Subcitys
                .Include(s => s.Region)
                .FirstOrDefaultAsync(m => m.id == id);
            if (subcity == null)
            {
                return NotFound();
            }

            return View(subcity);
        }

        // POST: Subcities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subcitys == null)
            {
                return Problem("Entity set 'employee_context.Subcitys'  is null.");
            }
            var subcity = await _context.Subcitys.FindAsync(id);
            if (subcity != null)
            {
                _context.Subcitys.Remove(subcity);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "You have deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool SubcityExists(int id)
        {
          return (_context.Subcitys?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
