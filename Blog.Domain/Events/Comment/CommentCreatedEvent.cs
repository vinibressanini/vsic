using Blog.Shared.Interfaces;

namespace Blog.Domain.Events.Comment
{
    public record CommentCreatedEvent (Entities.Comment Comment) : IDomainEvent
    {
    }
}
