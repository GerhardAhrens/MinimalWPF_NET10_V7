namespace System.Windows.Controls
{
    using System.Windows.Data;
    using System.Windows.Input;

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

        private static void OnListViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((FilterRow)d).Build();
        }

        private void Build()
        {
            Children.Clear();
            ColumnDefinitions.Clear();

            if (ListView == null)
            {
                return;
            }

            if (ListView.View is not GridView gridView)
                return;

            int columnIndex = 0;

            // Platz für die Zeilennummern
            if (ListView.ShowRowNumbers)
            {
                ColumnDefinitions.Add(
                    new ColumnDefinition
                    {
                        Width = new GridLength(ListView.EffectiveRowNumberWidth)
                    });

                Border spacer = new()
                {
                    Background = System.Windows.Media.Brushes.Transparent
                };

                SetColumn(spacer, columnIndex);
                Children.Add(spacer);

                columnIndex++;
            }

            // Für jede Datenspalte bleibt eine FilterRow-Spalte erhalten.
            foreach (GridViewColumn gridColumn in gridView.Columns)
            {
                // Breite an die jeweilige Datenspalte binden.
                ColumnDefinition columnDefinition = new();

                BindingOperations.SetBinding(
                    columnDefinition,
                    ColumnDefinition.WidthProperty,
                    new Binding(nameof(GridViewColumn.ActualWidth))
                    {
                        Source = gridColumn,
                        Converter = new DoubleToGridLengthConverter()
                    });

                ColumnDefinitions.Add(columnDefinition);

                // Nur AdvancedGridViewColumn kann ShowFilter bereitstellen.
                if (gridColumn is AdvancedGridViewColumn column &&
                    column.ShowFilter)
                {
                    TextBox tb = CreateFilterBox(column);

                    SetColumn(tb, columnIndex);
                    Children.Add(tb);
                }
                else
                {
                    // Unsichtbarer Platzhalter: Breite bleibt erhalten.
                    Border spacer = new()
                    {
                        Background = System.Windows.Media.Brushes.Transparent,
                        IsHitTestVisible = false
                    };

                    SetColumn(spacer, columnIndex);
                    Children.Add(spacer);
                }

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
            tb.PreviewKeyDown += FilterBox_PreviewKeyDown;
            tb.ToolTip = GetPlaceholder(column);

            return tb;
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.ListView == null)
            {
                return;
            }

            if (sender is not TextBox tb)
            {
                return;
            }

            if (tb.Tag is not AdvancedGridViewColumn column)
            {
                return;
            }

            this.ListView?.UpdateFilter(column, tb.Text);
        }

        private void FilterBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not TextBox tb)
            {
                return;
            }

            if (e.Key != Key.Escape)
            {
                return;
            }

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                this.ClearAllFilters();
            }
            else
            {
                tb.Clear();
                tb.Focus();
            }

            e.Handled = true;
        }

        private void ClearAllFilters()
        {
            TextBox first = null;
            foreach (UIElement element in Children)
            {
                if (element is TextBox tb)
                {
                    tb.Clear();
                    first ??= tb;
                }
            }

            first?.Focus();
        }

        private static string GetPlaceholder(AdvancedGridViewColumn column)
        {
            if (string.IsNullOrWhiteSpace(column.FilterPlaceholder) == false)
            {
                return column.FilterPlaceholder;
            }

            return column.FilterType switch
            {
                FilterType.Text => "enthält...",
                FilterType.Number => "=  >  <",
                FilterType.Date => "TT.MM.JJJJ",
                FilterType.Boolean => "Ja / Nein",
                _ => "Filter..."
            };
        }
    }
}
