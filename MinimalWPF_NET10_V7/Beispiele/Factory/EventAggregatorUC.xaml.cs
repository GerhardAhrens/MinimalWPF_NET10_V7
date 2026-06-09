//-----------------------------------------------------------------------
// <copyright file="EventAggregatorUC.cs" company="Lifeprojects.de">
//     Class: EventAggregatorUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>09.06.2026</date>
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
    /// Interaktionslogik für EventAggregatorUC.xaml
    /// </summary>
    public partial class EventAggregatorUC : UserControlBase
    {
        public EventAggregatorUC(ChangeViewEventArgs args) : base(typeof(EventAggregatorUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.SendWTCommand = new CommandBase(commandParam => this.OnChangeWT(commandParam), () => true);
            this.SendSBCommand = new CommandBase(commandParam => this.OnChangeSB(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase SendWTCommand { get; private set; }
        public CommandBase SendSBCommand { get; private set; }

        public string InputTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string InputStatusBar
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

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

        private async void OnChangeSB(object commandParam)
        {
            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent(this.InputStatusBar));
            }
        }

        private async void OnChangeWT(object commandParam)
        {
            if (App.EventAgg.IsSubscription<WindowsTitelEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new WindowsTitelEvent(this.InputTitel));
            }
        }

        #endregion Command Events

    }
}
