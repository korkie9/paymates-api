using paymatesapi.Models;

namespace paymatesapi.Services
{
    public interface IEmailService
    {
        EmailBody SendEmail(EmailBody email);

    }
}
