namespace System.Windows.Controls
{
    using System.Windows.Input;
    using System.Windows.Media;

    [TemplatePart(Name = "PART_ContentHost", Type = typeof(ScrollViewer))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    public class WindowTextBox : TextBox
    {
        private Button _clearButton;

        static WindowTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(WindowTextBox),
                new FrameworkPropertyMetadata(typeof(WindowTextBox)));
        }

        #region PlaceholderText

        internal static readonly DependencyProperty PlaceholderVisibilityProperty =
            DependencyProperty.Register(nameof(PlaceholderVisibility), typeof(Visibility), typeof(WindowTextBox), new FrameworkPropertyMetadata(Visibility.Visible));

        internal Visibility PlaceholderVisibility
        {
            get => (Visibility)GetValue(PlaceholderVisibilityProperty);
            set => SetValue(PlaceholderVisibilityProperty, value);
        }

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(WindowTextBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.AffectsRender,
                    OnPlaceholderTextChanged));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WindowTextBox textBox)
            {
                textBox.UpdatePlaceholder();
            }
        }

        private void UpdatePlaceholder()
        {
            PlaceholderVisibility = string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(PlaceholderText) ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region PlaceholderForeground

        public static readonly DependencyProperty PlaceholderForegroundProperty =
            DependencyProperty.Register(
                nameof(PlaceholderForeground),
                typeof(Brush),
                typeof(WindowTextBox),
                new FrameworkPropertyMetadata(Brushes.LightGray, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush PlaceholderForeground
        {
            get => (Brush)GetValue(PlaceholderForegroundProperty);
            set => SetValue(PlaceholderForegroundProperty, value);
        }

        #endregion

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(WindowTextBox),
                new FrameworkPropertyMetadata(
                    new CornerRadius(5)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region ShowClearButton

        public static readonly DependencyProperty ShowClearButtonProperty =
            DependencyProperty.Register(
                nameof(ShowClearButton),
                typeof(bool),
                typeof(WindowTextBox),
                new FrameworkPropertyMetadata(true));

        public bool ShowClearButton
        {
            get => (bool)GetValue(ShowClearButtonProperty);
            set => SetValue(ShowClearButtonProperty, value);
        }

        #endregion

        #region ClearButtonVisibility

        public static readonly DependencyProperty ClearButtonVisibilityProperty =
            DependencyProperty.Register(
                nameof(ClearButtonVisibility),
                typeof(Visibility),
                typeof(WindowTextBox),
                new FrameworkPropertyMetadata(Visibility.Visible));

        public Visibility ClearButtonVisibility
        {
            get => (Visibility)GetValue(ClearButtonVisibilityProperty);
            set => SetValue(ClearButtonVisibilityProperty, value);
        }

        #endregion

        #region SelectAllOnFocus

        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.Register(
                nameof(SelectAllOnFocus),
                typeof(bool),
                typeof(WindowTextBox),
                new FrameworkPropertyMetadata(false));

        public bool SelectAllOnFocus
        {
            get => (bool)GetValue(SelectAllOnFocusProperty);
            set => SetValue(SelectAllOnFocusProperty, value);
        }

        #endregion

        #region ClearButtonClicked

        public static readonly RoutedEvent ClearButtonClickedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(ClearButtonClicked),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(WindowTextBox));

        public event RoutedEventHandler ClearButtonClicked
        {
            add => AddHandler(ClearButtonClickedEvent, value);
            remove => RemoveHandler(ClearButtonClickedEvent, value);
        }

        #endregion

        #region Constructor

        public WindowTextBox()
        {
            Loaded += Window11TextBox_Loaded;
        }

        private void Window11TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateClearButton();
        }

        #endregion

        #region Template

        public override void OnApplyTemplate()
        {
            if (_clearButton != null)
            {
                _clearButton.Click -= ClearButton_Click;
            }

            base.OnApplyTemplate();

            _clearButton =
                GetTemplateChild("PART_ClearButton") as Button;

            if (_clearButton != null)
            {
                _clearButton.Click += ClearButton_Click;
            }

            UpdateClearButton();
            UpdatePlaceholder();
        }

        #endregion

        #region TextChanged

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            UpdateClearButton();
            UpdatePlaceholder();
        }

        #endregion

        #region Clear

        new public void Clear()
        {
            if (IsReadOnly)
                return;

            ClearButtonClickedEventHandler();

            Text = string.Empty;

            CaretIndex = 0;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Clear();

            e.Handled = true;
        }

        private void ClearButtonClickedEventHandler()
        {
            RaiseEvent(new RoutedEventArgs(ClearButtonClickedEvent, this));
        }

        #endregion

        #region Clear Button

        private void UpdateClearButton()
        {
            if (_clearButton == null)
                return;

            bool show =
                ShowClearButton &&
                ClearButtonVisibility == Visibility.Visible &&
                !IsReadOnly &&
                IsEnabled &&
                !string.IsNullOrEmpty(Text);

            _clearButton.Visibility =
                show
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == IsEnabledProperty ||
                e.Property == IsReadOnlyProperty ||
                e.Property == ShowClearButtonProperty ||
                e.Property == ClearButtonVisibilityProperty)
            {
                UpdateClearButton();
            }
        }
        #endregion

        #region Focus

        protected override void OnGotKeyboardFocus(
            KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);

            if (SelectAllOnFocus)
            {
                Dispatcher.BeginInvoke(
                    new Action(SelectAll),
                    System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        #endregion

        #region Keyboard

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape &&
                ShowClearButton &&
                !string.IsNullOrEmpty(Text) &&
                !IsReadOnly)
            {
                Clear();

                e.Handled = true;
                return;
            }

            base.OnKeyDown(e);
        }

        #endregion
    }
}
