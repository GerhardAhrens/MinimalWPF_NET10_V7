namespace System.Windows.Controls
{
    using System.Windows;

    public class ProgressOverlay : Control
    {
        static ProgressOverlay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ProgressOverlay),
                new FrameworkPropertyMetadata(typeof(ProgressOverlay)));
        }

        #region IsActived

        public static readonly DependencyProperty IsActivedProperty =
            DependencyProperty.Register(
                nameof(IsActived),
                typeof(bool),
                typeof(ProgressOverlay),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnIsActivedChanged));

        public bool IsActived
        {
            get => (bool)GetValue(IsActivedProperty);
            set => SetValue(IsActivedProperty, value);
        }

        private static void OnIsActivedChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (ProgressOverlay)d;

            // Beim erneuten Öffnen auf MinValue zurücksetzen.
            if ((bool)e.NewValue)
            {
                control.SetCurrentValue(
                    ValueProperty,
                    control.MinValue);
            }
        }

        #endregion

        #region MinValue

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(
                nameof(MinValue),
                typeof(double),
                typeof(ProgressOverlay),
                new FrameworkPropertyMetadata(
                    0.0,
                    OnRangeChanged));

        public double MinValue
        {
            get => (double)GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        #endregion

        #region MaxValue

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(
                nameof(MaxValue),
                typeof(double),
                typeof(ProgressOverlay),
                new FrameworkPropertyMetadata(
                    100.0,
                    OnRangeChanged));

        public double MaxValue
        {
            get => (double)GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        #endregion

        #region Value

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                nameof(Value),
                typeof(double),
                typeof(ProgressOverlay),
                new FrameworkPropertyMetadata(
                    0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnValueChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (ProgressOverlay)d;

            control.CheckValue();
        }

        #endregion

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(ProgressOverlay),
                new PropertyMetadata(string.Empty));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region RangeChanged

        private static void OnRangeChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (ProgressOverlay)d;

            control.CheckValue();
        }

        #endregion

        private void CheckValue()
        {
            if (!IsActived)
                return;

            // Ungültiger Bereich
            if (MaxValue <= MinValue)
            {
                IsActived = false;
                return;
            }

            // Value unter MinValue
            if (Value < MinValue)
            {
                SetCurrentValue(ValueProperty, MinValue);
                return;
            }

            // Maximum erreicht
            if (Value >= MaxValue)
            {
                SetCurrentValue(ValueProperty, MaxValue);
                SetCurrentValue(IsActivedProperty, false);
            }
        }
    }
}
