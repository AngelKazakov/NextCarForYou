using System.Text;
using System.Threading.Tasks;
using CarSalesSystem.Infrastructure.EmailConfiguration;
using MailKit.Net.Smtp;
using MimeKit;

namespace CarSalesSystem.Services.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration emailConfiguration;

        public EmailSender(EmailConfiguration emailConfiguration)
        => this.emailConfiguration = emailConfiguration;


        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Sender name: " + message.SenderName);
            sb.AppendLine("Phone number: " + message.SenderPhone);
            sb.AppendLine("Sender E-mail: " + message.SenderEmail);
            sb.AppendLine("Message: " + message.Content);

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.SenderName;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = sb.ToString() };

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(emailConfiguration.SmtpServer, emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(emailConfiguration.UserName, emailConfiguration.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
