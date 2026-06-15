using System.ComponentModel.DataAnnotations;

namespace Project.DTO
{
    public class IdentityDTO
    {
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public List<string>? Roles { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public PasswordDTO? Password { get; set; }
    }
}
