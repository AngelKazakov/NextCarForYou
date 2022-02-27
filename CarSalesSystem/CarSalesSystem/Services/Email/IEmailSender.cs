using System.Threading.Tasks;
using CarSalesSystem.Infrastructure.EmailConfiguration;

namespace CarSalesSystem.Services.Email
{
    public interface IEmailSender
    {
        public void SendEmail(Message message);

        Task SendEmailAsync(Message message);
    }
}
