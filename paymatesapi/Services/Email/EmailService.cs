using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using paymatesapi.Models;

namespace paymatesapi.Services
{
  public class EmailService(IConfiguration configuration) : IEmailService
  {
    private readonly IConfiguration _configuration = configuration;

    public EmailBody SendEmail(EmailBody email)
    {
      var message = new MimeMessage();
      message.From.Add(MailboxAddress.Parse(email.From));
      message.To.Add(MailboxAddress.Parse(email.To));
      message.Subject = email.Subject;
      message.Body = new TextPart(TextFormat.Html) { Text = email.Body };


      Console.WriteLine(_configuration.GetSection("Email:Host").Value!);
      using (var client = new SmtpClient())
      {
        string host = _configuration.GetSection("Email:Host").Value!;
        int port = int.Parse(_configuration.GetSection("Email:Port").Value!);
        string fromEmail = _configuration.GetSection("Email:FromEmail").Value!;
        string fromEmailPassword = _configuration
            .GetSection("Email:FromEmailPassword")
            .Value!;

        client.Connect(host, port, false);
        client.Authenticate(fromEmail, fromEmailPassword);
        var result = client.Send(message);
        Console.WriteLine($"email reult: {result}");
        client.Disconnect(true);
      }
      return email;
    }
  }
}
