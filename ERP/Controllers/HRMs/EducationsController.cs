
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using HRMS.Education_management;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using ERP.Models.HRMS.File_managment;
using ERP.Models.HRMS.Employee_managments;
using X.PagedList;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace ERP.Controllers.HRMs
{
    public class EducationsController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;
        private readonly IFileProvider fileProvider;
        public EducationsController(employee_context context, UserManager<User> userManager, IFileProvider fileProvider)
        {
            _context = context;
            _userManager = userManager;
            this.fileProvider = fileProvider;
        }

        // GET: Educations
        public async Task<IActionResult> Index()
        {
            var employee_context = _context.Educations.Include(e => e.Education_Level_Type).Include(e => e.Education_Program_Type).Include(e => e.Employee);
            return View(await employee_context.ToListAsync());
        }
        public async Task<IActionResult> Index_Personal()
        {
            User user = await _userManager.GetUserAsync(User);
            var check_employee = _context.Employees.FirstOrDefault(a => a.user_id == user.Id);

            if(check_employee != null)
            {
                var assign_education = _context.Educations.Where(e => e.employee_id == check_employee.id).Include(e => e.Employee).Include(e => e.Education_Level_Type).Include(e=>e.Education_Program_Type);
                return View(assign_education);
            }
            else
            {
                TempData["Warning"] = "No Employee Info";
                return View();
            }

           
        }
        // GET: Educations/Details/5
        // shows file uploaded by specific user
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var check_user = _context.Educations.Include(e => e.Employee.User).FirstOrDefault(e => e.id == id);
            var identify = _context.FileUploads.Where(e => e.Identificationnumbers == check_user.Identificationnumber).ToList().Count();

            if (identify == 0)
            {
                return NotFound();
            }

            if (identify == 1)
            {
                var filepath = _context.FileUploads.FirstOrDefault(e => e.Identificationnumbers == check_user.Identificationnumber);
                string fileEntries = Path.GetFileName(filepath.name);
                ViewBag.fileEntriess = fileEntries;
                ViewBag.identify = identify;
            }
            else
            {
                var filepath = _context.FileUploads.Where(e => e.Identificationnumbers == check_user.Identificationnumber).ToList();
                List<string> file_upload = new List<string>();
                foreach (var cons in filepath)
                {

                    string fileEntries = Path.GetFileName(cons.name);
                    file_upload.Add(fileEntries);

                }

                ViewBag.fileEntriess = file_upload;
                ViewBag.identify = identify;
            }

            var education = await _context.Educations.Include(e => e.Employee)
              .Include(e => e.Employee.User)
              .Include(a=> a.Education_Level_Type)
              .Include(a => a.Education_Program_Type)

              .FirstOrDefaultAsync(m => m.id == id);


            if (education == null)
            {
                return NotFound();
            }

            return View(education);

        }

        /* public async Task<IActionResult> Details(int id)
         {
             var education = await _context.Educations.FindAsync(id);

             if (education == null)
             {
                 return NotFound();
             }
             int currentPage = 1;

             // Pass the currentPage variable to the view
             ViewData["CurrentPage"] = currentPage;
             education.EducationDocumentPaths = education.EducationDocumentPathsField?
                 .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)?
                 .Select(path => path.Trim())
                 .Where(path => !string.IsNullOrEmpty(path))
                 .ToArray();

             return View(education);


         }
 */
        // download file uploaded 
        public async Task<IActionResult> Download(string filename)
        {
            string syspath = @"C:\systemfilestore";

            if (filename == null)
            {

                return Content("filename not present");
            }

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                            syspath, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
          {
  
               {".pdf", "application/pdf"},
      
          };
        }

        // GET: Educations/Create
        public IActionResult Create()
        {

            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name");
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name");
            
            return View();
        }

        // POST: Educations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,institution_name,institution_email,institution_website,filed_of_study,description,start_date,end_date,Identificationnumber,status,filestatus,feedback,created_date,updated_date,employee_id,educational_program_id,educational_level_type_id")] Education education, FileUpload FileUpload, List<IFormFile> FormFile)
        {

            User users = await _userManager.GetUserAsync(User);
            var check_employee = _context.Employees.FirstOrDefault(e => e.user_id == users.Id);
            long file_size = FormFile.Sum(f => f.Length);
            string syspath = @"c:\systemfilestore";
            var randomGenerator = new Random();
            var random_max = randomGenerator.Next(1, 1000000000);
            education.created_date = DateTime.Now;
            education.updated_date = DateTime.Now;
            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name", education.educational_level_type_id);
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name", education.educational_program_id);

            if (check_employee != null)
            {
                education.status = null;
                if (FormFile == null)
                {
                    education.filestatus = false;
                    education.employee_id = check_employee.id;
                    _context.Add(education);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    if (file_size < 524288000)
                    {
                        education.filestatus = true;
                        education.employee_id = check_employee.id;
                        education.Identificationnumber = random_max;
                        _context.Add(education);
                        await _context.SaveChangesAsync();

                        foreach (var formFile in FormFile)
                        {

                            if (formFile.Length > 0)
                            {

                                var filePath = Path.Combine(
                                  Directory.GetCurrentDirectory(), syspath, formFile.FileName);
                                var files = new FileUpload
                                {
                                    id = 0,
                                    created_at = DateTime.Now.Date,
                                    name = filePath,
                                    Identificationnumbers = random_max
                                };

                                _context.Add(files);
                                await _context.SaveChangesAsync();

                                using (var stream = System.IO.File.Create(filePath))
                                {
                                    await formFile.CopyToAsync(stream);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                TempData["Warning"] = "Fill in your information first";
                return View();
            }




            return RedirectToAction(nameof(Index));
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var ef = await _context.Educations.FindAsync(id);

            if (ef == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    ef.status = true;
                    ef.feedback = Convert.ToString(HttpContext.Request.Form["feedback"]);
                    _context.Update(ef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationExists(ef.id))
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

        }

        public async Task<IActionResult> Reject(int id)
        {
            var ef = await _context.Educations.FindAsync(id);

            if (ef == null)
            {
                return NotFound();
            }
            else
            {
                try
                {
                    ef.status = false;
                    ef.feedback = Convert.ToString(HttpContext.Request.Form["feedback"]);
                    _context.Update(ef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationExists(ef.id))
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
        }

        // GET: Educations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Educations == null)
            {
                return NotFound();
            }

            var education = await _context.Educations.FindAsync(id);
            if (education == null)
            {
                return NotFound();
            }
            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name", education.educational_level_type_id);
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name", education.educational_program_id);
           
            return View(education);
        }

        // POST: Educations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,institution_name,institution_email,institution_website,filed_of_study,description,start_date,end_date,Identificationnumber,status,filestatus,feedback,created_date,updated_date,employee_id,educational_program_id,educational_level_type_id")] Education education)
        {
            if (id != education.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(education);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EducationExists(education.id))
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
            ViewData["educational_level_type_id"] = new SelectList(_context.Education_Level_Types, "id", "name", education.educational_level_type_id);
            ViewData["educational_program_id"] = new SelectList(_context.Education_Program_Types, "id", "name", education.educational_program_id);
            ViewData["employee_id"] = new SelectList(_context.Employees, "id", "back_account_number", education.employee_id);
            return View(education);
        }

        // GET: Educations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Educations == null)
            {
                return NotFound();
            }

            var education = await _context.Educations
                .Include(e => e.Education_Level_Type)
                .Include(e => e.Education_Program_Type)
                .Include(e => e.Employee)
                .FirstOrDefaultAsync(m => m.id == id);
            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        // POST: Educations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Educations == null)
            {
                return Problem("Entity set 'employee_context.Educations'  is null.");
            }
            var education = await _context.Educations.FindAsync(id);
            if (education != null)
            {
                _context.Educations.Remove(education);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EducationExists(int id)
        {
          return (_context.Educations?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
