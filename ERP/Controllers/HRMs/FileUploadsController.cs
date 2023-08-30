
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.File_managment;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System;

namespace Erp.Controllers
{
    public class FileUploadsController : Controller
    {
        private readonly employee_context _context;
        private IWebHostEnvironment hostEnv;
        private readonly UserManager<User> _userManager;
        public FileUploadsController(employee_context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: FileUploads
        public async Task<IActionResult> Index()
        {
            return View(await _context.FileUploads.ToListAsync());
        }

        // GET: FileUploads/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileUpload = await _context.FileUploads
                .FirstOrDefaultAsync(m => m.id == id);
            if (fileUpload == null)
            {
                return NotFound();
            }

            return View(fileUpload);
        }

        // GET: FileUploads/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FileUploads/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FileUpload fileUpload)
        {
            User user = await _userManager.GetUserAsync(User);
            var emp = _context.Employees.FirstOrDefault(a => a.user_id == user.Id);
            if (fileUpload != null)
            {
                if (ModelState.IsValid)
                {
                    var filename = Path.GetFileName(fileUpload.name);
                    var fileExtension = Path.GetExtension(filename);
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    var objFiles = new FileUpload()
                    {

                        /*    Name = newFileName,
                            FileType =fileExtension,
                            CreateOn=DateTime.Now,
                            SenderEmpId= emp.Id,
                            RecevierEmpId=1,
    */
                    };

                    /*    using (var target = new MemoryStream())
                        {
                            fileUpload.CopyToAsync(target);
                            objFiles.DataFiles = target.ToArray();
                        }*/

                    _context.FileUploads.Add(objFiles);
                    _context.SaveChanges();

                }
            }

            return View(fileUpload);
        }

        // GET: FileUploads/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileUpload = await _context.FileUploads.FindAsync(id);
            if (fileUpload == null)
            {
                return NotFound();
            }
            return View(fileUpload);
        }

        // POST: FileUploads/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,FileType,DataFiles,CreateOn,SenderEmpId,RecevierEmpId")] FileUpload fileUpload)
        {
            if (id != fileUpload.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fileUpload);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileUploadExists(fileUpload.id))
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
            return View(fileUpload);
        }

        // GET: FileUploads/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fileUpload = await _context.FileUploads
                .FirstOrDefaultAsync(m => m.id == id);
            if (fileUpload == null)
            {
                return NotFound();
            }

            return View(fileUpload);
        }

        // POST: FileUploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fileUpload = await _context.FileUploads.FindAsync(id);
            _context.FileUploads.Remove(fileUpload);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileUploadExists(int id)
        {
            return _context.FileUploads.Any(e => e.id == id);
        }
    }
}
