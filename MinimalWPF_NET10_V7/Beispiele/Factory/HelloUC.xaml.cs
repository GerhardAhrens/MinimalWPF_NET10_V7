namespace MinimalWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für HelloUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class HelloUC : UserControlBase
    {
        public HelloUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.WindowTitel = LocalizationValue.Get("WindowsTitelZeile");

            this.QuitCommand = new CommandBase(commandParam => this.OnQuit(commandParam), () => true);
            this.ShowResultCommand = new CommandBase(commandParam => this.OnShowResult(commandParam), () => true);
            this.ShowMessageCommand = new CommandBase(commandParam => this.OnShowMessage(commandParam), () => true);
            this.ShowDialogServiceCommand = new CommandBase(commandParam => this.OnShowDialogService(commandParam), () => true);
            this.ShowSourceGenCommand = new CommandBase(commandParam => this.OnShowSourceGen(commandParam), () => true);
            this.ShowLocalizationCommand = new CommandBase(commandParam => this.OnLocalization(commandParam), () => true);
            this.ShowEventAggregatorCommand = new CommandBase(commandParam => this.OnEventAggregator(commandParam), () => true);

            this.InformationCommand = new CommandBase(commandParam =>  this.OnPopup(commandParam));
            this.SettingsCommand = new CommandBase(commandParam => this.OnPopup(commandParam));
            this.CloseInformationPopupCommand = new CommandBase(commandParam => this.OnPopup(commandParam));
            this.CloseSettingsPopupCommand = new CommandBase(commandParam => this.OnPopup(commandParam));

            this.DataContext = this;
        }

        #region Properties
        public CommandBase QuitCommand { get; private set; }
        public CommandBase HelpCommand { get; private set; }
        public CommandBase ShowResultCommand { get; private set; }
        public CommandBase ShowMessageCommand { get; private set; }
        public CommandBase ShowDialogServiceCommand { get; private set; }
        public CommandBase ShowSourceGenCommand { get; private set; }
        public CommandBase ShowLocalizationCommand { get; private set; }
        public CommandBase ShowEventAggregatorCommand { get; private set; }

        public CommandBase InformationCommand { get; private set; }
        public CommandBase SettingsCommand { get; private set; }
        public CommandBase CloseInformationPopupCommand { get; private set; }
        public CommandBase CloseSettingsPopupCommand { get; private set; }

        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent("Bereit"));
            }
        }
        #endregion Windows Events

        #region Command Events
        private void OnPopup(object commandParam)
        {
            CommandButtons cb = (CommandButtons)commandParam;

            switch (cb)
            {
                case CommandButtons.InformationPopup:
                    if (this.InformationPopup.IsOpen == false)
                    {
                        this.InformationPopup.SetValue(MaskLayerBehavior.IsOpenProperty, true);
                    }
                    else
                    {
                        this.InformationPopup.SetValue(MaskLayerBehavior.IsOpenProperty, false);
                    }

                    break;
                case CommandButtons.SettingsPopup:
                    if (this.SettingsPopup.IsOpen == false)
                    {
                        this.SettingsPopup.SetValue(MaskLayerBehavior.IsOpenProperty, true);
                    }
                    else
                    {
                        this.SettingsPopup.SetValue(MaskLayerBehavior.IsOpenProperty, false);
                    }
                    break;
            }
        }

        private async void OnQuit(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.AppQuit)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnShowResult(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.ShowResult)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;
                    args.FromPage = CommandButtons.Home;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnShowMessage(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.ShowMessage)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;
                    args.FromPage = CommandButtons.Home;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnShowDialogService(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.ShowDialogService)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;
                    args.FromPage = CommandButtons.Home;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnShowSourceGen(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.ShowSourceGen)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;
                    args.FromPage = CommandButtons.Home;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnLocalization(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.Localization)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;
                    args.FromPage = CommandButtons.Home;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnEventAggregator(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.ShowEventAggregator)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;
                    args.FromPage = CommandButtons.Home;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }
        #endregion Command Events
    }
}
