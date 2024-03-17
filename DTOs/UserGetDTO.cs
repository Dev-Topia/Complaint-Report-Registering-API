namespace Complaint_Report_Registering_API.DTOs
{
    public class UserGetDTO
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public List<ComplaintGetDTO>? Complaints { get; set; }
    }
}