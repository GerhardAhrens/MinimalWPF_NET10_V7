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
        private BindingBase _imageBinding;

        public BindingBase ImageBinding
        {
            get => this._imageBinding;
            set
            {
                this._imageBinding = value;
                this.CreateDefaultCellTemplate();
            }
        }

        public double ImageWidth { get; set; } = 20;
        public double ImageHeight { get; set; } = 20;

        public HorizontalAlignment ImageHorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        public VerticalAlignment ImageVerticalAlignment { get; set; } = VerticalAlignment.Center;

        private void CreateDefaultCellTemplate()
        {
            if (this._imageBinding == null)
            {
                return;
            }

            var factory = new FrameworkElementFactory(typeof(Image));

            factory.SetValue(FrameworkElement.WidthProperty, this.ImageWidth);
            factory.SetValue(FrameworkElement.HeightProperty, this.ImageHeight);
            factory.SetValue(FrameworkElement.HorizontalAlignmentProperty, this.ImageHorizontalAlignment);
            factory.SetValue(FrameworkElement.VerticalAlignmentProperty, this.ImageVerticalAlignment);
            factory.SetBinding(Image.SourceProperty, this._imageBinding);

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
            {
                if (value is DrawingImage di)
                {
                    return (DrawingImage)di;
                }
                else
                {
                    return null;
                }
            }

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
