namespace System.Windows.Controls
{
    using System.Windows.Media;
    using System.Windows.Data;

    public class MenuButton : Button
    {
        private readonly StackPanel _panel = new();
        private readonly Image _image = new();
        private readonly TextBlock _text = new();
        private Brush _oldBackground;

        public MenuButton()
        {
            this.Background = Brushes.Transparent;
            this.Padding = new Thickness(0,0,0,0);

            this._panel.HorizontalAlignment = HorizontalAlignment.Center;
            this._panel.VerticalAlignment = VerticalAlignment.Center;

            this._image.Stretch = Stretch.Uniform;
            this._image.HorizontalAlignment = HorizontalAlignment.Center;
            this._image.VerticalAlignment = VerticalAlignment.Center;

            this._text.HorizontalAlignment = HorizontalAlignment.Center;
            this._text.VerticalAlignment = VerticalAlignment.Center;
            this._text.TextAlignment = TextAlignment.Center;

            this.Content = this._panel;

            this._text.SetBinding(TextBlock.ForegroundProperty, new Binding(nameof(Foreground)) { Source = this });
            this._text.SetBinding(TextBlock.FontFamilyProperty, new Binding(nameof(FontFamily)) { Source = this });
            this._text.SetBinding(TextBlock.FontSizeProperty, new Binding(nameof(FontSize)) { Source = this });
            this._text.SetBinding(TextBlock.FontWeightProperty, new Binding(nameof(FontWeight)) { Source = this });
            this._text.SetBinding(TextBlock.FontStyleProperty, new Binding(nameof(FontStyle)) { Source = this });
            this._text.SetBinding(TextBlock.FontStretchProperty, new Binding(nameof(FontStretch)) { Source = this });

            this.MouseEnter += this.OnMouseEnter;
            this.MouseLeave += this.OnMouseLeave;

            this.UpdateVisual();
        }

        ~MenuButton()
        {
            this.MouseEnter -= this.OnMouseEnter;
            this.MouseLeave -= this.OnMouseLeave;
        }

        #region DependencyProperties
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(MenuButton),  new PropertyMetadata(new CornerRadius(3)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.Register(nameof(Spacing), typeof(double), typeof(MenuButton), new PropertyMetadata(3.0, OnVisualChanged));

        public double Spacing
        {
            get => (double)GetValue(SpacingProperty);
            set => SetValue(SpacingProperty, value);
        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(MenuButton), new PropertyMetadata(35.0, OnVisualChanged));

        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(MenuButton), new PropertyMetadata(35.0, OnVisualChanged));

        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(nameof(Image), typeof(ImageSource), typeof(MenuButton), new PropertyMetadata(null, OnVisualChanged));

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(MenuButton), new PropertyMetadata(string.Empty, OnVisualChanged));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }


        public static readonly DependencyProperty ToolTipTextProperty =
            DependencyProperty.Register(nameof(ToolTipText), typeof(string), typeof(MenuButton), new PropertyMetadata(string.Empty, OnVisualChanged));

        public string ToolTipText
        {
            get => (string)GetValue(ToolTipTextProperty);
            set => SetValue(ToolTipTextProperty, value);
        }


        public static readonly DependencyProperty TextPlacementProperty =
            DependencyProperty.Register(
                nameof(ButtonTextPlacement),
                typeof(ButtonTextPlacement),
                typeof(MenuButton),
                new PropertyMetadata(ButtonTextPlacement.Bottom, OnVisualChanged));

        public ButtonTextPlacement ButtonTextPlacement
        {
            get => (ButtonTextPlacement)GetValue(TextPlacementProperty);
            set => SetValue(TextPlacementProperty, value);
        }


        public static readonly DependencyProperty MouseOverBackgroundProperty =
            DependencyProperty.Register(
                nameof(MouseOverBackground),
                typeof(Brush),
                typeof(MenuButton),
                new PropertyMetadata(Brushes.LightGray));

        public Brush MouseOverBackground
        {
            get => (Brush)GetValue(MouseOverBackgroundProperty);
            set => SetValue(MouseOverBackgroundProperty, value);
        }

        #endregion

        private static void OnVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MenuButton)d).UpdateVisual();
        }

        private void UpdateVisual()
        {
            this._panel.Children.Clear();

            this._image.Source = this.Image;
            this._image.Width = this.ImageWidth;
            this._image.Height = this.ImageHeight;

            this._text.Text = this.Text;
            this._text.Foreground = this.Foreground;

            this.ToolTip = this.ToolTipText;

            switch (ButtonTextPlacement)
            {
                case ButtonTextPlacement.Top:

                    this._panel.Orientation = Orientation.Vertical;

                    this._text.Margin = new Thickness(0, 0, 0, this.Spacing);
                    this._image.Margin = new Thickness(0,0,0,0);

                    if (!string.IsNullOrWhiteSpace(Text))
                        this._panel.Children.Add(this._text);

                    if (Image != null)
                        this._panel.Children.Add(this._image);

                    break;

                case ButtonTextPlacement.Bottom:

                    this._panel.Orientation = Orientation.Vertical;

                    this._image.Margin = new Thickness(0, 0, 0, 0);
                    this._text.Margin = new Thickness(0, this.Spacing, 0, 0);

                    if (Image != null)
                        this._panel.Children.Add(this._image);

                    if (!string.IsNullOrWhiteSpace(Text))
                        this._panel.Children.Add(this._text);

                    break;

                case ButtonTextPlacement.Left:

                    this._panel.Orientation = Orientation.Horizontal;

                    this._text.Margin = new Thickness(0, 0, this.Spacing, 0);
                    this._image.Margin = new Thickness(0, 0, 0, 0);

                    if (!string.IsNullOrWhiteSpace(Text))
                        this._panel.Children.Add(this._text);

                    if (Image != null)
                        this._panel.Children.Add(this._image);

                    break;

                case ButtonTextPlacement.Right:

                    this._panel.Orientation = Orientation.Horizontal;

                    this._image.Margin = new Thickness(0, 0, 0, 0);
                    this._text.Margin = new Thickness(this.Spacing, 0, 0, 0);

                    if (Image != null)
                        this._panel.Children.Add(this._image);

                    if (!string.IsNullOrWhiteSpace(Text))
                        this._panel.Children.Add(this._text);

                    break;
            }
        }

        private void OnMouseLeave(object sender, Input.MouseEventArgs e)
        {
            Background = _oldBackground;
        }

        private void OnMouseEnter(object sender, Input.MouseEventArgs e)
        {
            _oldBackground = Background;
            Background = MouseOverBackground;
        }
    }
}
