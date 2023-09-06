using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Reward_managment;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

namespace ERP.Controllers.HRMs
{
    public class RewardsController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;

        public RewardsController(employee_context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rewards
        public async Task<IActionResult> IndexBasic()
        {
            User user = await _userManager.GetUserAsync(User);
            var check_employee = _context.Employees.FirstOrDefault(a => a.user_id == user.Id);
            if (check_employee != null)
            {
                var taining = _context.Rewards.Where(t => t.id == check_employee.id);
                return View(await taining.ToListAsync());
            }
            else
            {
                TempData["Warning"] = "Fill in your information";
                return View();
            }
        }
        
        // GET: Rewards
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Rewards.Include(r => r.Employee);
            return View(await employee_context.ToListAsync());
        }

        // GET: Rewards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Rewards == null)
            {
                return NotFound();
            }

            var reward = await _context.Rewards
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // GET: Rewards/Create
        public IActionResult Create()
        {
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number");
            return View();
        }

        // POST: Rewards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,reward_name,description,reason_of_reward,given_date,status,feedback,created_date,updated_date,employee_id,Created_by,Updated_by,approved_by")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reward);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", reward.employee_id);
            return View(reward);
        }

        // GET: Rewards/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Rewards == null)
            {
                return NotFound();
            }

            var reward = await _context.Rewards.FindAsync(id);
            if (reward == null)
            {
                return NotFound();
            }
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", reward.employee_id);
            return View(reward);
        }

        // POST: Rewards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,reward_name,description,reason_of_reward,given_date,status,feedback,created_date,updated_date,employee_id,Created_by,Updated_by,approved_by")] Reward reward)
        {
            if (id != reward.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reward);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RewardExists(reward.id))
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
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", reward.employee_id);
            return View(reward);
        }

        // GET: Rewards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rewards == null)
            {
                return NotFound();
            }

            var reward = await _context.Rewards
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (reward == null)
            {
                return NotFound();
            }

            return View(reward);
        }

        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rewards == null)
            {
                return Problem("Entity set 'employee_context.Rewards'  is null.");
            }
            var reward = await _context.Rewards.FindAsync(id);
            if (reward != null)
            {
                _context.Rewards.Remove(reward);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RewardExists(int id)
        {
          return (_context.Rewards?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
