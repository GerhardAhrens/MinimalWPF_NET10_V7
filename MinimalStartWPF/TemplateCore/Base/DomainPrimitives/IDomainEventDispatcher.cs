namespace System.Windows.Domain
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
