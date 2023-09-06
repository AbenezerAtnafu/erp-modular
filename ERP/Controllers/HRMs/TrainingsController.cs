using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Training;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

namespace ERP.Controllers.HRMs
{
    public class TrainingsController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;
        private readonly IFileProvider fileProvider;

        public TrainingsController(employee_context context, UserManager<User> userManager, IFileProvider fileProvider)
        {
            _context = context;
            _userManager = userManager;
            this.fileProvider = fileProvider;
        }

        // GET: Trainings
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            var check_employee = _context.Employees.FirstOrDefault(a => a.user_id == user.Id);
            if(check_employee != null)
            {
                var taining = _context.Training.Where(t => t.employee_id == check_employee.id);
                return View(await taining.ToListAsync());
            }
            else
            {
                return View();
            }
        }

        // GET: Trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Training == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: Trainings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,training_institution,description,country_of_training,email,training_type,training_situation,start_date,end_date,status,feedback,created_date,updated_date,employee_id,Created_by,Updated_by,approved_by")] Training training)
        {
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", training.employee_id);
            return View(training);
        }

        // GET: Trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Training == null)
            {
                return NotFound();
            }

            var training = await _context.Training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", training.employee_id);
            return View(training);
        }

        // POST: Trainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,training_institution,description,country_of_training,email,training_type,training_situation,start_date,end_date,status,feedback,created_date,updated_date,employee_id,Created_by,Updated_by,approved_by")] Training training)
        {
            if (id != training.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingExists(training.id))
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
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", training.employee_id);
            return View(training);
        }

        // GET: Trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Training == null)
            {
                return NotFound();
            }

            var training = await _context.Training
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: Trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Training == null)
            {
                return Problem("Entity set 'employee_context.Training'  is null.");
            }
            var training = await _context.Training.FindAsync(id);
            if (training != null)
            {
                _context.Training.Remove(training);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingExists(int id)
        {
          return (_context.Training?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
