using hidayah_collage.Interface;
using hidayah_collage.Models.Email;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hidayah_collage.Repository
{
    public class MailServiceRepository : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailServiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<HttpStatusCode> SendEmailAsync(EmailRequest emailRequest)
        {
            var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("myriacamellia@gmail.com", "Hidayah admin");
            var subject = emailRequest.Subject;
            var to = new EmailAddress("mahfudin14@hotmail.com");
            //var htmlContent = emailRequest.Body;
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            var status = response.StatusCode;

            return status;
        }
    }
}
