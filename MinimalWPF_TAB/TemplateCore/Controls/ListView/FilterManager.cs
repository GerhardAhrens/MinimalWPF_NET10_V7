namespace System.Windows.Controls
{
    using System.ComponentModel;
    using System.Data;
    using System.Text;
    using System.Windows.Data;

    internal class FilterManager
    {
        private readonly AdvancedListView _listView;

        private readonly Dictionary<AdvancedGridViewColumn, FilterInfo> _filters = new();

        public FilterManager(AdvancedListView listView)
        {
            _listView = listView;
        }

        public IEnumerable<FilterInfo> Filters
        {
            get => _filters.Values;
        }

        public void Clear()
        {
            _filters.Clear();
        }

        public FilterInfo RegisterColumn(AdvancedGridViewColumn column, string propertyName)
        {
            if (_filters.TryGetValue(column, out FilterInfo info))
                return info;

            info = new FilterInfo(column, propertyName);

            _filters.Add(column, info);

            return info;
        }

        public void SetFilter(AdvancedGridViewColumn column, string value)
        {
            if (!_filters.TryGetValue(column, out FilterInfo info))
                return;

            info.FilterText = value ?? string.Empty;

            Refresh();
        }

        public FilterInfo GetFilter(AdvancedGridViewColumn column)
        {
            _filters.TryGetValue(column, out FilterInfo filter);
            return filter;
        }

        private void Refresh()
        {
            if (_listView.ItemsSource == null)
                return;

            ICollectionView view =CollectionViewSource.GetDefaultView(_listView.ItemsSource);

            if (view is not BindingListCollectionView blcv)
                return;

            if (blcv.SourceCollection is not DataView dataView)
                return;

            dataView.RowFilter = BuildRowFilter();
        }

        private string BuildRowFilter()
        {
            StringBuilder sb = new();

            foreach (FilterInfo filter in _filters.Values)
            {
                if (filter.IsEmpty)
                    continue;

                string expression = BuildExpression(filter);

                if (string.IsNullOrWhiteSpace(expression))
                    continue;

                if (sb.Length > 0)
                    sb.Append(" AND ");

                sb.Append(expression);
            }

            return sb.ToString();
        }

        private static string BuildExpression(FilterInfo filter)
        {
            string field = $"[{filter.PropertyName}]";
            string value = filter.FilterText.Trim();

            string Escape(string s) => s.Replace("'", "''");

            if (value.StartsWith(">="))
            {
                string operand = value[2..].Trim();
                if (string.IsNullOrEmpty(operand))
                    return null;

                return $"{field} >= {operand}";
            }

            if (value.StartsWith("<="))
            {
                string operand = value[2..].Trim();
                if (string.IsNullOrEmpty(operand))
                    return null;

                return $"{field} <= {operand}";
            }

            if (value.StartsWith(">"))
            {
                string operand = value[1..].Trim();
                if (string.IsNullOrEmpty(operand))
                    return null;

                return $"{field} > {operand}";
            }

            if (value.StartsWith(">="))
                return $"{field} >= {Escape(value.Substring(2).Trim())}";

            if (value.StartsWith("<="))
                return $"{field} <= {Escape(value.Substring(2).Trim())}";

            if (value.StartsWith("<>"))
                return $"{field} <> '{Escape(value.Substring(2).Trim())}'";

            if (value.StartsWith(">"))
                return $"{field} > {Escape(value.Substring(1).Trim())}";

            if (value.StartsWith("<"))
                return $"{field} < {Escape(value.Substring(1).Trim())}";

            if (value.StartsWith("="))
                return $"Convert({field}, 'System.String') = '{Escape(value.Substring(1).Trim())}'";

            if (value.StartsWith("*"))
                return $"Convert({field}, 'System.String') LIKE '%{Escape(value.Substring(1))}'";

            if (value.EndsWith("*"))
                return $"Convert({field}, 'System.String') LIKE '{Escape(value[..^1])}%'";

            return $"Convert({field}, 'System.String') LIKE '%{Escape(value)}%'";
        }
    }
}
