//-----------------------------------------------------------------------
// <copyright file="NotificationBoxUC.cs" company="Lifeprojects.de">
//     Class: NotificationBoxUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.06.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Beispiel
{
    using System.Windows;
    using System.Windows.Controls;

    using MinimalWPF.Beispiele;

    /// <summary>
    /// Interaktionslogik für NotificationBoxUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class NotificationBoxUC : UserControlBase
    {
        public NotificationBoxUC(ChangeViewEventArgs args) : base(typeof(NotificationBoxUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.DefaultMessageBoxOKCommand = new CommandBase(commandParam => this.OnMessageBoxDefault(commandParam), () => true);
            this.CustomMessageBoxOKCommand = new CommandBase(commandParam => this.OnMessageBoxCustom(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase DefaultMessageBoxOKCommand { get; private set; }
        public CommandBase CustomMessageBoxOKCommand { get; private set; }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }

        private NotificationBase Notification { get; } = new NotificationBase();
        private MessageBase Message { get; } = new MessageBase();
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

        private void OnMessageBoxDefault(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("OK") == true)
            {
                MessageBoxResult result = this.Message.Hinweis("Information", "OK Button wurde geklickt!");
            }
            else if (commandParam != null && commandParam.Equals("YES_NO") == true)
            {
                MessageBoxResult result = this.Message.Question("Frage", "Soll 'Ja' oder 'Nein' ausgewählt werden?");
                if (result == MessageBoxResult.Yes)
                {
                    this.Message.Hinweis("Antwort", "Sie haben 'Ja' gewählt.");
                }
                else if (result == MessageBoxResult.No)
                {
                    this.Message.Warning("Antwort", "Sie haben 'Nein' gewählt.");
                }
            }
        }

        private void OnMessageBoxCustom(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("OK") == true)
            {
                MessageBoxResult result = this.Notification.Hinweis("Information", "OK Button wurde geklickt!");
            }
            else if (commandParam != null && commandParam.Equals("YES_NO") == true)
            {
                MessageBoxResult result = this.Notification.Question("Frage", "Soll 'Ja' oder 'Nein' ausgewählt werden?");
                if (result == MessageBoxResult.Yes)
                {
                    this.Notification.Hinweis("Antwort", "Sie haben 'Ja' gewählt.");
                }
                else if (result == MessageBoxResult.No)
                {
                    this.Notification.Hinweis("Antwort", "Sie haben 'Nein' gewählt.");
                }
            }
        }

        #endregion Command Events

    }
}
