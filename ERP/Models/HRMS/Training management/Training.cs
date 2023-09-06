using ERP.Models.HRMS.Employee_managments;
using ERP.Models.HRMS.Types;
using Microsoft.Data.SqlClient.DataClassification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Training
{
    public class Training
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "The training name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The 'training name' field must be between 2 and 100 characters.")]
        public string training_institution { get; set; }
    
        [StringLength(200, ErrorMessage = "The description must be between {2} and {200} characters long.", MinimumLength = 0)]
        public string? description { get; set; }    

        [StringLength(200, ErrorMessage = "The country must be between {2} and {200} characters long.", MinimumLength = 0)]
        public string country_of_training { get; set; }

        [StringLength(200, ErrorMessage = "The email field is required.", MinimumLength = 0)]
        public string? email { get; set; }
        [StringLength(200, ErrorMessage = "The training type must be between {2} and {200} characters long.", MinimumLength = 0)]
        public string training_type { get; set; }
        [StringLength(200, ErrorMessage = "The training situation field is required.", MinimumLength = 0)]
        public string training_situation { get; set; }

        [Required(ErrorMessage = "The 'start_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'start_date' field must be a valid Date.")]
        public DateTime start_date { get; set; }

        [Required(ErrorMessage = "The 'end_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'end_date' field must be a valid Date.")]
        public DateTime end_date { get; set; }
        [NotMapped]
        public string? training_document_path { get; set; }
        public bool? status { get; set; }
        public string? feedback { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime created_date { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime updated_date { get; set; }

        [ForeignKey("Employee")]
        public int employee_id { get; set; }
        public int? Created_by { get; set; }
        public int? Updated_by { get; set; } 
        public int? approved_by { get; set; }
        public Employee Employee { get; set; }
    }
}
