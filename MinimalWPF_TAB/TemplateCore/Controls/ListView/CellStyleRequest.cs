namespace System.Windows.Controls
{
    using System.Data;

    public sealed class CellStyleRequest
    {
        public required object Item { get; init; }

        public required object Value { get; init; }

        public required AdvancedGridViewColumn Column { get; init; }
    }
}
