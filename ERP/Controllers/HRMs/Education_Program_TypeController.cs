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
    public class Education_Program_TypeController : Controller
    {
        private readonly employee_context _context;

        public Education_Program_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Education_Program_Type
        public async Task<IActionResult> Index()
        {
            return _context.Education_Program_Types != null ?
                        View(await _context.Education_Program_Types.ToListAsync()) :
                        Problem("Entity set 'employee_context.Education_Program_Types'  is null.");
        }

        // GET: Education_Program_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Education_Program_Types == null)
            {
                return NotFound();
            }

            var education_Program_Type = await _context.Education_Program_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (education_Program_Type == null)
            {
                return NotFound();
            }

            return View(education_Program_Type);
        }

        // GET: Education_Program_Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Education_Program_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Education_Program_Type education_Program_Type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(education_Program_Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(education_Program_Type);
        }

        // GET: Education_Program_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Education_Program_Types == null)
            {
                return NotFound();
            }

            var education_Program_Type = await _context.Education_Program_Types.FindAsync(id);
            if (education_Program_Type == null)
            {
                return NotFound();
            }
            return View(education_Program_Type);
        }

        // POST: Education_Program_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Education_Program_Type education_Program_Type)
        {
            if (id != education_Program_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(education_Program_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Education_Program_TypeExists(education_Program_Type.id))
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
            return View(education_Program_Type);
        }

        // GET: Education_Program_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Education_Program_Types == null)
            {
                return NotFound();
            }

            var education_Program_Type = await _context.Education_Program_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (education_Program_Type == null)
            {
                return NotFound();
            }

            return View(education_Program_Type);
        }

        // POST: Education_Program_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Education_Program_Types == null)
            {
                return Problem("Entity set 'employee_context.Education_Program_Types'  is null.");
            }
            var education_Program_Type = await _context.Education_Program_Types.FindAsync(id);
            if (education_Program_Type != null)
            {
                _context.Education_Program_Types.Remove(education_Program_Type);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Education_Program_TypeExists(int id)
        {
            return (_context.Education_Program_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
