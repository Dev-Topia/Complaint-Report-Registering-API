using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ComplaintPostDTO
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public Guid? ComplaintTypeId { get; set; }
        // [Required]
        // public Guid? StatusTypeId { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
    }
}