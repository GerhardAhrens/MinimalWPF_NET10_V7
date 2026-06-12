//-----------------------------------------------------------------------
// <copyright file="InputBoxUC.cs" company="Lifeprojects.de">
//     Class: InputBoxUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.06.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Beispiel
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    using MinimalWPF.Beispiele;

    /// <summary>
    /// Interaktionslogik für InputBoxUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class InputBoxUC : UserControlBase
    {
        public InputBoxUC(ChangeViewEventArgs args) : base(typeof(InputBoxUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.InputBoxCommand = new CommandBase(commandParam => this.OnInputBox(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase InputBoxCommand { get; private set; }

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

        private void OnInputBox(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("IB_STRING") == true)
            {
                InputBoxResult<string> result = InputBox.Show<string>(Application.Current.MainWindow, "Namen eingeben");
                if (result != null && result.IsOk == true)
                {
                    this.Notification.Hinweis(result.Value);
                }
            }
            else if (commandParam != null && commandParam.Equals("IB_INT") == true)
            {
                InputBoxResult<int> result = InputBox.Show(Application.Current.MainWindow,
                            new InputBoxOptions<int>
                            {
                                Title = "Alter",
                                Message = "Bitte Alter eingeben",
                                DefaultValue = 18,
                                MinInt = 0,
                                MaxInt = 99
                            });
                if (result != null && result.IsOk == true)
                {
                    this.Notification.Hinweis(result.Value.ToString(CultureInfo.CurrentCulture));
                }
            }
            else if (commandParam != null && commandParam.Equals("IB_BOOL") == true)
            {
                InputBoxResult<bool> result = InputBox.Show(Application.Current.MainWindow,
                            new InputBoxOptions<bool>
                            {
                                Title = "Datensatz Aktive/Inaktive ",
                                Message = "Aktivieren/Deaktivieren",
                                DefaultValue = false,
                            });
                if (result != null && result.IsOk == true)
                {
                    this.Notification.Hinweis(result.Value.ToString(CultureInfo.CurrentCulture));
                }
            }
            else if (commandParam != null && commandParam.Equals("IB_DATETIME") == true)
            {
                InputBoxResult<DateTime> result = InputBox.Show(Application.Current.MainWindow,
                            new InputBoxOptions<DateTime>
                            {
                                Title = "Geburtstag",
                                Message = "Gib deinen Geburtstag ein",
                            });
                if (result != null && result.IsOk == true)
                {
                    this.Notification.Hinweis(result.Value.ToShortDateString());
                }
            }
        }

        #endregion Command Events

    }
}
