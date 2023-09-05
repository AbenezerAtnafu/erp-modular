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
    public class Trainign_TypeController : Controller
    {
        private readonly employee_context _context;

        public Trainign_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Trainign_Type
        public async Task<IActionResult> Index()
        {
              return _context.Trainign_Types != null ? 
                          View(await _context.Trainign_Types.ToListAsync()) :
                          Problem("Entity set 'employee_context.Trainign_Types'  is null.");
        }

        // GET: Trainign_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trainign_Types == null)
            {
                return NotFound();
            }

            var trainign_Type = await _context.Trainign_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainign_Type == null)
            {
                return NotFound();
            }

            return View(trainign_Type);
        }

        // GET: Trainign_Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainign_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Trainign_Type trainign_Type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainign_Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trainign_Type);
        }

        // GET: Trainign_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trainign_Types == null)
            {
                return NotFound();
            }

            var trainign_Type = await _context.Trainign_Types.FindAsync(id);
            if (trainign_Type == null)
            {
                return NotFound();
            }
            return View(trainign_Type);
        }

        // POST: Trainign_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Trainign_Type trainign_Type)
        {
            if (id != trainign_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainign_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Trainign_TypeExists(trainign_Type.id))
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
            return View(trainign_Type);
        }

        // GET: Trainign_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trainign_Types == null)
            {
                return NotFound();
            }

            var trainign_Type = await _context.Trainign_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (trainign_Type == null)
            {
                return NotFound();
            }

            return View(trainign_Type);
        }

        // POST: Trainign_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trainign_Types == null)
            {
                return Problem("Entity set 'employee_context.Trainign_Types'  is null.");
            }
            var trainign_Type = await _context.Trainign_Types.FindAsync(id);
            if (trainign_Type != null)
            {
                _context.Trainign_Types.Remove(trainign_Type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Trainign_TypeExists(int id)
        {
          return (_context.Trainign_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
