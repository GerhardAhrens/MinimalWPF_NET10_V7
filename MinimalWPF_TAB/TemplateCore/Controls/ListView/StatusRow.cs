namespace System.Windows.Controls
{
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class StatusRow : ContentControl
    {
        private readonly Border _border;
        private readonly DockPanel _panel;
        private readonly TextBlock _txtFilter;
        private readonly TextBlock _txtRows;
        private readonly Ellipse _statusIndicator;

        public StatusRow()
        {
            _txtFilter = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            _txtRows = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            _statusIndicator = new Ellipse
            {
                Width = 10,
                Height = 10,
                Margin = new Thickness(0, 0, 6, 0),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                Stroke = Brushes.DimGray,
                StrokeThickness = 1,
                Fill = Brushes.LimeGreen
            };

            DockPanel.SetDock(_txtFilter, Dock.Left);
            DockPanel.SetDock(_txtRows, Dock.Right);

            _panel = new DockPanel
            {
                LastChildFill = false,
                Margin = new Thickness(4, 2, 4, 2)
            };

            _panel.Children.Add(_statusIndicator);
            _panel.Children.Add(_txtFilter);
            _panel.Children.Add(_txtRows);

            _border = new Border
            {
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1, 0, 1, 1),
                Child = _panel
            };

            Content = _border;
        }

        #region ListView

        public static readonly DependencyProperty ListViewProperty =
            DependencyProperty.Register(
                nameof(ListView),
                typeof(AdvancedListView),
                typeof(StatusRow),
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
            StatusRow row = (StatusRow)d;

            if (e.OldValue is AdvancedListView oldList)
                oldList.StatusChanged -= row.OnStatusChanged;

            if (e.NewValue is AdvancedListView newList)
                newList.StatusChanged += row.OnStatusChanged;

            row.Refresh();
        }

        #endregion

        private void OnStatusChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        public void Refresh()
        {
            if (ListView == null)
            {
                _txtFilter.Text = "";
                _txtRows.Text = "";
                return;
            }

            if (ListView.IsFilterActive)
            {
                _statusIndicator.Fill = Brushes.Goldenrod;
                _statusIndicator.Fill = ListView.IsFilterActive ? CreateIndicatorBrush(Colors.Goldenrod) : CreateIndicatorBrush(Colors.LimeGreen);
                _txtFilter.Text = "Filter aktiv";
                _txtFilter.FontWeight = FontWeights.Bold;

                _txtRows.Text = $"{ListView.VisibleRowCount:N0} / {ListView.TotalRowCount:N0} Datensätze";
            }
            else
            {
                _statusIndicator.Fill = Brushes.LimeGreen;
                _statusIndicator.Fill = ListView.IsFilterActive ? CreateIndicatorBrush(Colors.Goldenrod) : CreateIndicatorBrush(Colors.LimeGreen);
                _txtFilter.Text = "Kein Filter";
                _txtFilter.FontWeight = FontWeights.Normal;
                _txtRows.Text = $"{ListView.TotalRowCount:N0} Datensätze";
            }
        }

        private static Brush CreateIndicatorBrush(Color color)
        {
            return new RadialGradientBrush
            {
                GradientOrigin = new Point(0.35, 0.35),
                Center = new Point(0.35, 0.35),
                RadiusX = 0.75,
                RadiusY = 0.75,
                GradientStops =
        {
            new GradientStop(Colors.White, 0.0),
            new GradientStop(color, 0.35),
            new GradientStop(color, 1.0)
        }
            };
        }
    }
}
