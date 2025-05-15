using Blog.Shared.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

public class MailKitSmtpClientWrapper : ISMTPClientWrapper
{
    private readonly SmtpClient _client = new();

    public override async Task ConnectAsync(string host, int port, CancellationToken cancellationToken)
    {
        await _client.ConnectAsync(host, port, SecureSocketOptions.Auto, cancellationToken);
    }

    public override async Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
    {
        await _client.AuthenticateAsync(username, password, cancellationToken);
    }

    public override async Task SendAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.To.Add(MailboxAddress.Parse(message.To));
        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = new TextPart(message.IsHtml ? "html" : "plain")
        {
            Text = message.Body
        };

        await _client.SendAsync(mimeMessage, cancellationToken);
    }

    public override Task DisconnectAsync(bool quit, CancellationToken cancellationToken)
    {
        return _client.DisconnectAsync(quit, cancellationToken);
    }

    public override bool SupportsAuthentication =>
        _client.Capabilities.HasFlag(SmtpCapabilities.Authentication);
}
