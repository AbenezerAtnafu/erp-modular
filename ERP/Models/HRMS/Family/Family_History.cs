using ERP.Models.HRMS.Employee_managments;
using HRMS.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Family
{
    public class Family_History
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The 'Full name ' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string full_name { get; set; }
        public string? job_name { get; set; }
        public string? house_number { get; set; }

        [Required(ErrorMessage = "The 'phone number' field is required.")]
        [MinLength(9, ErrorMessage = "The 'phone number' field must be a 9-digit number.")]
        public long phonenumber { get; set; }
        [MinLength(9, ErrorMessage = "The 'alternative phone number' field must be a 9-digit number.")]
        public int? alternative_phonenumber { get; set; }
        public string gender { get; set; }

        [ForeignKey("Family_RelationShip_Type")]
        public int family_relationship_id { get; set; }
        public string? primary_address { get; set; }
        
        public Family_RelationShip_Type RelationshipType { get; set; }

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
