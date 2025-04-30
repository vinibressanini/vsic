namespace Blog.Domain.Events.Post
{
    public record PostCreatedEvent(string PostName, string ContentPreview) : IDomainEvent {}
}
