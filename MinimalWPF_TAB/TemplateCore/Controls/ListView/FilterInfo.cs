namespace System.Windows.Controls
{
    internal class FilterInfo
    {
        public AdvancedGridViewColumn Column { get; }

        public string PropertyName { get; }

        public string FilterText { get; set; } = string.Empty;

        public FilterInfo(AdvancedGridViewColumn column, string propertyName)
        {
            Column = column;
            PropertyName = propertyName;
        }

        public bool IsEmpty =>  string.IsNullOrWhiteSpace(FilterText);
    }
}
