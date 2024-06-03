namespace Complaint_Report_Registering_API.DTOs.GetDTOs
{
    public class DepartmentReport
    {
        public int? DepartmentId { get; set; }
        public string? DeparmentName { get; set; }
        public int GradingAndAssessmentCount { get; set; }
        public int SpecialEducationServicesCount { get; set; }
        public int SafetyAndSecurityCount { get; set; }
        public int TeacherConductCount { get; set; }
    }
}