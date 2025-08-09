using Blog.Shared.Interfaces;

namespace Blog.Domain.Events.User
{
    public record UserCreatedEvent (string Username, string Email) : IDomainEvent
    {
    }
}
