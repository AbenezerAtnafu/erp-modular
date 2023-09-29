using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Types;

namespace ERP.Controllers.HRMs
{
    public class Position_Level_TypesController : Controller
    {
        private readonly employee_context _context;

        public Position_Level_TypesController(employee_context context)
        {
            _context = context;
        }

        // GET: Position_Level_Types
        public async Task<IActionResult> Index()
        {
              return _context.Position_Level_Types != null ? 
                          View(await _context.Position_Level_Types.ToListAsync()) :
                          Problem("Entity set 'employee_context.Position_Level_Types'  is null.");
        }

        // GET: Position_Level_Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Position_Level_Types == null)
            {
                return NotFound();
            }

            var position_Level_Types = await _context.Position_Level_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (position_Level_Types == null)
            {
                return NotFound();
            }

            return View(position_Level_Types);
        }

        // GET: Position_Level_Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Position_Level_Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Position_Level_Types position_Level_Types)
        {
            if (ModelState.IsValid)
            {
                var lastid = _context.Position_Level_Types.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


                if (lastid != 0)
                {
                    position_Level_Types.id = lastid + 1;
                }
                else
                {
                    position_Level_Types.id = 1;
                }
                _context.Add(position_Level_Types);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(position_Level_Types);
        }

        // GET: Position_Level_Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Position_Level_Types == null)
            {
                return NotFound();
            }

            var position_Level_Types = await _context.Position_Level_Types.FindAsync(id);
            if (position_Level_Types == null)
            {
                return NotFound();
            }
            return View(position_Level_Types);
        }

        // POST: Position_Level_Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Position_Level_Types position_Level_Types)
        {
            if (id != position_Level_Types.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(position_Level_Types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Position_Level_TypesExists(position_Level_Types.id))
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
            return View(position_Level_Types);
        }

        // GET: Position_Level_Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Position_Level_Types == null)
            {
                return NotFound();
            }

            var position_Level_Types = await _context.Position_Level_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (position_Level_Types == null)
            {
                return NotFound();
            }

            return View(position_Level_Types);
        }

        // POST: Position_Level_Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Position_Level_Types == null)
            {
                return Problem("Entity set 'employee_context.Position_Level_Types'  is null.");
            }
            var position_Level_Types = await _context.Position_Level_Types.FindAsync(id);
            if (position_Level_Types != null)
            {
                _context.Position_Level_Types.Remove(position_Level_Types);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Position_Level_TypesExists(int id)
        {
          return (_context.Position_Level_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
