using System.Net;
using System.Net.Mail;
using Dem.DAL.Models;

namespace Dem.PL.Helpers
{
    public static class EmailSettings
    {
        private static IConfiguration _configuration ;
        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static async Task SendEmail(Email email)
        {
            //
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = _configuration.GetValue<int>("EmailSettings:SmtpPort");
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];
            var enableSsl = _configuration.GetValue<bool>("EmailSettings:EnableSsl");
            var message = new MailMessage();
            message.From = new MailAddress("mohammedhaggagg@gmail.com");
            message.To.Add(email.To);
            message.Subject = email.Subject;
            message.Body = email.Body;
            message.IsBodyHtml = true;

            ///var Client = new SmtpClient("smtp.gmail.com", 587);
            ///Client.Credentials = new NetworkCredential("mohammedhaggagg@gmail.com", "kyfu omjo gqew rpko");//"your_app_password_here"
            ///Client.EnableSsl = true;
            ///Client.Send("mohammedhaggagg@gmail.com", email.To, email.Subject, email.Body);
            ///Client.Send(message);
            
            var Client = new SmtpClient(smtpServer,smtpPort);
            Client.Credentials = new NetworkCredential(senderEmail,senderPassword);
            Client.EnableSsl = enableSsl;
            try
            {
                await Client.SendMailAsync(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
