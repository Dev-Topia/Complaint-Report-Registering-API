namespace Complaint_Report_Registering_API.DTOs.GetDTOs
{
    public class UserDataFromJWT
    {
        public string? UserId { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}