using Blog.Domain.Entities;
using Blog.Domain.Events;
using Blog.Infra.Configs;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Context
{
    public class BlogDbContext : DbContext
    {
        #region PROPERTIES
        private readonly DatabaseConfiguration _configuration;

        #region DBSETS

        public DbSet<User> User { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<DomainEvent> DomainEvent { get; set; }

        #endregion

        public BlogDbContext(DatabaseConfiguration configuration) => _configuration = configuration;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={_configuration.Host};Database={_configuration.Database};Username={_configuration.User};Password={_configuration.Password};Port=5432");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BlogDbContext).Assembly);
        }
    }
}
