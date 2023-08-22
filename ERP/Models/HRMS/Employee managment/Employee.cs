using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Areas.Identity.Data;
using HRMS.Types;
using ERP.Models.HRMS.Employee_managments;

namespace ERP.HRMS.Employee_managment
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'profile picture' is can not be empty ", MinimumLength = 0)]
        public string profile_picture { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'first name ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string first_name { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'first name in amharic' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string first_name_am { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'father name' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string father_name { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'father name in amharic' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string father_name_am { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'grand father name' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string grand_father_name { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'grand father name in amharic' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string grand_father_name_am { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'gender' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string gender { get; set; }
        
        [Required(ErrorMessage = "The 'date of birth' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'date of birth' field must be a valid Date.")]
        public DateTime date_of_birth { get; set; }
        
        [Required(ErrorMessage = "The 'nationality' field is required.")]
        [StringLength(200, ErrorMessage = "The 'nationality' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string nationality { get; set; }
       
        public string? nation { get; set; }
        
        [Required(ErrorMessage = "The 'place of birth' field is required.")]
        [StringLength(200, ErrorMessage = "The 'place of birth' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string place_of_birth { get; set; }
        [Required(ErrorMessage = "The 'religion' field is required.")]
        [StringLength(200, ErrorMessage = "The 'religion' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string religion { get; set; }
        [Required(ErrorMessage = "The 'back account number' field is required.")]
        [MinLength(4, ErrorMessage = "The 'back account number' field must be a 4-digit number.")]
        public int back_account_number { get; set; }
        [Required(ErrorMessage = "The 'tin number' field is required.")]
        [MinLength(10, ErrorMessage = "The 'tin number' field must be a 10-digit number.")]
        public int tin_number { get; set; }
        [Required(ErrorMessage = "The 'pension number' field is required.")]
        [MinLength(4, ErrorMessage = "The 'pension number' field must be a 4-digit number.")]
        public int pension_number { get; set; }
        [Required(ErrorMessage = "The 'place of work' field is required.")]
        [StringLength(200, ErrorMessage = "The 'place of work' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string place_of_work { get; set; }

        [ForeignKey("Marital_Status_Types")]
        public int marital_status_type_id { get; set; }

        [ForeignKey("User")]
        public string user_id { get; set; }
        public Marital_Status_Types Marital_Status_Types { get; set; }
        public User User { get; set; }
        public Employee_Address Employee_Address { get; set; }
        public Employee_Contact Employee_Contact { get; set; }
        public Employee_Office Employee_Office { get; set; }

    }
}
