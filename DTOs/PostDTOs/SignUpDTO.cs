using System.ComponentModel.DataAnnotations;

namespace Complaint_Report_Registering_API.DTOs.PostDTOs
{
    public class SignUpDTO
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? Lastname { get; set; }
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string? Role { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}