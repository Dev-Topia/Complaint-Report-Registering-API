using Microsoft.AspNetCore.Identity;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class ListResponse(bool Flag, List<ComplaintGetDTO> Complaints);
        public record class ObjectResponse(bool Flag, object Data);
        public record class LoginResponse(string Token, string UserId, string Role, string Msg);
        public record class UserListResponse(List<IdentityRole> Users);
    }
}