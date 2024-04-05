using Complaint_Report_Registering_API.DTOs.GetDTOs;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Contracts
{
    public interface IUser
    {
        Task<List<UserGetDTO>> ViewUsers();
        Task<UserGetDTO> ViewUser(string userId);
        Task<bool> SignUp(SignUpDTO user);
        Task<LoginResponse> SignIn(SignInDTO user);
        Task<bool> SignOut();
        Task<bool> FindUserById(string userId);
        Task<bool> FindUserByEmail(string email);
    }
}