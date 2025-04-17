namespace Blog.Domain.Events.Post
{
    public record PostScheduledEvent (Guid postId, DateTime publishAt) : IDomainEvent
    {
    }
}
