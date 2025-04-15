using Blog.Domain.Events;

namespace Blog.Domain.Entities
{
    public class Comment : Entity
    {
        public Guid Id { get; private set; }
        public Guid? ParentId { get; private set; }
        public User Author { get; private set; }
        public string Content { get; private set; }
        public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;

        // ORM
        public Comment()
        {

        }

        public Comment(Guid id, Guid? parentId, User author, string content)
        {
            Id = id;
            ParentId = parentId;
            Author = author;
            Content = content;
        }

    }
}
