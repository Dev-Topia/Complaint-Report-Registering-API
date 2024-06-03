using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs.PostDTOs;

public class DepartmentCreateDto
{
    [Required]
    public string? DeparmentName { get; set; }
}
