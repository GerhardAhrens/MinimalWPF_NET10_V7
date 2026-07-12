namespace System.Windows
{
    using System.Diagnostics;

    [DebuggerDisplay("{value} <{this.GetType().Name,nq}>")]
    public abstract class KindOf<T, TPrimitive> : IEquatable<TPrimitive> where TPrimitive : KindOf<T, TPrimitive>
    {
        public T Value { get; }

        protected KindOf(T value)
        {
            if (this.GetType() != typeof(TPrimitive))
            {
                throw new ArgumentException($"Typparameter {nameof(TPrimitive)} muss sein {this.GetType().Name}, aber ist {typeof(TPrimitive).Name}");
            }

            this.Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override bool Equals(object obj) => obj is TPrimitive other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(typeof(TPrimitive), Value);

        public override string ToString() => Value.ToString();

        public bool Equals(TPrimitive other) => other != null && Equals(Value, other.Value);

        public static implicit operator T(KindOf<T, TPrimitive> kindOfT) => kindOfT.Value;

        public static bool operator ==(KindOf<T, TPrimitive> left, KindOf<T, TPrimitive> right) =>
            left is null ? right is null : left.Equals(right);

        public static bool operator !=(KindOf<T, TPrimitive> left, KindOf<T, TPrimitive> right) => !(left == right);

        public static bool operator ==(KindOf<T, TPrimitive> left, object right) =>
            right is TPrimitive other && left == other;

        public static bool operator !=(KindOf<T, TPrimitive> left, object right) =>
            !(left == right);
    }
}
