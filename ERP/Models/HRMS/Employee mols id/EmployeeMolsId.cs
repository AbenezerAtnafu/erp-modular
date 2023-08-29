using ERP.Models.HRMS.Employee_managments;
using Microsoft.CodeAnalysis.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Employee_id
{
    public class EmployeeMolsId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]

        public string employee_code  { get; set; }
        public int id_tracker { get; set; }  

        public bool is_generated { get; set; }

        [ForeignKey("Employees")]
        public int employee_id { get; set; }

        public Employee Employees { get; set; }

        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }
    }
}
