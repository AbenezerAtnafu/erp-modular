using ERP.Models.HRMS.Employee_managments;
using ERP.Models.HRMS.Types;
using HRMS.Office;
using HRMS.Types;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Work_history
{
    public class Work_history
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {  get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'name' of the organization should be atleast {2} leeters long.", MinimumLength = 0)]
        public string Organization_Name { get; set; } = null!;
        [Required]
        [StringLength(200, ErrorMessage = "The 'title' of the employee should be atleast {2} leeters long.", MinimumLength = 0)]
        public string Title { get; set; } = null!;
        public string Description { get; set; }
        [Required(ErrorMessage = "The 'Start_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'Start_date' field must be a valid Date.")]
        public DateTime? Start_date { get; set; }
        [Required(ErrorMessage = "The 'End_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'End_date' field must be a valid Date.")]
        public DateTime? End_date { get; set; }
        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "The 'Updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'Updated_date' field must be a valid Date.")]
        public DateTime? Updated_date { get; set; }

        public int created_by { get; set; }
        public int updated_by { get; set; }

        [ForeignKey("Employement_Type")]
        public int Employement_type_ID { get; set; }

        [ForeignKey("Employee, id")]
        public int Employee_id { get; set; }
        public Employee Employee { get; set; }
        
        [ForeignKey("Position")]
        public int Posiition_id { get; set; }
        public int Organization_type_ID { get; set; }
        public Organization_type Organization_Type { get; set; }
        public Employement_Type Employement_Type { get; set; }
        public Position position { get; set; } 
    }
}
