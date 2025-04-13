
using Blog.Domain.UserInteraction;

namespace Blog.Domain.UserManagement
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Favorite> Favorites { get; set; }

        // ORM
        public User() { }

        public User(Guid id, string name, string email, string password, ICollection<Favorite> favorites)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Favorites = favorites;
        }
    }
}
