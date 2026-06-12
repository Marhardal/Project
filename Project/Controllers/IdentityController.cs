using Humanizer;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
            }, identity.Password.Password);

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
                string emailbody = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <style>\r\n        body {\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f4f6f8;\r\n            font-family: Arial, Helvetica, sans-serif;\r\n        }\r\n\r\n        .email-container {\r\n            max-width: 520px;\r\n            margin: 40px auto;\r\n            background-color: #ffffff;\r\n            border-radius: 12px;\r\n            overflow: hidden;\r\n            box-shadow: 0 4px 16px rgba(0,0,0,0.08);\r\n        }\r\n\r\n        .email-header {\r\n            background-color: #0d6efd;\r\n            color: #ffffff;\r\n            padding: 24px;\r\n            text-align: center;\r\n        }\r\n\r\n        .email-header h2 {\r\n            margin: 0;\r\n            font-size: 22px;\r\n        }\r\n\r\n        .email-body {\r\n            padding: 32px 28px;\r\n            color: #333333;\r\n        }\r\n\r\n        .email-body p {\r\n            font-size: 15px;\r\n            line-height: 1.6;\r\n            margin: 0 0 16px;\r\n        }\r\n\r\n        .otp-box {\r\n            margin: 28px 0;\r\n            text-align: center;\r\n        }\r\n\r\n        .otp-code {\r\n            display: inline-block;\r\n            letter-spacing: 8px;\r\n            font-size: 34px;\r\n            font-weight: bold;\r\n            color: #0d6efd;\r\n            background-color: #eef4ff;\r\n            border: 1px dashed #0d6efd;\r\n            border-radius: 10px;\r\n            padding: 16px 24px;\r\n        }\r\n\r\n        .expiry-text {\r\n            text-align: center;\r\n            color: #dc3545;\r\n            font-weight: bold;\r\n            margin-top: 12px;\r\n        }\r\n\r\n        .email-footer {\r\n            background-color: #f1f3f5;\r\n            padding: 18px 24px;\r\n            text-align: center;\r\n            font-size: 13px;\r\n            color: #666666;\r\n        }\r\n\r\n        .small-text {\r\n            font-size: 13px;\r\n            color: #777777;\r\n        }\r\n    </style>\r\n</head>\r\n\r\n<body>\r\n    <div class=\"email-container\">\r\n        <div class=\"email-header\">\r\n            <h2>Account Verification</h2>\r\n        </div>\r\n\r\n        <div class=\"email-body\">\r\n            <p>Hello " + user.UserName + ",</p>\r\n\r\n            <p>Your One-Time Password (OTP) for account verification is:</p>\r\n\r\n            <div class=\"otp-box\">\r\n                <span class=\"otp-code\">" + token + "</span>\r\n                <div class=\"expiry-text\">This code expires in 5 minutes</div>\r\n            </div>\r\n\r\n            <p class=\"small-text\">\r\n                If you did not request this code, please ignore this email.\r\n                Do not share this code with anyone.\r\n            </p>\r\n\r\n            </div>\r\n\r\n        <div class=\"email-footer\">\r\n            &copy; " + DateTime.Now.Year + " Project Tracking Management System. All rights reserved.\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>";
                EmailUtility.SendMail(
                    dto.Email,
                    "PTMS OTP",
                    emailbody
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
