using Blog.Shared.Interfaces;

namespace Blog.Application.Models
{
    public class WelcomeEmailModel : IEmailModel
    {
        public string Username { get; set; }
        public List<PostEmailModel> Posts { get; set; }
    }

    public class PostEmailModel 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
    }
}
