using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.Entities
{
    public class StatusType
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? Type { get; set; }
        public List<Complaint>? Complaints { get; set; }
    }
}