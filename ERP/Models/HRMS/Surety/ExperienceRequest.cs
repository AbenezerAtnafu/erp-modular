using ERP.Models.HRMS.Employee_managments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Surety
{
    public class ExperienceRequest
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string reason { get; set; }
        public bool? status { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        [ForeignKey("Employee")]
        [Required]
        public int employee_id { get; set; }
        public Employee? Employee { get; set; }
    }
}
