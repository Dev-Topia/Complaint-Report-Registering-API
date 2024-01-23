using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Complaint_Report_Registering_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount(UserDTO userDTO)
        {
            var response = await account.CreateAccount(userDTO);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAccount(LoginDTO loginDTO)
        {
            var response = await account.LoginAccount(loginDTO);
            return Ok(response);
        }
    }
}