using System.ComponentModel.DataAnnotations;

namespace FrontEnd.Client.DTOs
{
    // Returned after Step 1 — credentials valid but no token yet
    public class PreAuthResponseDTO
    {
        public string UserId { get; set; } = string.Empty;
    }

    // Sent to verify the OTP
    public class OtpVerifyDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    // Form model for OTP input
    public class OtpDTO
    {
        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "OTP must be numeric")]
        public string Code { get; set; } = string.Empty;
    }
}
