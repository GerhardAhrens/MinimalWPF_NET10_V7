namespace System.Windows.Controls
{
    using System.Windows.Media;

    public class SymbolImage : Image
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(DrawingImage), typeof(SymbolImage),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, OnIconChanged));

        public DrawingImage Icon
        {
            get => (DrawingImage)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        private static readonly DrawingImage DefaultIcon = CreateDefaultIcon();

        public SymbolImage()
        {
            Width = 25;
            Height = 25;

            Stretch = Stretch.Uniform;

            this.UpdateSource();
        }

        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SymbolImage control)
            {
                control.UpdateSource();
            }
        }

        private void UpdateSource()
        {
            Source = Icon ?? DefaultIcon;
        }

        private static DrawingImage CreateDefaultIcon()
        {
            var group = new DrawingGroup();

            var pen = new Pen(Brushes.Red, 3);
            pen.Freeze();

            group.Children.Add(new GeometryDrawing(null, pen, Geometry.Parse("M 4,4 L 21,21")));
            group.Children.Add(new GeometryDrawing(null, pen, Geometry.Parse("M 21,4 L 4,21")));

            group.Freeze();

            var image = new DrawingImage(group);
            image.Freeze();

            return image;
        }
    }
}
