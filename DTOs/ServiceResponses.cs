using Microsoft.AspNetCore.Identity;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class SignInResponse(string Token, string UserId, string Role, string Msg);
        public record class SignUpResponse(bool Flag, string Msg);
    }
}