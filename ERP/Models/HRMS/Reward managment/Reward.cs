using ERP.Models.HRMS.Employee_managments;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Reward_managment
{
    public class Reward
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "The 'reward name' field is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "The 'training name' field must be between 2 and 100 characters.")]
        public string reward_name { get; set; }

        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description field must be between {2} and {1} characters long.")]
        public string? description { get; set; }


        [StringLength(200, MinimumLength = 5, ErrorMessage = "Reason of reward field must be between {2} and {1} characters long.")]
        public string? reason_of_reward { get; set; }

        [Required(ErrorMessage = "Reward given date is required.")]
        [DataType(DataType.Date)]
        public DateTime given_date { get; set; }
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
        public int Created_by { get; set; }
        public int Updated_by { get; set; }
        public int? approved_by { get; set; }
        public Employee Employee { get; set; }
    }
}
