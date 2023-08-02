using MailKit.Security;
using MimeKit;

namespace MaratukAdmin.Utils
{
    public class MailService
    {
        public static void SendEmail(string recipientEmail, string subject, string body)
        {
            // Replace these with your actual SMTP server details
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587; // Or the appropriate port for your SMTP server
            string smtpUsername = "sevakhayriyan07@gmail.com";
            string smtpPassword = "cnjpmeslxdgjfmza";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", smtpUsername));
            message.To.Add(new MailboxAddress("", recipientEmail)); // You can add a name here as the first parameter

            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect(smtpHost, smtpPort, SecureSocketOptions.StartTls);

                client.Authenticate(smtpUsername, smtpPassword);

                client.Send(message);

                client.Disconnect(true);
            }
        }
    }
}
