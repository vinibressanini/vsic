namespace Blog.Shared.Interfaces
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        public Task Handle(T @event);
    }
}
