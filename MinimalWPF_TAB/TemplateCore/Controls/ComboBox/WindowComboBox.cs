namespace System.Windows.Controls
{
    public class WindowComboBox : ComboBox
    {
        static WindowComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(typeof(WindowComboBox)));
        }

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(new CornerRadius(6)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region RevealHighlight

        public static readonly DependencyProperty RevealHighlightProperty =
            DependencyProperty.Register(
                nameof(RevealHighlight),
                typeof(bool),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(true));

        public bool RevealHighlight
        {
            get => (bool)GetValue(RevealHighlightProperty);
            set => SetValue(RevealHighlightProperty, value);
        }

        #endregion

        #region EnableOpenAnimation

        public static readonly DependencyProperty EnableOpenAnimationProperty =
            DependencyProperty.Register(
                nameof(EnableOpenAnimation),
                typeof(bool),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(true));

        public bool EnableOpenAnimation
        {
            get => (bool)GetValue(EnableOpenAnimationProperty);
            set => SetValue(EnableOpenAnimationProperty, value);
        }

        #endregion

        #region EnableSelectionHighlight

        public static readonly DependencyProperty EnableSelectionHighlightProperty =
            DependencyProperty.Register(
                nameof(EnableSelectionHighlight),
                typeof(bool),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(true));

        public bool EnableSelectionHighlight
        {
            get => (bool)GetValue(EnableSelectionHighlightProperty);
            set => SetValue(EnableSelectionHighlightProperty, value);
        }

        #endregion
    }
}
