using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Education_management;

namespace ERP.Controllers.HRMs
{
    public class EducationsController : Controller
    {
        private readonly employee_context _context;

        public EducationsController(employee_context context)
        {
            _context = context;
        }

        // GET: Educations
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Educations.Include(e => e.Education_Level_Type).Include(e => e.Education_Program_Type).Include(e => e.Employee);
            return View(await employee_context.ToListAsync());
        }

        // GET: Educations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Educations == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.Education_Level_Type)
                .Include(e => e.Education_Program_Type)
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        // GET: Educations/Create
        public IActionResult Create()
        {
            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name");
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name");
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number");
            return View();
        }

        // POST: Educations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,institution_name,institution_email,institution_website,filed_of_study,description,start_date,end_date,Identificationnumber,status,feedback,created_date,updated_date,employee_id,educational_program_id,educational_level_type_id")] Education education)
        {
            if (ModelState.IsValid)
            {
                _context.Add(education);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name", education.educational_level_type_id);
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name", education.educational_program_id);
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", education.employee_id);
            return View(education);
        }

        // GET: Educations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Educations == null)
            {
                return NotFound();
            }

            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                return NotFound();
            }
            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name", education.educational_level_type_id);
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name", education.educational_program_id);
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", education.employee_id);
            return View(education);
        }

        // POST: Educations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,institution_name,institution_email,institution_website,filed_of_study,description,start_date,end_date,Identificationnumber,status,feedback,created_date,updated_date,employee_id,educational_program_id,educational_level_type_id")] Education education)
        {
            if (id != education.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(education);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationExists(education.id))
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
            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name", education.educational_level_type_id);
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name", education.educational_program_id);
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", education.employee_id);
            return View(education);
        }

        // GET: Educations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Educations == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.Education_Level_Type)
                .Include(e => e.Education_Program_Type)
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        // POST: Educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Educations == null)
            {
                return Problem("Entity set 'employee_context.Educations'  is null.");
            }
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                _context.Educations.Remove(education);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EducationExists(int id)
        {
          return (_context.Educations?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
