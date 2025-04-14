
using Blog.Domain.UserInteraction;

namespace Blog.Domain.ContentManagement
{
    public class Post
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

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
        }

        public void AddComment(Comment comment)
        {

            if (string.IsNullOrWhiteSpace(comment.Content))
            {
                throw new Exception("The comment cant be null");
            }

            _comments.Add(comment);

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
