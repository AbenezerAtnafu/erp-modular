using ERP.Models.HRMS.Employee_managments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HRMS.Office
{
    public class Position
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required(ErrorMessage = "The 'name' field is required.")]
        [StringLength(50, ErrorMessage = "The 'name' field must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string name { get; set; }

        [StringLength(200, ErrorMessage = "The 'description' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string? description { get; set; }

        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]

        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }

        public ICollection<Employee_Office>? Employee_Office { get; set; }



    }
}
