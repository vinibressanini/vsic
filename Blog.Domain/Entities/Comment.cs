using Blog.Domain.Events;

namespace Blog.Domain.Entities
{
    public class Comment : Entity
    {
        public Guid Id { get;  set; }
        public Guid? ParentId { get;  set; }
        public User Author { get;  set; }
        public int AuthorId { get; set; }
        public Post Post { get; set; }
        public int PostId { get; set; }
        public string Content { get;  set; }
        public DateTime CreatedAt { get;  init; } = DateTime.UtcNow;

        // ORM
        public Comment()
        {

        }

        public Comment(Guid id, Guid? parentId, User author, Post post, string content)
        {
            Id = id;
            ParentId = parentId;
            Author = author;
            Content = content;
            Post = post;
        }

    }
}
