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

            AddDomainEvents(new UserCreatedEvent(username: name,email: email));
        }
        
        public void FavoritePost(Guid Id)
        {
            if (_favorites.Any(f => f.Id == Id))
            {
                //TODO: Exceção personalizada
                throw new Exception("Post already favorite");
            }

            ///_favorites.Add(new Favorite { Id = Id, UserId = Id });
        }

        public void UnfavoritePost(Guid Id)
        {
            Post? favorite = _favorites.FirstOrDefault(f => f.Id == Id);

            if (favorite == null)
            {
                //TODO: Exceção personalizada
                throw new Exception("Post insn't a favorite");
            }

            _favorites.Remove(favorite);

        }
    }
}
