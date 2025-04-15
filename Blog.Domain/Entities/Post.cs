using Blog.Domain.Events;
using Blog.Domain.Events.Comment;
using Blog.Domain.Events.Post;

namespace Blog.Domain.Entities
{
    public class Post : Entity
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; private init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private  set; } = DateTime.UtcNow;
        public ICollection<User> FavoritedBy { get; set; }

        private List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments;

        private List<Category> _categories = new();
        public IReadOnlyCollection<Category> Categories => _categories;

        // ORM
        public Post()
        {

        }

        public Post(Guid id, string title, string content)
        {
            Id = id;
            Title = title;
            Content = content;

            //TODO: Mover 
            var size = ((int)Math.Ceiling(content.Length * 0.33));
            var preview = content.Substring(0,size);

            AddDomainEvents(new PostCreatedEvent(postName : title,contentPreview : preview));
        }

        public void AddComment(Comment comment)
        {

            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                throw new Exception("The comment cant be null");
            }

            _comments.Add(comment);

            AddDomainEvents(new CommentCreatedEvent(comment));

        }

        public void AssignToCategory(Category category)
        {
            if (_categories.Any(c => c.Id == category.Id))
            {
                throw new Exception("Category already assigned");
            }
            _categories.Add(category);
        }

        public void RemoveAssignedCategory(Category category)
        {
            var assignedCategory = _categories.FirstOrDefault(c => c.Id == category.Id);

            if (assignedCategory == null)
            {
                throw new Exception("Category isn't assigned");
            }
            _categories.Remove(assignedCategory);
        }


    }
}
