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

        public Family_HistoryController(employee_context context)
        {
            _context = context;
        }

        // GET: Family_History
        public async Task<IActionResult> Index()
        {
            return _context.family_Histories != null ?
                        View(await _context.family_Histories.ToListAsync()) :
                        Problem("Entity set 'employee_context.family_Histories'  is null.");
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
            ViewData["family_relationship_id"] = new SelectList(_context.Family_RelationShip_Types, "id", "name");
            return View();
        }

        // POST: Family_History/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,full_name,job_name,house_number,phonenumber,alternative_phonenumber,gender,family_relationship_id,primary_address,employee_id,created_date,updated_date")] Family_History family_History)
        {
            if (ModelState.IsValid)
            {

                family_History.created_date = DateTime.Now.Date;
                family_History.updated_date= DateTime.Now.Date;
                ViewData["family_relationship_id"] = new SelectList(_context.Family_RelationShip_Types, "id", "name",family_History.family_relationship_id);
                _context.Add(family_History);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(family_History);
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
