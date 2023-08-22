﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Types;

namespace ERP.Controllers
{
    public class Marital_Status_TypesController : Controller
    {
        private readonly employee_context _context;

        public Marital_Status_TypesController(employee_context context)
        {
            _context = context;
        }

        // GET: Marital_Status_Types
        public async Task<IActionResult> Index()
        {
              return _context.Marital_Status_Types != null ? 
                          View(await _context.Marital_Status_Types.ToListAsync()) :
                          Problem("Entity set 'employee_context.Marital_Status_Types'  is null.");
        }

        // GET: Marital_Status_Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Marital_Status_Types == null)
            {
                return NotFound();
            }

            var marital_Status_Types = await _context.Marital_Status_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (marital_Status_Types == null)
            {
                return NotFound();
            }

            return View(marital_Status_Types);
        }

        // GET: Marital_Status_Types/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Marital_Status_Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Marital_Status_Types marital_Status_Types)
        {
            if (ModelState.IsValid)
            {
                _context.Add(marital_Status_Types);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(marital_Status_Types);
        }

        // GET: Marital_Status_Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Marital_Status_Types == null)
            {
                return NotFound();
            }

            var marital_Status_Types = await _context.Marital_Status_Types.FindAsync(id);
            if (marital_Status_Types == null)
            {
                return NotFound();
            }
            return View(marital_Status_Types);
        }

        // POST: Marital_Status_Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Marital_Status_Types marital_Status_Types)
        {
            if (id != marital_Status_Types.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marital_Status_Types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Marital_Status_TypesExists(marital_Status_Types.id))
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
            return View(marital_Status_Types);
        }

        // GET: Marital_Status_Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Marital_Status_Types == null)
            {
                return NotFound();
            }

            var marital_Status_Types = await _context.Marital_Status_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (marital_Status_Types == null)
            {
                return NotFound();
            }

            return View(marital_Status_Types);
        }

        // POST: Marital_Status_Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Marital_Status_Types == null)
            {
                return Problem("Entity set 'employee_context.Marital_Status_Types'  is null.");
            }
            var marital_Status_Types = await _context.Marital_Status_Types.FindAsync(id);
            if (marital_Status_Types != null)
            {
                _context.Marital_Status_Types.Remove(marital_Status_Types);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Marital_Status_TypesExists(int id)
        {
          return (_context.Marital_Status_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}