using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Entities;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Contracts
{
    public interface IComplaint
    {
        Task<ListResponse> ViewAllComplaint();
        Task<ObjectResponse> ViewComplaint(string id);
        Task<GeneralResponse> RegsiterComplaint(ComplaintPostDTO complaintPostDTO, string userId);
        Task<GeneralResponse> EditComplaint(string id, ComplaintPostDTO complaint);
        Task<GeneralResponse> DeleteComplaint(string id);
        Task<GeneralResponse> AddComplaintType(string typeName);
        Task<GeneralResponse> AddStatusType(string typeName);
    }
}