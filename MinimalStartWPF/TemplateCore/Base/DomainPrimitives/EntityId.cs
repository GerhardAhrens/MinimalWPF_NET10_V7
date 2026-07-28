namespace System.Windows.Domain
{
    public readonly record struct EntityId<T>(Guid Value)
    {
        public override string ToString()  => Value.ToString();

        public static implicit operator Guid(EntityId<T> id) => id.Value;

        public static explicit operator EntityId<T>(Guid value) => new(value);
    }

    public abstract class Entity<TId> where TId : notnull
    {
        public TId Id { get; }

        protected Entity(TId id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Entity<TId> other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetType() == other.GetType() && EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType(), Id);
        }

        public static bool operator ==(Entity<TId> left,Entity<TId> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !Equals(left, right);
        }
    }

    public static class EntityId
    {
        public static EntityId<T> New<T>() => new(Guid.CreateVersion7());

        public static EntityId<T> Empty<T>() => new(Guid.Empty);

        public static EntityId<T> Create<T>(Guid value)  => new(value);
    }
}
