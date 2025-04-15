namespace Blog.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; private init; }

        //ORM
        public Category()
        {

        }

        public Category(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
