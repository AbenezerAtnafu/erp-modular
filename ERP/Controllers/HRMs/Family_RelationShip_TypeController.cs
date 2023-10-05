using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Types;
using X.PagedList;
using ERP.Models.HRMS.Employee_managments;

namespace ERP.Controllers
{
    public class Family_RelationShip_TypeController : Controller
    {
        private readonly employee_context _context;

        public Family_RelationShip_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Family_RelationShip_Type
        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;

            IQueryable<Family_RelationShip_Type> all_Family_RelationShip_Types = _context.Family_RelationShip_Types;

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                all_Family_RelationShip_Types = all_Family_RelationShip_Types.Where(u =>
                    u.name.ToLower().Contains(searchTerm)
                );
            }

            var Family_RelationShip_Types_Count = await all_Family_RelationShip_Types.CountAsync();
            var Family_RelationShip_Types = await all_Family_RelationShip_Types
                .OrderBy(u => u.name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var paged_result = new StaticPagedList<Family_RelationShip_Type>(Family_RelationShip_Types, pageNumber, pageSize, Family_RelationShip_Types_Count);
            return View(paged_result);
        }

        // GET: Family_RelationShip_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Family_RelationShip_Types == null)
            {
                return NotFound();
            }

            var family_RelationShip_Type = await _context.Family_RelationShip_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (family_RelationShip_Type == null)
            {
                return NotFound();
            }

            return View(family_RelationShip_Type);
        }

        // GET: Family_RelationShip_Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Family_RelationShip_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Family_RelationShip_Type family_RelationShip_Type)
        {
            if (ModelState.IsValid)
            {


                var family_RelationShip_Typeid = _context.family_Histories.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


                if (family_RelationShip_Typeid != 0)
                {
                    family_RelationShip_Typeid = family_RelationShip_Typeid + 1;
                }
                else
                {
                    family_RelationShip_Typeid = 1;
                }
                family_RelationShip_Type.id = family_RelationShip_Typeid;


                family_RelationShip_Type.created_date = DateTime.Now.Date;
                family_RelationShip_Type.updated_date = DateTime.Now.Date;
                _context.Add(family_RelationShip_Type);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New Family relationship type is added.";
                return RedirectToAction(nameof(Index));
            }
            return View(family_RelationShip_Type);
        }

        // GET: Family_RelationShip_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Family_RelationShip_Types == null)
            {
                return NotFound();
            }

            var family_RelationShip_Type = await _context.Family_RelationShip_Types.FindAsync(id);
            if (family_RelationShip_Type == null)
            {
                return NotFound();
            }
            return View(family_RelationShip_Type);
        }

        // POST: Family_RelationShip_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Family_RelationShip_Type family_RelationShip_Type)
        {
            if (id != family_RelationShip_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    family_RelationShip_Type.updated_date = DateTime.Now.Date;
                    _context.Update(family_RelationShip_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Family_RelationShip_TypeExists(family_RelationShip_Type.id))
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
            return View(family_RelationShip_Type);
        }

        // GET: Family_RelationShip_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Family_RelationShip_Types == null)
            {
                return NotFound();
            }

            var family_RelationShip_Type = await _context.Family_RelationShip_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (family_RelationShip_Type == null)
            {
                return NotFound();
            }

            return View(family_RelationShip_Type);
        }

        // POST: Family_RelationShip_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Family_RelationShip_Types == null)
            {
                return Problem("Entity set 'employee_context.Family_RelationShip_Types'  is null.");
            }
            var family_RelationShip_Type = await _context.Family_RelationShip_Types.FindAsync(id);
            if (family_RelationShip_Type != null)
            {
                _context.Family_RelationShip_Types.Remove(family_RelationShip_Type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Family_RelationShip_TypeExists(int id)
        {
          return (_context.Family_RelationShip_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
