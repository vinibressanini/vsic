namespace Blog.Domain.Events.Post
{
    public record PostUpdatedEvent (string postName) : IDomainEvent
    {
    }
}
