namespace Blog.Shared.Interfaces
{
    public record EmailMessage(string To, string Subject, string Body, bool IsHtml = false);
    public interface ISMTPClient
    {
        public Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default);
    }
}
