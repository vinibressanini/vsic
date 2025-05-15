using Blog.Domain.Events;
using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PostCreatedEventHandler : IDomainEventHandler
{

    private readonly IEmailService emailService;
    private readonly BlogDbContext context;
    private readonly ILogger<PostCreatedEventHandler> logger;

    public PostCreatedEventHandler(IEmailService emailService, BlogDbContext context, ILogger<PostCreatedEventHandler> logger)
    {
        this.emailService = emailService;
        this.context = context;
        this.logger = logger;
    }

    public async Task Handle(PostCreatedEvent @event)
    {

        var subscribers = await context.Subscriber.AsNoTracking().ToListAsync();

        if (subscribers.Count == 0) return;

        try
        {
            foreach (var sub in subscribers)
            {

                EmailMessage message = new(To: sub.Email, Subject: $"New Post: {@event.PostName}", Body: @event.ContentPreview);


                await emailService.SendAsync(message);

            }

            logger.LogInformation($"Successfully notified all subscribers for Post: {@event.PostName}");

        }
        catch (Exception)
        {
            throw;
        }



    }

}