using CarSalesSystem.Infrastructure.EmailConfiguration;

namespace CarSalesSystem.Services.Email
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
