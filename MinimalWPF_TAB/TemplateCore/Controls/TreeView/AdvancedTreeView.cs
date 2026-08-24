namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    public class AdvancedTreeView : TreeView
    {
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
                return;

            if (e.NewValue == null)
                return;

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


        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);

            if (!ReferenceEquals(SelectedItem, e.NewValue))
            {
                SelectedItem = e.NewValue;
            }

            if (e.NewValue != null && SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(e.NewValue))
            {
                SelectionChangedCommand.Execute(e.NewValue);
            }
        }


        private void SelectAndFocusItem(object item)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Loaded,
                new Action(() =>
                {
                    var treeViewItem = FindTreeViewItem(
                        this,
                        item);

                    if (treeViewItem == null)
                        return;

                    treeViewItem.IsSelected = true;

                    treeViewItem.BringIntoView();

                    treeViewItem.Focus();
                }));
        }


        private static TreeViewItem FindTreeViewItem(ItemsControl parent, object item)
        {
            foreach (var child in parent.Items)
            {
                if (ReferenceEquals(child, item))
                {
                    return parent.ItemContainerGenerator
                        .ContainerFromItem(child) as TreeViewItem;
                }

                if (parent.ItemContainerGenerator.ContainerFromItem(child) is TreeViewItem treeViewItem)
                {
                    if (ReferenceEquals(treeViewItem.DataContext, item))
                    {
                        return treeViewItem;
                    }

                    if (treeViewItem.HasItems)
                    {
                        if (!treeViewItem.IsExpanded)
                        {
                            treeViewItem.IsExpanded = true;
                        }

                        var result = FindTreeViewItem(treeViewItem, item);

                        if (result != null)
                            return result;
                    }
                }
            }

            return null;
        }
    }


    public class ExpandedImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
            {
                return null;
            }

            var openImage = values[0] as DrawingImage;
            var expandedImage = values[1] as DrawingImage;

            if (values[2] is bool isExpanded && isExpanded == true && expandedImage != null)
            {
                return expandedImage;
            }

            return openImage;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
