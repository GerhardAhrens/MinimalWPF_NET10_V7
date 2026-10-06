//-----------------------------------------------------------------------
// <copyright file="ControlDemo2UC.cs" company="Lifeprojects.de">
//     Class: ControlDemo2UC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.10.2026</date>
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
    /// Interaktionslogik für ControlDemo2UC.xaml
    /// </summary>
    public partial class ControlDemo2UC : UserControlBase
    {
        public ControlDemo2UC(ChangeViewEventArgs args) : base(typeof(ControlDemo2UC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.SelectLBCommand = new CommandBase(commandParam => this.OnSelectLB(commandParam), () => true);
            this.ActionLBCommand = new CommandBase(commandParam => this.OnActionLB(commandParam), () => true);
            this.DoubleClickLBCommand = new CommandBase(commandParam => this.OnDoubleClickLB(commandParam), () => true);

            this.DemoDateSource = new List<DemoDataCB>
            {
                new DemoDataCB("Test 1"),
                new DemoDataCB("Test 2"),
                new DemoDataCB("Test 3"),
                new DemoDataCB("Test 4"),
                new DemoDataCB("Test 5"),
                new DemoDataCB("Donald Duck"),
                new DemoDataCB("Dagobert Duck"),
                new DemoDataCB("Gustav Gans"),
            };

            this.SelectedDemoDate = this.DemoDateSource.FirstOrDefault();

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase SelectLBCommand { get; private set; }
        public CommandBase ActionLBCommand { get; private set; }
        public CommandBase DoubleClickLBCommand { get; private set; }

        public List<DemoDataCB> DemoDateSource
        {
            get => base.GetValue<List<DemoDataCB>>();
            set => base.SetValue(value);
        }

        public DemoDataCB SelectedDemoDate
        {
            get => base.GetValue<DemoDataCB>();
            set => base.SetValue(value);
        }

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

        private void OnDoubleClickLB(object commandParam)
        {
            DemoDataCB demoDataCB = commandParam as DemoDataCB;
            this.Message.Hinweis("AdvancedListbox", $"Item Double-Click: {demoDataCB.ValueText.ToString()}");
        }

        private void OnActionLB(object commandParam)
        {
            DemoDataCB demoDataCB = commandParam as DemoDataCB;
            this.Message.Hinweis("AdvancedListbox", $"Item Action-Button: {demoDataCB.ValueText.ToString()}");
        }

        private void OnSelectLB(object commandParam)
        {
            SelectionChangedInfo demoDataCB = commandParam as SelectionChangedInfo;
            if (demoDataCB.SelectedItem != null && demoDataCB.SelectedItems.Any() == true)
            {
                this.Message.Hinweis("AdvancedListbox", $"Item Selection Changed: {((MinimalWPF.View.DemoDataCB)demoDataCB.SelectedItems[0]).ValueText}");
            }

        }

        #endregion Command Events

    }
}
