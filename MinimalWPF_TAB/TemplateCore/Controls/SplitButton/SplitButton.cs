namespace System.Windows.Controls
{
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    [TemplatePart(Name = PART_MainButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_DropDownButton, Type = typeof(ToggleButton))]
    [TemplatePart(Name = PART_Popup, Type = typeof(Popup))]
    public class SplitButton : HeaderedItemsControl
    {
        private const string PART_MainButton = "PART_MainButton";
        private const string PART_DropDownButton = "PART_DropDownButton";
        private const string PART_Popup = "PART_Popup";
        private Button _mainButton;

        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SplitButton),
                new FrameworkPropertyMetadata(
                    typeof(SplitButton)));

            ItemsControl.ItemsPanelProperty.OverrideMetadata(
                typeof(SplitButton),
                new FrameworkPropertyMetadata(
                    new ItemsPanelTemplate(
                        new FrameworkElementFactory(
                            typeof(SplitButtonMenuPanel)))));
        }

        #region Command

        public static readonly DependencyProperty CommandProperty =
            ButtonBase.CommandProperty.AddOwner(
                typeof(SplitButton),
                new FrameworkPropertyMetadata(null));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        #endregion

        #region CommandParameter

        public static readonly DependencyProperty CommandParameterProperty =
            ButtonBase.CommandParameterProperty.AddOwner(
                typeof(SplitButton),
                new FrameworkPropertyMetadata(null));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion

        #region CommandTarget

        public static readonly DependencyProperty CommandTargetProperty =
            ButtonBase.CommandTargetProperty.AddOwner(
                typeof(SplitButton),
                new FrameworkPropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get => (IInputElement)GetValue(CommandTargetProperty);
            set => SetValue(CommandTargetProperty, value);
        }

        #endregion

        #region IsDropDownOpen

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register(
                nameof(IsDropDownOpen),
                typeof(bool),
                typeof(SplitButton),
                new FrameworkPropertyMetadata(false));

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }

        #endregion

        #region Click

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent(
                nameof(Click),
                RoutingStrategy.Bubble,
                typeof(RoutedEventHandler),
                typeof(SplitButton));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }

        protected virtual void OnClick()
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent, this));
        }

        #endregion

        public override void OnApplyTemplate()
        {
            if (this._mainButton != null)
            {
                this._mainButton.Click -= MainButton_Click;
            }

            RemoveHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.AnyButton_Click));

            base.OnApplyTemplate();

            this._mainButton = this.GetTemplateChild(PART_MainButton) as Button;

            if (this._mainButton != null)
            {
                this._mainButton.Click += this.MainButton_Click;
            }

            AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.AnyButton_Click));
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            // Nur das Click-Event des SplitButton auslösen.
            // Der Command wird vom Button selbst ausgeführt.
            this.OnClick();
        }

        private void AnyButton_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is SplitButtonItem)
            {
                this.CloseDropDown();
            }
        }

        internal void CloseDropDown()
        {
            this.IsDropDownOpen = false;
        }
    }
}
