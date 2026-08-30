namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;

    public class SwitchThumbOffsetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3)
            {
                return 0.0;
            }

            if (!double.TryParse(values[0]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double width))
            {
                return 0.0;
            }

            if (!double.TryParse(values[1]?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double thumb))
            {
                return 0.0;
            }

            bool isChecked = values[2] is bool b && b;

            if (!isChecked)
            {
                return 0.0;
            }

            return width - thumb - 6.0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
