using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.DTOs.PostDTOs;

namespace Complaint_Report_Registering_API.Contracts
{
    public interface IComplaint
    {
        Task<List<ComplaintGetDTO>> ViewComplaints();
        Task<ComplaintGetDTO> ViewComplaint(int complaintId);
        Task<bool> CreateComplaint(ComplaintPostDTO complaint, string userId);
        Task<bool> EditComplaint(ComplaintPostDTO complaint, int complaintId);
        Task<bool> RemoveComplaint(int complaintId);
        Task<bool> AddComplaintType(ComplaintTypePostDTO complaintType);
        Task<bool> AddStatus(StatusPostDTO status);
        Task<bool> FindComplaint(int complaintId);
    }
}