namespace Blog.Domain.Events
{
    public class DomainEvent
    {
        public Guid Id { get; private init; }
        public string Event { get; private init; }
        public string Payload { get; private init; }
        public DateTime CreatedAt { get; private init; }
        public DateTime? ProcessedAt { get; private set; }
        public DomainEventStatus Status { get; private set; }

        // ORM 
        public DomainEvent() { }

        public DomainEvent(string dEvent, string payload)
        {
            Id = new Guid();
            Event = dEvent;
            Payload = payload;
            CreatedAt = DateTime.UtcNow;
            Status = DomainEventStatus.Pending;
        }

        public void Processed(DomainEventStatus status)
        {
            ProcessedAt = DateTime.UtcNow;
            Status = status;
        }
    }



    public enum DomainEventStatus
    {
        Pending,
        Processed,
        Failed
    }
}
