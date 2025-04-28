namespace Blog.Domain.Events.User
{
    public record UserCreatedEvent (string username, string email) : IDomainEvent
    {
    }
}
