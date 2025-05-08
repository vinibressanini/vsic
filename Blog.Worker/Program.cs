using Blog.Worker.Settings;
using Blog.API.Settings;
using Blog.Infra.Configs;
using Blog.Shared.Interfaces;
using Blog.Infra.Services.Notification;

namespace Blog.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<SmtpConfiguration>(
                builder.Configuration.GetSection("Smtp"));

            builder.Services.AddScoped<ISMTPClient, MailKitSmptClient>();
            builder.Services.AddScoped<PostCreatedEventHandler>();

            builder.AddBlogDbContext();
            builder.Services.AddHostedService<Listener>();
            builder.AddHangfire();
            builder.AddLogging();

            var app = builder.Build();


            // Configure the HTTP request pipeline.


            app.Run();
        }
    }
}
