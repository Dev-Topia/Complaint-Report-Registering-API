using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs.GetDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Complaint_Report_Registering_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController(AppDbContext context) : ControllerBase
    {
        [HttpGet("get-departments-report")]
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
        public IEnumerable<CategoryReport> GetCategoryReport()
        {
            var reports = context.Complaints
                .Include(ct => ct.ComplaintType)
                .GroupBy(c => c.ComplaintType!.Type)
                .Select(g => new CategoryReport
                {
                    TypeName = g.Key!,
                    Count = g.Count()
                })
                .ToList();

            return reports;
        }
    }
}