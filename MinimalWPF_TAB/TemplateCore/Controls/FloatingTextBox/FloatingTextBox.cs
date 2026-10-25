namespace System.Windows.Controls
{
    using System;
    using System.Windows.Media;

    [TemplatePart(Name = PART_ContentHost, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = PART_FloatingLabel, Type = typeof(TextBlock))]
    public class FloatingTextBox : TextBox
    {
        private const string PART_ContentHost = "PART_ContentHost";
        private const string PART_FloatingLabel = "PART_FloatingLabel";

        static FloatingTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata( typeof(FloatingTextBox), new FrameworkPropertyMetadata( typeof(FloatingTextBox)));
        }

        #region Title

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                nameof(Title),
                typeof(string),
                typeof(FloatingTextBox),
                new FrameworkPropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion

        #region TitleForeground

        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register(
                nameof(TitleForeground),
                typeof(Brush),
                typeof(FloatingTextBox),
                new FrameworkPropertyMetadata(
                    Brushes.Gray));

        public Brush TitleForeground
        {
            get => (Brush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }

        #endregion

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(FloatingTextBox),
                new FrameworkPropertyMetadata(
                    new CornerRadius(4)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region FloatingLabelFontSize

        public static readonly DependencyProperty FloatingLabelFontSizeProperty =
            DependencyProperty.Register(
                nameof(FloatingLabelFontSize),
                typeof(double),
                typeof(FloatingTextBox),
                new FrameworkPropertyMetadata(12.0));

        public double FloatingLabelFontSize
        {
            get => (double)GetValue(FloatingLabelFontSizeProperty);
            set => SetValue(FloatingLabelFontSizeProperty, value);
        }

        #endregion

        #region FloatingLabelDuration

        public static readonly DependencyProperty FloatingLabelDurationProperty =
            DependencyProperty.Register(
                nameof(FloatingLabelDuration),
                typeof(Duration),
                typeof(FloatingTextBox),
                new FrameworkPropertyMetadata(
                    new Duration(TimeSpan.FromMilliseconds(180))));

        public Duration FloatingLabelDuration
        {
            get => (Duration)GetValue(FloatingLabelDurationProperty);
            set => SetValue(FloatingLabelDurationProperty, value);
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.UpdateVisualState(false);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.UpdateVisualState(true);
        }

        private void UpdateVisualState(bool useTransitions = true)
        {
            string state = string.IsNullOrEmpty(Text) ? "Resting" : "Floating";
            VisualStateManager.GoToState(this, state, useTransitions);
        }
    }
}
