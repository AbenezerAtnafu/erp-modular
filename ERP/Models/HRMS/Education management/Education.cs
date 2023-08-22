using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
        public string start_date { get; set; }

        [Required(ErrorMessage = "The 'end_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'end_date' field must be a valid Date.")]
        public string end_date { get; set; }

        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }

    
    }
}
