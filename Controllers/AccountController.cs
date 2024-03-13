using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Complaint_Report_Registering_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccount account, IWebHostEnvironment env) : ControllerBase
    {
        [HttpPost("sign-up")]
        [User_ValidationCreateUserFliter]
        public async Task<IActionResult> CreateAccount(UserDTO userDTO)
        {
            var response = await account.CreateAccount(userDTO);
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
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
            else
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = !env.IsDevelopment(),
                    Expires = DateTime.Now.AddDays(1),
                };

                Response.Cookies.Append("auth_token", response.Token, cookieOptions);
                return Ok(response);
            }
        }
        [HttpPost("sign-out")]
        [Authorize]
        public async Task<IActionResult> LogoutAccount()
        {
            var response = await account.LogoutAccount();
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("get-user-data")]
        [Authorize]
        public async Task<IActionResult> GetUserData()
        {
            var authHeader = HttpContext.Request.Headers.Authorization;
            var token = authHeader.First()!["Bearer ".Length..].Trim();
            var response = await account.GetUserData(token);
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("get-user-profile/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile([FromRoute] string id)
        {
            var response = await account.GetUserProfile(id);
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        // [HttpGet("convert-token")]
        // [Authorize]
        // public async Task<IActionResult> ConvertToken([FromHeader] string token)
        // {
        //     var response = await account.ConvertToken(token);
        //     return Ok(response);
        // }
    }
}