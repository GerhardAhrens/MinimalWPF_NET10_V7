namespace System.Windows
{
    using System.Windows.Controls;
    using System.Windows.Input;

    public class AdvancedListView : ListView
    {
        private readonly SortingManager _sortingManager;

        static AdvancedListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedListView), new FrameworkPropertyMetadata(typeof(AdvancedListView)));
        }

        public AdvancedListView()
        {
            this._sortingManager = new SortingManager(this);

            AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));

            MouseDoubleClick += AdvancedListView_MouseDoubleClick;
        }

        #region EnableSorting

        public static readonly DependencyProperty EnableSortingProperty =
            DependencyProperty.Register(
                nameof(EnableSorting),
                typeof(bool),
                typeof(AdvancedListView),
                new PropertyMetadata(true));

        public bool EnableSorting
        {
            get => (bool)GetValue(EnableSortingProperty);
            set => SetValue(EnableSortingProperty, value);
        }

        #endregion

        #region ShowRowNumbers

        public static readonly DependencyProperty ShowRowNumbersProperty =
            DependencyProperty.Register(
                nameof(ShowRowNumbers),
                typeof(bool),
                typeof(AdvancedListView),
                new PropertyMetadata(false));

        public bool ShowRowNumbers
        {
            get => (bool)GetValue(ShowRowNumbersProperty);
            set => SetValue(ShowRowNumbersProperty, value);
        }

        #endregion

        #region AutoScrollToSelectedItem

        public static readonly DependencyProperty AutoScrollToSelectedItemProperty =
            DependencyProperty.Register(
                nameof(AutoScrollToSelectedItem),
                typeof(bool),
                typeof(AdvancedListView),
                new PropertyMetadata(false));

        public bool AutoScrollToSelectedItem
        {
            get => (bool)GetValue(AutoScrollToSelectedItemProperty);
            set => SetValue(AutoScrollToSelectedItemProperty, value);
        }

        #endregion

        #region ShowFilterRow

        public static readonly DependencyProperty ShowFilterRowProperty =
            DependencyProperty.Register(
                nameof(ShowFilterRow),
                typeof(bool),
                typeof(AdvancedListView),
                new PropertyMetadata(false));

        public bool ShowFilterRow
        {
            get => (bool)GetValue(ShowFilterRowProperty);
            set => SetValue(ShowFilterRowProperty, value);
        }

        #endregion

        #region FilterTemplate

        public static readonly DependencyProperty FilterTemplateProperty =
            DependencyProperty.Register(
                nameof(FilterTemplate),
                typeof(DataTemplate),
                typeof(AdvancedListView));

        public DataTemplate FilterTemplate
        {
            get => (DataTemplate)GetValue(FilterTemplateProperty);
            set => SetValue(FilterTemplateProperty, value);
        }

        #endregion

        #region SelectedItemChangedCommand

        public static readonly DependencyProperty SelectedItemChangedCommandProperty =
            DependencyProperty.Register(
                nameof(SelectedItemChangedCommand),
                typeof(ICommand),
                typeof(AdvancedListView));

        public ICommand SelectedItemChangedCommand
        {
            get => (ICommand)GetValue(SelectedItemChangedCommandProperty);
            set => SetValue(SelectedItemChangedCommandProperty, value);
        }

        #endregion

        #region DoubleClickCommand

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(
                nameof(DoubleClickCommand),
                typeof(ICommand),
                typeof(AdvancedListView));

        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        #endregion

        #region ContextMenuCommand

        public static readonly DependencyProperty ContextMenuCommandProperty =
            DependencyProperty.Register(
                nameof(ContextMenuCommand),
                typeof(ICommand),
                typeof(AdvancedListView));

        public ICommand ContextMenuCommand
        {
            get => (ICommand)GetValue(ContextMenuCommandProperty);
            set => SetValue(ContextMenuCommandProperty, value);
        }

        #endregion

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (AutoScrollToSelectedItem && SelectedItem != null)
            {
                ScrollIntoView(SelectedItem);
            }

            ExecuteCommand(SelectedItemChangedCommand);
        }

        private void AdvancedListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExecuteCommand(DoubleClickCommand);
        }

        protected override void OnContextMenuOpening(ContextMenuEventArgs e)
        {
            base.OnContextMenuOpening(e);

            ExecuteCommand(ContextMenuCommand);
        }

        private void ExecuteCommand(ICommand command)
        {
            if (command == null)
            {
                return;
            }

            if (command.CanExecute(SelectedItem))
            {
                command.Execute(SelectedItem);
            }
        }

        protected virtual void OnColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            if (EnableSorting == false)
            {
                return;
            }

            if (e.OriginalSource is not GridViewColumnHeader header)
            {
                return;
            }

            this._sortingManager.Sort(header);
        }
    }
}
