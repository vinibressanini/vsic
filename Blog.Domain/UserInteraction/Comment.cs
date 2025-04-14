using Blog.Domain.UserManagement;

namespace Blog.Domain.UserInteraction
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public User Author { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ORM
        public Comment()
        {
            
        }

        public Comment (Guid id, Guid? parentId , User author, string content)
        {
            Id = id;
            ParentId = parentId;
            Author = author;
            Content = content;
        }

    }
}
