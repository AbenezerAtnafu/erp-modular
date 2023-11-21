using ERP.Models.HRMS.Employee_managments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Surety
{
    public class Surety
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int emp_id { get; set; }
        public string? company_name { get; set;}
        public string? reason { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        public Employee? Employee { get; set; }

    }
}
