using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Employee_managments;

namespace ERP.Controllers.HRMs
{
    public class Family_HistoryController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;
       
        public Family_HistoryController(employee_context context , UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Family_History
        public async Task<IActionResult> Index()
        {
            var family_context = _context.family_Histories.Include(e => e.Family_RelationShip_Type).Include(e => e.Employee);
            return View(await family_context.ToListAsync());
        }

        public async Task<IActionResult> Index_Personal()
        {
            User user = await _userManager.GetUserAsync(User);
            var check_employee = _context.Employees.FirstOrDefault(a => a.user_id == user.Id);

            if (check_employee != null)
            {
                var assign_family_history = _context.family_Histories.Where(e => e.employee_id == check_employee.id).Include(e => e.Employee).Include(a=>a.Family_Relationship_Types);
                return View(assign_family_history);
            }
            else
            {
                TempData["Warning"] = "No Employee Info";
                return View();
            }
        }
        // GET: Family_History/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.family_Histories == null)
            {
                return NotFound();
            }

            var family_History = await _context.family_Histories
                .FirstOrDefaultAsync(m => m.id == id);
            if (family_History == null)
            {
                return NotFound();
            }

            return View(family_History);
        }

        // GET: Family_History/Create
        public IActionResult Create()
        {
            ViewData["Family_Relationship_Types"] = new SelectList(_context.Family_RelationShip_Types, "id", "name");
            return View();
        }

        // POST: Family_History/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,full_name,job_name,house_number,phonenumber,alternative_phonenumber,gender,family_relationship_id,primary_address,employee_id,created_date,updated_date")] Family_History family_History)
        {
            
                User users = await _userManager.GetUserAsync(User);
                var check_employee = _context.Employees.FirstOrDefault(e => e.user_id == users.Id);
                if (check_employee != null)
                {
                    family_History.employee_id = check_employee.id;
                    family_History.created_date = DateTime.Now.Date;
                    family_History.updated_date = DateTime.Now.Date;
                    ViewData["Family_Relationship_Types"] = new SelectList(_context.Family_RelationShip_Types, "id", "name", family_History.Family_Relationship_Types);
                    _context.Add(family_History);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        
        }

        // GET: Family_History/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.family_Histories == null)
            {
                return NotFound();
            }

            var family_History = await _context.family_Histories.FindAsync(id);
            if (family_History == null)
            {
                return NotFound();
            }
            return View(family_History);
        }

        // POST: Family_History/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,full_name,job_name,house_number,phonenumber,alternative_phonenumber,gender,family_relationship_id,primary_address,employee_id,created_date,updated_date")] Family_History family_History)
        {
            if (id != family_History.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(family_History);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Family_HistoryExists(family_History.id))
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
            return View(family_History);
        }

        // GET: Family_History/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.family_Histories == null)
            {
                return NotFound();
            }

            var family_History = await _context.family_Histories
                .FirstOrDefaultAsync(m => m.id == id);
            if (family_History == null)
            {
                return NotFound();
            }

            return View(family_History);
        }

        // POST: Family_History/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.family_Histories == null)
            {
                return Problem("Entity set 'employee_context.family_Histories'  is null.");
            }
            var family_History = await _context.family_Histories.FindAsync(id);
            if (family_History != null)
            {
                _context.family_Histories.Remove(family_History);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Family_HistoryExists(int id)
        {
            return (_context.family_Histories?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
