using Complaint_Report_Registering_API.Entities;

namespace Complaint_Report_Registering_API.DTOs
{
    public class ServiceResponses
    {
        public record class GeneralResponse(bool Flag, string Message);
        public record class ListResponse(bool Flag, List<ComplaintGetDTO> Complaints);
        public record class ObjectResponse(bool Flag, object Data);
        public record class LoginResponse(bool Flag, string Token, string UserId, string Role, string Message);
    }
}