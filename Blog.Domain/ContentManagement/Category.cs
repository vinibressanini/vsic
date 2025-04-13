namespace Blog.Domain.ContentManagement
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Post> Posts { get; set; }

        //ORM
        public Category()
        {
            
        }

        public Category(Guid id, string name, ICollection<Post> posts)
        {
            Id = id;
            Name = name;
            Posts = posts;
        }
    }
}
