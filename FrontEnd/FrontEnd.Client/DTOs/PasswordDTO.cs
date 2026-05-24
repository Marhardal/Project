
using System.ComponentModel.DataAnnotations;

public class PasswordDTO
    {
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
    ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }

