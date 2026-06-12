//-----------------------------------------------------------------------
// <copyright file="SettingsUC.cs" company="Lifeprojects.de">
//     Class: SettingsUC
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
    using System.IO;
    using System.Security.Cryptography;
    using System.Windows;
    using System.Windows.Controls;

    using MinimalWPF.Beispiele;

    /// <summary>
    /// Interaktionslogik für SettingsUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class SettingsUC : UserControlBase
    {
        public SettingsUC(ChangeViewEventArgs args) : base(typeof(SettingsUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.InitSettingsCommand = new CommandBase(commandParam => this.OnSettings(commandParam), () => true);
            this.ReadSettingsCommand = new CommandBase(commandParam => this.OnSettings(commandParam), () => true);
            this.SaveSettingsCommand = new CommandBase(commandParam => this.OnSettings(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase InitSettingsCommand { get; private set; }
        public CommandBase ReadSettingsCommand { get; private set; }
        public CommandBase SaveSettingsCommand { get; private set; }

        public string SettingsPfad
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string SettingsProperties
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

        private void OnSettings(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("init") == true)
            {
                using (ApplicationSettings settings = new ApplicationSettings())
                {
                    if (settings.IsExitSettings() == false)
                    {
                        settings.Username = $"{Environment.UserDomainName}\\{Environment.UserName}";
                        settings.LetzterZugriff = DateTime.Now;
                        settings.FrageExit = true;
                        settings.Save();
                    }
                    else
                    {
                        settings.Load();
                    }

                    App.Settings = settings;
                    this.SettingsPfad = settings.Pathname;
                    this.SettingsProperties = string.Join(";", settings.GetProperties.Select(s => s.Name).ToArray());
                }
            }
            else if (commandParam != null && commandParam.Equals("read") == true)
            {
                using (ApplicationSettings settings = new ApplicationSettings())
                {
                    if (settings.IsExitSettings() == true)
                    {
                        settings.Load();
                    }

                    App.Settings = settings;
                    this.SettingsPfad = Path.Combine(settings.Pathname, settings.Filename);
                    this.SettingsProperties = string.Join("; ", settings.GetProperties.Select(s => $"{s.Name} ({s.PropertyType.Name})").ToArray());
                }
            }
        }

        #endregion Command Events

    }
}
