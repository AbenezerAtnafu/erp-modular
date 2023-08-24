﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using ERP.Models.HRMS.Employee_managments;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using X.PagedList;

namespace ERP.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;

        public EmployeesController(employee_context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchTerm, int? page, string sortOrder, DateTime? birthdateFilter, string maritalStatusFilter)
        {
            var users = _userManager.GetUserId(HttpContext.User);
            var employee_list = _context.Employees.Where(q => q.user_id != users);
            var pageSize = 10;
            var pageNumber = page ?? 1;

            // Apply search term filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                employee_list = employee_list.Where(u =>
                    u.first_name.ToLower().Contains(searchTerm) ||
                    u.father_name.ToLower().Contains(searchTerm)
                );
            }

            // Apply birthdate filter
            if (birthdateFilter.HasValue)
            {
                employee_list = employee_list.Where(u => u.date_of_birth == birthdateFilter.Value);
            }

            // Apply marital status filter
            if (!string.IsNullOrEmpty(maritalStatusFilter))
            {
                employee_list = employee_list.Where(u => u.Marital_Status_Types.name == maritalStatusFilter);
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "asc":
                    employee_list = employee_list.OrderBy(u => u.first_name);
                    break;
                case "desc":
                    employee_list = employee_list.OrderByDescending(u => u.first_name);
                    break;
                default:
                    employee_list = employee_list.OrderBy(u => u.first_name);
                    break;
            }

            var totalCount = await employee_list.CountAsync();
            var employee_list_final = await employee_list
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedEmployees = new StaticPagedList<Employee>(employee_list_final, pageNumber, pageSize, totalCount);

            return View(pagedEmployees);
        
    }

        // GET: Employees
        public async Task<IActionResult> Profile()
        {
            var users = _userManager.GetUserId(HttpContext.User);
            var employee = _context.Employees.
                Include(e => e.Employee_Address.Region)
                .Include(e => e.Employee_Address.Zone)
                .Include(e => e.Employee_Address.Subcity)
                .Include(e => e.Employee_Address.Woreda)
                .Include(e => e.Employee_Office.Division)
                .Include(e => e.Employee_Office.Department)
                .Include(e => e.Employee_Office.Team)
                .Include(e => e.Employee_Office.Employement_Type)
                .Include(e => e.Employee_Office.Position)
                .Include(e => e.Marital_Status_Types)
                .FirstOrDefault(a => a.user_id == users);
            if (employee == null)
            {
                return View();
            }
            else
            {
                return View(employee);
            }
        }


        // GET: Employees/Create
        public IActionResult Create()
        {
            var users = _userManager.GetUserId(HttpContext.User);
            var emp = _context.Employees.
                Include(e => e.Employee_Address.Region)
                .Include(e => e.Employee_Address.Zone)
                .Include(e => e.Employee_Address.Subcity)
                .Include(e => e.Employee_Address.Woreda)
                .Include(e => e.Employee_Office.Division)
                .Include(e => e.Employee_Office.Department)
                .Include(e => e.Employee_Office.Team)
                .Include(e => e.Employee_Office.Employement_Type)
                .Include(e => e.Employee_Office.Position)
                .Include(e => e.Marital_Status_Types)
                .FirstOrDefault(a => a.user_id == users);

            if (emp != null)
            {
                ViewData["Employee"] = emp;
                ViewData["Employee_Address"] = _context.Employee_Addresss.FirstOrDefault(a => a.employee_id == emp.id);
                ViewData["Employee_Contact"] = _context.Employee_Contacts.FirstOrDefault(a => a.employee_id == emp.id);
                ViewData["Employee_Office"] = _context.Employee_Offices.FirstOrDefault(a => a.employee_id == emp.id);
            }


            ViewBag.Region = new SelectList(_context.Regions, "id", "name");
            ViewBag.zone = new SelectList(_context.Zones, "id", "name");
            ViewBag.subcity = new SelectList(_context.Subcitys, "id", "name");
            ViewBag.Woreda = new SelectList(_context.Woredas, "id", "name");
            ViewBag.Division = new SelectList(_context.Divisions, "id", "name");
            ViewBag.Department = new SelectList(_context.Departments, "id", "name");
            ViewBag.Team = new SelectList(_context.Teams, "id", "name");
            ViewBag.EmpType = new SelectList(_context.Employement_Types, "id", "name");
            ViewBag.Position = new SelectList(_context.Position, "id", "name");
            ViewBag.MaritalStatus = new SelectList(_context.Marital_Status_Types, "id", "name");

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateorUpdate(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);

            var emp = _context.Employees.FirstOrDefault(e => e.user_id == user.Id);

            if (emp == null)
            {
                Employee employee = new Employee();

                employee.first_name = Convert.ToString(HttpContext.Request.Form["FirstName"]);
                employee.father_name = Convert.ToString(HttpContext.Request.Form["FatherName"]);
                employee.grand_father_name = Convert.ToString(HttpContext.Request.Form["GrandFatherName"]);
                employee.first_name_am = Convert.ToString(HttpContext.Request.Form["FirstNameAm"]);
                employee.father_name_am = Convert.ToString(HttpContext.Request.Form["FatherNameAm"]);
                employee.grand_father_name_am = Convert.ToString(HttpContext.Request.Form["GrandFatherNameAm"]);
                employee.place_of_birth = Convert.ToString(HttpContext.Request.Form["PlaceofBirth"]);
                employee.date_of_birth = Convert.ToDateTime(HttpContext.Request.Form["DateofBirth"]);
                employee.nationality = Convert.ToString(HttpContext.Request.Form["Nationality"]);
                employee.gender = Convert.ToString(HttpContext.Request.Form["Gender"]);
                employee.religion = Convert.ToString(HttpContext.Request.Form["Religion"]);
                employee.marital_status_type_id = Convert.ToInt32(HttpContext.Request.Form["maritalstatus"]);
                employee.pension_number = Convert.ToString(HttpContext.Request.Form["PensionNumber"]);
                employee.tin_number = Convert.ToString(HttpContext.Request.Form["TinNumber"]);
                employee.back_account_number = Convert.ToString(HttpContext.Request.Form["BankNumber"]);
                employee.place_of_work = Convert.ToString(HttpContext.Request.Form["PlaceofWork"]);
                employee.profile_picture = UploadPicture(file);
                employee.user_id = user.Id;
                _context.Add(employee);
                await _context.SaveChangesAsync();


                Employee_Address address = new Employee_Address();

                address.region_id = Convert.ToInt32(HttpContext.Request.Form["Region"]);
                address.zone_id = Convert.ToInt32(HttpContext.Request.Form["Zone"]);
                address.subcity_id = Convert.ToInt32(HttpContext.Request.Form["Subcity"]);
                address.woreda_id = Convert.ToInt32(HttpContext.Request.Form["Woreda"]);
                address.kebele = Convert.ToString(HttpContext.Request.Form["kebele"]);
                address.primary_address = Convert.ToString(HttpContext.Request.Form["PrimaryAddress"]);
                address.employee_id = employee.id;
                _context.Add(address);
                await _context.SaveChangesAsync();


                Employee_Contact contact = new Employee_Contact();

                contact.phonenumber = Convert.ToString(HttpContext.Request.Form["PhoneNumber"]);
                contact.alternative_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                contact.home_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                contact.internal_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                contact.employee_id = employee.id;
                _context.Add(contact);
                await _context.SaveChangesAsync();


                Employee_Office office = new Employee_Office();

                office.division_id = Convert.ToInt32(HttpContext.Request.Form["Division"]);
                office.department_id = Convert.ToInt32(HttpContext.Request.Form["Department"]);
                office.team_id = Convert.ToInt32(HttpContext.Request.Form["Team"]);
                office.position_id = Convert.ToInt32(HttpContext.Request.Form["Position"]);
                office.employment_type_id = Convert.ToInt32(HttpContext.Request.Form["EmploymentType"]);
                /* office.office_number = Convert.ToInt32(HttpContext.Request.Form["OfficeNumber"]);*/
                office.employee_id = employee.id;
                _context.Add(office);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Successfully created new Employee.";
                return RedirectToAction(nameof(Create));
            }
            else
            {
                var emp_address = _context.Employee_Addresss.FirstOrDefault(e => e.employee_id == emp.id);
                var emp_contact = _context.Employee_Contacts.FirstOrDefault(e => e.employee_id == emp.id);
                var emp_office = _context.Employee_Offices.FirstOrDefault(e => e.employee_id == emp.id);

                emp.first_name = Convert.ToString(HttpContext.Request.Form["FirstName"]);
                emp.father_name = Convert.ToString(HttpContext.Request.Form["FatherName"]);
                emp.grand_father_name = Convert.ToString(HttpContext.Request.Form["GrandFatherName"]);
                emp.place_of_birth = Convert.ToString(HttpContext.Request.Form["PlaceofBirth"]);
                emp.date_of_birth = Convert.ToDateTime(HttpContext.Request.Form["DateofBirth"]);
                emp.nationality = Convert.ToString(HttpContext.Request.Form["Nationality"]);
                emp.gender = Convert.ToString(HttpContext.Request.Form["Gender"]);
                emp.religion = Convert.ToString(HttpContext.Request.Form["Religion"]);
                emp.marital_status_type_id = Convert.ToInt32(HttpContext.Request.Form["maritalstatus"]);
                emp.pension_number = Convert.ToString(HttpContext.Request.Form["PensionNumber"]);
                emp.tin_number = Convert.ToString(HttpContext.Request.Form["TinNumber"]);
                emp.back_account_number = Convert.ToString(HttpContext.Request.Form["BankNumber"]);
                emp.place_of_work = Convert.ToString(HttpContext.Request.Form["PlaceofWork"]);
                _context.Update(emp);
                await _context.SaveChangesAsync();


                emp_address.region_id = Convert.ToInt32(HttpContext.Request.Form["Region"]);
                emp_address.zone_id = Convert.ToInt32(HttpContext.Request.Form["Zone"]);
                emp_address.subcity_id = Convert.ToInt32(HttpContext.Request.Form["Subcity"]);
                emp_address.woreda_id = Convert.ToInt32(HttpContext.Request.Form["Woreda"]);
                emp_address.kebele = Convert.ToString(HttpContext.Request.Form["kebele"]);
                emp_address.primary_address = Convert.ToString(HttpContext.Request.Form["PrimaryAddress"]);
                _context.Update(emp_address);
                await _context.SaveChangesAsync();


                emp_contact.phonenumber = Convert.ToString(HttpContext.Request.Form["PhoneNumber"]);
                emp_contact.alternative_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                emp_contact.home_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                emp_contact.internal_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                _context.Update(emp_contact);
                await _context.SaveChangesAsync();


                emp_office.division_id = Convert.ToInt32(HttpContext.Request.Form["Division"]);
                emp_office.department_id = Convert.ToInt32(HttpContext.Request.Form["Department"]);
                emp_office.team_id = Convert.ToInt32(HttpContext.Request.Form["Team"]);
                emp_office.position_id = Convert.ToInt32(HttpContext.Request.Form["Position"]);
                emp_office.employment_type_id = Convert.ToInt32(HttpContext.Request.Form["EmploymentType"]);
                /*emp_office.office_number = Convert.ToInt32(HttpContext.Request.Form["OfficeNumber"]);*/
                _context.Update(emp_office);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Successfully Updated Employee.";
                return RedirectToAction(nameof(Create));
            }


        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,profile_picture,first_name,first_name_am,father_name,father_name_am,grand_father_name,grand_father_name_am,gender,date_of_birth,nationality,nation,place_of_birth,religion,back_account_number,tin_number,pension_number,place_of_work,marital_status_type_id,user_id")] Employee employee)
        {
            if (id != employee.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.id))
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
            ViewData["marital_status_type_id"] = new SelectList(_context.Marital_Status_Types, "id", "id", employee.marital_status_type_id);
            ViewData["user_id"] = new SelectList(_context.Users, "Id", "Id", employee.user_id);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Marital_Status_Types)
                .Include(e => e.User)
                .FirstOrDefaultAsync(m => m.id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'employee_context.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.id == id)).GetValueOrDefault();
        }


        //upload picture and return path
        public String UploadPicture(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Generate a unique file name
                string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

                // Save the file to a specific directory on the server
                string syspath = @"C:\systemfilestore";
                string path = Path.Combine(syspath, fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Optionally, you can perform additional tasks such as resizing the image, creating thumbnails, etc.

                return fileName;

            }
            else
            {
                return "file is empty";
            }
        }

    }
}
