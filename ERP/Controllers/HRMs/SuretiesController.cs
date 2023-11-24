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
using ERP.Models.HRMS.Employee_managments;

namespace ERP.Controllers.HRMs
{
    public class SuretiesController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;

        public SuretiesController(employee_context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Sureties
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Sureties.Include(a => a.Employees);
            return View(employee_context);
                       
        }
        public async Task<IActionResult> IndexPersonal()
        {
            User user = await _userManager.GetUserAsync(User);
            var check_employee = _context.Employees.FirstOrDefault(a => a.user_id == user.Id);

            if (check_employee != null)
            {
                var assign_sureties = _context.Sureties.Where(e => e.emp_id == check_employee.id).Include(a => a.Employees);
                return View(assign_sureties);
            }
            else
            {
                TempData["Warning"] = "No Employee Info";
                return View();
            }
        }
        // GET: Sureties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sureties == null)
            {
                return NotFound();
            }

            var surety = await _context.Sureties
                .FirstOrDefaultAsync(m => m.id == id);
            if (surety == null)
            {
                return NotFound();
            }

            return View(surety);
        }

        // GET: Sureties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sureties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,emp_id,company_name,reason,created_date,updated_date")] Surety surety)
        {
            var a = surety.created_date;
            var users = _userManager.GetUserId(HttpContext.User);
            var employee = _context.Employees.FirstOrDefault(a => a.user_id == users);
            var employment_type_id = _context.Employee_Offices.FirstOrDefault(e => e.employee_id == employee.id);

            if (employee != null)
            {
                if (employment_type_id.employment_type_id == 4)
                {
                    surety.created_date = DateTime.Now.Date;
                    surety.updated_date = DateTime.Now.Date;
                    bool valueExists = _context.Sureties.Any(a => a.emp_id == employee.id);
                    if (valueExists)
                    {
                        TempData["Warning"] = "you alrady have a record on surity table " + 
                            "detail:" + "date:" + surety.created_date.Date + "company name:" + surety.company_name;
                        return RedirectToAction(nameof(Create));
                    }
                    else
                    {
                        surety.emp_id = employee.id;
                        _context.Add(surety);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(IndexPersonal));
                    }
                }
                else
                {
                    TempData["Warning"] = "You are not Permanent Employee";
                    return RedirectToAction(nameof(Create));
                }
            }
            else
            {
                TempData["Warning"] = "You should First fill in Your detail.";
                return RedirectToAction(nameof(IndexPersonal));
            }

        }

        // GET: Sureties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Sureties == null)
            {
                return NotFound();
            }

            var surety = await _context.Sureties.FindAsync(id);
            if (surety == null)
            {
                return NotFound();
            }
            return View(surety);
        }

        // POST: Sureties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,emp_id,company_name,reason,created_date,updated_date")] Surety surety)
        {
            if (id != surety.id)
            {
                return NotFound();
            }

            surety.created_date = DateTime.Now;
            surety.updated_date = DateTime.Now;
            surety.emp_id = id;

            return View(surety);
        }

        // GET: Sureties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sureties == null)
            {
                return NotFound();
            }

            var surety = await _context.Sureties
                .FirstOrDefaultAsync(m => m.id == id);
            if (surety == null)
            {
                return NotFound();
            }

            return View(surety);
        }

        // POST: Sureties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sureties == null)
            {
                return Problem("Entity set 'employee_context.Sureties'  is null.");
            }
            var surety = await _context.Sureties.FindAsync(id);
            if (surety != null)
            {
                _context.Sureties.Remove(surety);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuretyExists(int id)
        {
            return (_context.Sureties?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
