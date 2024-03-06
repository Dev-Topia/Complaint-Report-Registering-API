using Complaint_Report_Registering_API.Entities;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class ArrayResponse(bool Flag, List<Complaint> Complaints);
        public record class ObjectResponse(bool Flag, object Data);
        public record class LoginResponse(bool Flag, string Token, string Message);
        public record class Test(bool Flag, Dictionary<string, string> Claims);
    }
}