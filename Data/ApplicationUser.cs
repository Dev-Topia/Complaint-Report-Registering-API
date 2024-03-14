using Complaint_Report_Registering_API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Complaint_Report_Registering_API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Complaint>? Complaints { get; set; }
    }
}