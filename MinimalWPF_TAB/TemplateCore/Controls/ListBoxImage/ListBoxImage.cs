namespace System.Windows.Controls
{
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media;

    public class ListBoxImage : ListBox
    {

        #region Orientation

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(ListBoxImage),
                new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure, OnOrientationChanged));

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBoxImage listBox)
            {
                listBox.UpdateItemsPanel();
            }
        }

        #endregion

        #region ImageSize

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(
                nameof(ImageWidth),
                typeof(double),
                typeof(ListBoxImage),
                new FrameworkPropertyMetadata(48.0));

        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register(
                nameof(ImageHeight),
                typeof(double),
                typeof(ListBoxImage),
                new FrameworkPropertyMetadata(48.0));

        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        #endregion

        #region ImageStretch

        public static readonly DependencyProperty ImageStretchProperty =
            DependencyProperty.Register(
                nameof(ImageStretch),
                typeof(Stretch),
                typeof(ListBoxImage),
                new FrameworkPropertyMetadata(Stretch.Uniform));

        public Stretch ImageStretch
        {
            get => (Stretch)GetValue(ImageStretchProperty);
            set => SetValue(ImageStretchProperty, value);
        }
        #endregion ImageStretch

        #region SymbolColor

        public static readonly DependencyProperty SymbolColorProperty =
            DependencyProperty.Register(
                nameof(SymbolColor),
                typeof(Brush),
                typeof(ListBoxImage),
                new FrameworkPropertyMetadata(Brushes.Black));

        public Brush SymbolColor
        {
            get => (Brush)GetValue(SymbolColorProperty);
            set => SetValue(SymbolColorProperty, value);
        }

        #endregion SymbolColor

        #region Constructor

        public ListBoxImage()
        {
            this.SelectionMode = SelectionMode.Single;
            VirtualizingStackPanel.SetIsVirtualizing(this, true);
            VirtualizingStackPanel.SetVirtualizationMode(this, VirtualizationMode.Recycling);

            ScrollViewer.SetCanContentScroll(this, true);
            // Der Key des Dictionary-Eintrags wird automatisch
            // als SelectedValue verwendet.
            this.SelectedValuePath = "Key";

            CreateControlTemplate();
            CreateItemTemplate();
            UpdateItemsPanel();
        }

        #endregion

        #region Template

        private void CreateControlTemplate()
        {
            /*
             * Die eigentliche ListBox benötigt kein besonderes
             * ControlTemplate. Das Standardtemplate der ListBox
             * kann verwendet werden.
             */
        }

        private void CreateItemTemplate()
        {
            var image = new FrameworkElementFactory(typeof(Image));

            image.SetValue(Image.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            image.SetValue(Image.VerticalAlignmentProperty, VerticalAlignment.Center);
            image.SetValue(Image.StretchProperty, this.ImageStretch);
            image.SetBinding(Image.WidthProperty,
                new Binding(nameof(ImageWidth))
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ListBoxImage), 1)
                });

            image.SetBinding(Image.HeightProperty,
                new Binding(nameof(ImageHeight))
                {
                    RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ListBoxImage), 1)
                });

            var multiBinding = new MultiBinding
            {
                Converter = new VectorImageMultiConverter()
            };

            multiBinding.Bindings.Add(new Binding("Value"));

            multiBinding.Bindings.Add(
                new Binding(nameof(SymbolColor))
                {
                    RelativeSource = new RelativeSource(
                        RelativeSourceMode.FindAncestor,
                        typeof(ListBoxImage),
                        1)
                });

            image.SetBinding(Image.SourceProperty, multiBinding);

            var border = new FrameworkElementFactory(typeof(Border));

            border.SetValue(Border.PaddingProperty, new Thickness(4));

            border.AppendChild(image);

            ItemTemplate = new DataTemplate
            {
                VisualTree = border
            };
        }

        #endregion

        #region ItemsPanel

        private void UpdateItemsPanel()
        {
            var panelType = Orientation == Orientation.Horizontal ? typeof(StackPanel) : typeof(StackPanel);
            var panel = new FrameworkElementFactory(typeof(VirtualizingStackPanel));

            panel.SetValue(VirtualizingStackPanel.OrientationProperty,Orientation);
            panel.SetValue(VirtualizingStackPanel.IsVirtualizingProperty, true);
            panel.SetValue(VirtualizingStackPanel.VirtualizationModeProperty, VirtualizationMode.Recycling);
            panel.SetValue(StackPanel.OrientationProperty, Orientation);

            var template = new ItemsPanelTemplate(panel);

            ItemsPanel = template;
        }

        #endregion
    }

    /// <summary>
    /// Wandelt den String eines Dictionary-Eintrags
    /// in ein ImageSource um.
    ///
    /// Unterstützt:
    ///   1. DrawingImage als XAML
    ///   2. PathGeometry / Geometry-String
    /// </summary>
    public sealed class VectorImageMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is not string text || string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            Brush brush = values[1] as Brush ?? Brushes.Black;

            if (parameter is Brush parameterBrush)
            {
                brush = parameterBrush;
            }

            text = text.Trim();

            try
            {
                // -------------------------------------------------
                // Fall 1:
                // DrawingImage als XAML
                //
                // Beispiel:
                //
                // <DrawingImage xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
                //   ...
                // </DrawingImage>
                // -------------------------------------------------

                if (text.StartsWith("<DrawingImage", StringComparison.OrdinalIgnoreCase))
                {
                    using var stringReader = new StringReader(text);
                    using var xmlReader = System.Xml.XmlReader.Create(stringReader);

                    var result = XamlReader.Load(xmlReader);

                    return result as DrawingImage;
                }

                // -------------------------------------------------
                // Fall 2:
                // PathGeometry / Geometry-String
                //
                // Beispiel:
                //
                // M 10,10 L 100,10 L 100,100 Z
                // -------------------------------------------------

                var geometry = Geometry.Parse(text);

                var drawing = new GeometryDrawing
                {
                    Geometry = geometry,
                    Brush = brush
                };

                return new DrawingImage(drawing);
            }
            catch
            {
                // Ungültiger Vektorinhalt.
                // Kein Fehler in der UI erzeugen.
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
