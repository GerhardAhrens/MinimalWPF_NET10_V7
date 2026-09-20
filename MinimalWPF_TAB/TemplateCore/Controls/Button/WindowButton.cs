namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class WindowButton : Button
    {
        static WindowButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowButton), new FrameworkPropertyMetadata(typeof(WindowButton)));
        }

        #region Image

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
                nameof(Image),
                typeof(DrawingImage),
                typeof(WindowButton),
                new FrameworkPropertyMetadata(null));

        public DrawingImage Image
        {
            get => (DrawingImage)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        #endregion

        #region ImagePlacement

        public static readonly DependencyProperty ImagePlacementProperty =
            DependencyProperty.Register(
                nameof(ImagePlacement),
                typeof(WindowButtonImagePlacement),
                typeof(WindowButton),
                new FrameworkPropertyMetadata(
                    WindowButtonImagePlacement.Left));

        public WindowButtonImagePlacement ImagePlacement
        {
            get => (WindowButtonImagePlacement)GetValue(ImagePlacementProperty);
            set => SetValue(ImagePlacementProperty, value);
        }

        #endregion

        #region ImageSize

        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register(
                nameof(ImageSize),
                typeof(double),
                typeof(WindowButton),
                new FrameworkPropertyMetadata(16.0));

        public double ImageSize
        {
            get => (double)GetValue(ImageSizeProperty);
            set => SetValue(ImageSizeProperty, value);
        }

        #endregion
    }

    public enum WindowButtonImagePlacement
    { 
        Left, 
        Right 
    }

    public sealed class ImageVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return Visibility.Collapsed;
            }

            var image = values[0];
            var placement = values[1];

            if (image == null)
            {
                return Visibility.Collapsed;
            }

            if (parameter is not WindowButtonImagePlacement requiredPlacement)
            {
                return Visibility.Collapsed;
            }

            return placement is WindowButtonImagePlacement actualPlacement && actualPlacement == requiredPlacement ? Visibility.Visible : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
