namespace System.Windows.Controls
{
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class ContextMenuVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection collection && collection.Count > 0)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
