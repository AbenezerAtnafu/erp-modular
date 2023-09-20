using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Types;
using X.PagedList;

namespace ERP.Controllers
{
    public class Marital_Status_TypesController : Controller
    {
        private readonly employee_context _context;

        public Marital_Status_TypesController(employee_context context)
        {
            _context = context;
        }

        // GET: Marital_Status_Types
        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;

            IQueryable<Marital_Status_Types> all_Marital_Status_Types = _context.Marital_Status_Types;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                all_Marital_Status_Types = all_Marital_Status_Types.Where(u =>
                    u.name.ToLower().Contains(searchTerm)
                );
            }

            var Marital_Status_Types_Count = await all_Marital_Status_Types.CountAsync();
            var Marital_Status_Types = await all_Marital_Status_Types
                .OrderBy(u => u.name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var paged_result = new StaticPagedList<Marital_Status_Types>(Marital_Status_Types, pageNumber, pageSize, Marital_Status_Types_Count);
            return View(paged_result);
        }

        // GET: Marital_Status_Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marital_Status_Types == null)
            {
                return NotFound();
            }

            var marital_Status_Types = await _context.Marital_Status_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (marital_Status_Types == null)
            {
                return NotFound();
            }

            return View(marital_Status_Types);
        }

        // GET: Marital_Status_Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marital_Status_Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Marital_Status_Types marital_Status_Types)
        {
            if (ModelState.IsValid)
            {
                marital_Status_Types.created_date = DateTime.Now.Date;
                marital_Status_Types.updated_date= DateTime.Now.Date;
                _context.Add(marital_Status_Types);
                await _context.SaveChangesAsync();
                TempData["Success"] = "You have created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(marital_Status_Types);
        }

        // GET: Marital_Status_Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marital_Status_Types == null)
            {
                return NotFound();
            }

            var marital_Status_Types = await _context.Marital_Status_Types.FindAsync(id);
            if (marital_Status_Types == null)
            {
                return NotFound();
            }
            return View(marital_Status_Types);
        }

        // POST: Marital_Status_Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Marital_Status_Types marital_Status_Types)
        {
            if (id != marital_Status_Types.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    marital_Status_Types.updated_date = DateTime.Now.Date;
                    _context.Update(marital_Status_Types);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "You have Updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Marital_Status_TypesExists(marital_Status_Types.id))
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
            return View(marital_Status_Types);
        }

        // GET: Marital_Status_Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marital_Status_Types == null)
            {
                return NotFound();
            }

            var marital_Status_Types = await _context.Marital_Status_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (marital_Status_Types == null)
            {
                return NotFound();
            }

            return View(marital_Status_Types);
        }

        // POST: Marital_Status_Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marital_Status_Types == null)
            {
                return Problem("Entity set 'employee_context.Marital_Status_Types'  is null.");
            }
            var marital_Status_Types = await _context.Marital_Status_Types.FindAsync(id);
            if (marital_Status_Types != null)
            {
                _context.Marital_Status_Types.Remove(marital_Status_Types);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "You have deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool Marital_Status_TypesExists(int id)
        {
          return (_context.Marital_Status_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
