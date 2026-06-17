using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = new Guid();
        public string UserID { get; set; } = null!;

        public string Token { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRevoked { get; set; }

        public IdentityUser User { get; set; } = null!;
    }
}
