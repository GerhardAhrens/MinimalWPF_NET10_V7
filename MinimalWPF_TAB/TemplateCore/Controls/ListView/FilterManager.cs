namespace System.Windows.Controls
{
    using System.ComponentModel;
    using System.Windows.Data;

    internal class FilterManager
    {
        private readonly AdvancedListView _listView;

        private readonly Dictionary<AdvancedGridViewColumn, FilterInfo> _filters = new();

        public FilterManager(AdvancedListView listView)
        {
            _listView = listView;
        }

        public void RegisterColumn(
            AdvancedGridViewColumn column,
            string propertyName)
        {
            if (_filters.ContainsKey(column))
                return;

            _filters.Add(
                column,
                new FilterInfo(column, propertyName));
        }

        public void SetFilter(
            AdvancedGridViewColumn column,
            string text)
        {
            if (!_filters.TryGetValue(column, out var filter))
                return;

            filter.FilterText = text ?? "";

            Refresh();
        }

        private void Refresh()
        {
            ICollectionView view =
                CollectionViewSource.GetDefaultView(
                    _listView.ItemsSource);

            view?.Refresh();
        }
    }
}
