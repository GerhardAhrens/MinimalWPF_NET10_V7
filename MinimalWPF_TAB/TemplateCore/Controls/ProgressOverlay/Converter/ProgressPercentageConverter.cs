namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;

    public class ProgressPercentageConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
            {
                return "0 %";
            }

            if (values[0] is not double value || values[1] is not double minimum || values[2] is not double maximum)
            {
                return "0 %";
            }

            if (maximum <= minimum)
            {
                return "0 %";
            }

            double percentage = (value - minimum) / (maximum - minimum) * 100.0;

            percentage = Math.Clamp(percentage, 0.0, 100.0);

            return $"{percentage:0} %";
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
