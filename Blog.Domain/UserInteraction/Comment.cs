namespace Blog.Domain.UserInteraction
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }

        // ORM
        public Comment()
        {
            
        }

        public Comment (Guid id, Guid? parentId , string author, string content)
        {
            Id = id;
            ParentId = parentId;
            Author = author;
            Content = content;
        }

    }
}
