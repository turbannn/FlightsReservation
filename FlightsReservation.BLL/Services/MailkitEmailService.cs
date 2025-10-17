using FlightsReservation.DAL.Entities.Utils.Email;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using FlightsReservation.BLL.Interfaces;

namespace FlightsReservation.BLL.Services
{
    public class MailkitEmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public MailkitEmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public async Task SendEmailAsync(string to, CancellationToken ct)
        {
            if (!Parse(to))
            {
                return;
            }
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_emailSettings.ApplicationName, _emailSettings.Email));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = _emailSettings.Subject;


            // HTMLBody
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = _emailSettings.HtmlBody;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient(new ProtocolLogger("smtp.log")))
            {
                int attempts = 0;
                while (attempts <= 3)
                {
                    try
                    {
                        await smtp.ConnectAsync(_emailSettings.SmtpServer,
                            _emailSettings.SmtpPort,
                            SecureSocketOptions.SslOnConnect,
                            cancellationToken: ct);
                        await smtp.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password, cancellationToken: ct);
                        await smtp.SendAsync(emailMessage, cancellationToken: ct);
                        await smtp.DisconnectAsync(true, ct);
                        break;
                    }
                    catch
                    {
                        await smtp.DisconnectAsync(true, ct);
                        attempts++;
                    }
                }
            }

        }

        public bool Parse(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            if (!email.Contains("@") || !email.Contains("."))
                return false;

            var parts = email.Split('@');
            if (parts.Length != 2)
                return false;

            var localPart = parts[0];
            var domainPart = parts[1];

            //Letters, numbers, dot, underscore, plus
            var localRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9._%+-]+$");

            //Letters, numbers, hyphen, period, at least one period, does not start/end with a period/hyphen
            var domainRegex = new System.Text.RegularExpressions.Regex(@"^(?!\-)([a-zA-Z0-9\-]+\.)+[a-zA-Z]{2,}$");

            if (!localRegex.IsMatch(localPart))
                return false;

            if (!domainRegex.IsMatch(domainPart))
                return false;

            return true;
        }

    }
}
