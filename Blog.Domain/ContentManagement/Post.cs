
using Blog.Domain.UserInteraction;

namespace Blog.Domain.ContentManagement
{
    public class Post
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Comment> Comments { get; set; }

        // ORM
        public Post()
        {

        }

        public Post(Guid id, string title, string content, ICollection<Category> categories, ICollection<Comment> comments)
        {
            Id = id;
            Title = title;
            Content = content;
            Categories = categories;
            Comments = comments;
        }


    }
}
