
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HRMS.Types;
using ERP.Models.HRMS.Employee_managments;

namespace HRMS.Education_management
{
    public class Education
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        
        [Required(ErrorMessage = "The 'institution_name' field is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The 'institution_name' field must be between 2 and 100 characters.")]
        public string institution_name { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The 'institution email' field must be between 2 and 100 characters.")]
        public string? institution_email { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The 'institution website' field must be between 2 and 100 characters.")]
        public string? institution_website { get; set; }

        [Required(ErrorMessage = "The 'filed of study' field is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The 'filed of study' field must be between 2 and 100 characters.")]
        public string filed_of_study { get; set; }
        public float? gpa { get;}
        [StringLength(200, ErrorMessage = "The 'description' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string? description { get; set; }

        [Required(ErrorMessage = "The 'start_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'start_date' field must be a valid Date.")]
        public DateTime start_date { get; set; }

        [Required(ErrorMessage = "The 'end_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'end_date' field must be a valid Date.")]
        public DateTime end_date { get; set; }

        public double Identificationnumber { get; set; }

        public bool? status { get; set; }
        public bool? filestatus { get; set; }

        public string? feedback { get; set; }

        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }

        [ForeignKey("Employee")]
        public int employee_id { get; set; }
        public Employee? Employee { get; set; }

        [ForeignKey("Education_Program_Type")]
        public int educational_program_id { get; set; }
        public Education_Program_Type? Education_Program_Type { get; set; }

        [ForeignKey("Education_Level_Type")]
        public int educational_level_type_id { get; set; }
        public Education_Level_Type? Education_Level_Type { get; set; }






    }
}
