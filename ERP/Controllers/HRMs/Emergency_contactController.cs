using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Emergency_contact;
using HRMS.Types;

namespace ERP.Controllers.HRMs
{
    public class Emergency_contactController : Controller
    {
        private readonly employee_context _context;

        public Emergency_contactController(employee_context context)
        {
            _context = context;
        }

        // GET: Emergency_contact
        public async Task<IActionResult> Index()
        {
              return _context.emergency_Contacts != null ? 
                          View(await _context.emergency_Contacts.ToListAsync()) :
                          Problem("Entity set 'employee_context.emergency_Contacts'  is null.");
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
            return View();
        }

        // POST: Emergency_contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,full_name,phonenumber,alternative_phonenumber,Relationship,employee_id,created_date,updated_date")] Emergency_contact emergency_contact)
        {
            if (ModelState.IsValid)
            {
                emergency_contact.created_date = DateTime.Now.Date;
                emergency_contact.updated_date = DateTime.Now.Date;
                _context.Add(emergency_contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emergency_contact);
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emergency_contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Emergency_contactExists(emergency_contact.id))
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
            return View(emergency_contact);
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
