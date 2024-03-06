using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ComplaintGetDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}