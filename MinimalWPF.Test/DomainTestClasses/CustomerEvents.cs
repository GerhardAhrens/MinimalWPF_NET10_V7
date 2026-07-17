namespace MinimalWPF.Test.Sample
{

    using System.Windows.Domain;

    public sealed record CustomerCreated(EntityId<Customer> CustomerId) : DomainEvent;

    public sealed record CustomerRenamed(EntityId<Customer> CustomerId, PersonName Fullname) : DomainEvent;

    public sealed record CustomerEmailChanged(EntityId<Customer> CustomerId, Email Email) : DomainEvent;

    public sealed record CustomerMoved(EntityId<Customer> CustomerId, Address Address) : DomainEvent;

    public sealed record CustomerDeleted(EntityId<Customer> CustomerId, Customer Customer) : DomainEvent;
}
