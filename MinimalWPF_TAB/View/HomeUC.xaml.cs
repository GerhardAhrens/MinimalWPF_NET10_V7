//-----------------------------------------------------------------------
// <copyright file="HomeUC.cs" company="Lifeprojects.de">
//     Class: HomeUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.Windows;
    using System.Windows.Controls;

    using MinimalWPF.Core;

    /// <summary>
    /// Interaktionslogik für HomeUC.xaml
    /// </summary>
    public partial class HomeUC : UserControlBase
    {
        public HomeUC() : base(typeof(HomeUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.QuitCommand = new CommandBase(commandParam => this.OnQuit(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase QuitCommand { get; private set; }

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

        #endregion Command Events

    }
}
