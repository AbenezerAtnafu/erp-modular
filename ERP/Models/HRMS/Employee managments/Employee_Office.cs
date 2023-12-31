﻿using HRMS.Office;
using HRMS.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Employee_managments
{
    public class Employee_Office
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [ForeignKey("Division")]
        public int division_id { get; set; }
        [ForeignKey("Department")]
        public int department_id { get; set; }
        [ForeignKey("Team")]
        public int team_id { get; set; }
        [ForeignKey("Position")]
        public int position_id { get; set; }
        [ForeignKey("Employement_Type")]
        public int employment_type_id { get; set; }
        public int? office_number { get; set; }
     
        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }


        [ForeignKey("Employees")]
        public int employee_id { get; set; }
        public Employee Employees { get; set; }
        public Division Division { get; set; }
        public Department Department { get; set; }
        public Team Team { get; set; } 
        public Position Position { get; set; } 
        public Employement_Type Employement_Type { get; set; } 




    }
}
