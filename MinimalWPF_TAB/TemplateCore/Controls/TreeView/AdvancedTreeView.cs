namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class AdvancedTreeView : TreeView
    {
        static AdvancedTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTreeView), new FrameworkPropertyMetadata(typeof(AdvancedTreeView)));
        }
    }

    public class ExpandedImageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
                return null;

            var image = values[0] as DrawingImage;
            var expandedImage = values[1] as DrawingImage;

            if (values[2] is bool isExpanded &&
                isExpanded &&
                expandedImage != null)
            {
                return expandedImage;
            }

            return image;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
