using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ComplaintPostDTO
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Type { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }
    }
}