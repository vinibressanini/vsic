using Blog.Worker.Settings;
using Blog.API.Settings;
using Blog.Infra.Configs;
using Blog.Shared.Interfaces;
using Blog.Infra.Services.Notification;
using Blog.Application.Handlers;
using Blog.Infra.Services.EmailRenderer;
using Blog.Domain.Events.Post;
using Blog.Domain.Events.User;
using Hangfire;
using Blog.Domain.Events;
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
            builder.Services.AddScoped<IDomainEventHandler<PostCreatedEvent>,PostCreatedEventHandler>();
            builder.Services.AddScoped<IDomainEventHandler<UserCreatedEvent>,UserCreatedEventHandler>();
            builder.Services.AddScoped<IDomainEventHandler<PostScheduledEvent>,PostScheduledEventHandler>();

            builder.AddBlogDbContext();
            builder.Services.AddHostedService<Listener>();
            builder.AddHangfire();
            builder.AddLogging();

            var app = builder.Build();

            // Recurring job for handling failed domain events
            RecurringJob.AddOrUpdate<JobHandler>(
                "failed-jobs",
                h => h.Handle(DomainEventStatus.Failed),
                Cron.Hourly);


            // Configure the HTTP request pipeline.


            app.Run();
        }
    }
}
