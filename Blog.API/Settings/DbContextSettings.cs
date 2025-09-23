using Blog.Infra.Context;

namespace Blog.API.Settings
{
    public static class DbContextSettings
    {
        public static WebApplicationBuilder AddBlogDbContext(this WebApplicationBuilder builder)
        {

            builder.Services.AddDbContext<BlogDbContext>(options =>
            { 
                BlogDbContextConfigurator.Configure(options, builder.Configuration);

            });

            return builder;

        }
    }
}
