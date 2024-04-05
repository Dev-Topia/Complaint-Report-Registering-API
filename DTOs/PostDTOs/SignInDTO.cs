using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs.PostDTOs
{
    public class SignInDTO
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}