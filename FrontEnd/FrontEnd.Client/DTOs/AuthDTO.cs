using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{
    public class AuthDTO
    {
        [Required]
        public string? Token { get; set; }
        [Required]
        public string? UserID { get; set; }
        public string Role { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiry { get; set; }
        public DateTime? TokenExpiry { get; set; }
    }
    public class AuthStateService
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
