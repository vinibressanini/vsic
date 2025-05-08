using Blog.Application.Exceptions;
using Blog.Domain.Events;
using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class PostCreatedEventHandler : IDomainEventHandler
{

    private readonly ISMTPClient smtpClient;
    private readonly BlogDbContext context;
    private readonly ILogger<PostCreatedEventHandler> logger;

    public PostCreatedEventHandler(ISMTPClient smtpClient, BlogDbContext context, ILogger<PostCreatedEventHandler> logger)
    {
        this.smtpClient = smtpClient;
        this.context = context;
        this.logger = logger;
    }

    public async Task Handle(PostCreatedEvent @event)
    {

        var subscribers = await context.Subscriber.AsNoTracking().ToListAsync();

        if (subscribers.Count == 0) return;

        try
        {
            subscribers.ForEach(async sub =>
            {
                EmailMessage message = new(To: sub.Email, Subject: $"New Post: {@event.PostName}", Body: @event.ContentPreview);

                await smtpClient.SendAsync(message);
            });

            logger.LogInformation($"Successfully notified all subscriber | Post: {@event.PostName}");

        }
        catch (EmailSendException ex)
        {
            logger.LogError(ex, "An error occurred while sending emails");
            throw;
        }



    }

}