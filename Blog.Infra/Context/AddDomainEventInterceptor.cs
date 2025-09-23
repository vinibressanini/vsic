using Blog.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace Blog.Infra.Context
{
    public class AddDomainEventInterceptor : SaveChangesInterceptor
    {

        public async override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null) await AddDomainEvents(eventData.Context);

            return result;
        }


        private async Task AddDomainEvents(DbContext context)
        {


            var events = context
                .ChangeTracker
                .Entries<Entity>()
                .Select(e => e.Entity)
                .SelectMany(entity =>
                {
                    var domainEvents = entity.GetDomainEvents();

                    if (domainEvents.Count == 0) return [];

                    entity.ClearDomainEvents();

                    return domainEvents.Select(evt =>
                    {

                        var type = evt.GetType().Name;
                        var payload = JsonConvert.SerializeObject(evt);

                        return new DomainEvent(dEvent: type, payload: payload);
                    });

                }).ToList();


            await Task.Run(() => events.ForEach(async domainEvent => await context.AddAsync(domainEvent)));
        }

    }
}
