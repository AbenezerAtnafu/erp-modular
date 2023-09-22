
using ERP.Models.HRMS.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Employee_managments
{
    public class Training
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "The training name is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The 'training name'    must be between {2} and {1} characters long.")]
        public string training_institution { get; set; }
    
       
        [StringLength(200, MinimumLength = 5, ErrorMessage = "description  must be between {2} and {1} characters long.")]
        public string? description { get; set; }    

        [StringLength(200, MinimumLength = 5, ErrorMessage = "The country must be between {2} and {1} characters long.")]
        public string country_of_training { get; set; }


        [StringLength(200, MinimumLength = 5, ErrorMessage = "The email field must be between {2} and {1} characters long.")]

        public string? email { get; set; }

        [ForeignKey("Trainign_Type")]
        public int training_type_id { get; set; }
        
    
        [StringLength(200, MinimumLength = 5, ErrorMessage = "The training situation  must be between {2} and {1} characters long.")]
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
        public Trainign_Type Training_Type { get; set; }
    }
}
