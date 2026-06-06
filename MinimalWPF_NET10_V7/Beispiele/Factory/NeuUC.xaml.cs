//-----------------------------------------------------------------------
// <copyright file="NeuUC.cs" company="Lifeprojects.de">
//     Class: NeuUC
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
    /// Interaktionslogik für NeuUC.xaml
    /// </summary>
    public partial class NeuUC : UserControlBase
    {
        public NeuUC(ChangeViewEventArgs args) : base(typeof(NeuUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);

            this.DataContext = this;
        }


        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }

        #endregion Properties


        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent("Bereit"));
            }

            if (App.EventAgg.IsSubscription<WindowsTitelEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new WindowsTitelEvent(CommandButtons.ShowResult.ToDescription()));
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
