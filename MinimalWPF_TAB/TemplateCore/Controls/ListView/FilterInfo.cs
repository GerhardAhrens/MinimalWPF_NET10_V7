namespace System.Windows.Controls
{
    internal class FilterInfo
    {
        public FilterInfo(AdvancedGridViewColumn column, string propertyName)
        {
            Column = column;
            PropertyName = propertyName.Trim('[', ']');
        }

        public AdvancedGridViewColumn Column { get; }

        public string PropertyName { get; }

        public FilterType FilterType { get; set; }

        public string FilterText { get; set; } = string.Empty;

        public bool IsEmpty =>  string.IsNullOrWhiteSpace(FilterText);
    }
}
