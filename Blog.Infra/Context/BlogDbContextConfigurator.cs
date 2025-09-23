using Blog.Infra.Configs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Infra.Context
{
    public static class BlogDbContextConfigurator
    {
        public static void Configure(DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
        {
            var config = configuration.GetSection("Database").Get<DatabaseConfiguration>();

            var connString = $"Host={config!.Host};Database={config.Database};Username={config.User};Password={config.Password};Port=5432";
            
            optionsBuilder.UseNpgsql(connString);

            optionsBuilder.AddInterceptors(new AddDomainEventInterceptor());
        }
    }
}
