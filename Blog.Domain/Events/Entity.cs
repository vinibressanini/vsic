
namespace Blog.Domain.Events
{
    public class Entity
    {

        private List<IDomainEvent> _domainEvents = new();


        public void AddDomainEvents(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents.Clear();


    }
}
