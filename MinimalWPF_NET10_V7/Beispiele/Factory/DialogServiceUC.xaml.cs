//-----------------------------------------------------------------------
// <copyright file="DialogServiceUC.cs" company="Lifeprojects.de">
//     Class: DialogServiceUC
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
    /// Interaktionslogik für DialogServiceUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class DialogServiceUC : UserControlBase
    {
        public DialogServiceUC(ChangeViewEventArgs args) : base(typeof(DialogServiceUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.DialogServiceCommand = new CommandBase(commandParam => this.OnDialogService(commandParam), () => true);
            this.DataContext = this;
        }


        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase DialogServiceCommand { get; private set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }

        private NotificationBase Notification { get; } = new NotificationBase();
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

        private void OnDialogService(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("1") == true)
            {
                object parm = $"Einfacher Aufruf: \n var response = new DialogService<DialogWindow>().ShowDialog();";
                DialogResponse<DialogWindow> response = new DialogService<DialogWindow>(parm).WithOwner(Application.Current.MainWindow).ShowDialog();
                if (response.DialogResult == true)
                {
                    // OK
                }
                else
                {
                    // Abbrechen
                }
            }
            else if (commandParam != null && commandParam.Equals("2") == true)
            {
                object parm = $"Aufruf mit Fluent-API: \n var response = new DialogService<DialogWindow>()\n.WithTitle(\"Dialog Window\")\n.WithSize(700, 450)\n.CenterToScreen()\n.WithFont(\"Segoe UI\")\n.TopMost()\n.ShowDialog();";
                var response = new DialogService<DialogWindow>(parm)
                    .WithTitle("Dialog Window")
                    .WithSize(700, 450)
                    .CenterToScreen()
                    .WithFont("Segoe UI")
                    .TopMost()
                    .ShowDialog();
                if (response.DialogResult == true)
                {
                    // OK
                }
                else
                {
                    // Abbrechen
                }
            }
            else if (commandParam != null && commandParam.Equals("3") == true)
            {
                object parm = $"Aufruf mit Konstruktor Parameter: \n var response = new DialogService<DialogWindow>(parm, \"Max Mustermann\", 42)\n.WithTitle(\"Benutzer\")\n.ShowDialog();";
                var response = new DialogService<DialogWindow>(parm, "Max Mustermann", 42)
                    .WithTitle("Benutzer")
                    .ShowDialog();
                if (response.DialogResult == true)
                {
                    // OK
                }
                else
                {
                    // Abbrechen
                }
            }
            else if (commandParam != null && commandParam.Equals("4") == true)
            {
                object parm = $"Aufruf mit; Animation: \n var response = new DialogService<DialogWindow>()\n.WithFadeAnimation()\n.ShowDialog();";
                var response = new DialogService<DialogWindow>(parm).WithOwner(Application.Current.MainWindow).WithFadeAnimation().ShowDialog();

                if (response.DialogResult == true)
                {
                    // OK
                }
                else
                {
                    // Abbrechen
                }
            }
            else if (commandParam != null && commandParam.Equals("5") == true)
            {
                object parm = $"Einfacher Aufruf: \n var response = new DialogService<DialogWindow>()\n.Show();";
                var response = new DialogService<DialogWindow>(parm).WithFadeAnimation().Show();

                if (response.DialogResult == true)
                {
                    // OK
                }
                else
                {
                    // Abbrechen
                }
            }
            else if (commandParam != null && commandParam.Equals("6") == true)
            {
                object parm = $"Aufruf mit Konfiguration (.Show()/.ShowDialog()): \n var response = new DialogService<DialogWindow>()\nConfigure(w => \n{{ w.Width = 400;\nw.Height = 200;\nw.Background = System.Windows.Media.Brushes.AliceBlue;}})\n.Show();";
                var response = new DialogService<DialogWindow>(parm).Configure(w =>
                {
                    w.Width = 450;
                    w.Height = 250;
                    w.Background = System.Windows.Media.Brushes.AliceBlue;
                }).ShowDialog();

                if (response.DialogResult == true)
                {
                    // OK
                }
                else
                {
                    // Abbrechen
                }
            }
            else if (commandParam != null && commandParam.Equals("7") == true)
            {
                DialogResult response = Baustelle.Show();
                if (response.Accepted == true)
                {
                    // OK
                }
            }
            else if (commandParam != null && commandParam.Equals("8") == true)
            {
                DialogResponse<FolderPickerDialog> response = new DialogService<FolderPickerDialog>().WithOwner(Application.Current.MainWindow).ShowDialog();
                if (response.DialogResult == true)
                {
                    this.Notification.Hinweis($"Ausgewähltes Verzeichhnis: {response.ResponseObject}");
                }
                else
                {
                    // Abbrechen
                }
            }
        }

        #endregion Command Events

    }
}
