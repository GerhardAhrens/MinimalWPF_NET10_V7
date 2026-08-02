namespace System.Windows.Controls
{
    using System.Data;
    using System.Diagnostics;
    using System.Windows.Media;

    internal static class CellAppearanceManager
    {
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached(
                "Column",
                typeof(AdvancedGridViewColumn),
                typeof(CellAppearanceManager));

        public static void SetColumn(
            DependencyObject obj,
            AdvancedGridViewColumn value)
        {
            obj.SetValue(ColumnProperty, value);
        }

        public static AdvancedGridViewColumn GetColumn(DependencyObject obj)
        {
            return (AdvancedGridViewColumn)obj.GetValue(ColumnProperty);
        }

        public static void OnCellLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBlock tb)
            {
                return;
            }

            Apply(tb);
        }

        private static void Apply(TextBlock tb)
        {
            AdvancedGridViewColumn column = GetColumn(tb);

            if (column == null)
            {
                return;
            }

            AdvancedListView listView = FindParent<AdvancedListView>(tb);

            if (listView == null)
                return;

            if (listView.CellStyleProvider == null)
                return;

            object value = GetCellValue(tb.DataContext, column);

            CellStyleInfo style = listView.CellStyleProvider(
                    new CellStyleRequest
                    {
                        Item = tb.DataContext!,
                        Column = column,
                        Value = value
                    });

            if (style == null)
            {
                return;
            }

            ApplyStyle(tb, style);
        }

        private static object GetCellValue(object item,AdvancedGridViewColumn column)
        {
            if (item is not DataRowView row)
                return null;

            if (string.IsNullOrEmpty(column.BindingPath))
                return null;

            string name =
                column.BindingPath.Trim('[', ']');

            if (!row.DataView.Table.Columns.Contains(name))
                return null;

            return row[name];
        }

        private static void ApplyStyle(TextBlock tb, CellStyleInfo style)
        {
            if (style.Foreground != null)
                tb.Foreground = ToMediaBrush(style.Foreground);

            if (style.Background != null)
                tb.Background = ToMediaBrush(style.Background);

            if (style.FontWeight.HasValue)
                tb.FontWeight = style.FontWeight.Value;

            if (style.FontStyle.HasValue)
                tb.FontStyle = style.FontStyle.Value;

            if (style.TextDecorations != null)
                tb.TextDecorations = style.TextDecorations;
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T result)
                    return result;

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        private static System.Windows.Media.Brush ToMediaBrush(object drawingBrushObj)
        {
            if (drawingBrushObj is System.Drawing.SolidBrush sb)
            {
                System.Drawing.Color c = sb.Color;
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B));
            }

            return null;
        }
    }
}
