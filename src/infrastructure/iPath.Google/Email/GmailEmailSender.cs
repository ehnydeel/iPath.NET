using FluentResults;
using iPath.Application.Contracts;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.ComponentModel.DataAnnotations;


namespace iPath.Google.Email;

public class GmailEmailSender(IOptions<GmailConfig> opts, ILogger<GmailEmailSender> logger) : IEmailSender
{
    public async Task<Result> SendMailAsync([EmailAddress] string address, string subject, string body, CancellationToken ct)
    {
        try
        {
            var cfg = opts.Value;

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(cfg.SenderName, cfg.SenderEmail));
            email.To.Add(new MailboxAddress("", address));

            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                var tls = MailKit.Security.SecureSocketOptions.StartTls;

                await smtp.ConnectAsync(cfg.SmtpServer, cfg.SmtpPort, tls, ct);

                // Note: only needed if the SMTP server requires authentication
                await smtp.AuthenticateAsync(cfg.AppUsername, cfg.AppPassword, ct);

                await smtp.SendAsync(email, ct);
                await smtp.DisconnectAsync(true, ct);
            }

            logger.LogInformation("E-Mail sent to {0}", address);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending email to {0}", address);
            return Result.Fail(ex.Message);
        }
    }
}