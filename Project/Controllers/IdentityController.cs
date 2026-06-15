using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Project.Data;
using Project.DTO;
using Project.Models;
using Project.Notifications;
using Project.Services;
using System.Text;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly DBContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AuthService authService;
        private readonly NotificationService Notification;
        private readonly IConfiguration configuration;
        private readonly Templates templates;


        public IdentityController(DBContext _dbContext, UserManager<IdentityUser> _userManager, AuthService _authService, RoleManager<IdentityRole> _roleManager, NotificationService notificationService, IConfiguration _configuration, Templates _templates)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            authService = _authService; // ✅ assigned
            roleManager = _roleManager;
            Notification = notificationService;
            configuration = _configuration;
            templates = _templates;
        }

        // POST: api/Identity
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost("Register")]
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

            var user = new IdentityUser
            {
                UserName = identity?.Username,
                Email = identity?.Email,
                PhoneNumber = identity?.Phone,
                TwoFactorEnabled = true,
            };

            var result = await userManager.CreateAsync(user, identity.Password.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (identity.Roles != null && identity.Roles.Count > 0)
            {
                var addRolesResult = await userManager.AddToRolesAsync(user, identity.Roles);
                if (!addRolesResult.Succeeded)
                    return BadRequest(addRolesResult.Errors);
            }

            var loginUrl = configuration["App:BaseUrl"] ?? "https://localhost:7217/login";
            var Company = configuration["App:Company"];
            var body = templates.WelcomeUser(
                username: identity.Username,
                email: identity.Email,
                password: identity.Password.Password
            );

            try
            {
                await Notification.SendMail(
                    identity.Email,
                    subject: $"Welcome to {Company.ToString()} — Your Account Details",
                    body
                );
            }
            catch (Exception ex)
            {
                // Don't fail the whole request if email fails
                // Log it instead
                Console.WriteLine($"Email failed: {ex.Message}");
            }

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

                var body = templates.LoginOTP(username: dto.Email, otp: token);

                var Company = configuration["App:Company"];
                try
                {
                    await Notification.SendMail(
                        dto.Email,
                        subject: $"Welcome to {Company.ToString()} — Your Login OTP",
                        body
                    );
                }
                catch (Exception ex)
                {
                    // Don't fail the whole request if email fails
                    // Log it instead
                    Console.WriteLine($"Email failed: {ex.Message}");
                }

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
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var resetLink = $"{configuration["App:BaseUrl"]}/reset-password?email={dto.Email}&token={encodedToken}";

            var body = templates.ForgotPassword(user.UserName!, resetLink);
            var Company = configuration["App:Company"];

            try
            {
                await Notification.SendMail(user.Email!, $"{Company} — Password Reset Request", body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reset email failed: {ex.Message}");
            }

            return Ok("If the email exists, a reset link has been sent.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return BadRequest("Invalid reset request.");

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            var result = await userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("Password has been reset successfully.");
        }
    }
}
