using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ERP.Areas.Identity.Data;
using HRMS.Types;
using Newtonsoft.Json;

namespace ERP.Models.HRMS.Employee_managments
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'profile picture' is can not be empty ", MinimumLength = 0)]
        public string profile_picture { get; set; }
       
        public string first_name { get; set; }
        
        public string first_name_am { get; set; }
        
        public string father_name { get; set; }
        
        public string father_name_am { get; set; }
        
        public string grand_father_name { get; set; }
        
        public string grand_father_name_am { get; set; }
        public string gender { get; set; }

        [Required(ErrorMessage = "The 'date of birth' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'date of birth' field must be a valid Date.")]
        public DateTime date_of_birth { get; set; } 
        
        [Required(ErrorMessage = "The Start date field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'date of birth' field must be a valid Date.")]
        public DateTime start_date { get; set; }
       // public DateTime? end_date { get; set; }
      

        [Required(ErrorMessage = "The 'nationality' field is required.")]
        [StringLength(200, ErrorMessage = "The 'nationality' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string nationality { get; set; }

        public string? nation { get; set; }

        [Required(ErrorMessage = "The 'place of birth' field is required.")]
        [StringLength(200, ErrorMessage = "The 'place of birth' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string place_of_birth { get; set; }
        
        public double? salary { get; set; }
        
        public string? religion { get; set; }
        [Required(ErrorMessage = "The 'back account number' field is required.")]
        [MinLength(4, ErrorMessage = "The 'back account number' field must be a 4-digit number.")]
        public string back_account_number { get; set; }
        
        public string? tin_number { get; set; }
       
        public string? pension_number { get; set; }

        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }
        [Required(ErrorMessage = "The 'place of work' field is required.")]
        [StringLength(200, ErrorMessage = "The 'place of work' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string place_of_work { get; set; }
        public string? employee_code { get; set; }
        public bool? profile_status { get; set; }
        public int? lastid { get; set; }
        public bool? work_status { get; set; }
        public string? feedback { get; set; }
        [ForeignKey("Marital_Status_Types")]
        public int marital_status_type_id { get; set; }

        [ForeignKey("User")]
        public string user_id { get; set; }
        public Marital_Status_Types Marital_Status_Types { get; set; }
        public User User { get; set; }
        public Employee_Address Employee_Address { get; set; }
        [JsonIgnore]
        public Employee_Contact Employee_Contact { get; set; }
        public Employee_Office Employee_Office { get; set; }

        public ICollection<Language> Language { get; set; }
        public ICollection<Family_History> Family_History { get; set; }
        public ICollection<Emergency_contact> Emergency_contact { get; set; }
        public ICollection<Reward> Rewards { get; set; }
        public ICollection<Training> Trainings { get; set; }
    }
}
