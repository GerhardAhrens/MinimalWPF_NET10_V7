//-----------------------------------------------------------------------
// <copyright file="MessageUC.cs" company="Lifeprojects.de">
//     Class: MessageUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.06.2026</date>
//
// <summary>
// Template für eine neue UserControl, die über die Factory erstellt wird. Sie enthält einen GoBackCommand,
// der es ermöglicht, zur vorherigen Ansicht zurückzukehren. Beim Laden der UserControl werden
// Status- und WindowsTitel-Events veröffentlicht, um den aktuellen Status und Titel der Anwendung zu aktualisieren.
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für MessageUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class MessageUC : UserControlBase
    {
        public MessageUC(ChangeViewEventArgs args) : base(typeof(MessageUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.MessageBoxOKCommand = new CommandBase(commandParam => this.OnMessageBox(commandParam), () => true);

            this.DataContext = this;
        }

        private void OnMessageBox(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("OK") == true)
            {
                MessageBoxResult result = this.Message.ShowMessage("Information","OK Button wurde geklickt!");
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
            else if (commandParam != null && commandParam.Equals("YES_NO_CANCEL") == true)
            {
                MessageBoxResult result = this.Message.QuestionCancel("Frage", "Soll 'Ja', 'Nein' oder 'Abbrechen' ausgewählt werden?");
                if (result == MessageBoxResult.Yes)
                {
                    this.Message.Hinweis("Antwort", "Sie haben 'Ja' gewählt.");
                }
                else if (result == MessageBoxResult.No)
                {
                    this.Message.Warning("Antwort", "Sie haben 'Nein' gewählt.");
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    this.Message.Warning("Antwort", "Sie haben 'Abbrechen' gewählt.");
                }
            }
            else if (commandParam != null && commandParam.Equals("YES_NO_CANCEL") == true)
            {
                MessageBoxResult result = this.Message.QuestionCancel("Frage", "Soll 'Ja', 'Nein' oder 'Abbrechen' ausgewählt werden?");
                if (result == MessageBoxResult.Yes)
                {
                    this.Message.Hinweis("Antwort", "Sie haben 'Ja' gewählt.");
                }
                else if (result == MessageBoxResult.No)
                {
                    this.Message.Warning("Antwort", "Sie haben 'Nein' gewählt.");
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    this.Message.Warning("Antwort", "Sie haben 'Abbrechen' gewählt.");
                }
            }
            else if (commandParam != null && commandParam.Equals("ABORT_RETRY_IGNORE") == true)
            {
                MessageBoxResult result = this.Message.QuestionAbortRetryIgnore("Frage", "Soll 'Abbrechen', 'Retry' oder 'Ignore' ausgewählt werden?");
                if (result == MessageBoxResult.Abort)
                {
                    this.Message.Warning("Antwort", "Sie haben 'Abbrechen' gewählt.");
                }
                else if (result == MessageBoxResult.Retry)
                {
                    this.Message.Hinweis("Antwort", "Sie haben 'Retry' gewählt.");
                }
                else if (result == MessageBoxResult.Ignore)
                {
                    this.Message.Warning("Antwort", "Sie haben 'Ignore' gewählt.");
                }
            }
        }


        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase MessageBoxOKCommand { get; private set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
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
        #endregion Command Events

    }
}
