using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.DTO;
using Project.Models;
using Project.Services;
using System.Net;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly AuthService authService; // ✅ readonly

        public IdentityController(DBContext _dbContext, UserManager<IdentityUser> _userManager, AuthService _authService)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            authService = _authService; // ✅ assigned
        }

        // POST: api/Identity
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(IdentityDTO identity)
        {
            if (identity == null)
            {
                return NoContent();
            }

            var existingUser = await userManager.FindByEmailAsync(identity.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists.");
            }

            var existingUsername = await userManager.FindByNameAsync(identity.Username);
            if (existingUsername != null)
            {
                return BadRequest("Username already exists.");
            }

            var result = await userManager.CreateAsync(new IdentityUser
            {
                UserName = identity?.Username,
                Email = identity?.Email,
                PhoneNumber = identity?.Phone,
                TwoFactorEnabled = true,
            }, identity.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<PreAuthResponseDTO>> Login(LoginDTO dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return Unauthorized("Invalid login attempt");

            var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
                return Unauthorized("Invalid login attempt");

            if (await userManager.GetTwoFactorEnabledAsync(user))
            {
                var token = await userManager.GenerateTwoFactorTokenAsync(
                    user,
                    TokenOptions.DefaultEmailProvider
                );

                // Send token by email here

                EmailUtility.SendMail(
                    dto.Email,
                    "Reset your password",
                    $"Here is your OTP {token}"
                );

                return Ok(new PreAuthResponseDTO
                {
                    UserId = user.Id
                });
            }

            // create JWT / login normally here

            return Ok(new
            {
                requiresTwoFactor = false,
                message = "Login successful"
            });
        }

        [HttpPost("login-2fa")]
        public async Task<ActionResult<AuthDTO>> Login2FA(Login2FADTO dto)
        {
            var user = await userManager.FindByIdAsync(dto.userID);
            if (user == null)
                return Unauthorized();

            var valid = await userManager.VerifyTwoFactorTokenAsync(
                user,
                TokenOptions.DefaultEmailProvider,
                dto.Code
            );

            if (!valid)
                return Unauthorized("Invalid OTP");

            // create JWT / login normally here
            //var roles = await userManager.GetRolesAsync(user);
            var token = authService.GenerateJwtToken(user);
            return Ok(new AuthDTO
            {
                Token = token,
                UserID = dto.userID
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            // Do not reveal whether email exists
            if (user == null)
                return Ok("If the email exists, a reset link has been sent.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = WebUtility.UrlEncode(token);

            var resetLink =
                $"https://yourfrontend.com/reset-password?email={dto.Email}&token={encodedToken}";

            EmailUtility.SendMail(
                dto.Email,
                "Reset your password",
                $"Click this link to reset your password: {resetLink}"

            );

            return Ok("If the email exists, a reset link has been sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return BadRequest("Invalid reset request.");

            var result = await userManager.ResetPasswordAsync(
                user,
                dto.Token,
                dto.NewPassword
            );

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset successfully.");
        }
    }
}
