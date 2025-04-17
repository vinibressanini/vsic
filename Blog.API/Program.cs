using Blog.Infra.Configs;
using Blog.Infra.Context;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Blog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.Configure<DatabaseConfiguration>(builder.Configuration.GetSection("Database"));
            builder.Services.AddDbContext<BlogDbContext>();
            
            
            var app = builder.Build();



            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
