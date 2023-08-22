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
    public class Reward_TypesController : Controller
    {
        private readonly employee_context _context;

        public Reward_TypesController(employee_context context)
        {
            _context = context;
        }

        // GET: Reward_Types
        public async Task<IActionResult> Index()
        {
              return _context.Reward_Types != null ? 
                          View(await _context.Reward_Types.ToListAsync()) :
                          Problem("Entity set 'employee_context.Reward_Types'  is null.");
        }

        // GET: Reward_Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reward_Types == null)
            {
                return NotFound();
            }

            var reward_Types = await _context.Reward_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (reward_Types == null)
            {
                return NotFound();
            }

            return View(reward_Types);
        }

        // GET: Reward_Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reward_Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Reward_Types reward_Types)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reward_Types);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reward_Types);
        }

        // GET: Reward_Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reward_Types == null)
            {
                return NotFound();
            }

            var reward_Types = await _context.Reward_Types.FindAsync(id);
            if (reward_Types == null)
            {
                return NotFound();
            }
            return View(reward_Types);
        }

        // POST: Reward_Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Reward_Types reward_Types)
        {
            if (id != reward_Types.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reward_Types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Reward_TypesExists(reward_Types.id))
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
            return View(reward_Types);
        }

        // GET: Reward_Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reward_Types == null)
            {
                return NotFound();
            }

            var reward_Types = await _context.Reward_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (reward_Types == null)
            {
                return NotFound();
            }

            return View(reward_Types);
        }

        // POST: Reward_Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reward_Types == null)
            {
                return Problem("Entity set 'employee_context.Reward_Types'  is null.");
            }
            var reward_Types = await _context.Reward_Types.FindAsync(id);
            if (reward_Types != null)
            {
                _context.Reward_Types.Remove(reward_Types);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Reward_TypesExists(int id)
        {
          return (_context.Reward_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
