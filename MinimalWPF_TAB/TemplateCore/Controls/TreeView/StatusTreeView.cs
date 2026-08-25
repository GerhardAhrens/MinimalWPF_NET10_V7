namespace System.Windows.Controls
{
    using System.Windows.Media;
    using System.Windows.Shapes;

    public class StatusTreeView : ContentControl
    {
        private readonly Border _border;
        private readonly DockPanel _panel;
        private readonly TextBlock _txtFilter;
        private readonly TextBlock _txtRows;
        private readonly Ellipse _statusIndicator;

        public StatusTreeView()
        {
            this._txtFilter = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            this._txtRows = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            this._statusIndicator = new Ellipse
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

            DockPanel.SetDock(this._txtFilter, Dock.Left);
            DockPanel.SetDock(this._txtRows, Dock.Right);

            this._panel = new DockPanel
            {
                LastChildFill = false,
                Margin = new Thickness(4, 2, 4, 2)
            };

            this._panel.Children.Add(this._statusIndicator);
            this._panel.Children.Add(this._txtFilter);
            this._panel.Children.Add(this._txtRows);

            this._border = new Border
            {
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1, 0, 1, 1),
                Child = this._panel
            };

            Content = this._border;
        }

        #region TreeView

        public static readonly DependencyProperty TreeViewProperty =
            DependencyProperty.Register(
                nameof(TreeView),
                typeof(AdvancedTreeView),
                typeof(StatusTreeView),
                new PropertyMetadata(null, OnTreeViewChanged));

        public AdvancedTreeView TreeView
        {
            get => (AdvancedTreeView)GetValue(TreeViewProperty);
            set => SetValue(TreeViewProperty, value);
        }

        #endregion

        #region Filter Information

        public static readonly DependencyProperty IsFilteredProperty =
            DependencyProperty.Register(
                nameof(IsFiltered),
                typeof(bool),
                typeof(StatusTreeView),
                new PropertyMetadata(false, OnTreeViewChanged));


        public bool IsFiltered
        {
            get => (bool)GetValue(IsFilteredProperty);
            set => SetValue(IsFilteredProperty, value);
        }



        public static readonly DependencyProperty TotalItemCountProperty =
            DependencyProperty.Register(
                nameof(TotalItemCount),
                typeof(int),
                typeof(StatusTreeView),
                new PropertyMetadata(0, OnTreeViewChanged));


        public int TotalItemCount
        {
            get => (int)GetValue(TotalItemCountProperty);
            set => SetValue(TotalItemCountProperty, value);
        }



        public static readonly DependencyProperty FilteredItemCountProperty =
            DependencyProperty.Register(
                nameof(FilteredItemCount),
                typeof(int),
                typeof(StatusTreeView),
                new PropertyMetadata(0, OnTreeViewChanged));


        public int FilteredItemCount
        {
            get => (int)GetValue(FilteredItemCountProperty);
            set => SetValue(FilteredItemCountProperty, value);
        }

        #endregion

        private static void OnTreeViewChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            StatusTreeView row = (StatusTreeView)d;

            if (e.OldValue is AdvancedListView oldList)
            {
                oldList.StatusChanged -= row.OnStatusChanged;
            }

            if (e.NewValue is AdvancedListView newList)
            {
                newList.StatusChanged += row.OnStatusChanged;
            }

            row.Refresh();
        }

        private void OnStatusChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public void Refresh()
        {
            if (this.IsFiltered == true)
            {
                this._statusIndicator.Fill = Brushes.Goldenrod;
                this._statusIndicator.Fill = this.IsFiltered ? CreateIndicatorBrush(Colors.Goldenrod) : CreateIndicatorBrush(Colors.LimeGreen);
                this._txtFilter.Text = "Filter aktiv";
                this._txtFilter.FontWeight = FontWeights.Bold;

                this._txtRows.Text = $"{this.FilteredItemCount:N0} / {this.TotalItemCount:N0} Datensätze";
            }
            else
            {
                this._statusIndicator.Fill = Brushes.LimeGreen;
                this._statusIndicator.Fill = this.IsFiltered ? CreateIndicatorBrush(Colors.Goldenrod) : CreateIndicatorBrush(Colors.LimeGreen);
                this._txtFilter.Text = "Kein Filter";
                this._txtFilter.FontWeight = FontWeights.Normal;
                this._txtRows.Text = $"{this.TotalItemCount:N0} Datensätze";
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
