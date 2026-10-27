namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Media;

    public partial class IconTextBox : UserControl
    {
        public IconTextBox()
        {
            this.InitializeComponent();
        }

        #region Icon

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(
                nameof(Icon),
                typeof(DrawingImage),
                typeof(IconTextBox),
                new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Das anzuzeigende Icon.
        /// Ist kein Icon gesetzt, wird das Standardsymbol T angezeigt.
        /// </summary>
        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        #endregion

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(IconTextBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Der eingegebene Text.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region Watermark

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                nameof(Watermark),
                typeof(string),
                typeof(IconTextBox),
                new FrameworkPropertyMetadata("Beschreibung"));

        /// <summary>
        /// Der Wasserzeichentext.
        /// Standard: Beschreibung
        /// </summary>
        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        #endregion

        #region WatermarkForeground

        public static readonly DependencyProperty WatermarkForegroundProperty =
            DependencyProperty.Register(
                nameof(WatermarkForeground),
                typeof(Brush),
                typeof(IconTextBox),
                new FrameworkPropertyMetadata(Brushes.Gray));

        /// <summary>
        /// Die Farbe des Wasserzeichens.
        /// Standard: Grau
        /// </summary>
        public Brush WatermarkForeground
        {
            get => (Brush)GetValue(WatermarkForegroundProperty);
            set => SetValue(WatermarkForegroundProperty, value);
        }

        #endregion
    }
}
