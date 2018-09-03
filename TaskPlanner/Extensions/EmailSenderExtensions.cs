using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TaskPlanner.Services;

namespace TaskPlanner.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
        public static Task SendResetPasswordLink(this IEmailSender emailSender,string email,string link)
        {
            return emailSender.SendEmailAsync(email, "Password Reset",
                $"You have requested a password reset operation. If you did not initiate this request, ignore this email. If you did, click <a href='{HtmlEncoder.Default.Encode(link)}'>here</a> to reset your password");
        }
    }
}
