using Blog.Application.Exceptions;
using Blog.Infra.Configs;
using Blog.Shared.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using System.Net.Sockets;

namespace Blog.Infra.Services.Notification
{
    public class MailKitEmailService : IEmailService
    {

        private readonly SmtpConfiguration _smtpConfiguration;
        private readonly ISMTPClientWrapper _smtpClient;

        public MailKitEmailService(IOptions<SmtpConfiguration> options, ISMTPClientWrapper smtpClient)
        {
            _smtpConfiguration = options.Value;
            _smtpClient = smtpClient;
        }
        public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
        {

            try
            {
                await _smtpClient.ConnectAsync(_smtpConfiguration.Server, _smtpConfiguration.Port, cancellationToken);
            }
            catch (SmtpCommandException ex)
            {
                throw new EmailSendException("Error while connecting to SMTP server", ex);
            }
            catch (SocketException ex)
            {
                throw new EmailSendException("Error while connecting to SMTP server. Check the host address", ex);
            }

            if (_smtpClient.SupportsAuthentication)
            {
                try
                {
                    await _smtpClient.AuthenticateAsync(_smtpConfiguration.Username, _smtpConfiguration.Password, cancellationToken);

                }
                catch (AuthenticationException ex)
                {
                    throw new EmailSendException("Error while authenticating. Wrong credentials", ex);
                }
                catch (SmtpCommandException ex)
                {
                    throw new EmailSendException("Error trying to authenticate", ex);
                }

            }
            try
            {
                await _smtpClient.SendAsync(message, cancellationToken);
            }
            catch (SmtpCommandException ex)
            {
                throw new EmailSendException($"Error [{ex.ErrorCode}] while sending message: {ex.Message}", ex);
            }

            await _smtpClient.DisconnectAsync(true, cancellationToken);

        }
    }
}
