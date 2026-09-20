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
    using System.IO;
    using System.Reflection;
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
            this.MenuArtikellisteCommand = new CommandBase(commandParam => this.OnMenuArtikelliste(commandParam), () => true);
            this.MenuKategorienCommand = new CommandBase(commandParam => this.OnMenuKategorien(commandParam), () => true);
            this.MenuControlDemoCommand = new CommandBase(commandParam => this.OnMenuControlDemo(commandParam), () => true);
            this.LogoffCommand = new CommandBase(commandParam => this.OnLogoff(commandParam), () => true);
            this.InformationCommand = new CommandBase(commandParam => this.OnPopup(commandParam));
            this.SettingsCommand = new CommandBase(commandParam => this.OnPopup(commandParam));
            this.CloseInformationPopupCommand = new CommandBase(commandParam => this.OnPopup(commandParam));
            this.CloseSettingsPopupCommand = new CommandBase(commandParam => this.OnPopup(commandParam));
            this.SelectionChangedCommand = new CommandBase(commandParam => this.OnSelectionChanged(commandParam), () => true);
            this.CloseTabCommand = new CommandBase(commandParam => this.OnCloseTab(commandParam), () => true);
            this.DropDownCommand = new CommandBase(commandParam => this.OnDropDown(commandParam), () => true);
        }

        private void OnCloseTab(object commandParam)
        {
            AdvancedTabItem tabItem = (AdvancedTabItem)commandParam;
        }

        private void OnDropDown(object commandParam)
        {
            Button button = (Button)commandParam;
            if (button != null && button.ContextMenu != null)
            {
                // Setzt den Button als Bezugspunkt, damit das Menü bündig darunter erscheint
                button.ContextMenu.PlacementTarget = button;

                // Öffnet das Menü
                button.ContextMenu.IsOpen = true;
            }
        }

        private void OnSelectionChanged(object commandParam)
        {
        }

        #region Properties
        public CommandBase QuitCommand { get; private set; }
        public CommandBase MenuArtikellisteCommand { get; private set; }
        public CommandBase MenuKategorienCommand { get; private set; }
        public CommandBase MenuControlDemoCommand { get; private set; }
        public CommandBase LogoffCommand { get; private set; }
        public CommandBase InformationCommand { get; private set; }
        public CommandBase SettingsCommand { get; private set; }
        public CommandBase CloseInformationPopupCommand { get; private set; }
        public CommandBase CloseSettingsPopupCommand { get; private set; }
        public CommandBase SelectionChangedCommand { get; private set; }
        public CommandBase CloseTabCommand { get; private set; }
        public CommandBase DropDownCommand { get; private set; }

        public string PopupTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string InstallFolder
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string SettingsFolder
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.InstallFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this.SettingsFolder = App.Settings.Pathname;
            this.PopupTitel = LocalizationValue.Get("WindowsTitelShort");

            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent("Bereit"));
            }

            this.DataContext = this;
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

        private async void OnMenuArtikelliste(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.Artikelliste)
                {
                    ChangeViewEventArgs args = new();
                    args.FromPage = CommandButtons.Home;
                    args.MenuButton = button;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnMenuKategorien(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.Kategorien)
                {
                    ChangeViewEventArgs args = new();
                    args.FromPage = CommandButtons.Home;
                    args.MenuButton = button;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private async void OnMenuControlDemo(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.ControlDemo)
                {
                    ChangeViewEventArgs args = new();
                    args.FromPage = CommandButtons.Home;
                    args.MenuButton = button;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private void OnPopup(object commandParam)
        {
            CommandButtons cb = (CommandButtons)commandParam;

            switch (cb)
            {
                case CommandButtons.InformationPopup:
                    if (this.InformationPopup.IsOpen == false)
                    {
                        this.InformationPopup.SetValue(MaskLayerBehavior.IsOpenProperty, true);
                    }
                    else
                    {
                        this.InformationPopup.SetValue(MaskLayerBehavior.IsOpenProperty, false);
                    }

                    break;
                case CommandButtons.SettingsPopup:
                    if (this.SettingsPopup.IsOpen == false)
                    {
                        this.SettingsPopup.SetValue(MaskLayerBehavior.IsOpenProperty, true);
                    }
                    else
                    {
                        this.SettingsPopup.SetValue(MaskLayerBehavior.IsOpenProperty, false);
                    }
                    break;
            }
        }

        private async void OnLogoff(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.Login)
                {
                    ChangeViewEventArgs args = new();
                    args.FromPage = CommandButtons.Home;
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
