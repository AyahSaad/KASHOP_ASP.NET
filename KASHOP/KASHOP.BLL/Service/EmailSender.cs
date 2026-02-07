using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration) {
            _configuration = configuration;

        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var senderEmail = _configuration["EmailSettings:Email"];
            var password = _configuration["EmailSettings:Password"];
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, password)
            };

            return client.SendMailAsync(
                new MailMessage(from: senderEmail,
                                to: email,
                                subject,
                                htmlMessage
                                )
                {IsBodyHtml=true});
        }
    }
}
