using Blog.Domain.Entities;
using Blog.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Context
{
    public class BlogDbContext : DbContext
    {

        #region DBSETS

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<DomainEvent> DomainEvent { get; set; }

        #endregion

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
        }
    }
}
