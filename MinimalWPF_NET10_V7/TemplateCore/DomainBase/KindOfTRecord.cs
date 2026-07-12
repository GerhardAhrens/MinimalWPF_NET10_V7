namespace System.Windows
{
    public abstract record KindOfRecord<T>
    {
        protected KindOfRecord(T value) => Value = value ?? throw new ArgumentNullException(nameof(value));

        public T Value { get; }

        public override string ToString() => Value.ToString();

        public static implicit operator T(KindOfRecord<T> kindOf) => kindOf.Value;
    }

    public abstract record KindOfRecordTooSimple<T>(T Value)
    {
        public static implicit operator T(KindOfRecordTooSimple<T> kindOf) => kindOf.Value;
    }
}
