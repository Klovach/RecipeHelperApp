using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace RecipeHelperApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        private readonly string _sendGridKey; 

        // Email Sender
        public EmailSender(IOptions<AuthMessageSenderOptions> config,
                           ILogger<EmailSender> logger)
        {
            _sendGridKey = config.Value.SendGridKey;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(_sendGridKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(_sendGridKey, subject, message, toEmail);
        }

        public async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            // var sender = new SendSenderClient(sender); 
            var msg = new SendGridMessage()
            {
                // Change this email address as needed. Email **must** be the same as sender in SendGrid. 
                From = new EmailAddress("mealmaven@em8996.markusportfolio.pro", "Meal Maven : " + subject),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Email to {toEmail} failed with status code {response.StatusCode}");
        }
    }
}

