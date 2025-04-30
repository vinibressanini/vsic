using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Blog.Infra.Context
{
    public class BlogDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
    {
        public BlogDbContext CreateDbContext(string[] args)
        {
            BuildConfiguration(out IConfiguration configuration);

            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();

            BlogDbContextConfigurator.Configure(optionsBuilder, configuration);

            return new BlogDbContext(optionsBuilder.Options);
        }

        private void BuildConfiguration(out IConfiguration configuration)
        {
            configuration = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Blog.API"))
               .AddJsonFile("appsettings.json", optional: false)
               .AddJsonFile("appsettings.Development.json", optional: true)
               .AddEnvironmentVariables()
               .Build();
        }
    }
}
