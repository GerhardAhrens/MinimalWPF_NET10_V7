namespace System.Windows.Controls
{
    using System.ComponentModel;
    using System.Windows.Data;
    using System.Windows.Documents;

    internal class SortingManager
    {
        private readonly AdvancedListView _listView;

        private GridViewColumnHeader _lastHeader;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private SortAdorner _currentAdorner;
        private AdornerLayer _adornerLayer;

        public SortingManager(AdvancedListView listView)
        {
            this._listView = listView;
        }

        public void Sort(GridViewColumnHeader header)
        {
            if (this._listView.EnableSorting == false)
            {
                return;
            }

            if (this._listView.ItemsSource == null)
            {
                return;
            }

            if (header.Column is not AdvancedGridViewColumn column)
            {
                return;
            }

            if (column.AllowSorting == false)
            {
                return;
            }

            string propertyName = GetSortProperty(column);

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }

            ListSortDirection direction = GetNextDirection(header);

            ICollectionView view = CollectionViewSource.GetDefaultView(this._listView.ItemsSource);

            using (view.DeferRefresh())
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }

            UpdateSortGlyph(header, direction);

            this._lastHeader = header;
            this._lastDirection = direction;
        }

        private string GetSortProperty(AdvancedGridViewColumn column)
        {
            if (string.IsNullOrWhiteSpace(column.SortMemberPath) == false)
            {
                return column.SortMemberPath;
            }

            if (column.DisplayMemberBinding is Binding binding)
            {
                return binding.Path?.Path;
            }

            return null;
        }

        private ListSortDirection GetNextDirection(GridViewColumnHeader header)
        {
            if (this._lastHeader == header)
            {
                return this._lastDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending
                    : ListSortDirection.Ascending;
            }

            return ListSortDirection.Ascending;
        }

        private void UpdateSortGlyph(GridViewColumnHeader header, ListSortDirection direction)
        {
            if (this._currentAdorner != null)
            {
                this._adornerLayer?.Remove(this._currentAdorner);
            }

            this._adornerLayer = AdornerLayer.GetAdornerLayer(header);

            if (this._adornerLayer == null)
                return;

            this._currentAdorner = new SortAdorner(header, direction);

            this._adornerLayer.Add(this._currentAdorner);
        }

        public void ClearSorting()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(this._listView.ItemsSource);

            view.SortDescriptions.Clear();

            if (this._currentAdorner != null)
            {
                this._adornerLayer?.Remove(this._currentAdorner);
            }

            this._lastHeader = null;
            this._currentAdorner = null;
        }
    }
}
