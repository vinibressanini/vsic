namespace Blog.Domain.Events.Post
{
    public record PostCreatedEvent(string postName, string contentPreview) : IDomainEvent {}
}
