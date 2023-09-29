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
    public class Disability_TypeController : Controller
    {
        private readonly employee_context _context;

        public Disability_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Disability_Type
        public async Task<IActionResult> Index()
        {
              return _context.Disability_Types != null ? 
                          View(await _context.Disability_Types.ToListAsync()) :
                          Problem("Entity set 'employee_context.Disability_Types'  is null.");
        }

        // GET: Disability_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Disability_Types == null)
            {
                return NotFound();
            }

            var disability_Type = await _context.Disability_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (disability_Type == null)
            {
                return NotFound();
            }

            return View(disability_Type);
        }

        // GET: Disability_Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Disability_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Disability_Type disability_Type)
        {
            if (ModelState.IsValid)
            {


                var lastid = _context.Disability_Types.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


                if (lastid != 0)
                {
                    disability_Type.id = lastid + 1;
                }
                else
                {
                    disability_Type.id = 1;
                }

                _context.Add(disability_Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disability_Type);
        }

        // GET: Disability_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Disability_Types == null)
            {
                return NotFound();
            }

            var disability_Type = await _context.Disability_Types.FindAsync(id);
            if (disability_Type == null)
            {
                return NotFound();
            }
            return View(disability_Type);
        }

        // POST: Disability_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Disability_Type disability_Type)
        {
            if (id != disability_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disability_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Disability_TypeExists(disability_Type.id))
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
            return View(disability_Type);
        }

        // GET: Disability_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Disability_Types == null)
            {
                return NotFound();
            }

            var disability_Type = await _context.Disability_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (disability_Type == null)
            {
                return NotFound();
            }

            return View(disability_Type);
        }

        // POST: Disability_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Disability_Types == null)
            {
                return Problem("Entity set 'employee_context.Disability_Types'  is null.");
            }
            var disability_Type = await _context.Disability_Types.FindAsync(id);
            if (disability_Type != null)
            {
                _context.Disability_Types.Remove(disability_Type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Disability_TypeExists(int id)
        {
          return (_context.Disability_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
