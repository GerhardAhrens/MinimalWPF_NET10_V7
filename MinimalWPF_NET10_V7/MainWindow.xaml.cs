//-----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Lifeprojects.de">
//     Class: MainWindow
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.03.2026 18:21:36</date>
//
// <summary>
// WPF Template mit Minimalfunktionen
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using MinimalWPF.Beispiel;
    using MinimalWPF.Beispiele;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<WindowBase, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<WindowBase, CancelEventArgs>.AddHandler(this, "Closing", this.OnWindowClosing);

            this.SetVectorIcon("IconApplicationLogo", 64);
            this.WindowTitel = LocalizationValue.Get("WindowsTitelZeile");

            this.RegisterFactory();

            this.ApplikationVersion = base.ApplicationVersion.ToString();
            this.LaufzeitVersion = base.RuntimeVersion;
            this.WinVersion = base.WindowsVersion;
            this.DataContext = this;
        }

        #region Properties


        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string ApplikationVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string LaufzeitVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string WinVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public System.Windows.Controls.UserControl WorkContent
        {
            get { return base.GetValue<System.Windows.Controls.UserControl>(); }
            set { base.SetValue(value); }
        }

        private NotificationBase Notification { get;  } = new NotificationBase();
        private MessageBase Message { get; } = new MessageBase();

        #endregion Properties

        #region Windows Events

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            App.EventAgg.Subscribe<ChangeViewEventArgs>(async (evt, ct) => this.ChangeControl(evt));
            App.EventAgg.Subscribe<WindowsTitelEvent>(async (evt, ct) => this.OnUpdateWindowTitel(evt));
            App.EventAgg.Subscribe<StatusEvent>(async (evt, ct) => this.OnUpdateStatusBar(evt));

            StatusbarMain.Statusbar.DatabaseInfo = "Keine";
            StatusbarMain.Statusbar.DatabaseInfoTooltip = "Keine Datenbank verbunden";
            StatusbarMain.Statusbar.Notification = "Bereit";

            ChangeViewEventArgs args = new();
            args.MenuButton = CommandButtons.Home;
            args.FromPage = CommandButtons.Home;
            this.ChangeControl(args);

        }
        #endregion Windows Events

        #region Command Event Handler
        private void OnCloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnQuit()
        {
            /* this.QuitParamCommand.TryExecute(); */
            //this.Tag = param;
            this.Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

            /*
            if (App.Settings.FrageExit == false)
            {
                App.ApplicationExit();
                return;
            }
            */

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
        #endregion Command Event Handler

        #region Event Aggregator Handler
        private void OnUpdateStatusBar(StatusEvent evt)
        {
            StatusbarMain.Statusbar.Notification = evt.Notification;

            if (string.IsNullOrEmpty(evt.DatabaseInfo) == false)
            {
                StatusbarMain.Statusbar.DatabaseInfo = evt.DatabaseInfo;
                StatusbarMain.Statusbar.DatabaseInfoTooltip = evt.DatabaseInfoTooltip;
            }
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
                    else if (button.In(CommandButtons.Home, CommandButtons.GoBack, CommandButtons.ShowResult, CommandButtons.ShowMessage, 
                        CommandButtons.ShowDialogService, CommandButtons.ShowSourceGen, CommandButtons.Localization, 
                        CommandButtons.ShowEventAggregator, CommandButtons.ShowFactoryPattern, CommandButtons.ShowNotificationBox))
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
            Factory.RegisterSingleton<CommandButtons>(CommandButtons.Home, () => new HelloUC());
            Factory.RegisterTransient<CommandButtons>(CommandButtons.ShowResult, (param) => new ResultUC((ChangeViewEventArgs)param!));
            Factory.RegisterTransient<CommandButtons>(CommandButtons.ShowMessage, (param) => new MessageUC((ChangeViewEventArgs)param!));
            Factory.RegisterTransient<CommandButtons>(CommandButtons.ShowDialogService, (param) => new DialogServiceUC((ChangeViewEventArgs)param!));
            Factory.RegisterTransient<CommandButtons>(CommandButtons.ShowSourceGen, (param) => new SourceGenUC((ChangeViewEventArgs)param!));
            Factory.RegisterTransient<CommandButtons>(CommandButtons.Localization, (param) => new LocalizationUC((ChangeViewEventArgs)param!));
            Factory.RegisterTransient<CommandButtons>(CommandButtons.ShowEventAggregator, (param) => new EventAggregatorUC((ChangeViewEventArgs)param!));
            Factory.RegisterTransient<CommandButtons>(CommandButtons.ShowFactoryPattern, (param) => new FactoryUC((ChangeViewEventArgs)param!));
            Factory.RegisterTransient<CommandButtons>(CommandButtons.ShowNotificationBox, (param) => new NotificationBoxUC((ChangeViewEventArgs)param!));
        }
    }
}