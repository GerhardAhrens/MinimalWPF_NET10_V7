namespace System.Windows.Domain
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; }

        DateTime? DeletedOn { get; }
    }
}
