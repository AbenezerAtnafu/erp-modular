using ERP.Models.HRMS.Employee_managments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Language
{
    public class Language
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'language name ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string name { get; set; }

        [StringLength(200, ErrorMessage = "The 'ability to listen ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string? ability_to_listen { get; set; }
        [StringLength(200, ErrorMessage = "The 'ability to speak ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string? ability_to_Speak { get; set; }
        [StringLength(200, ErrorMessage = "The 'ability to read ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string? ability_to_Read { get; set; }
        [StringLength(200, ErrorMessage = "The 'ability to write ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string? ability_to_write { get; set; }


        [ForeignKey("Employee")]
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
