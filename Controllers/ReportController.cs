using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs.GetDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Complaint_Report_Registering_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController(AppDbContext context) : ControllerBase
    {
        [HttpGet("get-departments-report")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<DepartmentReport> GetDepartmentReport()
        {
            var reports = context.Departments
                .Include(c => c.Complaints!)
                .Select(d => new DepartmentReport
                {
                    DepartmentId = d.DepartmentId,
                    DeparmentName = d.DepartmentName,
                    GradingAndAssessmentCount = d.Complaints!.Count(c => c.ComplaintTypeId == 3),
                    SpecialEducationServicesCount = d.Complaints!.Count(c => c.ComplaintTypeId == 4),
                    SafetyAndSecurityCount = d.Complaints!.Count(c => c.ComplaintTypeId == 5),
                    TeacherConductCount = d.Complaints!.Count(c => c.ComplaintTypeId == 6)
                }).ToList();
            return reports;
        }
        [HttpGet("get-categories-total-complaint")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<CategoryReport> GetCategoryReport()
        {
            var reports = context.ComplaintTypes
                .GroupJoin(context.Complaints, complaintType => complaintType.ComplaintTypeId, complaint => complaint.ComplaintTypeId,
                    (complaintType, complaints) => new CategoryReport
                    {
                        TypeName = complaintType.Type,
                        Count = complaints.Count()
                    })
                .ToList();
            return reports;
        }
        [HttpGet("get-departments-total-complaint")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetDepartmentTotalReport()
        {
            var reports = context.Departments
            .GroupJoin(context.Complaints, department => department.DepartmentId, complaint => complaint.DepartmentId,
            (department, complaint) => new
            {
                department.DepartmentName,
                Count = complaint.Count()
            }).ToList();
            return Ok(reports);
        }
    }
}