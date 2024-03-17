using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ComplaintGetDTO
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ProfileDTO? User { get; set; }
    }
}