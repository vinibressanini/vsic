namespace Blog.Domain.Events.Post
{
    public record PostScheduledEvent (Guid PostId, DateTime PublishAt) : IDomainEvent
    {
    }
}
