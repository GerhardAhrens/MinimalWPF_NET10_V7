namespace System.Windows.Domain
{
    public abstract class AuditableAggregateRoot<TId> : AggregateRoot<TId>, IAuditable
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedFrom { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public string ModifiedFrom { get; set; }

        protected AuditableAggregateRoot(TId id) : base(id)
        {
        }
    }
}
