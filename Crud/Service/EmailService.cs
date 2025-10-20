using Crud.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Crud.Service
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }

        public class EmailTemplateBuilder
        {
            public string BuildContactEmailTemplate(string body, string subject)
        {
              return $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <title>{subject}</title>
                </head>
                <body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f4f4f4;"">
                    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""padding: 30px 0;"">
                        <tr>
                            <td align=""center"">
                                <table width=""600"" cellpadding=""0"" cellspacing=""0""
                                    style=""background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 10px rgba(0,0,0,0.1);"">
                    
                                    <!-- Logo Header -->
                                    <tr>
                                        <td style=""background-color: #FBD241; padding: 20px; text-align: center;"">
                                            <img src=""https://res.cloudinary.com/dnh4lkqlw/image/upload/v1747454491/logo_yoqhzt.png""
                                                alt=""Your Logo"" style=""max-width: 70px;"">
                                        </td>
                                    </tr>

                                    <!-- Content -->
                                    <tr>
                                        <td style=""padding: 30px;"">
                                            <h2 style=""color: #333;"">{subject}</h2>

                                            {body}
                                        </td>
                                    </tr>

                                    <!-- Footer -->
                                    <tr>
                                        <td style=""background-color: #f0f0f0; text-align: center; padding: 20px; font-size: 12px; color: #777;"">
                                            &copy; {DateTime.Now.Year} nice2strangers. All rights reserved.
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
               }
        }


        //public async Task SendEmailAsync(string toEmail, string subject, string rawBody)
        //{
        //    var template = new EmailTemplateBuilder();
        //    string finalHtml = template.BuildContactEmailTemplate(rawBody, subject);

        //    var email = new MimeMessage();
        //    email.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.Email));
        //    email.To.Add(MailboxAddress.Parse(toEmail));
        //    email.Subject = subject;
        //    email.Body = new TextPart("html") { Text = finalHtml };

        //    using var smtp = new SmtpClient();
        //    await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
        //    await smtp.AuthenticateAsync(_smtpSettings.Email, _smtpSettings.Password);
        //    await smtp.SendAsync(email);
        //    await smtp.DisconnectAsync(true);
        //}


        public async Task SendEmailAsync(string toEmail, string subject, string rawBody)
        {
            var template = new EmailTemplateBuilder();
            string finalHtml = template.BuildContactEmailTemplate(rawBody, subject);

            var email = new MimeMessage();

            // 👇 use support@nice2strangers.org as sender (if configured)
            email.From.Add(new MailboxAddress(_smtpSettings.SenderName,
                string.IsNullOrEmpty(_smtpSettings.FromAddress)
                    ? _smtpSettings.Email
                    : _smtpSettings.FromAddress));

            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("html") { Text = finalHtml };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.Email, _smtpSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }


    }
}
