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
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public bool TwoFactorEnabled { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
        public string? Password { get; set; }
    }
}
