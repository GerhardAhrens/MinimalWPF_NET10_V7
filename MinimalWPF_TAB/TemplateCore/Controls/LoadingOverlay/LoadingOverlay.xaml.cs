namespace System.Windows.Controls
{
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    /// <summary>
    /// Interaktionslogik für LoadingOverlay.xaml
    /// </summary>
    public partial class LoadingOverlay : UserControl
    {
        public LoadingOverlay()
        {
            InitializeComponent();
        }

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(LoadingOverlay), new PropertyMetadata("Bitte warten..."));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region IsActived

        public static readonly DependencyProperty IsActivedProperty =
            DependencyProperty.Register(nameof(IsActived), typeof(bool), typeof(LoadingOverlay),
                new PropertyMetadata(false, OnIsActivedChanged));

        public bool IsActived
        {
            get => (bool)GetValue(IsActivedProperty);
            set => SetValue(IsActivedProperty, value);
        }

        private static void OnIsActivedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LoadingOverlay control)
            {
                control.UpdateLoadingState((bool)e.NewValue);
            }
        }

        #endregion

        #region Loading

        private void UpdateLoadingState(bool active)
        {
            if (active == true)
            {
                this.Show();
            }
            else
            {
                this.Hide();
            }
        }

        private void Show()
        {
            // Laufende Animation abbrechen
            this.Root.BeginAnimation(UIElement.OpacityProperty, null);

            // Sofort sichtbar machen
            this.Root.Visibility = Visibility.Visible;
            this.Root.IsHitTestVisible = true;

            // Falls vorher noch Opacity = 0 war
            this.Root.Opacity = 0;

            this.StartSpinner();

            // Fade In
            var animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            this.Root.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        private void Hide()
        {
            // Laufende Animation abbrechen
            this.Root.BeginAnimation(UIElement.OpacityProperty, null);

            // Wenn bereits unsichtbar
            if (Root.Visibility != Visibility.Visible)
            {
                return;
            }

            var animation = new DoubleAnimation
            {
                From = Root.Opacity,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new QuadraticEase
                {
                    EasingMode = EasingMode.EaseIn
                }
            };

            animation.Completed += (_, _) =>
            {
                // Erst nach dem Fade-Out ausblenden
                this.Root.Visibility = Visibility.Collapsed;
                this.Root.IsHitTestVisible = false;

                this.StopSpinner();

                this.Root.Opacity = 0;
            };

            this.Root.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        #endregion

        #region Spinner

        private void StartSpinner()
        {
            this.SpinnerRotate.BeginAnimation(RotateTransform.AngleProperty, null);

            var animation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromMilliseconds(2000),
                RepeatBehavior = RepeatBehavior.Forever
            };

            this.SpinnerRotate.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        private void StopSpinner()
        {
            this.SpinnerRotate.BeginAnimation(RotateTransform.AngleProperty, null);
            this.SpinnerRotate.Angle = 0;
        }

        #endregion
    }
}
