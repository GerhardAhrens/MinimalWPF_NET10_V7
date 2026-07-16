namespace System.Windows.Domain
{
    public interface IDomainEvent
    {
        DateTime OccurredOn { get; }
    }
}
