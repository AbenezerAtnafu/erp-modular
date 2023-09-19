using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Office;
using X.PagedList;
using Microsoft.EntityFrameworkCore.Query;

namespace ERP.Controllers.HRMs
{
    public class DepartmentsController : Controller
    {
        private readonly employee_context _context;

        public DepartmentsController(employee_context context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;

            IQueryable<Department> all_departments = _context.Departments.Include(d => d.Division);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                all_departments = all_departments.Where(u =>
                    u.name.ToLower().Contains(searchTerm)
                );
            }

            var department_count = await all_departments.CountAsync();
            var departments = await all_departments
                .OrderBy(u => u.name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var paged_departments = new StaticPagedList<Department>(departments, pageNumber, pageSize, department_count);
            return View(paged_departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Division)
                .FirstOrDefaultAsync(m => m.id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            ViewData["division_id"] = new SelectList(_context.Divisions, "id", "name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date,division_id")] Department department)
        {
            if (ModelState.IsValid)
            {
                department.created_date = DateTime.Now;
                department.updated_date = DateTime.Now;
                _context.Add(department);
                await _context.SaveChangesAsync();
                TempData["Success"] = "You have created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["division_id"] = new SelectList(_context.Divisions, "id", "name", department.division_id);
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["division_id"] = new SelectList(_context.Divisions, "id", "name", department.division_id);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date,division_id")] Department department)
        {
            if (id != department.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    department.updated_date = DateTime.Now;
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "You have Updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.id))
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
            ViewData["division_id"] = new SelectList(_context.Divisions, "id", "name", department.division_id);
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .Include(d => d.Division)
                .FirstOrDefaultAsync(m => m.id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departments == null)
            {
                return Problem("Entity set 'employee_context.Departments'  is null.");
            }
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "You have deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
          return (_context.Departments?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
