using Blog.Domain.Events;
using Blog.Domain.Events.Comment;
using Blog.Domain.Events.Post;
using System.Text.Json.Serialization;

namespace Blog.Domain.Entities
{
    public class Post : Entity
    {
        #region PROPERTIES
        public Guid Id { get; private init; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Slug { get; private set; }
        public PostStatus Status { get; private set; }
        public DateTime? PublishAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public ICollection<User> FavoritedBy { get; set; }

        private List<Comment> _comments = new();        
        public IReadOnlyCollection<Comment> Comments => _comments;

        private List<Category> _categories = new();        
        public IReadOnlyCollection<Category> Categories => _categories;
        #endregion

        #region CONSTRUCTORS
        // ORM
        public Post()
        {

        }

        public Post(Guid id, string title, string content)
        {
            Id = id;
            Title = title;
            Content = content;
        }
        #endregion

        #region DOMAIN LOGIC
        public void AddComment(Comment comment)
        {

            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                throw new Exception("The comment cant be null");
            }

            _comments.Add(comment);

            AddDomainEvents(new CommentCreatedEvent(comment));

        }

        public void Publish()
        {
            if (postHasAnyCategory())
            {
                CreatedAt = DateTime.UtcNow;
                UpdatedAt = DateTime.UtcNow;
                Status = PostStatus.Active;
                generatePostSlug();

                AddDomainEvents(new PostCreatedEvent(PostName: Title, ContentPreview: Content));
            }
        }

        public void PublishAtDate(DateTime publishAt)
        {
            if (publishAt < DateTime.UtcNow) throw new Exception("The publish date should be greater than now");

            if (postHasAnyCategory())
            {
                PublishAt = publishAt;
                CreatedAt = publishAt;
                UpdatedAt = publishAt;
                Status = PostStatus.Inactive;
                generatePostSlug();

                AddDomainEvents(new PostScheduledEvent(PostId: Id, PublishAt: publishAt));
            }


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

        private bool postHasAnyCategory()
        {
            if (!_categories.Any())
            {
                throw new Exception("Post must have atleast one category assigned");
            }

            return true;
        }

        private void generatePostSlug()
        {
            var text = Title.ToLower();

            Slug = text.Replace(" ", "-");
        }

        #endregion
    }

    public enum PostStatus
    {
        Active,
        Inactive,
    }
}
