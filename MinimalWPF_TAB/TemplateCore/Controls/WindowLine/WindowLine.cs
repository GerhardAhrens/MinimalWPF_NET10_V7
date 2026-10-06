namespace System.Windows.Controls
{
    using System;
    using System.Windows.Media;

    /// <summary>
    /// Defines the visual style of a WindowLine.
    /// </summary>
    public enum WindowLineStyle
    {
        Solid,
        Dashed,
        Dotted
    }

    /// <summary>
    /// Defines the alignment of the WindowLine header.
    ///
    /// For horizontal lines:
    /// Left   = header on the left
    /// Center = header in the center
    /// Right  = header on the right
    ///
    /// For vertical lines:
    /// Left   = header at the top
    /// Center = header in the center
    /// Right  = header at the bottom
    /// </summary>
    public enum WindowLineHeaderAlignment
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// A Windows 11 styled horizontal or vertical separator line
    /// with optional header text.
    /// </summary>
    public class WindowLine : Control
    {
        #region Dependency Properties

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    Orientation.Horizontal,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register(
                nameof(LineColor),
                typeof(Brush),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register(
                nameof(Thickness),
                typeof(double),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    1.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender),
                ValidateThickness);

        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register(
                nameof(LineStyle),
                typeof(WindowLineStyle),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    WindowLineStyle.Solid,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(string),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeaderAlignmentProperty =
            DependencyProperty.Register(
                nameof(HeaderAlignment),
                typeof(WindowLineHeaderAlignment),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    WindowLineHeaderAlignment.Center,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register(
                nameof(HeaderMargin),
                typeof(double),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    8.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender),
                ValidateNonNegative);

        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(
                nameof(HeaderForeground),
                typeof(Brush),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.Register(
                nameof(HeaderFontSize),
                typeof(double),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    13.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender),
                ValidateFontSize);

        public static readonly DependencyProperty HeaderFontWeightProperty =
            DependencyProperty.Register(
                nameof(HeaderFontWeight),
                typeof(FontWeight),
                typeof(WindowLine),
                new FrameworkPropertyMetadata(
                    FontWeights.Normal,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion

        #region CLR Properties

        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);
            set => SetValue(OrientationProperty, value);
        }

        public Brush LineColor
        {
            get => (Brush)GetValue(LineColorProperty);
            set => SetValue(LineColorProperty, value);
        }

        public double Thickness
        {
            get => (double)GetValue(ThicknessProperty);
            set => SetValue(ThicknessProperty, value);
        }

        public WindowLineStyle LineStyle
        {
            get => (WindowLineStyle)GetValue(LineStyleProperty);
            set => SetValue(LineStyleProperty, value);
        }

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        public WindowLineHeaderAlignment HeaderAlignment
        {
            get => (WindowLineHeaderAlignment)GetValue(HeaderAlignmentProperty);
            set => SetValue(HeaderAlignmentProperty, value);
        }

        public double HeaderMargin
        {
            get => (double)GetValue(HeaderMarginProperty);
            set => SetValue(HeaderMarginProperty, value);
        }

        public Brush HeaderForeground
        {
            get => (Brush)GetValue(HeaderForegroundProperty);
            set => SetValue(HeaderForegroundProperty, value);
        }

        public double HeaderFontSize
        {
            get => (double)GetValue(HeaderFontSizeProperty);
            set => SetValue(HeaderFontSizeProperty, value);
        }

        public FontWeight HeaderFontWeight
        {
            get => (FontWeight)GetValue(HeaderFontWeightProperty);
            set => SetValue(HeaderFontWeightProperty, value);
        }

        #endregion

        #region Constructor

        static WindowLine()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowLine), new FrameworkPropertyMetadata(typeof(WindowLine)));
        }

        public WindowLine()
        {
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;

            // Windows 11-like default colors.
            LineColor = new SolidColorBrush(Color.FromArgb(80, 128, 128, 128));

            HeaderForeground = new SolidColorBrush(Color.FromRgb(60, 60, 60));

            Background = Brushes.Transparent;
        }

        #endregion

        #region Measure / Render

        protected override Size MeasureOverride(Size availableSize)
        {
            bool hasHeader = !string.IsNullOrWhiteSpace(Header);

            if (!hasHeader)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    return new Size(0, Math.Max(Thickness, 1));
                }

                return new Size(Math.Max(Thickness, 1), 0);
            }

            var dpi = VisualTreeHelper.GetDpi(this);

            var formattedText = CreateFormattedText(dpi);

            if (Orientation == Orientation.Horizontal)
            {
                double height = Math.Max(Thickness, formattedText.Height);

                return new Size(0, height);
            }
            else
            {
                double width = Math.Max(Thickness, formattedText.Height);

                return new Size(width, 0);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ActualWidth <= 0 || ActualHeight <= 0)
                return;

            var lineBrush = LineColor ?? SystemColors.ControlDarkBrush;
            var textBrush = HeaderForeground ?? SystemColors.ControlTextBrush;

            bool hasHeader = !string.IsNullOrWhiteSpace(Header);

            if (!hasHeader)
            {
                DrawSimpleLine(drawingContext, lineBrush);

                return;
            }

            var dpi = VisualTreeHelper.GetDpi(this);
            var formattedText = CreateFormattedText(dpi);

            if (Orientation == Orientation.Horizontal)
            {
                DrawHorizontal(drawingContext, formattedText, lineBrush, textBrush);
            }
            else
            {
                DrawVertical(drawingContext, formattedText, lineBrush, textBrush);
            }
        }

        #endregion

        #region Horizontal

        private void DrawHorizontal(DrawingContext dc, FormattedText text, Brush lineBrush, Brush textBrush)
        {
            double y = ActualHeight / 2.0;

            double textWidth = text.Width;
            double textHeight = text.Height;

            double startX = 0;
            double endX = ActualWidth;

            double textX;

            switch (HeaderAlignment)
            {
                case WindowLineHeaderAlignment.Left:
                    textX = 0;

                    DrawLine(dc, lineBrush, startX, y, textX - HeaderMargin, y);
                    DrawText(dc, text, textBrush, textX, (ActualHeight - textHeight) / 2.0);
                    DrawLine(dc, lineBrush, textX + textWidth + HeaderMargin, y,endX, y);

                    break;

                case WindowLineHeaderAlignment.Right:
                    textX = ActualWidth - textWidth;

                    DrawLine(dc, lineBrush, startX, y, textX - HeaderMargin, y);
                    DrawText(dc, text, textBrush, textX, (ActualHeight - textHeight) / 2.0);
                    DrawLine(dc, lineBrush, textX + textWidth + HeaderMargin, y, endX, y);

                    break;

                default:
                    textX = (ActualWidth - textWidth) / 2.0;

                    DrawLine(dc, lineBrush, startX, y, textX - HeaderMargin, y);
                    DrawText(dc, text, textBrush, textX, (ActualHeight - textHeight) / 2.0);
                    DrawLine(dc, lineBrush, textX + textWidth + HeaderMargin, y, endX, y);

                    break;
            }
        }

        #endregion

        #region Vertical

        private void DrawVertical(DrawingContext dc, FormattedText text, Brush lineBrush, Brush textBrush)
        {
            double x = ActualWidth / 2.0;

            // For vertical lines the text is rotated by 90 degrees.
            double textWidth = text.Height;
            double textHeight = text.Width;

            double textY;

            switch (HeaderAlignment)
            {
                case WindowLineHeaderAlignment.Left:
                    textY = 0;
                    break;

                case WindowLineHeaderAlignment.Right:
                    textY = ActualHeight - textHeight;
                    break;

                default:
                    textY = (ActualHeight - textHeight) / 2.0;
                    break;
            }

            double centerY = textY + textHeight / 2.0;

            // Upper part of the line
            DrawLine(dc, lineBrush, x, 0, x, centerY - textHeight / 2.0 - HeaderMargin);

            // Lower part of the line
            DrawLine(dc, lineBrush, x, centerY + textHeight / 2.0 + HeaderMargin, x, ActualHeight);

            // Rotate header text by 90 degrees.
            dc.PushTransform(new RotateTransform(-90, x, centerY));

            DrawText(dc, text, textBrush, x - text.Width / 2.0, centerY - text.Height / 2.0);

            dc.Pop();
        }
        #endregion

        #region Drawing

        private void DrawSimpleLine(DrawingContext dc, Brush brush)
        {
            double halfThickness = Thickness / 2.0;

            if (Orientation == Orientation.Horizontal)
            {
                DrawLine(dc, brush, 0, halfThickness, ActualWidth, halfThickness);
            }
            else
            {
                DrawLine( dc, brush, halfThickness, 0, halfThickness, ActualHeight);
            }
        }

        private void DrawLine(DrawingContext dc, Brush brush, double x1, double y1, double x2, double y2)
        {
            if (x2 <= x1 && Math.Abs(y2 - y1) < 0.01)
                return;

            if (y2 <= y1 && Math.Abs(x2 - x1) < 0.01)
                return;

            var pen = new Pen(
                brush,
                Math.Max(0.1, Thickness));

            switch (LineStyle)
            {
                case WindowLineStyle.Dashed:
                    pen.DashStyle = new DashStyle(
                        new double[] { 6, 4 },
                        0);
                    break;

                case WindowLineStyle.Dotted:
                    pen.DashStyle = new DashStyle(
                        new double[] { 1, 3 },
                        0);

                    pen.DashCap = PenLineCap.Round;
                    break;

                case WindowLineStyle.Solid:
                default:
                    break;
            }

            pen.StartLineCap = PenLineCap.Flat;
            pen.EndLineCap = PenLineCap.Flat;

            dc.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
        }

        private void DrawText(DrawingContext dc, FormattedText text, Brush brush, double x, double y)
        {
            dc.DrawText(text, new Point(x, y));
        }

        #endregion

        #region Text

        private FormattedText CreateFormattedText(DpiScale dpi)
        {
            return new FormattedText(Header ?? string.Empty, System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(
                    FontFamily ?? new FontFamily("Segoe UI"),
                    FontStyle,
                    HeaderFontWeight,
                    FontStretch),
                HeaderFontSize,
                HeaderForeground ?? SystemColors.ControlTextBrush, dpi.PixelsPerDip);
        }

        #endregion

        #region Validation

        private static bool ValidateThickness(object value)
        {
            return value is double d &&
                   !double.IsNaN(d) &&
                   !double.IsInfinity(d) &&
                   d >= 0;
        }

        private static bool ValidateNonNegative(object value)
        {
            return value is double d &&
                   !double.IsNaN(d) &&
                   !double.IsInfinity(d) &&
                   d >= 0;
        }

        private static bool ValidateFontSize(object value)
        {
            return value is double d &&
                   !double.IsNaN(d) &&
                   !double.IsInfinity(d) &&
                   d > 0;
        }

        #endregion
    }
}
