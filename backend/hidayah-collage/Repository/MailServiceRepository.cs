using hidayah_collage.Interface;
using hidayah_collage.Models.Email;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class MailServiceRepository : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<EmailConfig> _options;

        public MailServiceRepository(IConfiguration configuration, IOptions<EmailConfig> options)
        {
            _configuration = configuration;
            _options = options;
        }
        public async Task<HttpStatusCode> SendEmailAsync(EmailRequest emailRequest)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@gmail.com", "Hidayah admin");
            var subject = emailRequest.Subject;
            var to = new EmailAddress("test@hotmail.com");
            //var htmlContent = emailRequest.Body;
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            var status = response.StatusCode;

            return status;
        }

        public async Task SendEmailSMTPAsync(EmailRequest emailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_options.Value.Mail);
            email.To.Add(MailboxAddress.Parse(emailRequest.ToEmail));
            email.Subject = emailRequest.Subject;
            var builder = new BodyBuilder();
            if (emailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in emailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = emailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_options.Value.Host, _options.Value.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Value.Mail, _options.Value.Password);
            var status = await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
