using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs.PostDTOs
{
    public class StatusPostDTO
    {
        [Required]
        public string? Status { get; set; }
    }
}