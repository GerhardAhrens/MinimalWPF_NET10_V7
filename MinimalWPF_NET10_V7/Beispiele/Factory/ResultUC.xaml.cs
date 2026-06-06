namespace MinimalWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Linq;

    /// <summary>
    /// Interaktionslogik für ResultUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class ResultUC : UserControlBase
    {
        public ResultUC(ChangeViewEventArgs args) : base(typeof(ResultUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.CheckInputCommand = new CommandBase(commandParam => this.OnCheckInput(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase CheckInputCommand { get; private set; }

        public string InputValue
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private MessageBase Message { get; } = new MessageBase();
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }

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
        private async void OnGoBack(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.GoBack)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = this.CurrentCtorArgs.FromPage;
                    args.FromPage = this.CurrentCtorArgs.MenuButton;
                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private void OnCheckInput(object commandParam)
        {
            Result<long> numberMixLong = Parsing.ParseLong(this.InputValue);
            if (numberMixLong.IsSuccess == true)
            {
                this.Message.Hinweis("Erfolgreich",$"Erfolgreich geparst: {numberMixLong.Value}");
            }
            else
            {
                this.Message.Hinweis("Fehler",$"Fehler beim Parsen: {numberMixLong.FailMessage}");
            }
        }

        #endregion Command Events

    }
}
