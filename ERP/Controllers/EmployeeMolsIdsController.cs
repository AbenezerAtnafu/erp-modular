using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ERP.Areas.Identity.Data;
using ERP.Models.HRMS.Employee_id;
using Barcoder.Code128;
using Barcoder.Renderer.Image;
using QRCoder;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace ERP.Controllers
{
    public class EmployeeMolsIdsController : Controller
    {
        private readonly employee_context _context;

        public EmployeeMolsIdsController(employee_context context)
        {
            _context = context;
        }

        public int IDtracker()
        {
            if (_context.employeeMolsIds.Count() == 0)
            {
                return 0;
            }
            else
            {
                var employeelast = _context.employeeMolsIds.OrderByDescending(e => e.id_tracker).FirstOrDefault()?.id_tracker ?? 0;
                return employeelast;
            }
        }
        public string GenerateEmployeeID(bool isApproved)
        {
            if (!isApproved)
            {
                return string.Empty;
            }

            int lastID = IDtracker();

            string idTracker = (lastID + 1).ToString("D4");
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
        public static string GetDataURL(string imgFile)
        {
            var bytes = System.IO.File.ReadAllBytes(imgFile);
            var b64String = Convert.ToBase64String(bytes);
            var dataUrl = "data:image/png;base64," + b64String;

            return dataUrl;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateId(int id)
        {
            var emp = await _context.Employees
                        .Include(e => e.User)
                        .Include(e => e.Employee_Address)
                        .Include(e => e.Employee_Office)
                        .Include(e => e.Employee_Contact)
                        .FirstOrDefaultAsync(e => e.id == id);

            var employee_code = _context.employeeMolsIds.FirstOrDefault(q => q.employee_id == emp.id);
            var empcontact = _context.Employee_Contacts.FirstOrDefault(q => q.employee_id == emp.id);
            var empaddress = _context.Employee_Addresss.FirstOrDefault(q => q.employee_id == emp.id);
            var empoffice = _context.Employee_Offices.FirstOrDefault(q => q.employee_id == emp.id);

            if (emp == null)
            {
                TempData["Warning"] = "Employee Not found";
                return View();
            }

            if (emp.profile_picture == null)
            {
                TempData["Warning"] = "Employee Photo Not found";
                return View();
            }

            var qrCodeUri = GenerateQRCode(employee_code.employee_code);
            var barCodeUri = GenerateBarCode(employee_code.employee_code);

            int gregorianMonth = DateTime.Now.Month;
            int ethiopianYear = (gregorianMonth < 9) ? DateTime.Now.Year - 8 : DateTime.Now.Year - 7;
            int ethiopianMonth = (gregorianMonth - 8 <= 0) ? gregorianMonth - 8 + 12 : gregorianMonth - 8;
            string[] monthNames = { "መስከረም", "ትቅምት", "ሂዳር", "ታህሳስ", "ጥር", "Yekatit", "የካቲት", "መጋቢት", "ግንቦት", "ሰኔ", "ሃምሌ", "ነሃሴ", "ጳጉሜ" };
            string monthinamh = monthNames[ethiopianMonth - 1];

            string sex_amh_after = (emp.gender == "Male") ? "ወንድ" : "ሴት";

            ViewBag.QrCodeUri = qrCodeUri;
            ViewBag.BarCodeUri = barCodeUri;
            ViewBag.profile = emp.profile_picture;
            ViewBag.id = employee_code.employee_code;
            ViewBag.name = emp.first_name + " " + emp.father_name + " " + emp.grand_father_name;
            ViewBag.nameam = emp.first_name_am + " " + emp.father_name_am + " " + emp.grand_father_name_am;
            ViewBag.PhoneNumber = empcontact.phonenumber;
            ViewBag.Sex = emp.gender;
            ViewBag.sex_amh = sex_amh_after;
            ViewBag.IssuedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
            ViewBag.monthinamh = monthinamh;
            ViewBag.IssuedYear = DateTime.Now.Year;
            ViewBag.ExpairyYear = DateTime.Now.Year + 14;
            ViewBag.EmployeeID = employee_code.employee_code;
            ViewBag.Region = empaddress.Region;
            ViewBag.SubCity = empaddress.subcity_id;
            ViewBag.Woreda = empaddress.woreda_id;
            ViewBag.Position = empoffice.Position.name;

            employee_code.is_generated = true;
            employee_code.updated_date = DateTime.Now.Date;
            employee_code.created_date = DateTime.Now.Date;

            return View();
        }




        [HttpPost]
        public async Task<IActionResult> Generate_Id(int id)
        {
            try
            {
                var emp = await _context.Employees
                            .Include(e => e.User)
                            .Include(e => e.Employee_Address)
                            .Include(e => e.Employee_Office)
                            .Include(e => e.Employee_Contact)
                            .FirstOrDefaultAsync(e => e.id == id);

                if (emp == null)
                {
                    TempData["Warning"] = "Employee Not found";
                    return View();
                }

                if (emp.profile_picture == null)
                {
                    TempData["Warning"] = "Employee Photo Not found";
                    return View();
                }

                var employeeCode = _context.employeeMolsIds.FirstOrDefault(q => q.employee_id == emp.id);
                var empcontact = _context.Employee_Contacts.FirstOrDefault(q => q.employee_id == emp.id);
                var empaddress = _context.Employee_Addresss.FirstOrDefault(q => q.employee_id == emp.id);
                var empoffice = _context.Employee_Offices.FirstOrDefault(q => q.employee_id == emp.id);

                ViewBag.QrCodeUri = GenerateQRCode(employeeCode.employee_code);
                ViewBag.BarCodeUri = GenerateBarCode(employeeCode.employee_code);

                int gregorianMonth = DateTime.Now.Month;
                int ethiopianYear = (gregorianMonth < 9) ? DateTime.Now.Year - 8 : DateTime.Now.Year - 7;
                int ethiopianMonth = (gregorianMonth - 8 <= 0) ? gregorianMonth - 8 + 12 : gregorianMonth - 8;
                string[] monthNames = { "መስከረም", "ትቅምት", "ሂዳር", "ታህሳስ", "ጥር", "Yekatit", "የካቲት", "መጋቢት", "ግንቦት", "ሰኔ", "ሃምሌ", "ነሃሴ", "ጳጉሜ" };
                string monthinamh = monthNames[ethiopianMonth - 1];

                string sex_amh_after = (emp.gender == "Male") ? "ወንድ" : "ሴት";

                ViewBag.profile = emp.profile_picture;
                ViewBag.id = employeeCode.employee_code;
                ViewBag.name = $"{emp.first_name} {emp.father_name} {emp.grand_father_name}";
                ViewBag.nameam = $"{emp.first_name_am} {emp.father_name_am} {emp.grand_father_name_am}";
                ViewBag.PhoneNumber = empcontact.phonenumber;
                ViewBag.Sex = emp.gender;
                ViewBag.sex_amh = sex_amh_after;
                ViewBag.IssuedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                ViewBag.monthinamh = monthinamh;
                ViewBag.IssuedYear = DateTime.Now.Year;
                ViewBag.ExpairyYear = DateTime.Now.Year + 14;
                ViewBag.EmployeeID = employeeCode.employee_code;
                ViewBag.Region = empaddress.Region;
                ViewBag.SubCity = empaddress.subcity_id;
                ViewBag.Woreda = empaddress.woreda_id;
                ViewBag.Position = empoffice.Position.name;

                employeeCode.is_generated = true;
                employeeCode.updated_date = DateTime.Now.Date;
                employeeCode.created_date = DateTime.Now.Date;

                await _context.SaveChangesAsync();

                return View(employeeCode);
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                TempData["Error"] = "An error occurred. Please try again.";
                return View();
            }
        }
    }
  /*  public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }*/
}
