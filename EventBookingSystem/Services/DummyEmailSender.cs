using Microsoft.AspNetCore.Identity.UI.Services;

namespace EventBookingSystem.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        { 
            Console.WriteLine($"Email to {email}, Subject: {subject}, Message: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}