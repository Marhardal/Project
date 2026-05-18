using System.ComponentModel.DataAnnotations;

namespace Project.DTO
{
    public class AuthDTO
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? UserID { get; set; }
        public string Role { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
