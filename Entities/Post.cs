using System.ComponentModel.DataAnnotations;


namespace Complaint_Report_Registering_API.Entities
{
    public class Post
    {
        [Key]
        public string? Id { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
    }
}