namespace Complaint_Report_Registering_API.DTOs
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class LoginResponse(bool Flag, string Token, string Message);
        public record class Test(bool Flag, Dictionary<string, string> Claims);
    }
}