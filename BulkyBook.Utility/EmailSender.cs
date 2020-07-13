using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BulkyBook.Utility
{
    public class EmailSender: IEmailSender
    {
        private readonly EmailOptions _emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }
         public Task SendEmailAsync(string email, string subject, string htmlMessage)
         {
             return Execute(_emailOptions.SendGridKey, email, subject, htmlMessage);
         }
         private Task Execute(string sendGridKey, string email, string subject, string htmlMessage)
         {
             Console.WriteLine(sendGridKey);
             Console.WriteLine(email);
             var client = new SendGridClient(sendGridKey);
             var from = new EmailAddress("yufei.z222@gmail.com", "Yufei");
             // var subject = "Sending with SendGrid is Fun";
             var to = new EmailAddress(email, "End User");
             // var plainTextContent = "and easy to do anywhere, even with C#";
             // var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
             var msg = 
                 MailHelper.CreateSingleEmail(from, to, subject, "",  htmlMessage);
                
             return client.SendEmailAsync(msg);
         }
    }
}