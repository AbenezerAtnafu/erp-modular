﻿
using ERP.HRMS.Employee_managment;
using ERP.Models.HRMS.Address;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ERP.Models.HRMS.Employee_managments
{
    public class Employee_Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey("Region")]
        public int region_id { get; set; }
        [ForeignKey("Zone")]
        public int? zone_id { get; set; }
        [ForeignKey("Subcity")]
        public int? subcity_id { get; set; }
        [ForeignKey("Woreda")]
        public int? woreda_id { get; set; }
        public string? kebele{ get; set; }
        public string? primary_address { get; set; }
        public Region Region { get; set; } 
        public Zone Zone { get; set; }
        public Subcity Subcity { get; set; }
        public Woreda Woreda { get; set; }

        [ForeignKey("Employees")]
        public int employee_id { get; set; }
        public Employee Employees { get; set; }

    }
}
