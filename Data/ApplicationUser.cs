using System.Text.Json.Serialization;
using Complaint_Report_Registering_API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Complaint_Report_Registering_API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public int DepartmentId { get; set; }
        public bool IsSchoolVerified { get; set; }

        [JsonIgnore]
        public Department? Department { get; set; }
        public List<Complaint>? Complaints { get; set; }
    }
}
