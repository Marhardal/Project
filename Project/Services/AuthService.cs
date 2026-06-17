using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.Data;
using Project.Models;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project.Services
{
    public class AuthService(IConfiguration configuration, DBContext context)
    {
        // ✅ Capture the primary constructor parameter as a field
        private readonly IConfiguration _configuration = configuration;
        private readonly DBContext _db = context;

        //public string GenerateJwtToken(IdentityUser user)
        //{
        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, user.Id),
        //        new Claim(ClaimTypes.Email, user.Email ?? ""),
        //        new Claim(ClaimTypes.Name, user.UserName ?? "")
        //    };

        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(
        //            _configuration["Jwt:Key"]  // ✅ use the field
        //            ?? throw new InvalidOperationException("Jwt:Key is not configured.")
        //        )
        //    );

        //    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(2),
        //        signingCredentials: credentials
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        public string GenerateAccessToken(IdentityUser user, IList<string> roles)
        {
            var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!)
        };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),  // short-lived
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId)
        {
            // Revoke any existing tokens for this user
            var existing = await _db.RefreshTokens
                .Where(r => r.UserID == userId && !r.IsRevoked).ToListAsync();

            existing.ForEach(r => r.IsRevoked = true);

            var refreshToken = new RefreshToken
            {
                UserID = userId,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();
            return refreshToken;
        }
    }
}
