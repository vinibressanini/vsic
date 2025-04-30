using Blog.Domain.Entities;

namespace Blog.Domain.Events.Comment
{
    public record CommentCreatedEvent (Entities.Comment Comment) : IDomainEvent
    {
    }
}
