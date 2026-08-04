namespace MinimalWPF
{
    using System.ComponentModel;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using MinimalWPF.Core;
    using MinimalWPF.View;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<WindowBase, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<WindowBase, CancelEventArgs>.AddHandler(this, "Closing", this.OnWindowClosing);

            this.StatusBarAccountCommand = new CommandBase(commandParam => this.OnStatusBarCommand(commandParam), () => true);

            this.SetVectorIcon("IconApplicationLogo", 64);
            this.WindowTitel = LocalizationValue.Get("WindowsTitelZeile");

            this.RegisterFactory();

            this.DataContext = this;
        }

        #region Properties
        public CommandBase StatusBarAccountCommand { get; private set; }

        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public System.Windows.Controls.UserControl WorkContent
        {
            get { return base.GetValue<System.Windows.Controls.UserControl>(); }
            set { base.SetValue(value); }
        }

        private MessageBase Message { get; } = new MessageBase();
        #endregion Properties


        #region Windows Events
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            App.EventAgg.Subscribe<ChangeViewEventArgs>(async (evt, ct) => this.ChangeControl(evt));
            App.EventAgg.Subscribe<StatusEvent>(async (evt, ct) => this.OnUpdateStatusBar(evt));
            App.EventAgg.Subscribe<WindowsTitelEvent>(async (evt, ct) => this.OnUpdateWindowTitel(evt));


            this.ConfigurationStatusInfoBar();

            ChangeViewEventArgs args = new();
            args.MenuButton = CommandButtons.Home;
            args.FromPage = CommandButtons.Home;
            this.ChangeControl(args);
        }

        private void OnUpdateStatusBar(StatusEvent evt)
        {
            StatusBar.SetNotification(evt.Notification);
        }

        private void OnUpdateWindowTitel(WindowsTitelEvent evt)
        {
            if (string.IsNullOrEmpty(evt.DialogTitel) == true)
            {
                this.WindowTitel = $"{LocalizationValue.Get("WindowsTitelZeile")} ({base.ApplicationVersion})";
                return;
            }
            else
            {
                this.WindowTitel = $"{LocalizationValue.Get("WindowsTitelZeile")} ({base.ApplicationVersion}) [{evt.DialogTitel}]";
            }
        }

        private void ConfigurationStatusInfoBar()
        {
            #region Test Visibility
            StatusBar.Rights.Show(false);
            StatusBar.Datasource.Show(false);
            #endregion Test Visibility

            #region Lange Text in Notification
            //StatusBar.SetNotification("Dies ist eine sehr lange Meldung welche den gesamten freien Platz innerhalb der StatusInfoBar ausfüllen sollte. Danach muss der Text automatisch mit Ellipsis abgeschnitten werden.");
            #endregion Lange Text in Notification

            #region Test Text
            //StatusBar.Rights.Text = "Benutzerrechte";
            //StatusBar.Date.Text = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            //StatusBar.Datasource.Text = "Datenquelle";
            #endregion Test Text

            #region Test Farben
            //StatusBar.Account.SetColors(Brushes.Green, Brushes.AliceBlue);
            #endregion Test Farben

            #region Command
            StatusBar.Account.Command = this.StatusBarAccountCommand;
            #endregion Command

            #region Auto Timer
            StatusBar.AutoUpdateDateTime = true;
            #endregion Auto Timer
        }

        private void OnCloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnQuit()
        {
            this.Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;


            MessageBoxResult msgYN;
            if (this.Tag != null)
            {
                msgYN = this.Message.AppExitMessage(this.Tag.ToString());
            }
            else
            {
                msgYN = this.Message.AppExitMessage();
            }

            if (msgYN == MessageBoxResult.Yes)
            {
                App.ApplicationExit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        #endregion Windows Events

        #region Command Events
        private void OnStatusBarCommand(object commandParam)
        {
            if (commandParam is StatusInfoBarItem item && item.ItemType == StatusItemType.Account)
            {
                string accountText = item.Text;
                this.Message.Hinweis("StatusBar", $"Klick auf StatusBar {item.ItemType} => {accountText}");
            }
        }
        #endregion Command Events

        #region Event Aggregator Handler

        private async void ChangeControl(ChangeViewEventArgs commandParam)
        {
            try
            {
                this.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);

                if (commandParam != null && commandParam.MenuButton is CommandButtons button)
                {
                    if (button == CommandButtons.AppQuit)
                    {
                        this.OnQuit();
                    }
                    else if (button.In(CommandButtons.Home, CommandButtons.Artikelliste))
                    {
                        if (App.EventAgg.IsSubscription<WindowsTitelEvent>() == true)
                        {
                            await App.EventAgg.PublishAsync(new WindowsTitelEvent(button.ToDescription()));
                        }

                        this.WorkContent = null;
                        this.WorkContent = (UserControl)Factory.Get<UserControlBase, CommandButtons>((CommandButtons)commandParam.MenuButton, commandParam);
                    }
                    else if (button.In(CommandButtons.Home, CommandButtons.GoBack))
                    {

                        if (App.EventAgg.IsSubscription<WindowsTitelEvent>() == true)
                        {
                            await App.EventAgg.PublishAsync(new WindowsTitelEvent(button.ToDescription()));
                        }

                        this.WorkContent = null;
                        this.WorkContent = (UserControl)Factory.Get<UserControlBase, CommandButtons>((CommandButtons)commandParam.MenuButton, commandParam);
                    }
                }

                this.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                App.ErrorMessage(ex, $"Fehler in {this.GetType().Name}");
            }
        }

        #endregion Event Aggregator Handler

        /// <summary>
        /// Dialog aus UserControls werden hier für die Factory registriert 😊
        /// </summary>
        private void RegisterFactory()
        {
            Factory.RegisterSingleton<CommandButtons>(CommandButtons.Home, () => new HomeUC());
            Factory.RegisterTransient<CommandButtons>(CommandButtons.Artikelliste, (param) => new ArtikellisteUC((ChangeViewEventArgs)param!));
        }
    }
}