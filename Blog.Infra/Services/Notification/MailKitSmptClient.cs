using Blog.Application.Exceptions;
using Blog.Infra.Configs;
using Blog.Shared.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Blog.Infra.Services.Notification
{
    public class MailKitSmptClient : ISMTPClient
    {

        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly ILogger<MailKitSmptClient> _logger;

        public MailKitSmptClient(IOptions<SmtpConfiguration> options, ILogger<MailKitSmptClient> logger)
        {
            _smtpConfiguration = options.Value;
            _logger = logger;
        }
        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {


            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(_smtpConfiguration.Username));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;
            email.Body = new TextPart(message.IsHtml ? "html" : "plain ")
            {
                Text = message.Body
            };


            using var smtp = new SmtpClient();


            try
            {
                await smtp.ConnectAsync(_smtpConfiguration.Server, _smtpConfiguration.Port, SecureSocketOptions.StartTls, cancellationToken);
            }
            catch (SmtpCommandException ex)
            {
                _logger.LogError(ex, $"Error while connecting to SMTP server {_smtpConfiguration.Server}");
                throw new EmailSendException("Error while connecting to SMTP server",ex);
            }

            if (smtp.Capabilities.HasFlag(SmtpCapabilities.Authentication))
            {
                try
                {
                    await smtp.AuthenticateAsync(_smtpConfiguration.Username, _smtpConfiguration.Password, cancellationToken);

                }
                catch (AuthenticationException ex)
                {
                    _logger.LogError(ex, "Error while authenticating. Wrong credentials");
                    throw new EmailSendException("Error while authenticating. Wrong credentials", ex);
                }
                catch (SmtpCommandException ex)
                {
                    _logger.LogError(ex, $"Error trying to authenticate: {ex.Message}");
                    throw new EmailSendException("Error trying to authenticate", ex);
                }

                try
                {
                    await smtp.SendAsync(email, cancellationToken);
                }
                catch (SmtpCommandException ex)
                {
                    _logger.LogError(ex,$"Error [{ex.ErrorCode}] while sending message: {ex.Message}");
                    throw new EmailSendException($"Error [{ex.ErrorCode}] while sending message: {ex.Message}", ex);
                }
            }

            await smtp.DisconnectAsync(true, cancellationToken);

        }
    }
}
