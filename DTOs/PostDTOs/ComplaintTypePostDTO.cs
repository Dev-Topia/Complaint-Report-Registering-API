using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs.PostDTOs
{
    public class ComplaintTypePostDTO
    {
        [Required]
        public string? ComplaintType { get; set; }
    }
}