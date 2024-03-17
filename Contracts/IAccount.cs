using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Entities;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Contracts
{
    public interface IAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
        Task<GeneralResponse> LogoutAccount();
        Task<ObjectResponse> GetUserProfile(string userId);
    }
}