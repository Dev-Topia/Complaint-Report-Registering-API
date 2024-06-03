using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Complaint_Report_Registering_API.Data;

namespace Complaint_Report_Registering_API.Entities
{
    public class Complaint
    {
        [Key]
        [Required]
        public int ComplaintId { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? DepartmentId { get; set; }
        [JsonIgnore]
        public Department? Department { get; set; }
        public string? ApplicationUserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? ApplicationUser { get; set; }
        public int ComplaintTypeId { get; set; }
        [JsonIgnore]
        public ComplaintType? ComplaintType { get; set; }
        public int StatusId { get; set; }
        [JsonIgnore]
        public Status? Status { get; set; }
    }
}