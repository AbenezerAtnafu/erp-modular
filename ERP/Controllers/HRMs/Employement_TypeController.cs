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
    public class Employement_TypeController : Controller
    {
        private readonly employee_context _context;

        public Employement_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Employement_Type
     public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;

            IQueryable<Employement_Type> all_employement_types= _context.Employement_Types;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                all_employement_types = all_employement_types.Where(u =>
                    u.name.ToLower().Contains(searchTerm)
                );
            }

            var all_employement_types_count= await all_employement_types.CountAsync();
            var employement_types = await all_employement_types
                .OrderBy(u => u.name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var paged_employement_types = new StaticPagedList<Employement_Type>(employement_types, pageNumber, pageSize, all_employement_types_count);
            return View(paged_employement_types);
        }
        // GET: Employement_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employement_Types == null)
            {
                return NotFound();
            }

            var employement_Type = await _context.Employement_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (employement_Type == null)
            {
                return NotFound();
            }

            return View(employement_Type);
        }

        // GET: Employement_Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employement_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Employement_Type employement_Type)
        {
            if (ModelState.IsValid)
            {

                var employement_Typeid = _context.Employement_Types.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


                if (employement_Typeid != 0)
                {
                    employement_Typeid = employement_Typeid + 1;
                }
                else
                {
                    employement_Typeid = 1;
                }
                employement_Type.id = employement_Typeid;


                _context.Add(employement_Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employement_Type);
        }

        // GET: Employement_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employement_Types == null)
            {
                return NotFound();
            }

            var employement_Type = await _context.Employement_Types.FindAsync(id);
            if (employement_Type == null)
            {
                return NotFound();
            }
            return View(employement_Type);
        }

        // POST: Employement_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Employement_Type employement_Type)
        {
            if (id != employement_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employement_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Employement_TypeExists(employement_Type.id))
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
            return View(employement_Type);
        }

        // GET: Employement_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employement_Types == null)
            {
                return NotFound();
            }

            var employement_Type = await _context.Employement_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (employement_Type == null)
            {
                return NotFound();
            }

            return View(employement_Type);
        }

        // POST: Employement_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employement_Types == null)
            {
                return Problem("Entity set 'employee_context.Employement_Types'  is null.");
            }
            var employement_Type = await _context.Employement_Types.FindAsync(id);
            if (employement_Type != null)
            {
                _context.Employement_Types.Remove(employement_Type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Employement_TypeExists(int id)
        {
          return (_context.Employement_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
