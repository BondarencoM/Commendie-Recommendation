using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Threading.Tasks;

namespace AuthenticationService.Services
{
    public class EmailSenderStub : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("Sending message to: " + email + " Subject: " + subject);
                Console.WriteLine(htmlMessage);
            });
        }
    }
}
