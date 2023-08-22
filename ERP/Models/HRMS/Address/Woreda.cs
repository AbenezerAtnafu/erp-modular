using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Models.HRMS.Address
{
    public class Woreda
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required(ErrorMessage = "The 'name' field is required.")]
        public string name { get; set; }


        [StringLength(200, ErrorMessage = "The 'description' field must be between {2} and {1} characters long.", MinimumLength = 0)]
        public string? description { get; set; }

        [Required(ErrorMessage = "The 'created_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'created_date' field must be a valid Date.")]
        public DateTime created_date { get; set; }

        [Required(ErrorMessage = "The 'updated_date' field is required.")]
        [DataType(DataType.Date, ErrorMessage = "The 'updated_date' field must be a valid Date.")]
        public DateTime updated_date { get; set; }

        // Foreign keys
        [ForeignKey("Subcity")]
        public int? subcity_id { get; set; }

        [ForeignKey("Zone")]
        public int? zone_id { get; set; }

        // Navigation properties
        public Subcity Subcity { get; set; }
        public Zone Zone { get; set; }
    }
}
