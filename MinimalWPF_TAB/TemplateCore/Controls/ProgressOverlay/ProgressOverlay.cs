namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Media;

    public class ProgressOverlay : Control
    {
        static ProgressOverlay()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressOverlay), new FrameworkPropertyMetadata(typeof(ProgressOverlay)));
        }

        #region IsActived

        public static readonly DependencyProperty IsActivedProperty =
            DependencyProperty.Register(
                nameof(IsActived),
                typeof(bool),
                typeof(ProgressOverlay),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsActived
        {
            get => (bool)GetValue(IsActivedProperty);
            set => SetValue(IsActivedProperty, value);
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

        #endregion


        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(ProgressOverlay),
                new FrameworkPropertyMetadata(string.Empty));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region PercentageForeground

        public static readonly DependencyProperty PercentageForegroundProperty =
            DependencyProperty.Register(nameof(PercentageForeground), typeof(Brush), typeof(ProgressOverlay), new FrameworkPropertyMetadata(Brushes.Black));

        public Brush PercentageForeground
        {
            get => (Brush)GetValue(PercentageForegroundProperty);
            private set => SetValue(PercentageForegroundProperty, value);
        }

        #endregion

        #region Events

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ProgressOverlay)d;

            control.UpdatePercentageForeground();
            control.CheckProgress();
        }

        private void UpdatePercentageForeground()
        {
            if (MaxValue <= MinValue)
            {
                this.PercentageForeground = Brushes.Black;
                return;
            }

            double percentage = (Value - MinValue) / (MaxValue - MinValue) * 100.0;

            this.PercentageForeground = percentage >= 50.0 ? Brushes.White : Brushes.Black;
        }
        private static void OnRangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ProgressOverlay)d;

            control.UpdatePercentageForeground();
            control.CheckProgress();
        }
        #endregion

        #region Progress

        private void CheckProgress()
        {
            if (IsActived == false)
            {
                return;
            }

            // Ungültiger Wertebereich
            if (MaxValue <= MinValue)
            {
                SetCurrentValue(IsActivedProperty, false);
                return;
            }

            // Wert kleiner als Minimum
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

        #endregion
    }
}
