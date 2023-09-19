using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Office;
using X.PagedList;

namespace ERP.Controllers
{
    public class TeamsController : Controller
    {
        private readonly employee_context _context;

        public TeamsController(employee_context context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;

            IQueryable<Team> all_teams = _context.Teams.Include(d => d.Department);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower(); // Convert the search term to lowercase

                all_teams = all_teams.Where(u =>
                    u.name.ToLower().Contains(searchTerm)
                );
            }

            var teams_count = await all_teams.CountAsync();
            var teams = await all_teams
                .OrderBy(u => u.name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var paged_teams = new StaticPagedList<Team>(teams, pageNumber, pageSize, teams_count);
            return View(paged_teams);
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewData["department_id"] = new SelectList(_context.Departments, "id", "name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,name,description,created_date,updated_date,department_id")] Team team)
        {
            if (ModelState.IsValid)
            {
                team.created_date = DateTime.Now;
                team.updated_date = DateTime.Now;
                _context.Add(team);
                await _context.SaveChangesAsync();
                TempData["Success"] = "You have created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["department_id"] = new SelectList(_context.Departments, "id", "name", team.department_id);
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            ViewData["department_id"] = new SelectList(_context.Departments, "id", "name", team.department_id);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,name,description,created_date,updated_date,department_id")] Team team)
        {
            if (id != team.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    team.updated_date= DateTime.Now;
                    _context.Update(team);                   
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "You have Updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.id))
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
            ViewData["department_id"] = new SelectList(_context.Departments, "id", "name", team.department_id);
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teams == null)
            {
                return Problem("Entity set 'employee_context.Teams'  is null.");
            }
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }
            
            await _context.SaveChangesAsync();
            TempData["Success"] = "You have deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
