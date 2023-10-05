using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using ERP.Models.HRMS.Employee_managments;
using X.PagedList;
using System.Drawing;
using Barcoder.Renderer.Image;
using Barcoder.Code128;
using System.Globalization;
using QRCoder;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ERP.Models.HRMS.Address;
using HRMS.Office;
using ERP.Interface;
using HRMS.Education_management;

namespace ERP.Controllers
{

    public class EmployeesController : Controller
    {
        private readonly employee_context _context;
        private readonly UserManager<User> _userManager;
        /*private readonly ICacheService _cacheService;*/
        public EmployeesController(employee_context context, UserManager<User> userManager/*,ICacheService cacheService*/)
        {
            _context = context;
            _userManager = userManager;
        /*    _cacheService = cacheService;*/
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchTerm, int? page)
        {
            var pageSize = 10;
            var pageNumber = page ?? 1;
            var users = _userManager.GetUserId(HttpContext.User);
            var employee_list = _context.Employees
                .Include(e => e.Employee_Office.Department)
                .Where(q => q.user_id != null);
          
            // Apply search term filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                employee_list = employee_list.Where(u =>
                    u.first_name.ToLower().Contains(searchTerm) ||
                    u.father_name.ToLower().Contains(searchTerm)
                );
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
            var users =  _userManager.GetUserId(HttpContext.User);

            var employee = await _context.Employees
                     .Include(e => e.Employee_Address.Region)
                     .Include(e => e.Employee_Address.Zone)
                     .Include(e => e.Employee_Address.Subcity)
                     .Include(e => e.Employee_Address.Woreda)
                     .Include(e => e.Employee_Contact)
                    .Include(e => e.Employee_Office.Division)
                   .Include(e => e.Employee_Office.Department)
                   .Include(e => e.Employee_Office.Team)
                   .Include(e => e.Employee_Office.Employement_Type)
                   .Include(e => e.Employee_Office.Position)
                    .Include(e => e.Marital_Status_Types)
                    .Include(e => e.Language)
                     .Include(e => e.Family_History)
                    .Include(e => e.Emergency_contact)
                .FirstOrDefaultAsync(a => a.user_id == users);
                
            if (employee == null)
            {
                return View();
            }
            else
            {
                ViewData["Employee"] = employee;
                return View();
            }
            
        }
        
        [HttpGet]
        [Route("Detail/{id}")]
        //Hr employee Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if(id != 0)
            {
                var employee = await _context.Employees
                      .Include(e => e.Employee_Address.Region)
                      .Include(e => e.Employee_Address.Zone)
                      .Include(e => e.Employee_Address.Subcity)
                      .Include(e => e.Employee_Address.Woreda)
                      .Include(e => e.Employee_Contact)
                      .Include(e => e.Employee_Office.Division)
                        .Include(e => e.Employee_Office.Department)
                        .Include(e => e.Employee_Office.Team)
                        .Include(e => e.Employee_Office.Position)
                        .Include(e => e.Employee_Office.Employement_Type)
                        .Include(e => e.Marital_Status_Types)
                         .Include(e => e.Emergency_contact)
                         .Include(e => e.Family_History)
                         .Include(e => e.Language)
                      .FirstOrDefaultAsync(e => e.id == id);
              
                if (employee == null)
                {
                    ViewData["Employee"] = null;
                    return View();
                }
                else
                {
                    if (employee.employee_code != null && employee.employee_code.Length >= 4)
                    {
                        ViewBag.IssuedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                        ViewBag.monthinamh = getEthiopianMonth();
                        ViewBag.IssuedYear = DateTime.Now.Year;
                        ViewBag.ExpairyYear = DateTime.Now.Year + 14;
                        ViewBag.QrCodeUri = GenerateQRCode(employee.employee_code);
                        ViewBag.BarCodeUri = GenerateBarCode(employee.employee_code);
                      
                    }

                    ViewData["Employee"] = employee;
                    return View();
                }
            }
            else
            {
                return NotFound();
            }

        }


        [HttpGet]

        public IEnumerable<Subcity> GetSubcitiesByRegionId(int regionId)
        {
            return _context.Subcitys
                .Where(s => s.region_id == regionId)
                .ToList();
        }

        [HttpGet]
        public IEnumerable<Zone> GetZoneByRegionId(int regionId)
        {
            return _context.Zones
                .Where(s => s.region_id == regionId)
                .ToList();
        }

        [HttpGet]
        public IEnumerable<Woreda> GetWoredaByZoneId(int zoneId)
        {
            return _context.Woredas
                .Where(s => s.zone_id == zoneId)
                .ToList();
        }

        [HttpGet]
        public IEnumerable<Woreda> GetWoredaBySubcityId(int subcityId)
        {
            return _context.Woredas
                .Where(s => s.subcity_id == subcityId)
                .ToList();
        }

        public IEnumerable<Department> GetDepartmentByDivisionId(int divisionId)
        {
            return _context.Departments
                .Where(s => s.division_id == divisionId)
                .ToList();
        }

        public IEnumerable<Team> GetTeamByDepartmentId(int departmentId)
        {
            return _context.Teams
                .Where(s => s.department_id == departmentId)
                .ToList();
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
                employee.start_date = Convert.ToDateTime(HttpContext.Request.Form["StartDate"]);
                employee.nationality = Convert.ToString(HttpContext.Request.Form["Nationality"]);
                employee.gender = Convert.ToString(HttpContext.Request.Form["Gender"]);
                employee.nation = Convert.ToString(HttpContext.Request.Form["Nation"]);
                employee.marital_status_type_id = Convert.ToInt32(HttpContext.Request.Form["maritalstatus"]);
                employee.pension_number = Convert.ToString(HttpContext.Request.Form["PensionNumber"]);
                employee.tin_number = Convert.ToString(HttpContext.Request.Form["TinNumber"]);
                employee.back_account_number = Convert.ToString(HttpContext.Request.Form["BankNumber"]);
                employee.place_of_work = Convert.ToString(HttpContext.Request.Form["PlaceofWork"]);
                employee.lastid = IDtracker();
                employee.profile_status = null;
                employee.work_status = null;
                var employeesid = _context.Employees.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();
                if (employeesid != 0)
                {
                    employeesid = employeesid + 1;
                }
                else
                {
                    employeesid = 1;
                }
                employee.id = employeesid;


                if (double.TryParse(HttpContext.Request.Form["Salary"], out double salary))
                {
                    employee.salary = salary;
                }
                else
                {
                    employee.salary = null;
                }

                employee.work_status = true;
                employee.profile_picture = UploadPicture(file);
                employee.user_id = user.Id;

                Employee_Address address = new Employee_Address();
              
;               address.region_id = Convert.ToInt32(HttpContext.Request.Form["Region"]);
                if (int.TryParse(HttpContext.Request.Form["Subcity"], out int subcity))
                {
                    address.subcity_id = subcity;
                }
                else
                {
                    address.subcity_id = null;
                }
                if (int.TryParse(HttpContext.Request.Form["Zone"], out int zone))
                {
                    address.zone_id = zone;
                }
                else
                {
                    address.zone_id = null;
                }

            


                if (double.TryParse(HttpContext.Request.Form["Salary"], out double Salary))
                {
                    employee.salary = Salary;
                }
                else
                {
                    employee.salary = null;
                }

                var addressid = _context.Employee_Addresss.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


                if (addressid != 0)
                {
                    addressid = addressid + 1;
                }
                else
                {
                    addressid = 1;
                }
                address.id = addressid;




                address.woreda_id = Convert.ToInt32(HttpContext.Request.Form["Woreda"]);
                address.kebele = Convert.ToString(HttpContext.Request.Form["kebele"]);
                address.primary_address = Convert.ToString(HttpContext.Request.Form["PrimaryAddress"]);
               

                Employee_Contact contact = new Employee_Contact();



                var contactid = _context.Employee_Contacts.OrderByDescending(l => l.Id).Select(l => l.Id).FirstOrDefault();


                if (contactid != 0)
                {
                    contactid = contactid + 1;
                }
                else
                {
                    contactid = 1;
                }
                contact.Id = contactid;



                contact.phonenumber = Convert.ToString(HttpContext.Request.Form["PhoneNumber"]);
                contact.alternative_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                contact.home_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                contact.internal_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                

                Employee_Office office = new Employee_Office();

                var officeid = _context.Employee_Offices.OrderByDescending(l => l.id).Select(l => l.id).FirstOrDefault();


                if (officeid != 0)
                {
                    officeid = officeid + 1;
                }
                else
                {
                    officeid = 1;
                }
                office.id = officeid;




                office.division_id = Convert.ToInt32(HttpContext.Request.Form["Division"]);
                office.department_id = Convert.ToInt32(HttpContext.Request.Form["Department"]);
                office.team_id = Convert.ToInt32(HttpContext.Request.Form["Team"]);
                office.position_id = Convert.ToInt32(HttpContext.Request.Form["Position"]);
                office.employment_type_id = Convert.ToInt32(HttpContext.Request.Form["EmploymentType"]);
                if (int.TryParse(HttpContext.Request.Form["OfficeNumber"], out int offices))
                {
                    office.office_number = offices;
                }
                else
                {
                    office.office_number = null;
                }

                _context.Add(employee);
                await _context.SaveChangesAsync(); 

                address.employee_id = employee.id;
                _context.Add(address);
                await _context.SaveChangesAsync();

                contact.employee_id = employee.id;
                _context.Add(contact);
                await _context.SaveChangesAsync();

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
                emp.start_date = Convert.ToDateTime(HttpContext.Request.Form["StartDate"]);
                emp.nationality = Convert.ToString(HttpContext.Request.Form["Nationality"]);
                emp.gender = Convert.ToString(HttpContext.Request.Form["Gender"]);
                emp.nation = Convert.ToString(HttpContext.Request.Form["Nation"]);
                emp.marital_status_type_id = Convert.ToInt32(HttpContext.Request.Form["maritalstatus"]);
                emp.pension_number = Convert.ToString(HttpContext.Request.Form["PensionNumber"]);
                emp.tin_number = Convert.ToString(HttpContext.Request.Form["TinNumber"]);
                emp.back_account_number = Convert.ToString(HttpContext.Request.Form["BankNumber"]);
                emp.place_of_work = Convert.ToString(HttpContext.Request.Form["PlaceofWork"]);
                emp.profile_status = null;

                if(file!= null)
                {
                    emp.profile_picture = UploadPicture(file);
                }
                if (double.TryParse(HttpContext.Request.Form["Salary"], out double salary))
                {
                    emp.salary = salary;
                }
                else
                {
                    emp.salary = null;
                }
                if (file != null)
                {
                    emp.profile_picture = UploadPicture(file);

                }
                if (Convert.ToInt32(HttpContext.Request.Form["Subcity"]) != 0)
                {
                    emp_address.subcity_id = Convert.ToInt32(HttpContext.Request.Form["Subcity"]);
                }

                if (Convert.ToInt32(HttpContext.Request.Form["Zone"]) != 0)
                {
                    emp_address.zone_id = Convert.ToInt32(HttpContext.Request.Form["Zone"]);
                }

                if (Convert.ToInt32(HttpContext.Request.Form["Woreda"]) != 0)
                {
                    emp_address.woreda_id = Convert.ToInt32(HttpContext.Request.Form["Woreda"]);
                }

                emp_address.kebele = Convert.ToString(HttpContext.Request.Form["kebele"]);
                emp_address.primary_address = Convert.ToString(HttpContext.Request.Form["PrimaryAddress"]);
               
                emp_contact.phonenumber = Convert.ToString(HttpContext.Request.Form["PhoneNumber"]);
                emp_contact.alternative_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                emp_contact.home_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);
                emp_contact.internal_phonenumber = Convert.ToString(HttpContext.Request.Form["AlternativePhoneNumber"]);

                emp_office.division_id = Convert.ToInt32(HttpContext.Request.Form["Division"]);
                emp_office.department_id = Convert.ToInt32(HttpContext.Request.Form["Department"]);
                emp_office.team_id = Convert.ToInt32(HttpContext.Request.Form["Team"]);
                emp_office.position_id = Convert.ToInt32(HttpContext.Request.Form["Position"]);
                emp_office.employment_type_id = Convert.ToInt32(HttpContext.Request.Form["EmploymentType"]);
                if (int.TryParse(HttpContext.Request.Form["OfficeNumber"], out int office))
                {
                    emp_office.office_number = office;
                }
                else
                {
                    emp_office.office_number = null;
                }

                _context.Update(emp);
                await _context.SaveChangesAsync();

                _context.Update(emp_address);
                await _context.SaveChangesAsync();

                _context.Update(emp_contact);
                await _context.SaveChangesAsync();

                _context.Update(emp_office);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Successfully Updated Employee.";
                return RedirectToAction(nameof(Create));
            }
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
                string syspath = @"/wwwroot/img/";
                string path = Path.Combine(Directory.GetCurrentDirectory() + syspath, fileName);
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

        // approve employee and set mols id
        public async Task<IActionResult> Approve(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if(employee != null)
            {
                if (employee.profile_status == true) {
                    TempData["Warning"] = "Employee Profile is Already Approved.";
                    return RedirectToAction(nameof(Detail), new { id = id });
                }
                else
                {
                    employee.profile_status = true;
                    employee.employee_code = GenerateEmployeeID();
                    employee.updated_date = DateTime.Now;
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Employee Profile is Approved.";
                    return RedirectToAction(nameof(Detail), new { id = id });
                }
            }
            else
            {
                TempData["Warning"] = "Employee not found.";
                return RedirectToAction(nameof(Detail), new { id = id });
            }
        }
        
        // reject employee and set feedback
        public async Task<IActionResult> RejectEmployee()
        {
            var id = Convert.ToInt32(HttpContext.Request.Form["RejectEmpId"]);
            var employee = await _context.Employees.FindAsync(id);

            if (employee != null)
            {
                if (employee.profile_status == false)
                {
                    TempData["Warning"] = "Employee Profile is Already Rejected.";
                    return RedirectToAction(nameof(Detail), new { id = id });
                }
                else
                {
                    employee.profile_status = false;
                    employee.feedback = Convert.ToString(HttpContext.Request.Form["EmpRejectMessage"]);
                    employee.updated_date = DateTime.Now;
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Employee Profile is Rejected.";
                    return RedirectToAction(nameof(Detail), new { id = id });
                }
            }
            else
            {
                TempData["Warning"] = "Employee not found.";
                return RedirectToAction(nameof(Detail), new { id = id });
            }
        }

        public static string GetDataURL(string imgFile)
        {
            var bytes = System.IO.File.ReadAllBytes(imgFile);
            var b64String = Convert.ToBase64String(bytes);
            var dataUrl = "data:image/png;base64," + b64String;

            return dataUrl;
        }

        public int IDtracker()
        {
            if (_context.Employees.Count() == 0)
            {
                return 0;
            }
            else
            {
                var employeelast = _context.Employees
                    .OrderByDescending(e => e.lastid)
                    .Select(e => e.lastid)
                    .FirstOrDefault();

                return employeelast ?? 0;
            }

        }

        public string getEthiopianMonth()
        {
            int gregorianMonth = DateTime.Now.Month;
            int ethiopianYear = (gregorianMonth < 9) ? DateTime.Now.Year - 8 : DateTime.Now.Year - 7;
            int ethiopianMonth = (gregorianMonth - 8 <= 0) ? gregorianMonth - 8 + 12 : gregorianMonth - 8;
            string[] monthNames = { "መስከረም", "ትቅምት", "ሂዳር", "ታህሳስ", "ጥር", "Yekatit", "የካቲት", "መጋቢት", "ግንቦት", "ሰኔ", "ሃምሌ", "ነሃሴ", "ጳጉሜ" };
           return monthNames[ethiopianMonth - 1];
        }
        
        public string GenerateEmployeeID()
        {

            var lastid = IDtracker();

            string idTracker = string.Format("{0:D4}", lastid + 1);
            string employeeID = $"Mols-{idTracker}-15";

            return employeeID;
        }

        public string GenerateQRCode(string employeeCode)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(employeeCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrBitmap = qrCode.GetGraphic(60);
            byte[] bitmapArray = qrBitmap.BitmapToByteArray();
            string qrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(bitmapArray));

            return qrUri;
        }
        public string GenerateBarCode(string employeeCode)
        {
            var barcode = Code128Encoder.Encode(employeeCode);
            var renderer = new ImageRenderer(new ImageRendererOptions { ImageFormat = Barcoder.Renderer.Image.ImageFormat.Png });
            var convobarcode = barcode.Content;

            using (var stream = new FileStream("C:\\systemfilestore\\Barcodegemerated\\" + employeeCode + ".png", FileMode.Create))
            {
                renderer.Render(barcode, stream);
            }

            string BarUri = GetDataURL("C:\\systemfilestore\\Barcodegemerated\\" + employeeCode + ".png");
            return BarUri;
        }
    }


    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}