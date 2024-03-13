using Microsoft.AspNetCore.Identity;

namespace Complaint_Report_Registering_API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}