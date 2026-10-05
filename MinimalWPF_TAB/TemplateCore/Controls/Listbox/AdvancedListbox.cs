namespace System.Windows.Controls
{
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    public class AdvancedListbox : ListBox
    {
        private ICollectionView _view;
        private Predicate<object> _previousFilter;
        private bool _installedFilter;
        private string _searchText = string.Empty;

        static AdvancedListbox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AdvancedListbox),
                new FrameworkPropertyMetadata(typeof(AdvancedListbox)));
        }

        public AdvancedListbox()
        {
            Loaded += (_, _) => AttachView();
            Unloaded += (_, _) => DetachView();
            AddHandler(MouseDoubleClickEvent, new MouseButtonEventHandler(OnMouseDoubleClick), true);

        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(AdvancedListbox),
                new FrameworkPropertyMetadata(string.Empty, OnSearchTextChanged));

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        public static readonly DependencyProperty SearchWatermarkProperty =
            DependencyProperty.Register(nameof(SearchWatermark), typeof(string), typeof(AdvancedListbox), new PropertyMetadata("Suchen…"));

        public string SearchWatermark
        {
            get => (string)GetValue(SearchWatermarkProperty);
            set => SetValue(SearchWatermarkProperty, value);
        }

        public static readonly DependencyProperty DisplayMemberPathForSearchProperty =
            DependencyProperty.Register(nameof(DisplayMemberPathForSearch), typeof(string), typeof(AdvancedListbox),
                new FrameworkPropertyMetadata(string.Empty, OnFilterPropertyChanged));

        /// <summary>
        /// Optional property path used to obtain the text searched, e.g. "Name".
        /// If empty, DisplayMemberPath is used, then ToString().
        /// </summary>
        public string DisplayMemberPathForSearch
        {
            get => (string)GetValue(DisplayMemberPathForSearchProperty);
            set => SetValue(DisplayMemberPathForSearchProperty, value);
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(nameof(SelectionChangedCommand), typeof(ICommand), typeof(AdvancedListbox));

        public ICommand SelectionChangedCommand
        {
            get => (ICommand)GetValue(SelectionChangedCommandProperty);
            set => SetValue(SelectionChangedCommandProperty, value);
        }

        public static readonly DependencyProperty SelectionChangedCommandParameterProperty =
            DependencyProperty.Register(nameof(SelectionChangedCommandParameter), typeof(object), typeof(AdvancedListbox));

        public object SelectionChangedCommandParameter
        {
            get => GetValue(SelectionChangedCommandParameterProperty);
            set => SetValue(SelectionChangedCommandParameterProperty, value);
        }

        public static readonly DependencyProperty DoubleClickCommandProperty =
            DependencyProperty.Register(nameof(DoubleClickCommand), typeof(ICommand), typeof(AdvancedListbox));

        /// <summary>
        /// Command executed when an item is double-clicked.
        /// The clicked item is used as the default command parameter.
        /// </summary>
        public ICommand DoubleClickCommand
        {
            get => (ICommand)GetValue(DoubleClickCommandProperty);
            set => SetValue(DoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty DoubleClickCommandParameterProperty =
            DependencyProperty.Register(nameof(DoubleClickCommandParameter), typeof(object), typeof(AdvancedListbox));

        /// <summary>
        /// Optional explicit parameter for DoubleClickCommand.
        /// If null, the double-clicked item is passed.
        /// </summary>
        public object DoubleClickCommandParameter
        {
            get => GetValue(DoubleClickCommandParameterProperty);
            set => SetValue(DoubleClickCommandParameterProperty, value);
        }

        public static readonly DependencyProperty FooterVisibilityProperty =
            DependencyProperty.Register(nameof(FooterVisibility), typeof(Visibility), typeof(AdvancedListbox), new PropertyMetadata(Visibility.Visible));

        public Visibility FooterVisibility
        {
            get => (Visibility)GetValue(FooterVisibilityProperty);
            set => SetValue(FooterVisibilityProperty, value);
        }


        public static readonly DependencyProperty ActionButtonVisibilityProperty =
            DependencyProperty.Register(nameof(ActionButtonVisibility), typeof(Visibility), typeof(AdvancedListbox),
                new FrameworkPropertyMetadata(Visibility.Collapsed));

        public Visibility ActionButtonVisibility
        {
            get => (Visibility)GetValue(ActionButtonVisibilityProperty);
            set => SetValue(ActionButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty ActionButtonImageProperty =
            DependencyProperty.Register(nameof(ActionButtonImage), typeof(DrawingImage), typeof(AdvancedListbox));

        public DrawingImage ActionButtonImage
        {
            get => (DrawingImage)GetValue(ActionButtonImageProperty);
            set => SetValue(ActionButtonImageProperty, value);
        }

        public static readonly DependencyProperty ActionButtonCommandProperty =
            DependencyProperty.Register(nameof(ActionButtonCommand), typeof(ICommand), typeof(AdvancedListbox));

        public ICommand ActionButtonCommand
        {
            get => (ICommand)GetValue(ActionButtonCommandProperty);
            set => SetValue(ActionButtonCommandProperty, value);
        }

        public static readonly DependencyProperty ActionButtonCommandParameterProperty =
            DependencyProperty.Register(nameof(ActionButtonCommandParameter), typeof(object), typeof(AdvancedListbox));

        public object ActionButtonCommandParameter
        {
            get => GetValue(ActionButtonCommandParameterProperty);
            set => SetValue(ActionButtonCommandParameterProperty, value);
        }

        public static readonly DependencyProperty TotalCountProperty =
            DependencyProperty.Register(nameof(TotalCount), typeof(int), typeof(AdvancedListbox),
                new FrameworkPropertyMetadata(0));

        public int TotalCount
        {
            get => (int)GetValue(TotalCountProperty);
            private set => SetValue(TotalCountProperty, value);
        }

        public static readonly DependencyProperty FilteredCountProperty =
            DependencyProperty.Register(nameof(FilteredCount), typeof(int), typeof(AdvancedListbox),
                new FrameworkPropertyMetadata(0));

        public int FilteredCount
        {
            get => (int)GetValue(FilteredCountProperty);
            private set => SetValue(FilteredCountProperty, value);
        }

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(AdvancedListbox),
                new FrameworkPropertyMetadata(string.Empty));

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public static readonly DependencyProperty SearchPlaceholderProperty =
            DependencyProperty.Register(nameof(SearchPlaceholder), typeof(string), typeof(AdvancedListbox),
                new FrameworkPropertyMetadata("Suchen…"));

        public string SearchPlaceholder
        {
            get => (string)GetValue(SearchPlaceholderProperty);
            set => SetValue(SearchPlaceholderProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            AttachView();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            DetachView();
            base.OnItemsSourceChanged(oldValue, newValue);
            AttachView();
            UpdateCounts();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            var command = SelectionChangedCommand;
            var parameter = SelectionChangedCommandParameter ?? new SelectionChangedInfo(
                SelectedItem, SelectedItems.Cast<object>().ToArray(), e.AddedItems.Cast<object>().ToArray(),
                e.RemovedItems.Cast<object>().ToArray());

            if (command?.CanExecute(parameter) == true)
                command.Execute(parameter);
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClickCommand is not { } command)
                return;

            var item = FindItemFromOriginalSource(e.OriginalSource as DependencyObject);
            if (item is null)
                return;

            // A double click on the action button must not also trigger the row command.
            if (IsInsideActionButton(e.OriginalSource as DependencyObject))
                return;

            var parameter = DoubleClickCommandParameter ?? item;

            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
                e.Handled = true;
            }
        }


        /// <summary>Scrolls an item into view. The item must be present in the filtered view.</summary>
        new public void ScrollIntoView(object item)
        {
            if (item is null) return;

            Dispatcher.BeginInvoke(() =>
            {
                if (Items.Contains(item))
                {
                    base.ScrollIntoView(item);
                    UpdateLayout();
                }
            }, DispatcherPriority.Loaded);
        }

        private static bool IsInsideActionButton(DependencyObject source)
        {
            while (source is not null)
            {
                if (source is Button button &&
                    button.ReadLocalValue(StyleProperty) != DependencyProperty.UnsetValue)
                    return button.Name == "PART_ActionButton";

                source = VisualTreeHelper.GetParent(source);
            }

            return false;
        }

        private static object FindItemFromOriginalSource(DependencyObject source)
        {
            while (source is not null)
            {
                if (source is ListBoxItem item)
                    return item.Content;

                source = VisualTreeHelper.GetParent(source);
            }

            return null;
        }

        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AdvancedListbox)d;
            control._searchText = e.NewValue as string ?? string.Empty;
            control.RefreshFilter();
        }

        private static void OnFilterPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((AdvancedListbox)d).RefreshFilter();

        private void AttachView()
        {
            if (!IsLoaded || ItemsSource is null) return;

            var view = CollectionViewSource.GetDefaultView(ItemsSource);
            if (ReferenceEquals(_view, view) && _installedFilter) return;

            DetachView();
            _view = view;
            _previousFilter = view.Filter;
            view.Filter = FilterItem;
            _installedFilter = true;
            view.CollectionChanged += ViewCollectionChanged;
            UpdateCounts();
        }

        private void DetachView()
        {
            if (_view is null) return;

            _view.CollectionChanged -= ViewCollectionChanged;

            // Restore only if our predicate is still installed.
            if (_installedFilter && _view.Filter == (Predicate<object>)FilterItem)
                _view.Filter = _previousFilter;

            _view = null;
            _previousFilter = null;
            _installedFilter = false;
        }

        private void ViewCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            => UpdateCounts();

        private void RefreshFilter()
        {
            if (_view is null)
            {
                AttachView();
                return;
            }

            _view.Refresh();
            UpdateCounts();
        }

        private bool FilterItem(object item)
        {
            if (_previousFilter is not null && !_previousFilter(item))
                return false;

            var needle = SearchText?.Trim();
            if (string.IsNullOrEmpty(needle))
                return true;

            var text = GetSearchText(item);
            return text.Contains(needle, StringComparison.OrdinalIgnoreCase);
        }

        private string GetSearchText(object item)
        {
            var path = !string.IsNullOrWhiteSpace(DisplayMemberPathForSearch)
                ? DisplayMemberPathForSearch
                : DisplayMemberPath;

            if (string.IsNullOrWhiteSpace(path))
                return item?.ToString() ?? string.Empty;

            object value = item;
            foreach (var part in path.Split('.'))
            {
                if (value is null) return string.Empty;
                var property = TypeDescriptor.GetProperties(value)[part];
                if (property is not null)
                {
                    value = property.GetValue(value);
                    continue;
                }

                var pi = value.GetType().GetProperty(part, BindingFlags.Instance | BindingFlags.Public);
                if (pi is null) return item?.ToString() ?? string.Empty;
                value = pi.GetValue(value);
            }

            return value?.ToString() ?? string.Empty;
        }

        private void UpdateCounts()
        {
            if (!IsLoaded && ItemsSource is null)
            {
                TotalCount = 0;
                FilteredCount = 0;
                return;
            }

            var source = ItemsSource as IEnumerable;
            if (source is null)
            {
                TotalCount = Items.Count;
                FilteredCount = Items.Count;
                return;
            }

            var count = 0;
            foreach (var _ in source) count++;
            TotalCount = count;
            FilteredCount = _view?.Cast<object>().Count() ?? Items.Count;
        }
    }

    public sealed record SelectionChangedInfo(
        object SelectedItem,
        IReadOnlyList<object> SelectedItems,
        IReadOnlyList<object> AddedItems,
        IReadOnlyList<object> RemovedItems);
}