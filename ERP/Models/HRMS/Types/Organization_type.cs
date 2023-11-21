using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Types
{
    public class Organization_type
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "The 'name' od the organization should be atleast {2} leeters long.", MinimumLength = 0)]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

    }
}
