using System.ComponentModel.Design;
using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg.Sig;

namespace Complaint_Report_Registering_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUser user) : ControllerBase
    {
        [HttpPost("sign-up")]
        public async Task<IActionResult> PostSignUpUser([FromBody] SignUpDTO newUser)
        {
            var findUser = await user.FindUserByEmail(newUser.Email);
            if (findUser) return BadRequest(new { msg = "User already exist" });
            var response = await user.SignUp(newUser);
            if (!response.Flag) return BadRequest(new { msg = response.Msg });
            return Ok(new { msg = response.Msg });
        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> PostSignInUser([FromBody] SignInDTO loginUser)
        {
            var response = await user.SignIn(loginUser);
            if (response.UserId == null)
            {
                return NotFound(new { msg = response.Msg });
            }
            else
            {
                if (response.Token == null)
                {
                    return BadRequest(new { msg = response.Msg });
                }
                Response.Cookies.Append("token", response.Token, new CookieOptions() { Expires = DateTime.Now.AddDays(7), HttpOnly = true, Secure = true, SameSite = SameSiteMode.None });
                return Ok(response);
            }
        }
        [HttpPost("sign-out")]
        [Authorize]
        public async Task<IActionResult> PostSignOutUser()
        {
            var response = await user.SignOut();
            if (!response)
            {
                return BadRequest(new { msg = "Something went wrong" });
            }
            Response.Cookies.Delete("token");
            return Ok(new { msg = "Log out successfully" });
        }
        [HttpGet("get-user-data")]
        public IActionResult GetUserData([FromHeader] string cookie)
        {
            if (string.IsNullOrWhiteSpace(cookie))
            {
                return BadRequest(new { msg = "Cookie Is Not Provided" });
            }
            int tokenIndex = cookie.IndexOf("token=");
            if (tokenIndex == -1)
            {
                return BadRequest(new { msg = "Token Is Not Provided" });
            }
            string tokenData = cookie[(tokenIndex + "token=".Length)..];
            var res = user.DecodeJwt(tokenData);
            if (res == null)
            {
                return BadRequest(new { msg = "Token Is Not Valid" });
            }
            return Ok(res);
        }
        [HttpGet("get-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var getUsers = await user.ViewUsers();
            return Ok(new { data = getUsers });
        }
        [HttpGet("get-user/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser(string userId)
        {
            var getUser = await user.FindUserById(userId);
            if (!getUser) return NotFound(new { msg = "User Not Found" });
            var response = await user.ViewUser(userId);
            return Ok(new { data = response });
        }
        [HttpPut("update-user/{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromRoute] string userId, [FromBody] UserUpdateDTO userUpdate)
        {
            var findUser = await user.FindUserById(userId);
            if (!findUser)
            {
                return NotFound(new { msg = "User Not Found" });
            }
            var response = await user.EditUser(userId, userUpdate);
            if (!response)
            {
                return BadRequest(new { msg = "User update failed" });
            }
            return Ok(new { msg = "User update successfully" });
        }
        [HttpDelete("delete-user/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
        {
            var findUser = await user.FindUserById(userId);
            if (!findUser)
            {
                return NotFound(new { msg = "User Not Found" });
            }
            var response = await user.RemoveUser(userId);
            if (!response)
            {
                return BadRequest(new { msg = "User Delete failed" });
            }
            return Ok(new { msg = "User Delete successfully" });
        }
    }
}
