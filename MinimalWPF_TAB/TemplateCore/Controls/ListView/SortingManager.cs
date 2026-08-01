namespace System.Windows.Controls
{
    using System.ComponentModel;
    using System.Windows.Data;

    internal class SortingManager
    {
        private readonly ListView _listView;

        private GridViewColumnHeader _lastHeader;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public SortingManager(ListView listView)
        {
            this._listView = listView;
        }

        public void Sort(GridViewColumnHeader header)
        {
            if (this._listView.ItemsSource == null)
            {
                return;
            }

            if (header.Column is not Controls.AdvancedGridViewColumn column)
            {
                return;
            }

            if (column.AllowSorting == false)
            {
                return;
            }

            string property = column.SortMemberPath;

            if (string.IsNullOrWhiteSpace(property))
            {
                if (column.DisplayMemberBinding is Binding binding)
                {
                    property = binding.Path.Path;
                }
            }

            if (string.IsNullOrWhiteSpace(property))
            {
                return;
            }

            var direction = GetDirection(header);

            ICollectionView view = CollectionViewSource.GetDefaultView(_listView.ItemsSource);

            using (view.DeferRefresh())
            {
                view.SortDescriptions.Clear();
                view.SortDescriptions.Add(new SortDescription(property, direction));
            }

            this._lastHeader = header;
            this._lastDirection = direction;
        }

        private ListSortDirection GetDirection(GridViewColumnHeader header)
        {
            if (this._lastHeader == header)
            {
                return this._lastDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }

            return ListSortDirection.Ascending;
        }
    }
}
