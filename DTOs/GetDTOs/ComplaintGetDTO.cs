namespace Complaint_Report_Registering_API.DTOs
{
    public class ComplaintGetDTO
    {
        public int ComplaintId { get; set; }
        public string? Title { get; set; }
        public string? ComplaintType { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public string? Department { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ProfileDTO? User { get; set; }
    }
}