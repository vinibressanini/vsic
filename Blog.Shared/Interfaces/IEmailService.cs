namespace Blog.Shared.Interfaces
{
    public record EmailMessage(string To, string Subject, string Body, bool IsHtml = false);
    public interface IEmailService
    {
        public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }

    public abstract class ISMTPClientWrapper
    {


        public abstract Task ConnectAsync(string host, int port, CancellationToken cancellationToken);
        public abstract Task AuthenticateAsync(string username, string password, CancellationToken cancellationToken);
        public abstract Task SendAsync(EmailMessage message, CancellationToken cancellationToken);
        public abstract Task DisconnectAsync(bool quit, CancellationToken cancellationToken);
        public abstract bool SupportsAuthentication { get; }

    }
}
