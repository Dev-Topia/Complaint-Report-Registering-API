using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Entities;
using Complaint_Report_Registering_API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Complaint_Report_Registering_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "data1", "data2" };
        }

        [HttpPost("sign-up")]
        [User_ValidationCreateUserFliter]
        public async Task<IActionResult> CreateAccount(UserDTO userDTO)
        {
            if (userDTO == null) return BadRequest();
            var response = await account.CreateAccount(userDTO);
            return Ok(response);
        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> LoginAccount(LoginDTO loginDTO)
        {
            var response = await account.LoginAccount(loginDTO);
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        // [HttpGet("status")]
        // public async Task<IActionResult> IsUserLoggedIn(string token)
        // {
        //     var response = await account.IsUserLoggedIn(token);
        //     if (response == false)
        //     {
        //         return BadRequest(response);
        //     }
        //     return Ok(response);
        // }
        [HttpPost("sign-out")]
        public async Task<IActionResult> LogoutAccount()
        {
            var response = await account.LogoutAccount();
            return Ok(response);
        }
        [HttpPost("generate-reset-token")]
        public async Task<IActionResult> CreateResetToken(MailData mailData)
        {
            var response = await account.CreateResetToken(mailData);
            return Ok(response);
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(string resetToken, string email, string newPassword)
        {
            var response = await account.ResetPassword(resetToken, email, newPassword);
            return Ok(response);
        }
        [HttpPost("{token}")]
        [Authorize]
        public async Task<IActionResult> ConvertToken(string token)
        {
            var response = await account.ConvertToken(token);
            return Ok(response);
        }
    }
}