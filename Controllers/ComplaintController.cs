using System.Security.Claims;
using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
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
        public async Task<ActionResult> GetComplaints()
        {
            var response = await complaint.ViewComplaints();
            return Ok(new { data = response });
        }

        [HttpGet("get-single-complaint/{complaintId}")]
        [Authorize]
        public async Task<IActionResult> GetComplaint([FromRoute] int complaintId)
        {
            var response = await complaint.ViewComplaint(complaintId);
            if (response.ComplaintId == 0)
            {
                return NotFound(new { msg = "Complaint not found" });
            }
            return Ok(new { data = response });
        }

        [HttpPost("register-complaint")]
        [Authorize]
        public async Task<IActionResult> PostComplaint([FromBody] ComplaintPostDTO complaintPostDTO)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await complaint.CreateComplaint(complaintPostDTO, userId!);
            if (response.Flag == false)
            {
                return BadRequest(new { msg = "Something went wrong" });
            }
            return Ok(
                new { complaintId = response.ComplaintId, msg = "Complaint successfully created" }
            );
        }

        [HttpPut("update-complaint/{complaintId}")]
        [Authorize]
        public async Task<IActionResult> PutComplaint(
            [FromRoute] int complaintId,
            [FromBody] ComplaintPostDTO complaintPostDTO
        )
        {
            var findComplaint = await complaint.FindComplaint(complaintId);
            if (!findComplaint)
                return NotFound(new { msg = "Complaint not found" });
            var response = await complaint.EditComplaint(complaintPostDTO, complaintId);
            if (!response)
            {
                return BadRequest(new { msg = "Something went wrong" });
            }
            return Ok(new { mgs = "Complaint updated successfully" });
        }

        [HttpDelete("delete-complaint/{complaintId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComplaint([FromRoute] int complaintId)
        {
            var findComplaint = await complaint.FindComplaint(complaintId);
            if (!findComplaint)
                return NotFound(new { msg = "Complaint not found" });
            var response = await complaint.RemoveComplaint(complaintId);
            if (!response)
            {
                return BadRequest(new { msg = "Something went wrong" });
            }
            return Ok(new { mgs = "Complaint deleted successfully" });
        }

        [HttpGet("get-complaint-type")]
        [Authorize]
        public async Task<IActionResult> GetComplaintTypes()
        {
            var response = await complaint.ViewComplaintTypes();
            return Ok(new { data = response });
        }

        [HttpPost("add-complaint-type")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddComplaintType(
            [FromBody] ComplaintTypePostDTO complaintType
        )
        {
            var response = await complaint.AddComplaintType(complaintType);
            if (!response)
                return BadRequest(new { msg = "Complaint Type is already exist" });
            return Ok(new { msg = "Complaint type added successfully" });
        }

        [HttpDelete("delete-complaint-type/{complaintTypeId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComplaintType([FromRoute] int complaintTypeId)
        {
            var findComplaintType = await complaint.FindComplaintType(complaintTypeId);
            if (!findComplaintType)
            {
                return NotFound(new { msg = "Complaint type not found" });
            }
            var response = await complaint.RemoveComplaintType(complaintTypeId);
            if (!response.Flag)
            {
                return BadRequest(new { msg = response.Msg });
            }
            return Ok(new { mgs = response.Msg });
        }

        [HttpPost("add-status-type")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStatus([FromBody] StatusPostDTO status)
        {
            var response = await complaint.AddStatus(status);
            if (!response)
                return BadRequest(new { msg = "Status is already exist" });
            return Ok(new { msg = "Status added successfully" });
        }
    }
}
