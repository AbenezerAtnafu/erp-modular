using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Models.HRMS.Employee_managments;

namespace ERP.Models.HRMS.Employee_managments
{
    public class Emergency_contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The 'Full name ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string full_name { get; set; }

        [Required(ErrorMessage = "The 'phone number' field is required.")]
        [MinLength(9, ErrorMessage = "The 'phone number' field must be a 9-digit number.")]
        public string phonenumber { get; set; }
        [MinLength(9, ErrorMessage = "The 'alternative phone number' field must be a 9-digit number.")]
        public string? alternative_phonenumber { get; set; }

        [StringLength(200, ErrorMessage = "The 'Relationship' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string Relationship { get; set; }

        [ForeignKey("Employees")]
        public int employee_id { get; set; }
        public Employee? Employees { get; set; }

        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }
    }
}
