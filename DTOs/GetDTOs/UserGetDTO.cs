namespace Complaint_Report_Registering_API.DTOs.GetDTOs
{
    public class UserGetDTO
    {
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public List<string>? Role { get; set; }
        public List<ComplaintGetUserDTO>? Complaints { get; set; }
    }
}