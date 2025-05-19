using Blog.Domain.Entities;
using Blog.Infra.Context;
using Blog.Infra.Services.Notification.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Fixtures
{
    public class BlogDbContextFixture
    {
        public BlogDbContext context;
        public BlogDbContextFixture()
        {


            var options = new DbContextOptionsBuilder<BlogDbContext>()
                .UseInMemoryDatabase(databaseName: new Guid().ToString())
                .AddInterceptors(new AddDomainEventInterceptor())
                .Options;


            context = new BlogDbContext(options);

            context.Database.EnsureDeleted();

            SeedData();


        }

        private void SeedData()
        {

            context.Subscriber.AddRange(
                new Subscriber() { Id = new Guid(), Email = "subscriber1@email.com" },
                new Subscriber() { Id = new Guid(), Email = "subscriber2@email.com" },
                new Subscriber() { Id = new Guid(), Email = "subscriber3@email.com" }
                );

            var categories = new List<Category>()
            {
                new Category() { Id = new Guid(), Name = "Tech"},
                new Category() { Id = new Guid(), Name = "Games"},
            };

            var scheduledPost = new Post(id: new Guid(), title: "Scheduled Post", content: "Content for the scheduled post");
            scheduledPost.AssignToCategory(categories.First());
            scheduledPost.PublishAtDate(DateTime.Now.AddDays(2));


            var posts = new List<Post>()
            {
                new Post(id: new Guid(), title: "First Post", content: "Generic content for the first post"),
                new Post(id: new Guid(), title: "Second Post", content: "Generic content for the second post"),
                new Post(id: new Guid(), title: "Third Post", content: "Generic content for the third post")

             };

            foreach (var post in posts)
            {
                post.AssignToCategory(categories.First());
                post.Publish();
            }

            context.Category.AddRange(categories);

            context.Post.AddRange(posts);
            context.Post.Add(scheduledPost);

            context.SaveChanges();

        }

    }
}
