using System.Security.Claims;
using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Entities;
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
        public async Task<ActionResult<ComplaintGetDTO>> GetAllComplaint()
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
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("register-complaint")]
        [Authorize]
        public async Task<IActionResult> RegisterComplaint([FromBody] ComplaintPostDTO complaintPostDTO)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newComplaint = new Complaint
            {
                Id = Guid.NewGuid(),
                Title = complaintPostDTO.Title,
                Type = complaintPostDTO.Type,
                Description = complaintPostDTO.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ApplicationUserId = userId,
            };
            var response = await complaint.RegsiterComplaint(newComplaint);
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
                return BadRequest(response);
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
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}