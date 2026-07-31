namespace System.Windows.Domain
{
    public abstract class AggregateRoot<TId>: Entity<TId>
        where TId : notnull
    {
        private readonly List<IDomainEvent> _events = new();

        protected AggregateRoot(TId id) : base(id)
        {
        }

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

        protected void Raise(IDomainEvent domainEvent)
        {
            _events.Add(domainEvent);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }
    }
}
