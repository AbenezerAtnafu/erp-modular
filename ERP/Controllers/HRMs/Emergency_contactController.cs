using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Employee_managments;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using HRMS.Types;

namespace ERP.Controllers.HRMs
{
    public class Emergency_contactController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;


        public Emergency_contactController(employee_context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Emergency_contact
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.GetUserAsync(User);
            var check_employee = _context.Employees.FirstOrDefault(a => a.user_id == user.Id);
            if (check_employee != null)
            {
                var emergency_Contacts = _context.emergency_Contacts.Where(t => t.employee_id == check_employee.id);
                return View(await emergency_Contacts.ToListAsync());
            }
            else
            {
                TempData["Warning"] = "Fill in your information";
                return View();
            }
        }

        // GET: Emergency_contact/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.emergency_Contacts == null)
            {
                return NotFound();
            }

            var emergency_contact = await _context.emergency_Contacts
                .FirstOrDefaultAsync(m => m.id == id);
            if (emergency_contact == null)
            {
                return NotFound();
            }

            return View(emergency_contact);
        }

        // GET: Emergency_contact/Create
        public IActionResult Create()
        {
            ViewData["family_relationship_id"] = new SelectList(_context.Family_RelationShip_Types, "id", "name");
            return View();
        }

        // POST: Emergency_contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,full_name,phonenumber,alternative_phonenumber,Relationship,employee_id")] Emergency_contact emergency_contact)
        {

            var users = _userManager.GetUserId(HttpContext.User);
            var employee = _context.Employees.FirstOrDefault(a => a.user_id == users);


            var emergency_contactid = _context.emergency_Contacts.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


            if (emergency_contactid != 0)
            {
                emergency_contactid = emergency_contactid + 1;
            }
            else
            {
                emergency_contactid = 1;
            }
            emergency_contact.id = emergency_contactid;

            if (employee != null)
            {
                emergency_contact.employee_id = employee.id;
                emergency_contact.created_date = DateTime.Now.Date;
                emergency_contact.updated_date = DateTime.Now.Date;
                _context.Add(emergency_contact);
                await _context.SaveChangesAsync();

                TempData["Success"] = "New Emergency contact is added.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Warning"] = "You should First fill in Your detail.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Emergency_contact/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.emergency_Contacts == null)
            {
                return NotFound();
            }

            var emergency_contact = await _context.emergency_Contacts.FindAsync(id);
            if (emergency_contact == null)
            {
                return NotFound();
            }
            ViewData["family_relationship_id"] = new SelectList(_context.Family_RelationShip_Types, "id", "name", emergency_contact.Relationship);
            return View(emergency_contact);
        }

        // POST: Emergency_contact/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,full_name,phonenumber,alternative_phonenumber,Relationship,employee_id,created_date,updated_date")] Emergency_contact emergency_contact)
        {

            if (id != emergency_contact.id)
            {
                return NotFound();
            }

            try
            {
                emergency_contact.updated_date = DateTime.Now;
                _context.Update(emergency_contact);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Language is Updated.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Emergency_contactExists(emergency_contact.id))
                {
                    TempData["Warning"] = "Something went wrong.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Emergency_contact/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.emergency_Contacts == null)
            {
                return NotFound();
            }

            var emergency_contact = await _context.emergency_Contacts
                .FirstOrDefaultAsync(m => m.id == id);
            if (emergency_contact == null)
            {
                return NotFound();
            }

            return View(emergency_contact);
        }

        // POST: Emergency_contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.emergency_Contacts == null)
            {
                return Problem("Entity set 'employee_context.emergency_Contacts'  is null.");
            }
            var emergency_contact = await _context.emergency_Contacts.FindAsync(id);
            if (emergency_contact != null)
            {
                _context.emergency_Contacts.Remove(emergency_contact);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Emergency_contactExists(int id)
        {
            return (_context.emergency_Contacts?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }


}
