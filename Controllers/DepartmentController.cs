using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
using Complaint_Report_Registering_API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Complaint_Report_Registering_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController(AppDbContext context) : ControllerBase
    {
        [HttpGet("get-all-department")]
        [Authorize(Roles = "Admin")]
        public IEnumerable<Department> GetAllDepartment()
        {
            var list = context.Departments
                // .Include(d => d.Users)
                // .Include(c => c.Complaints)
                .ToList();
            return list;
        }
        [HttpGet("get-single-department/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public ActionResult<Department> GetSingleDepartment(int departmentId)
        {
            var department = context.Departments
                .Include(d => d.Users)
                .Include(c => c.Complaints)
                .FirstOrDefault(d => d.DepartmentId == departmentId);
            if (department == null)
            {
                return NotFound();
            }
            return department;
        }
        [HttpPost("create-department")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateDepartment([FromBody] DepartmentCreateDto departmentCreateDto)
        {
            var findDepartment = context.Departments.FirstOrDefault(d =>
                d.DepartmentName == departmentCreateDto.DeparmentName
            );
            if (findDepartment != null)
            {
                return BadRequest(new { msg = "Department already exist!" });
            }
            context.Departments.Add(new Department
            {
                DepartmentName = departmentCreateDto.DeparmentName
            });
            context.SaveChanges();
            return Ok(new { msg = "Department create successfully" });
        }
        [HttpDelete("delete-department/{departmentId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteDepartment([FromRoute] int departmentId)
        {
            var findDepartment = context.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);
            if (findDepartment == null)
            {
                return NotFound(new { msg = "Department not found" });
            }
            context.Departments.Remove(findDepartment!);
            context.SaveChanges();
            return Ok(new { msg = "Department delete successfully" });
        }
    }
}

// int pageNumber = 1, int pageSize = 5
// .Skip((pageNumber - 1) * pageSize).Take(pageSize)