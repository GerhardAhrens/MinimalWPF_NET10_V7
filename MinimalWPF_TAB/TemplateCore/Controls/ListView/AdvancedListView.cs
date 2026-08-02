namespace System.Windows.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    using MinimalWPF_TAB.TemplateCore.Controls.ListView;

    public class AdvancedListView : ListView
    {
        private readonly SortingManager _sortingManager;
        private readonly FilterManager _filterManager;
        private RowNumberGridViewColumn _rowNumberColumn;
        private bool _rowNumberInitialized;
        private DataTemplate _rowNumberTemplate;
        //private Grid _layoutRoot;

        static AdvancedListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedListView), new FrameworkPropertyMetadata(typeof(AdvancedListView)));
        }

        public AdvancedListView()
        {
            this._sortingManager = new SortingManager(this);
            this._filterManager = new FilterManager(this);

            AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnColumnHeaderClick));

            MouseDoubleClick += AdvancedListView_MouseDoubleClick;
            Loaded += AdvancedListView_Loaded;
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
                new PropertyMetadata(false, OnShowRowNumbersChanged));

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

        #region RowNumberTextStyle
        public static readonly DependencyProperty RowNumberTextStyleProperty =
            DependencyProperty.Register(
                nameof(RowNumberTextStyle),
                typeof(Style),
                typeof(AdvancedListView),
                new PropertyMetadata(null));

        public Style RowNumberTextStyle
        {
            get => (Style)GetValue(RowNumberTextStyleProperty);
            set => SetValue(RowNumberTextStyleProperty, value);
        }
        #endregion RowNumberTextStyle

        #region RowNumberHeader
        public static readonly DependencyProperty RowNumberHeaderProperty =
            DependencyProperty.Register(
                nameof(RowNumberHeader),
                typeof(string),
                typeof(AdvancedListView),
                new PropertyMetadata("#", OnRowNumberPropertyChanged));

        public string RowNumberHeader
        {
            get => (string)GetValue(RowNumberHeaderProperty);
            set => SetValue(RowNumberHeaderProperty, value);
        }
        #endregion RowNumberHeader

        #region RowNumberWidth
        public static readonly DependencyProperty RowNumberWidthProperty =
            DependencyProperty.Register(
                nameof(RowNumberWidth),
                typeof(double),
                typeof(AdvancedListView),
                new PropertyMetadata(45.0, OnRowNumberPropertyChanged));

        public double RowNumberWidth
        {
            get => (double)GetValue(RowNumberWidthProperty);
            set => SetValue(RowNumberWidthProperty, value);
        }
        #endregion RowNumberWidth


        private void AdvancedListView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ContextMenu != null)
                {
                    ContextMenu.Opened -= ContextMenu_Opened;
                    ContextMenu.Opened += ContextMenu_Opened;
                }
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.InitializeColumns();
        }

        private void InitializeColumns()
        {
            this.PrepareColumns();
            this.UpdateRowNumberColumn();
            this.RegisterColumns();
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            if (ContextMenuCommand == null)
                return;

            if (sender is not ContextMenu menu)
            {
                return;
            }

            var args = new ContextMenuCommandArgs
            {
                ListView = this,
                ContextMenu = menu,
                SelectedItem = SelectedItem,
                SelectedIndex = SelectedIndex,
                MousePosition = Mouse.GetPosition(this),
                ClickedElement = menu.PlacementTarget as FrameworkElement
            };

            if (ContextMenuCommand.CanExecute(args))
            {
                ContextMenuCommand.Execute(args);
            }
        }

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

        private static void OnShowRowNumbersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdvancedListView)d).UpdateRowNumberColumn();
        }

        private static void OnRowNumberPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AdvancedListView)d).UpdateRowNumberColumn();
        }

        private void UpdateRowNumberColumn()
        {
            if (View is not GridView gridView)
                return;

            if (ShowRowNumbers)
            {
                if (!_rowNumberInitialized)
                {
                    _rowNumberColumn = new RowNumberGridViewColumn();

                    _rowNumberColumn.CellTemplate = GetRowNumberTemplate();

                    gridView.Columns.Insert(0, _rowNumberColumn);

                    _rowNumberInitialized = true;
                }

                _rowNumberColumn.Header = RowNumberHeader;
                _rowNumberColumn.Width = RowNumberWidth;
            }
            else
            {
                if (_rowNumberInitialized == false)
                {
                    return;
                }

                gridView.Columns.Remove(_rowNumberColumn);

                _rowNumberInitialized = false;
            }
        }

        private DataTemplate GetRowNumberTemplate()
        {
            if (_rowNumberTemplate != null)
                return _rowNumberTemplate;

            var factory = new FrameworkElementFactory(typeof(TextBlock));

            factory.SetBinding(TextBlock.TextProperty,
                new Binding()
                {
                    RelativeSource = new RelativeSource(
                        RelativeSourceMode.FindAncestor,
                        typeof(ListViewItem),
                        1),
                    Converter = new RowNumberConverter()
                });

            // Standarddarstellung
            factory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            factory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            factory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Right);
            factory.SetValue(TextBlock.MarginProperty, new Thickness(2, 0, 4, 0));

            // Optionaler benutzerdefinierter Style
            factory.SetBinding(
                FrameworkElement.StyleProperty,
                new Binding(nameof(RowNumberTextStyle))
                {
                    Source = this
                });

            _rowNumberTemplate = new DataTemplate
            {
                VisualTree = factory
            };

            return _rowNumberTemplate;
        }


        private void PrepareColumns()
        {
            if (View is not GridView gridView)
                return;

            foreach (var column in gridView.Columns.OfType<AdvancedGridViewColumn>())
            {
                column.CreateDefaultCellTemplate();
            }
        }

        private void RegisterColumns()
        {
            _filterManager.Clear();

            if (View is not GridView gridView)
                return;

            foreach (var column in gridView.Columns.OfType<AdvancedGridViewColumn>())
            {
                string property = column.EffectiveFilterMemberPath;

                if (string.IsNullOrWhiteSpace(property))
                    continue;

                _filterManager.RegisterColumn(column, property);
            }
        }

        internal void SetFilter(AdvancedGridViewColumn column, string value)
        {
            _filterManager.SetFilter(column, value);
        }

        internal void UpdateFilter(AdvancedGridViewColumn column, string value)
        {
            _filterManager.SetFilter(column, value);
        }
    }

    public class RowNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not ListViewItem item)
            {
                return string.Empty;
            }

            ListView listView = ItemsControl.ItemsControlFromItemContainer(item) as ListView;

            if (listView == null)
            {
                return string.Empty;
            }

            return listView.ItemContainerGenerator.IndexFromContainer(item) + 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    internal class SortAdorner : Adorner
    {
        private readonly ListSortDirection _direction;

        public SortAdorner(UIElement adornedElement,
                           ListSortDirection direction)
            : base(adornedElement)
        {
            _direction = direction;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            Geometry geometry;

            if (_direction == ListSortDirection.Ascending)
            {
                geometry = Geometry.Parse("M 0 6 L 4 0 L 8 6 Z");
            }
            else
            {
                geometry = Geometry.Parse("M 0 0 L 8 0 L 4 6 Z");
            }

            geometry.Freeze();

            dc.PushTransform(new TranslateTransform(AdornedElement.RenderSize.Width - 14, (AdornedElement.RenderSize.Height - 6) / 2));

            dc.DrawGeometry(Brushes.Gray, null, geometry);

            dc.Pop();
        }
    }

    public sealed class ContextMenuCommandArgs
    {
        public AdvancedListView ListView { get; init; } = null!;

        public ContextMenu ContextMenu { get; init; } = null!;

        public object SelectedItem { get; init; }

        public int SelectedIndex { get; init; }

        public FrameworkElement ClickedElement { get; init; }

        public Point MousePosition { get; init; }
    }
}
