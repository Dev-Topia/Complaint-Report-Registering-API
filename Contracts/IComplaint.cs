using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Entities;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Contracts
{
    public interface IComplaint
    {
        Task<ArrayResponse> ViewAllComplaint();
        Task<ObjectResponse> ViewComplaint(string id);
        Task<GeneralResponse> RegsiterComplaint(Complaint complaint);
        Task<GeneralResponse> EditComplaint(string id, ComplaintPostDTO complaint);
        Task<GeneralResponse> DeleteComplaint(string id);
    }
}