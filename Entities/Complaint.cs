using System.ComponentModel.DataAnnotations;

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
        public string? Type { get; set; }
        [Required]
        [StringLength(250)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // public Complaint()
        // {
        //     this.Id = Guid.NewGuid();
        // }
    }
}