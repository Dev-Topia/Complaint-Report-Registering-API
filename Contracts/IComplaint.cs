using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.DTOs.GetDTOs;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Contracts
{
    public interface IComplaint
    {
        Task<List<ComplaintGetDTO>> ViewComplaints();
        Task<ComplaintGetDTO> ViewComplaint(int complaintId);
        Task<bool> FindComplaint(int complaintId);
        Task<bool> FindComplaintType(int complaintTypeId);
        Task<CreateComplaintResponse> CreateComplaint(ComplaintPostDTO complaint, string userId);
        Task<bool> EditComplaint(ComplaintPostDTO complaint, int complaintId);
        Task<bool> RemoveComplaint(int complaintId);
        Task<bool> AddStatus(StatusPostDTO status);
        Task<List<ComplaintTypeGetDTO>> ViewComplaintTypes();
        Task<bool> AddComplaintType(ComplaintTypePostDTO complaintType);
        Task<GeneralResponse> RemoveComplaintType(int complaintTypeId);
    }
}