using Blog.Application.Handlers;
using Blog.Domain.Events;
using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace Blog.Worker
{
    public class JobHandler
    {

        private readonly BlogDbContext context;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<JobHandler> logger;

        public JobHandler(BlogDbContext context, IServiceProvider serviceProvider, ILogger<JobHandler> logger)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        public async Task Handle()
        {
            using var scope = serviceProvider.CreateScope();

            var domainEvents = await context
                .DomainEvent
                .Where(de => de.Status == DomainEventStatus.Pending)
                .ToListAsync();

            foreach (var @event in domainEvents)
            {
                try
                {
                    var domainEvent = DeserializeEvent(@event);
                    var handlerType = FindEventHandler(@event);

                    if (domainEvent is PostScheduledEvent)
                    {
                        BackgroundJob.Schedule<PostScheduledEventHandler>(h => h.Handle((PostScheduledEvent)domainEvent), (domainEvent as PostScheduledEvent).PublishAt);
                    }
                    else
                    {
                        await HandleDomainEvent(handlerType, scope, domainEvent);
                    }
                    @event.Processed(DomainEventStatus.Processed);

                }
                catch (Exception ex)
                {
                    @event.Processed(DomainEventStatus.Failed);
                    logger.LogError(ex, $"Error while processing event {@event.Event}");
                }

                await context.SaveChangesAsync();

            }

        }

        private IDomainEvent DeserializeEvent(DomainEvent domainEvent)
        {
            var type = Assembly.Load("Blog.Domain").GetTypes().FirstOrDefault(t => t.Name == domainEvent.Event);
            return (IDomainEvent)JsonConvert.DeserializeObject(domainEvent.Payload, type!)!;
        }

        private Type FindEventHandler(DomainEvent domainEvent)
        {
            return Assembly.Load("Blog.Application").GetTypes().FirstOrDefault(t => t.Name == $"{domainEvent.Event}Handler")!;
        }

        private async Task HandleDomainEvent(Type handlerType, IServiceScope scope, IDomainEvent @event)
        {
            var handleMethod = handlerType.GetMethod("Handle");
            dynamic handler = scope.ServiceProvider.GetService(handlerType)!;

            await (Task)handleMethod!.Invoke(handler, new object[] { @event });
        }
    }
}
