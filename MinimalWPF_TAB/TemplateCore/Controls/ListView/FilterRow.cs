namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;

    public class FilterRow : Grid
    {
        public static readonly DependencyProperty ListViewProperty =
            DependencyProperty.Register(
                nameof(ListView),
                typeof(AdvancedListView),
                typeof(FilterRow),
                new PropertyMetadata(null, OnListViewChanged));

        public AdvancedListView ListView
        {
            get => (AdvancedListView)GetValue(ListViewProperty);
            set => SetValue(ListViewProperty, value);
        }

        private static void OnListViewChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            ((FilterRow)d).Build();
        }

        private void Build()
        {
            Children.Clear();
            ColumnDefinitions.Clear();

            if (ListView == null)
                return;

            if (ListView.View is not GridView gridView)
                return;

            int columnIndex = 0;

            foreach (AdvancedGridViewColumn column in
                     gridView.Columns.OfType<AdvancedGridViewColumn>())
            {
                if (!column.ShowFilter)
                    continue;

                ColumnDefinition columnDefinition = new();

                BindingOperations.SetBinding(
                    columnDefinition,
                    ColumnDefinition.WidthProperty,
                    new Binding(nameof(GridViewColumn.Width))
                    {
                        Source = column,
                        Converter = new DoubleToGridLengthConverter()
                    });

                ColumnDefinitions.Add(columnDefinition);

                TextBox tb = CreateFilterBox(column);

                SetColumn(tb, columnIndex);

                Children.Add(tb);

                columnIndex++;
            }
        }

        private TextBox CreateFilterBox(AdvancedGridViewColumn column)
        {
            TextBox tb = new();

            tb.Margin = new Thickness(1);
            tb.VerticalContentAlignment = VerticalAlignment.Center;
            tb.HorizontalAlignment = HorizontalAlignment.Stretch;

            tb.Tag = column;

            tb.TextChanged += FilterBox_TextChanged;

            return tb;
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ListView == null)
                return;

            if (sender is not TextBox tb)
                return;

            if (tb.Tag is not AdvancedGridViewColumn column)
                return;

            ListView?.UpdateFilter(column, tb.Text);
        }
    }

    internal sealed class DoubleToGridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return new GridLength(d);

            return GridLength.Auto;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GridLength g)
                return g.Value;

            return 0.0;
        }
    }
}
