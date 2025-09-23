using Blog.Domain.Events;
using Blog.Domain.Events.User;

namespace Blog.Domain.Entities
{
    public class User : Entity
    {
        public Guid Id { get; private init; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public ICollection<Comment> Comments { get; set; }

        private List<Post> _favorites = new();
        public IReadOnlyCollection<Post> Favorites => _favorites.AsReadOnly();

        // ORM
        public User() { }

        public User(Guid id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;

            AddDomainEvents(new UserCreatedEvent(Username: name, Email: email));
        }


        public void FavoritePost(Post post)
        {
            Post? favorite = _favorites.FirstOrDefault(f => f.Id == post.Id);

            if (favorite == null)
            {
                _favorites.Add(post);
            }
            else
            {
                _favorites.Remove(post);
            }


        }
    }
}
