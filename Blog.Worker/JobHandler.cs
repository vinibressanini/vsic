using Blog.Application.Handlers;
using Blog.Domain.Events;
using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

namespace Blog.Worker
{
    public class JobHandler
    {

        private readonly BlogDbContext context;
        private readonly IServiceScopeFactory serviceProvider;
        private readonly ILogger<JobHandler> logger;
        private readonly IBackgroundJobClient backgroundJobClient;

        public JobHandler(BlogDbContext context, IServiceScopeFactory serviceProvider, IBackgroundJobClient backgroundJobClient, ILogger<JobHandler> logger)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.backgroundJobClient = backgroundJobClient;
        }

        // Handles events based on the given status 

        public async Task Handle(DomainEventStatus eventStatus)
        {
            using var scope = serviceProvider.CreateScope();

            var domainEvents = await context
                .DomainEvent
                .Where(de => de.Status == eventStatus)
                .ToListAsync();

            foreach (var @event in domainEvents)
            {
                try
                {
                    var domainEvent = DeserializeEvent(@event);
                    var handlerType = FindEventHandler(domainEvent);

                    if (domainEvent is PostScheduledEvent scheduledEvent)
                    {
                        BackgroundJob.Schedule<PostScheduledEventHandler>(h => h.Handle(scheduledEvent), scheduledEvent.PublishAt);
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

        private Type FindEventHandler(IDomainEvent domainEvent)
        {
            return typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        }

        private async Task HandleDomainEvent(Type handlerType, IServiceScope scope, IDomainEvent @event)
        {
            var handleMethod = handlerType.GetMethod("Handle");
            dynamic handler = scope.ServiceProvider.GetService(handlerType)!;

            await (Task)handleMethod!.Invoke(handler, new object[] { @event });
        }
    }
}
