using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Complaint_Report_Registering_API.Data;

namespace Complaint_Report_Registering_API.Entities
{
    public class Complaint
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }
        public string? FileUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? ApplicationUserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? ApplicationUser { get; set; }
        public Guid? ComplaintTypeId { get; set; }
        [JsonIgnore]
        public ComplaintType? ComplaintType { get; set; }
        public Guid? StatusTypeId { get; set; }
        [JsonIgnore]
        public StatusType? StatusType { get; set; }
    }
}