using System.ComponentModel.DataAnnotations;

namespace Project.DTO
{
    public class ForgotPasswordDTO
    {
        [Required, EmailAddress]
        public string? Email { get; set; } 
    }
    public class ResetPasswordDTO
    {
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? NewPassword { get; set; }
    }
}
