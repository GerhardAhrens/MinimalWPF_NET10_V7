namespace System.Windows.Controls
{
    using System.Collections.Specialized;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    public class AdvancedTreeView : TreeView
    {
        private bool _isApplyingFilter;

        static AdvancedTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AdvancedTreeView),
                new FrameworkPropertyMetadata(
                    typeof(AdvancedTreeView)));
        }


        #region SelectedItem

        new public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                nameof(SelectedItem),
                typeof(object),
                typeof(AdvancedTreeView),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedItemPropertyChanged));

        new public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not AdvancedTreeView treeView)
            {
                return;
            }

            if (e.NewValue == null)
            {
                return;
            }

            treeView.SelectAndFocusItem(e.NewValue);
        }

        #endregion

        #region SelectionChangedCommand

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(
                nameof(SelectionChangedCommand),
                typeof(ICommand),
                typeof(AdvancedTreeView),
                new PropertyMetadata(null));

        public ICommand SelectionChangedCommand
        {
            get => (ICommand)GetValue(SelectionChangedCommandProperty);
            set => SetValue(SelectionChangedCommandProperty, value);
        }

        #endregion

        #region DoubleClickCommand

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(AdvancedTreeView), new PropertyMetadata(null));

        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        #endregion

        #region Filter

        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(string), typeof(AdvancedTreeView),
                new PropertyMetadata(string.Empty, OnFilterChanged));

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }


        private static void OnFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AdvancedTreeView treeView)
            {
                treeView.ApplyFilter();
            }
        }

        #endregion

        #region FilterPredicate

        public static readonly DependencyProperty FilterPredicateProperty =
            DependencyProperty.Register(nameof(FilterPredicate), typeof(Func<AdvancedTreeNode, string, bool>),
                typeof(AdvancedTreeView),
                new PropertyMetadata(null, OnFilterPredicateChanged));

        public Func<AdvancedTreeNode, string, bool> FilterPredicate
        {
            get => (Func<AdvancedTreeNode, string, bool>)
                GetValue(FilterPredicateProperty);

            set => SetValue(FilterPredicateProperty, value);
        }


        private static void OnFilterPredicateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AdvancedTreeView treeView)
            {
                treeView.ApplyFilter();
            }
        }

        #endregion

        #region Filter Information

        public static readonly DependencyProperty IsFilteredProperty =
            DependencyProperty.Register(
                nameof(IsFiltered),
                typeof(bool),
                typeof(AdvancedTreeView),
                new PropertyMetadata(false));


        public bool IsFiltered
        {
            get => (bool)GetValue(IsFilteredProperty);
            private set => SetValue(IsFilteredProperty, value);
        }



        public static readonly DependencyProperty TotalItemCountProperty =
            DependencyProperty.Register(
                nameof(TotalItemCount),
                typeof(int),
                typeof(AdvancedTreeView),
                new PropertyMetadata(0));


        public int TotalItemCount
        {
            get => (int)GetValue(TotalItemCountProperty);
            private set => SetValue(TotalItemCountProperty, value);
        }



        public static readonly DependencyProperty FilteredItemCountProperty =
            DependencyProperty.Register(
                nameof(FilteredItemCount),
                typeof(int),
                typeof(AdvancedTreeView),
                new PropertyMetadata(0));


        public int FilteredItemCount
        {
            get => (int)GetValue(FilteredItemCountProperty);
            private set => SetValue(FilteredItemCountProperty, value);
        }

        #endregion Filter Information

        #region DisplayMemberPath
        new public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(AdvancedTreeView), new PropertyMetadata(string.Empty));

        new public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }
        #endregion DisplayMemberPath

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);


            // Während des Filteraufbaus kann WPF eine momentan
            // nicht mehr sichtbare Auswahl entfernen.
            //
            // Diese automatische Zwischenänderung darf unsere
            // eigentliche SelectedItem-Auswahl nicht überschreiben.
            if (_isApplyingFilter)
            {
                return;
            }


            if (!ReferenceEquals(SelectedItem, e.NewValue))
            {
                SelectedItem = e.NewValue;
            }


            if (e.NewValue != null && SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(e.NewValue))
            {
                SelectionChangedCommand.Execute(e.NewValue);
            }
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (DoubleClickCommand == null)
                return;

            var treeViewItem = FindParentTreeViewItem(e.OriginalSource as DependencyObject);

            if (treeViewItem == null)
                return;

            var node = treeViewItem.DataContext;

            if (node == null)
                return;

            if (DoubleClickCommand.CanExecute(node))
            {
                DoubleClickCommand.Execute(node);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.ApplyFilter();
        }


        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            this.ApplyFilter();
        }

        private void SelectAndFocusItem(object item)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                new Action(() =>
                {
                    var node = item as AdvancedTreeNode;

                    if (node == null)
                        return;


                    // Eine durch den Filter ausgeblendete Node
                    // darf nicht automatisch sichtbar gemacht werden.
                    if (IsFiltered && !node.IsFilterVisible)
                    {
                        return;
                    }


                    var treeViewItem = FindTreeViewItem(this, item);

                    if (treeViewItem == null)
                        return;


                    treeViewItem.IsSelected = true;
                    treeViewItem.BringIntoView();
                    //treeViewItem.Focus();
                }));
        }
        private static TreeViewItem FindParentTreeViewItem(DependencyObject element)
        {
            while (element != null)
            {
                if (element is TreeViewItem treeViewItem)
                {
                    return treeViewItem;
                }

                element = VisualTreeHelper.GetParent(element);
            }

            return null;
        }


        private static TreeViewItem FindTreeViewItem(ItemsControl parent, object item)
        {
            foreach (var child in parent.Items)
            {
                if (ReferenceEquals(child, item))
                {
                    return parent.ItemContainerGenerator.ContainerFromItem(child) as TreeViewItem;
                }

                if (parent.ItemContainerGenerator.ContainerFromItem(child) is not TreeViewItem treeViewItem)
                {
                    continue;
                }

                // Prüfen, ob sich das gesuchte Element
                // tatsächlich unterhalb dieser Node befindet.
                if (!ContainsItem(treeViewItem, item))
                {
                    continue;
                }

                // Erst jetzt wissen wir, dass diese Node
                // ein Elternknoten des gesuchten Elements ist.
                if (!treeViewItem.IsExpanded)
                {
                    treeViewItem.IsExpanded = true;
                }

                var result = FindTreeViewItem(treeViewItem, item);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private static bool ContainsItem(TreeViewItem parent, object item)
        {
            foreach (var child in parent.Items)
            {
                if (ReferenceEquals(child, item))
                {
                    return true;
                }

                if (child is TreeViewItem node)
                {
                    if (ContainsItem(node, item))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #region Filter

        private void ApplyFilter()
        {
            // Die aktuell ausgewählte Node merken.
            AdvancedTreeNode selectedNode = SelectedItem as AdvancedTreeNode;


            this._isApplyingFilter = true;

            try
            {
                string filter = this.Filter ?? string.Empty;

                Func<AdvancedTreeNode,string,bool> predicate = this.FilterPredicate ?? DefaultFilterPredicate;

                this.IsFiltered = !string.IsNullOrWhiteSpace(filter);


                int totalCount = 0;
                int filteredCount = 0;


                foreach (var item in Items)
                {
                    if (item is AdvancedTreeNode node)
                    {
                        totalCount += CountNodes(node);


                        bool visible = node.ApplyFilter(filter, predicate);


                        if (visible)
                        {
                            filteredCount += CountFilteredNodes(node);
                        }
                    }
                }


                this.TotalItemCount = totalCount;
                this.FilteredItemCount = filteredCount;
            }
            finally
            {
                this._isApplyingFilter = false;
            }


            // Die ursprüngliche Auswahl beibehalten.
            if (selectedNode != null)
            {
                selectedNode.IsSelected = true;


                // Nur wenn die Node momentan sichtbar ist,
                // darf sie im TreeView tatsächlich selektiert
                // und fokussiert werden.
                if (!this.IsFiltered || selectedNode.IsFilterVisible)
                {
                    this.SelectAndFocusItem(selectedNode);
                }
            }
        }

        private static bool DefaultFilterPredicate(AdvancedTreeNode node, string filter)
        {
            return node.Text.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
        }

        private static int CountNodes(AdvancedTreeNode node)
        {
            int count = 1;

            foreach (var child in node.Children)
            {
                count += CountNodes(child);
            }

            return count;
        }

        private static int CountFilteredNodes(AdvancedTreeNode node)
        {
            if (!node.IsFilterVisible)
                return 0;

            int count = 1;

            foreach (var child in node.FilteredChildren)
            {
                count += CountFilteredNodes(child);
            }

            return count;
        }
        #endregion Filter

    }
}
