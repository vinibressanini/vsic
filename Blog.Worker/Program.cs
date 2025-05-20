using Blog.Worker.Settings;
using Blog.API.Settings;
using Blog.Infra.Configs;
using Blog.Shared.Interfaces;
using Blog.Infra.Services.Notification;
using Blog.Application.Handlers;
using Blog.Infra.Services.EmailRenderer;
using Blog.Application.Models;
namespace Blog.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // email service
            builder.Services.Configure<SmtpConfiguration>(
                builder.Configuration.GetSection("Smtp"));
            builder.Services.AddScoped<ISMTPClientWrapper, MailKitSmtpClientWrapper>();
            builder.Services.AddScoped<IEmailService, MailKitEmailService>();
            builder.Services.AddScoped(typeof(IEmailTemplateRenderer<>), typeof(EmailRenderer<>));

            //handlers
            builder.Services.AddScoped<PostCreatedEventHandler>();
            builder.Services.AddScoped<UserCreatedEventHandler>();

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
