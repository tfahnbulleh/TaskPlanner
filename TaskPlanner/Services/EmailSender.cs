using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPlanner.CSFiles;

namespace TaskPlanner.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private EmailSenderOption _emailSenderOption;

        public EmailSender(IOptions<EmailSenderOption> options)
        {
            _emailSenderOption = options.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            
            //obtain the api key form secret.json
            var client = new SendGridClient(_emailSenderOption.SendGridKey);
            var from = new EmailAddress(_emailSenderOption.SenderEmail);
            var to = new EmailAddress("tfahnbulleh20@gmail.com");
            var plainTextContent = message;
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
