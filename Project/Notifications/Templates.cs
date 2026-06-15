using Microsoft.Extensions.Options;
using static System.Net.Mime.MediaTypeNames;

namespace Project.Notifications
{
    public class Templates
    {
        private readonly AppSettings _app;

        public Templates(IOptions<AppSettings> app) => _app = app.Value;

        public string WelcomeUser(string username, string email, string password) => $"""
        <!DOCTYPE html>
        <html>
        <body style="font-family:Arial,sans-serif;background:#f4f4f4;padding:20px;">
            <div style="max-width:600px;margin:auto;background:#fff;border-radius:8px;overflow:hidden;">
                <div style="background:#185FA5;padding:24px;text-align:center;">
                    <h1 style="color:#fff;margin:0;">{_app.Name}</h1>
                    <p style="color:#B5D4F4;margin:4px 0 0;">{_app.Tagline}</p>
                </div>
                <div style="padding:32px;">
                    <h2 style="color:#1a1a1a;">Welcome, {username}!</h2>
                    <p style="color:#555;">Your account has been created on the {_app.Name} system. Here are your login details:</p>
                    <div style="background:#f8f9fa;border-radius:6px;padding:16px;margin:20px 0;">
                        <p style="margin:4px 0;"><strong>Email:</strong> {email}</p>
                        <p style="margin:4px 0;"><strong>Temporary Password:</strong> {password}</p>
                    </div>
                    <p style="color:#555;">Please log in and change your password immediately.</p>
                    <div style="text-align:center;margin:28px 0;">
                        <a href="{_app.BaseUrl}/login"
                           style="background:#185FA5;color:#fff;padding:12px 32px;border-radius:6px;text-decoration:none;font-weight:bold;">
                            Login to {_app.Name}
                        </a>
                    </div>
                    <p style="color:#999;font-size:12px;">
                        If you did not expect this email, please contact your system administrator.
                    </p>
                </div>
                <div style="background:#f4f4f4;padding:16px;text-align:center;">
                    <p style="color:#999;font-size:12px;margin:0;">
                        &copy; {DateTime.Now.Year} {_app.Name} — {_app.Description}
                    </p>
                </div>
            </div>
        </body>
        </html>
        """;

        public string LoginOTP(string username, string otp) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <body style="margin:0;padding:0;background-color:#f0f4f8;font-family:Arial,Helvetica,sans-serif;">
            <div style="max-width:560px;margin:40px auto;background-color:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 20px rgba(0,0,0,0.08);">
                <div style="background:linear-gradient(135deg,#185FA5,#0c3f72);padding:32px 24px;text-align:center;">
                    <h1 style="margin:0;color:#ffffff;font-size:20px;font-weight:600;">{_app.Description}</h1>
                    <p style="margin:6px 0 0;color:#B5D4F4;font-size:13px;">{_app.Tagline}</p>
                </div>
                <div style="padding:36px 32px;">
                    <p style="margin:0 0 8px;font-size:15px;color:#333;">Hello <strong>{username}</strong>,</p>
                    <p style="margin:0 0 28px;font-size:15px;color:#555;line-height:1.6;">
                        We received a request to sign in to your {_app.Name} account.
                        Use the one-time password below to complete your login.
                    </p>
                    <div style="text-align:center;margin:0 0 28px;">
                        <p style="margin:0 0 12px;font-size:13px;color:#777;text-transform:uppercase;letter-spacing:1px;">Your One-Time Password</p>
                        <div style="display:inline-block;background-color:#EEF4FF;border:1.5px dashed #185FA5;border-radius:10px;padding:18px 40px;">
                            <span style="font-size:38px;font-weight:700;letter-spacing:12px;color:#185FA5;">{otp}</span>
                        </div>
                        <p style="margin:12px 0 0;font-size:13px;color:#dc3545;font-weight:600;">⏱ This code expires in 5 minutes</p>
                    </div>
                    <div style="border-top:1px solid #f0f0f0;margin:0 0 24px;"></div>
                    <div style="background-color:#fffbeb;border-left:4px solid #f59e0b;border-radius:4px;padding:14px 16px;margin:0 0 24px;">
                        <p style="margin:0 0 6px;font-size:13px;font-weight:700;color:#92400e;">🔒 Security Notice</p>
                        <ul style="margin:0;padding-left:18px;font-size:13px;color:#78350f;line-height:1.8;">
                            <li>Never share this code with anyone</li>
                            <li>{_app.Name} staff will never ask for your OTP</li>
                            <li>If you did not request this, secure your account immediately</li>
                        </ul>
                    </div>
                </div>
                <div style="background-color:#f8f9fa;border-top:1px solid #e9ecef;padding:20px 32px;text-align:center;">
                    <p style="margin:0 0 4px;font-size:12px;color:#aaa;">This is an automated message — please do not reply.</p>
                    <p style="margin:0;font-size:12px;color:#aaa;">&copy; {DateTime.Now.Year} {_app.Name} — {_app.Description}. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>
        """;

        public string ForgotPassword(string username, string resetLink) => $"""
        <!DOCTYPE html>
        <html lang="en">
        <body style="margin:0;padding:0;background-color:#f0f4f8;font-family:Arial,Helvetica,sans-serif;">
            <div style="max-width:560px;margin:40px auto;background-color:#ffffff;border-radius:12px;overflow:hidden;box-shadow:0 4px 20px rgba(0,0,0,0.08);">
                <div style="background:linear-gradient(135deg,#185FA5,#0c3f72);padding:32px 24px;text-align:center;">
                    <h1 style="margin:0;color:#ffffff;font-size:20px;font-weight:600;">{_app.Description}</h1>
                    <p style="margin:6px 0 0;color:#B5D4F4;font-size:13px;">{_app.Tagline}</p>
                </div>
                <div style="padding:36px 32px;">
                    <div style="text-align:center;margin:0 0 28px;">
                        <div style="display:inline-block;background-color:#FEF3C7;border-radius:50%;padding:16px;">
                            <span style="font-size:32px;">🔑</span>
                        </div>
                        <h2 style="margin:16px 0 0;font-size:22px;color:#1a1a1a;">Password Reset Request</h2>
                    </div>
                    <p style="margin:0 0 8px;font-size:15px;color:#333;">Hello <strong>{username}</strong>,</p>
                    <p style="margin:0 0 28px;font-size:15px;color:#555;line-height:1.6;">
                        We received a request to reset your {_app.Name} account password.
                        Click the button below to set a new password. This link is valid for <strong>30 minutes</strong>.
                    </p>
                    <div style="text-align:center;margin:0 0 32px;">
                        <a href="{resetLink}"
                           style="display:inline-block;background-color:#185FA5;color:#ffffff;text-decoration:none;font-size:15px;font-weight:600;padding:14px 40px;border-radius:8px;">
                            Reset My Password
                        </a>
                    </div>
                    <div style="background-color:#f8f9fa;border-radius:8px;padding:14px 16px;margin:0 0 28px;word-break:break-all;">
                        <p style="margin:0 0 6px;font-size:12px;color:#999;text-transform:uppercase;letter-spacing:0.5px;">Or copy this link into your browser</p>
                        <p style="margin:0;font-size:13px;color:#185FA5;">{resetLink}</p>
                    </div>
                    <div style="border-top:1px solid #f0f0f0;margin:0 0 24px;"></div>
                    <div style="background-color:#FFF1F2;border-left:4px solid #dc3545;border-radius:4px;padding:14px 16px;margin:0 0 24px;">
                        <p style="margin:0 0 6px;font-size:13px;font-weight:700;color:#991b1b;">⚠️ Didn't request this?</p>
                        <ul style="margin:0;padding-left:18px;font-size:13px;color:#7f1d1d;line-height:1.8;">
                            <li>Ignore this email — your password will not change</li>
                            <li>Consider securing your email account</li>
                            <li>Contact your system administrator if you suspect unauthorized access</li>
                        </ul>
                    </div>
                    <p style="margin:0;font-size:13px;color:#999;line-height:1.6;">
                        This link expires in <strong>30 minutes</strong>. After that you'll need to submit a new request.
                    </p>
                </div>
                <div style="background-color:#f8f9fa;border-top:1px solid #e9ecef;padding:20px 32px;text-align:center;">
                    <p style="margin:0 0 4px;font-size:12px;color:#aaa;">This is an automated message — please do not reply.</p>
                    <p style="margin:0;font-size:12px;color:#aaa;">&copy; {DateTime.Now.Year} {_app.Name} — {_app.Description}. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>
        """;
    }
    public class AppSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;
    }
}
