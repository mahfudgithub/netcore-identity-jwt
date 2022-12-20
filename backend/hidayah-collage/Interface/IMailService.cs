using hidayah_collage.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hidayah_collage.Interface
{
    public interface IMailService
    {
        Task<HttpStatusCode> SendEmailAsync(EmailRequest emailRequest);
        Task SendEmailSMTPAsync(EmailRequest emailRequest);
    }
}
