namespace System.Windows.Controls
{
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;
    using System.Windows.Media;
    using System.Xml;

    public class AdvancedImageColumn : GridViewColumn
    {
        public static readonly DependencyProperty ImageBindingProperty =
            DependencyProperty.Register(
                nameof(ImageBinding),
                typeof(BindingBase),
                typeof(AdvancedImageColumn),
                new PropertyMetadata(null, OnImageBindingChanged));

        public BindingBase ImageBinding
        {
            get => (BindingBase)GetValue(ImageBindingProperty);
            set => SetValue(ImageBindingProperty, value);
        }

        public double ImageWidth { get; set; } = 20;
        public double ImageHeight { get; set; } = 20;

        public HorizontalAlignment ImageHorizontalAlignment { get; set; }
            = HorizontalAlignment.Center;

        public VerticalAlignment ImageVerticalAlignment { get; set; }
            = VerticalAlignment.Center;

        private static void OnImageBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AdvancedImageColumn column)
            {
                column.CreateDefaultCellTemplate();
            }
        }

        private void CreateDefaultCellTemplate()
        {
            if (ImageBinding == null)
                return;

            var factory = new FrameworkElementFactory(typeof(Image));

            factory.SetValue(FrameworkElement.WidthProperty, ImageWidth);
            factory.SetValue(FrameworkElement.HeightProperty, ImageHeight);
            factory.SetValue(
                FrameworkElement.HorizontalAlignmentProperty,
                ImageHorizontalAlignment);
            factory.SetValue(
                FrameworkElement.VerticalAlignmentProperty,
                ImageVerticalAlignment);

            var imageBinding = new Binding("Symbol")
            {
                Converter = new DrawingImageConverter()
            };

            factory.SetBinding(Image.SourceProperty, imageBinding);

            CellTemplate = new DataTemplate
            {
                VisualTree = factory
            };
        }
    }

    public class DrawingImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string xaml || string.IsNullOrWhiteSpace(xaml))
                return null;

            try
            {
                using var stringReader = new StringReader(xaml);
                using var xmlReader = XmlReader.Create(stringReader);

                return XamlReader.Load(xmlReader) as DrawingImage;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
