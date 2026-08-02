namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;

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
