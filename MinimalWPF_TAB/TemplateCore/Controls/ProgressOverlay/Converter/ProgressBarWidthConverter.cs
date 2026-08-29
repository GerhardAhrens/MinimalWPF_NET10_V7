namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;

    public class ProgressBarWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 4)
            {
                return 0.0;
            }

            if (values[0] is not double value ||
                values[1] is not double minimum ||
                values[2] is not double maximum ||
                values[3] is not double width)
            {
                return 0.0;
            }

            if (maximum <= minimum || width <= 0)
            {
                return 0.0;
            }

            double percentage = (value - minimum) / (maximum - minimum);

            percentage = Math.Clamp(percentage, 0.0, 1.0);

            return width * percentage;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
