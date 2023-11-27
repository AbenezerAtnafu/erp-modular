using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Surety;
using Microsoft.AspNetCore.Identity;

namespace ERP.Controllers.HRMs
{
    public class ExperienceRequestsController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;

        public ExperienceRequestsController(employee_context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: ExperienceRequests
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            var employee = _context.Employees.FirstOrDefault(e => e.user_id == user.Id && e.profile_status == true);
            if (employee != null)
            {
                var employee_context = _context.ExperienceRequest
                    .Where(e => e.employee_id == employee.id)
                    .Include(e=>e.Employee.Employee_Office.Team)
                    .Include(e=>e.Employee.Employee_Office.Position)
                    .Include(e => e.Employee)
                    .ToListAsync();
                return View(employee_context);
            }
            else
            {
                TempData["Warning"] = "Please fill in your employment information and Approve.";
                return View();
            }
        }

        // GET: ExperienceRequests
        public async Task<IActionResult> IndexAll()
        {
            var employee_context = _context.ExperienceRequest.Include(e => e.Employee);
            return View(await employee_context.ToListAsync());
        }

        // GET: ExperienceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ExperienceRequest == null)
            {
                return NotFound();
            }

            var experienceRequest = await _context.ExperienceRequest
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (experienceRequest == null)
            {
                return NotFound();
            }

            return View(experienceRequest);
        }

        // GET: ExperienceRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ExperienceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,reason,status,created_date,updated_date,employee_id")] ExperienceRequest experienceRequest)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.GetUserAsync(User);
                var employee = _context.Employees.FirstOrDefault(e => e.user_id == user.Id && e.profile_status == true);
                if (employee != null)
                {
                    experienceRequest.employee_id = employee.id;
                    _context.Add(experienceRequest);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Warning"] = "Please fill in your employment information and Approve.";
                    return View();
                }
               
            }
            return View(experienceRequest);
        }

        // GET: ExperienceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ExperienceRequest == null)
            {
                return NotFound();
            }

            var experienceRequest = await _context.ExperienceRequest.FindAsync(id);
            if (experienceRequest == null)
            {
                return NotFound();
            }
            return View(experienceRequest);
        }

        // POST: ExperienceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,reason,status,created_date,updated_date,employee_id")] ExperienceRequest experienceRequest)
        {
            if (id != experienceRequest.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(experienceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExperienceRequestExists(experienceRequest.id))
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
            return View(experienceRequest);
        }

        // GET: ExperienceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ExperienceRequest == null)
            {
                return NotFound();
            }

            var experienceRequest = await _context.ExperienceRequest
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (experienceRequest == null)
            {
                return NotFound();
            }

            return View(experienceRequest);
        }

        // POST: ExperienceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ExperienceRequest == null)
            {
                return Problem("Entity set 'employee_context.ExperienceRequest'  is null.");
            }
            var experienceRequest = await _context.ExperienceRequest.FindAsync(id);
            if (experienceRequest != null)
            {
                _context.ExperienceRequest.Remove(experienceRequest);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExperienceRequestExists(int id)
        {
          return (_context.ExperienceRequest?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
