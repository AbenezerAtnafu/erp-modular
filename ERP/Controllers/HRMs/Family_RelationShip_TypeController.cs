using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Types;

namespace ERP.Controllers
{
    public class Family_RelationShip_TypeController : Controller
    {
        private readonly employee_context _context;

        public Family_RelationShip_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Family_RelationShip_Type
        public async Task<IActionResult> Index()
        {
              return _context.Family_RelationShip_Types != null ? 
                          View(await _context.Family_RelationShip_Types.ToListAsync()) :
                          Problem("Entity set 'employee_context.Family_RelationShip_Types'  is null.");
        }

        // GET: Family_RelationShip_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Family_RelationShip_Types == null)
            {
                return NotFound();
            }

            var family_RelationShip_Type = await _context.Family_RelationShip_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (family_RelationShip_Type == null)
            {
                return NotFound();
            }

            return View(family_RelationShip_Type);
        }

        // GET: Family_RelationShip_Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Family_RelationShip_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Family_RelationShip_Type family_RelationShip_Type)
        {
            if (ModelState.IsValid)
            {
                family_RelationShip_Type.created_date = DateTime.Now.Date;
                family_RelationShip_Type.updated_date = DateTime.Now.Date;
                _context.Add(family_RelationShip_Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(family_RelationShip_Type);
        }

        // GET: Family_RelationShip_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Family_RelationShip_Types == null)
            {
                return NotFound();
            }

            var family_RelationShip_Type = await _context.Family_RelationShip_Types.FindAsync(id);
            if (family_RelationShip_Type == null)
            {
                return NotFound();
            }
            return View(family_RelationShip_Type);
        }

        // POST: Family_RelationShip_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Family_RelationShip_Type family_RelationShip_Type)
        {
            if (id != family_RelationShip_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    family_RelationShip_Type.updated_date = DateTime.Now.Date;
                    _context.Update(family_RelationShip_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Family_RelationShip_TypeExists(family_RelationShip_Type.id))
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
            return View(family_RelationShip_Type);
        }

        // GET: Family_RelationShip_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Family_RelationShip_Types == null)
            {
                return NotFound();
            }

            var family_RelationShip_Type = await _context.Family_RelationShip_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (family_RelationShip_Type == null)
            {
                return NotFound();
            }

            return View(family_RelationShip_Type);
        }

        // POST: Family_RelationShip_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Family_RelationShip_Types == null)
            {
                return Problem("Entity set 'employee_context.Family_RelationShip_Types'  is null.");
            }
            var family_RelationShip_Type = await _context.Family_RelationShip_Types.FindAsync(id);
            if (family_RelationShip_Type != null)
            {
                _context.Family_RelationShip_Types.Remove(family_RelationShip_Type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Family_RelationShip_TypeExists(int id)
        {
          return (_context.Family_RelationShip_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
