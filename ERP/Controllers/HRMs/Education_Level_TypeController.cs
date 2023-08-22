﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Types;

namespace ERP.Models.HRMS.Types
{
    public class Education_Level_TypeController : Controller
    {
        private readonly employee_context _context;

        public Education_Level_TypeController(employee_context context)
        {
            _context = context;
        }

        // GET: Education_Level_Type
        public async Task<IActionResult> Index()
        {
              return _context.Education_Level_Types != null ? 
                          View(await _context.Education_Level_Types.ToListAsync()) :
                          Problem("Entity set 'employee_context.Education_Level_Types'  is null.");
        }

        // GET: Education_Level_Type/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Education_Level_Types == null)
            {
                return NotFound();
            }

            var education_Level_Type = await _context.Education_Level_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (education_Level_Type == null)
            {
                return NotFound();
            }

            return View(education_Level_Type);
        }

        // GET: Education_Level_Type/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Education_Level_Type/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date")] Education_Level_Type education_Level_Type)
        {
            if (ModelState.IsValid)
            {
                _context.Add(education_Level_Type);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(education_Level_Type);
        }

        // GET: Education_Level_Type/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Education_Level_Types == null)
            {
                return NotFound();
            }

            var education_Level_Type = await _context.Education_Level_Types.FindAsync(id);
            if (education_Level_Type == null)
            {
                return NotFound();
            }
            return View(education_Level_Type);
        }

        // POST: Education_Level_Type/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date")] Education_Level_Type education_Level_Type)
        {
            if (id != education_Level_Type.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(education_Level_Type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Education_Level_TypeExists(education_Level_Type.id))
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
            return View(education_Level_Type);
        }

        // GET: Education_Level_Type/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Education_Level_Types == null)
            {
                return NotFound();
            }

            var education_Level_Type = await _context.Education_Level_Types
                .FirstOrDefaultAsync(m => m.id == id);
            if (education_Level_Type == null)
            {
                return NotFound();
            }

            return View(education_Level_Type);
        }

        // POST: Education_Level_Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Education_Level_Types == null)
            {
                return Problem("Entity set 'employee_context.Education_Level_Types'  is null.");
            }
            var education_Level_Type = await _context.Education_Level_Types.FindAsync(id);
            if (education_Level_Type != null)
            {
                _context.Education_Level_Types.Remove(education_Level_Type);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Education_Level_TypeExists(int id)
        {
          return (_context.Education_Level_Types?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}