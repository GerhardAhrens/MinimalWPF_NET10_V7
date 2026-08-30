namespace System.Windows.Controls
{
    using System.Windows.Media;

    public class SwitchBox : CheckBox
    {
        static SwitchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(typeof(SwitchBox)));
        }


        #region OnContent

        public static readonly DependencyProperty OnContentProperty =
            DependencyProperty.Register(
                nameof(OnContent),
                typeof(object),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata("Ein"));

        public object OnContent
        {
            get => GetValue(OnContentProperty);
            set => SetValue(OnContentProperty, value);
        }

        #endregion


        #region OffContent

        public static readonly DependencyProperty OffContentProperty =
            DependencyProperty.Register(
                nameof(OffContent),
                typeof(object),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata("Aus"));

        public object OffContent
        {
            get => GetValue(OffContentProperty);
            set => SetValue(OffContentProperty, value);
        }

        #endregion


        #region OnColor

        public static readonly DependencyProperty OnColorProperty =
            DependencyProperty.Register(
                nameof(OnColor),
                typeof(Brush),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Color.FromRgb(0, 120, 212))));

        public Brush OnColor
        {
            get => (Brush)GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }

        #endregion


        #region OffColor

        public static readonly DependencyProperty OffColorProperty =
            DependencyProperty.Register(
                nameof(OffColor),
                typeof(Brush),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(
                    new SolidColorBrush(Color.FromRgb(110, 110, 110))));

        public Brush OffColor
        {
            get => (Brush)GetValue(OffColorProperty);
            set => SetValue(OffColorProperty, value);
        }

        #endregion


        #region ThumbColor

        public static readonly DependencyProperty ThumbColorProperty =
            DependencyProperty.Register(
                nameof(ThumbColor),
                typeof(Brush),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(Brushes.White));

        public Brush ThumbColor
        {
            get => (Brush)GetValue(ThumbColorProperty);
            set => SetValue(ThumbColorProperty, value);
        }

        #endregion


        #region SwitchWidth

        public static readonly DependencyProperty SwitchWidthProperty =
            DependencyProperty.Register(
                nameof(SwitchWidth),
                typeof(double),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(44.0));

        public double SwitchWidth
        {
            get => (double)GetValue(SwitchWidthProperty);
            set => SetValue(SwitchWidthProperty, value);
        }

        #endregion


        #region SwitchHeight

        public static readonly DependencyProperty SwitchHeightProperty =
            DependencyProperty.Register(
                nameof(SwitchHeight),
                typeof(double),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(24.0));

        public double SwitchHeight
        {
            get => (double)GetValue(SwitchHeightProperty);
            set => SetValue(SwitchHeightProperty, value);
        }

        #endregion


        #region ThumbSize

        public static readonly DependencyProperty ThumbSizeProperty =
            DependencyProperty.Register(
                nameof(ThumbSize),
                typeof(double),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(18.0));

        public double ThumbSize
        {
            get => (double)GetValue(ThumbSizeProperty);
            set => SetValue(ThumbSizeProperty, value);
        }

        #endregion


        #region AnimationDuration

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(
                nameof(AnimationDuration),
                typeof(Duration),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(
                    new Duration(TimeSpan.FromMilliseconds(150))));

        public Duration AnimationDuration
        {
            get => (Duration)GetValue(AnimationDurationProperty);
            set => SetValue(AnimationDurationProperty, value);
        }

        #endregion


        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SwitchBox),
                new FrameworkPropertyMetadata(
                    new CornerRadius(12)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion
    }
}
