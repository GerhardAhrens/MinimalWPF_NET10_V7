namespace System.Windows.Controls
{
    using System;
    using System.Collections;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// A Windows 11 styled horizontal or vertical separator line
    /// with arbitrary content displayed above/before the line.
    ///
    /// Unlike WindowLine, the header does not interrupt the line.
    /// The header is rendered as WPF content and can therefore contain
    /// text, icons, images or arbitrary controls.
    /// </summary>
    public class WindowLineHeader : ContentControl
    {
        #region Dependency Properties

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation),
                typeof(Orientation),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    Orientation.Horizontal,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty LineColorProperty =
            DependencyProperty.Register(
                nameof(LineColor),
                typeof(Brush),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register(
                nameof(Thickness),
                typeof(double),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    1.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender),
                ValidateThickness);

        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register(
                nameof(LineStyle),
                typeof(WindowLineStyle),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    WindowLineStyle.Solid,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeaderAlignmentProperty =
            DependencyProperty.Register(
                nameof(HeaderAlignment),
                typeof(WindowLineHeaderAlignment),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    WindowLineHeaderAlignment.Center,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeaderMarginProperty =
            DependencyProperty.Register(
                nameof(HeaderMargin),
                typeof(double),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    8.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange |
                    FrameworkPropertyMetadataOptions.AffectsRender),
                ValidateNonNegative);

        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(
                nameof(HeaderForeground),
                typeof(Brush),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty HeaderFontSizeProperty =
            DependencyProperty.Register(
                nameof(HeaderFontSize),
                typeof(double),
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(
                    13.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsRender),
                ValidateFontSize);

        public static readonly DependencyProperty HeaderFontWeightProperty =
            DependencyProperty.Register(
                nameof(HeaderFontWeight),
                typeof(FontWeight),
                typeof(WindowLineHeader),
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

        /// <summary>
        /// Foreground brush used for the header content.
        /// This is independent from LineColor.
        /// </summary>
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

        static WindowLineHeader()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(WindowLineHeader),
                new FrameworkPropertyMetadata(typeof(WindowLineHeader)));
        }

        public WindowLineHeader()
        {
            SnapsToDevicePixels = true;
            UseLayoutRounding = true;

            // Windows 11-like default colors.
            LineColor = new SolidColorBrush(
                Color.FromArgb(80, 128, 128, 128));

            HeaderForeground = new SolidColorBrush(
                Color.FromRgb(60, 60, 60));

            Background = Brushes.Transparent;

            HorizontalContentAlignment = HorizontalAlignment.Left;
            VerticalContentAlignment = VerticalAlignment.Center;
        }

        #endregion

        #region Measure / Arrange / Render

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Content == null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    return new Size(
                        0,
                        Math.Max(Thickness, 1));
                }

                return new Size(
                    Math.Max(Thickness, 1),
                    0);
            }

            EnsureContentPresenter();

            if (Orientation == Orientation.Horizontal)
            {
                double contentAvailableWidth = availableSize.Width;

                if (double.IsInfinity(contentAvailableWidth))
                    contentAvailableWidth = double.PositiveInfinity;

                _contentPresenter.Measure(
                    new Size(
                        contentAvailableWidth,
                        double.IsInfinity(availableSize.Height)
                            ? double.PositiveInfinity
                            : Math.Max(0, availableSize.Height)));

                double width = _contentPresenter.DesiredSize.Width;
                double height =
                    _contentPresenter.DesiredSize.Height +
                    HeaderMargin +
                    Math.Max(Thickness, 1);

                return new Size(width, height);
            }

            // Vertical orientation:
            // Header is placed before the line in the vertical direction.
            _contentPresenter.Measure(
                new Size(
                    double.IsInfinity(availableSize.Width)
                        ? double.PositiveInfinity
                        : Math.Max(0, availableSize.Width),
                    double.IsInfinity(availableSize.Height)
                        ? double.PositiveInfinity
                        : Math.Max(0, availableSize.Height)));

            double verticalWidth =
                _contentPresenter.DesiredSize.Width +
                HeaderMargin +
                Math.Max(Thickness, 1);

            return new Size(
                verticalWidth,
                _contentPresenter.DesiredSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (_contentPresenter == null)
                return finalSize;

            if (Orientation == Orientation.Horizontal)
            {
                double contentWidth = _contentPresenter.DesiredSize.Width;
                double contentHeight = _contentPresenter.DesiredSize.Height;

                double x = GetHorizontalHeaderX(
                    finalSize.Width,
                    contentWidth);

                _contentPresenter.Arrange(
                    new Rect(
                        x,
                        0,
                        contentWidth,
                        contentHeight));
            }
            else
            {
                double contentWidth = _contentPresenter.DesiredSize.Width;
                double contentHeight = _contentPresenter.DesiredSize.Height;

                double y = GetVerticalHeaderY(
                    finalSize.Height,
                    contentHeight);

                _contentPresenter.Arrange(
                    new Rect(
                        0,
                        y,
                        contentWidth,
                        contentHeight));
            }

            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (ActualWidth <= 0 || ActualHeight <= 0)
                return;

            var lineBrush =
                LineColor ??
                SystemColors.ControlDarkBrush;

            if (Orientation == Orientation.Horizontal)
            {
                DrawHorizontalLine(
                    drawingContext,
                    lineBrush);
            }
            else
            {
                DrawVerticalLine(
                    drawingContext,
                    lineBrush);
            }
        }

        #endregion

        #region Horizontal

        private void DrawHorizontalLine(
            DrawingContext dc,
            Brush brush)
        {
            double lineY =
                Math.Max(
                    _contentPresenter?.DesiredSize.Height ?? 0,
                    0) +
                HeaderMargin +
                Math.Max(Thickness / 2.0, 0);

            // Keep the line inside the actual control.
            if (lineY > ActualHeight)
                lineY = ActualHeight - Thickness / 2.0;

            DrawLine(
                dc,
                brush,
                0,
                lineY,
                ActualWidth,
                lineY);
        }

        private double GetHorizontalHeaderX(
            double availableWidth,
            double contentWidth)
        {
            switch (HeaderAlignment)
            {
                case WindowLineHeaderAlignment.Left:
                    return 0;

                case WindowLineHeaderAlignment.Right:
                    return Math.Max(
                        0,
                        availableWidth - contentWidth);

                case WindowLineHeaderAlignment.Center:
                default:
                    return Math.Max(
                        0,
                        (availableWidth - contentWidth) / 2.0);
            }
        }

        #endregion

        #region Vertical

        private void DrawVerticalLine(
            DrawingContext dc,
            Brush brush)
        {
            double lineX =
                Math.Max(
                    _contentPresenter?.DesiredSize.Width ?? 0,
                    0) +
                HeaderMargin +
                Math.Max(Thickness / 2.0, 0);

            if (lineX > ActualWidth)
                lineX = ActualWidth - Thickness / 2.0;

            DrawLine(
                dc,
                brush,
                lineX,
                0,
                lineX,
                ActualHeight);
        }

        private double GetVerticalHeaderY(
            double availableHeight,
            double contentHeight)
        {
            switch (HeaderAlignment)
            {
                case WindowLineHeaderAlignment.Left:
                    return 0;

                case WindowLineHeaderAlignment.Right:
                    return Math.Max(
                        0,
                        availableHeight - contentHeight);

                case WindowLineHeaderAlignment.Center:
                default:
                    return Math.Max(
                        0,
                        (availableHeight - contentHeight) / 2.0);
            }
        }

        #endregion

        #region Drawing

        private void DrawLine(
            DrawingContext dc,
            Brush brush,
            double x1,
            double y1,
            double x2,
            double y2)
        {
            if (x2 <= x1 &&
                Math.Abs(y2 - y1) < 0.01)
            {
                return;
            }

            if (y2 <= y1 &&
                Math.Abs(x2 - x1) < 0.01)
            {
                return;
            }

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

            dc.DrawLine(
                pen,
                new Point(x1, y1),
                new Point(x2, y2));
        }

        #endregion

        #region Content

        private ContentPresenter _contentPresenter;

        private void EnsureContentPresenter()
        {
            if (_contentPresenter != null)
                return;

            _contentPresenter = new ContentPresenter
            {
                ContentSource = "Content",
                RecognizesAccessKey = true,
                SnapsToDevicePixels = true,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            AddVisualChild(_contentPresenter);
            AddLogicalChild(_contentPresenter);
        }

        protected override int VisualChildrenCount =>
            _contentPresenter == null ? 0 : 1;

        protected override Visual GetVisualChild(int index)
        {
            if (index != 0 || _contentPresenter == null)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _contentPresenter;
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (_contentPresenter != null)
                    yield return _contentPresenter;
            }
        }

        protected override void OnContentChanged(
            object oldContent,
            object newContent)
        {
            base.OnContentChanged(
                oldContent,
                newContent);

            EnsureContentPresenter();

            _contentPresenter.Content = newContent;

            InvalidateMeasure();
            InvalidateArrange();
            InvalidateVisual();
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