namespace System.Windows.Controls
{
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
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

                if (sb.Length > 0)
                    sb.Append(" AND ");

                string value = filter.FilterText
                    .Replace("'", "''");

                sb.AppendFormat(
                    "Convert([{0}], 'System.String') LIKE '%{1}%'",
                    filter.PropertyName,
                    value);
            }

            return sb.ToString();
        }
    }
}
