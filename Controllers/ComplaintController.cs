using System.Security.Claims;
using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Complaint_Report_Registering_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintController(IComplaint complaint) : ControllerBase
    {
        [HttpGet("get-all-complaint")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllComplaint()
        {
            var response = await complaint.ViewAllComplaint();
            return Ok(response);
        }
        [HttpGet("get-single-complaint/{id}")]
        [Authorize]
        public async Task<IActionResult> GetSingleComplaint([FromRoute] string id)
        {
            var response = await complaint.ViewComplaint(id);
            if (response.Flag == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost("register-complaint")]
        [Authorize]
        public async Task<IActionResult> RegisterComplaint([FromBody] ComplaintPostDTO complaintPostDTO)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await complaint.RegsiterComplaint(complaintPostDTO, userId!);
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPut("update-complaint/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComplaint([FromRoute] string id, [FromBody] ComplaintPostDTO complaintPostDTO)
        {
            var response = await complaint.EditComplaint(id, complaintPostDTO);
            if (response.Flag == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpDelete("delete-complaint/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComplaint([FromRoute] string id)
        {
            var response = await complaint.DeleteComplaint(id);
            if (response.Flag == false)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpPost("add-complaint-type")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddComplaintType([FromBody] string typeName)
        {
            var response = await complaint.AddComplaintType(typeName);
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("add-status-type")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStatusType([FromBody] string typeName)
        {
            var response = await complaint.AddStatusType(typeName);
            if (response.Flag == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}