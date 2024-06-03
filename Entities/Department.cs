using System.ComponentModel.DataAnnotations;
using Complaint_Report_Registering_API.Data;

namespace Complaint_Report_Registering_API.Entities;

public class Department
{
    [Key]
    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public string? DepartmentName { get; set; }
    public List<ApplicationUser>? Users { get; set; }
    public List<Complaint>? Complaints { get; set; }
}
