using Blog.Domain.Events;
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
        private List<Exception> ExceptionsAggregate = new();

        public JobHandler(BlogDbContext context,IServiceProvider serviceProvider)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
        }

        [AutomaticRetry(Attempts = 5, DelaysInSeconds = new[] { 450, 900, 1800, 1800, 1800 })]
        public async Task Handle()
        {
            using var scope = serviceProvider.CreateScope();

            var domainEvents = await context
                .DomainEvent
                .Where(de => de.Status == DomainEventStatus.Pending)
                .ToListAsync();

            foreach ( var @event in domainEvents )
            {
                try
                {
                    var domainEvent = DeserializeEvent(@event);
                    var handlerType = FindEventHandler(@event);

                    await HandleDomainEvent(handlerType, scope, domainEvent);

                    @event.Processed(DomainEventStatus.Processed);


                } catch ( Exception  )
                {
                    @event.Processed(DomainEventStatus.Failed);
                } finally
                {
                    context.DomainEvent.Update(@event);
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
