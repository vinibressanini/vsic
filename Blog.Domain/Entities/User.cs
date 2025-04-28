using Blog.Domain.Events;
using Blog.Domain.Events.User;

namespace Blog.Domain.Entities
{
    public class User : Entity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        private List<Favorite> _favorites = new();
        public IReadOnlyCollection<Favorite> Favorites => _favorites.AsReadOnly();

        // ORM
        public User() { }

        public User(Guid id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;

            AddDomainEvents(new UserCreatedEvent(username: name,email: email));
        }

        public void FavoritePost(Guid postId)
        {
            if (_favorites.Any(f => f.PostId == postId))
            {
                //TODO: Exceção personalizada
                throw new Exception("Post already favorite");
            }
            _favorites.Add(new Favorite { PostId = postId, UserId = Id });
        }

        public void UnfavoritePost(Guid postId)
        {
            Favorite? favorite = _favorites.FirstOrDefault(f => f.PostId == postId);

            if (favorite == null)
            {
                //TODO: Exceção personalizada
                throw new Exception("Post insn't a favorite");
            }

            _favorites.Remove(favorite);

        }
    }
}
