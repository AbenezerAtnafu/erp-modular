
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Employee_managments
{
    public class Employee_Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "The 'phone number' field is required.")]
        [MinLength(9, ErrorMessage = "The 'phone number' field must be a 9-digit number.")]
        public string phonenumber { get; set; }
        [MinLength(9, ErrorMessage = "The 'alternative phone number' field must be a 9-digit number.")]
        public string? alternative_phonenumber { get; set; }
        public string? internal_phonenumber { get; set; }
        [MinLength(6, ErrorMessage = "The 'home number' field must be a 6-digit number.")]
        public string? home_phonenumber { get; set; }

        [ForeignKey("Employees")]
        public int employee_id { get; set; }
        public Employee Employees { get; set; }


    }
}
