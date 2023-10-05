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
using HRMS.Education_management;

namespace ERP.Models.HRMS.Types
{
    public class Education_Level_TypeController : Controller
    {
        private readonly employee_context _context;

        public Education_Level_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Education_Level_Type
        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;

            IQueryable<Education_Level_Type> all_Education_Level_Type = _context.Education_Level_Types;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                all_Education_Level_Type = all_Education_Level_Type.Where(u =>
                    u.name.ToLower().Contains(searchTerm)
                );
            }

            var Education_Level_Type_count = await all_Education_Level_Type.CountAsync();
            var Education_Level_Type = await all_Education_Level_Type
                .OrderBy(u => u.name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var paged_result = new StaticPagedList<Education_Level_Type>(Education_Level_Type, pageNumber, pageSize, Education_Level_Type_count);
            return View(paged_result);
        }

        // GET: Education_Level_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Education_Level_Types == null)
            {
                return NotFound();
            }

            var education_Level_Type = await _context.Education_Level_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (education_Level_Type == null)
            {
                return NotFound();
            }

            return View(education_Level_Type);
        }

        // GET: Education_Level_Type/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Education_Level_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Education_Level_Type education_Level_Type)
        {
            if (ModelState.IsValid)
            {
                var education_Level_Typeid = _context.Education_Level_Types.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


                if (education_Level_Typeid != 0)
                {
                    education_Level_Typeid = education_Level_Typeid + 1;
                }
                else
                {
                    education_Level_Typeid = 1;
                }
                education_Level_Type.id = education_Level_Typeid;

                education_Level_Type.created_date = DateTime.Now.Date;
                education_Level_Type.updated_date = DateTime.Now.Date;
                _context.Add(education_Level_Type);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New Education is added.";
                return RedirectToAction(nameof(Index));

            }
            return View(education_Level_Type);
        }

        // GET: Education_Level_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Education_Level_Types == null)
            {
                return NotFound();
            }

            var education_Level_Type = await _context.Education_Level_Types.FindAsync(id);
            if (education_Level_Type == null)
            {
                return NotFound();
            }
            return View(education_Level_Type);
        }

        // POST: Education_Level_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Education_Level_Type education_Level_Type)
        {
            if (id != education_Level_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    education_Level_Type.updated_date = DateTime.Now.Date;
                    _context.Update(education_Level_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Education_Level_TypeExists(education_Level_Type.id))
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
            return View(education_Level_Type);
        }

        // GET: Education_Level_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Education_Level_Types == null)
            {
                return NotFound();
            }

            var education_Level_Type = await _context.Education_Level_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (education_Level_Type == null)
            {
                return NotFound();
            }

            return View(education_Level_Type);
        }

        // POST: Education_Level_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Education_Level_Types == null)
            {
                return Problem("Entity set 'employee_context.Education_Level_Types'  is null.");
            }
            var education_Level_Type = await _context.Education_Level_Types.FindAsync(id);
            if (education_Level_Type != null)
            {
                _context.Education_Level_Types.Remove(education_Level_Type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Education_Level_TypeExists(int id)
        {
          return (_context.Education_Level_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
