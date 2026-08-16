namespace System.Windows.Controls
{
    public sealed class DialogValueResult
    {
        public bool Accepted { get; set; }
        public object Tag { get; set; }

        public object ResultValue { get; set; }
    }

    public sealed class DialogValueResult<T>
    {
        public bool Accepted { get; set; }
        public object Tag { get; set; }

        public T ResultValue { get; set; }
    }
}
