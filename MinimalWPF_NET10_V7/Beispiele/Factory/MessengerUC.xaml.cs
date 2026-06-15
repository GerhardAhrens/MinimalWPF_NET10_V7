
//-----------------------------------------------------------------------
// <copyright file="MessengerUC.cs" company="Lifeprojects.de">
//     Class: MessengerUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>13.06.2026</date>
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
    /// Interaktionslogik für MessengerUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class MessengerUC : UserControlBase
    {
        public MessengerUC(ChangeViewEventArgs args) : base(typeof(MessengerUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.ReadMessengerCommand = new CommandBase(commandParam => this.OnReadMessenger(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase ReadMessengerCommand { get; private set; }

        public string MenuItem
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

        private void OnReadMessenger(object commandParam)
        {
            _ = new ModulAService(App.CurrentMessenger);
            _ = new ModulBService(App.CurrentMessenger);

            var menuItems = App.CurrentMessenger.RequestAll<GetMenuItemsRequest, MenuItemInfo>(new GetMenuItemsRequest());
            this.MenuItem = string.Join("; ", menuItems.Select(s => s.Header));
        }

        #endregion Command Events

    }

    public record GetMenuItemsRequest();

    public record MenuItemInfo(string Header, Action Execute);

    public class ModulAService
    {
        public ModulAService(Messenger messenger)
        {
            messenger.Register<GetMenuItemsRequest, MenuItemInfo>(_ => new MenuItemInfo("Kunden", () => Console.WriteLine("Kunden Öffnen")));
        }
    }

    public class ModulBService
    {
        public ModulBService(Messenger messenger)
        {
            messenger.Register<GetMenuItemsRequest, MenuItemInfo>(_ => new MenuItemInfo("Rechnungen", () => Console.WriteLine("Rechnungen öffnen")));
        }
    }
}
