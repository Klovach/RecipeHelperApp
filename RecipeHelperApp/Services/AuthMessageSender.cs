using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using RecipeHelperApp.Interfaces;
using RecipeHelperApp.Settings;

namespace RecipeHelperApp.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender
    {
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        public AuthMessageSender(IOptions<SMSoptions> optionsAccessor, IEmailSender emailSender, ISmsSender smsSender)
        {
            Options = optionsAccessor.Value;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public SMSoptions Options { get; }  // set only via Secret Manager


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await _emailSender.SendEmailAsync(email, subject, message);
            
        }

        public async Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            await _smsSender.SendSmsAsync(number, message);
        }
    }
}
