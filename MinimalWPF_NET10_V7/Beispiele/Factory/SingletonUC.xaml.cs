//-----------------------------------------------------------------------
// <copyright file="SingletonUC.cs" company="Lifeprojects.de">
//     Class: SingletonUC
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
    using System.Windows.Input;

    using MinimalWPF.Beispiele;

    /// <summary>
    /// Interaktionslogik für SingletonUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class SingletonUC : UserControlBase
    {
        public SingletonUC(ChangeViewEventArgs args) : base(typeof(SingletonUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.InitInstanzCommand = new CommandBase(commandParam => this.OnInitInstanz(commandParam), () => true);
            this.ReloadInstanzCommand = new CommandBase(commandParam => this.OnReloadInstanz(commandParam), () => true);


            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase InitInstanzCommand { get; private set; }
        public CommandBase ReloadInstanzCommand { get; private set; }

        public string AppName
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string AppNameInit
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string AppNameDefault
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string InstanzEquals
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

        private void OnInitInstanz(object commandParam)
        {
            ConfigurationManager conf = SingletonBase<ConfigurationManager>.Instance;
            if (conf != null)
            {
                this.AppName = conf.ApplicationName;
            }
        }

        private void OnReloadInstanz(object commandParam)
        {
            ConfigurationManager conf = SingletonBase<ConfigurationManager>.Instance;
            if (conf != null)
            {
                this.AppNameInit = conf.ApplicationName;
                App.DoEvents();
                this.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);

                Thread.Sleep(5000);
                conf.ReloadContent();

                this.AppNameDefault = conf.ApplicationName;
                App.DoEvents();

                this.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);

            }

            ConfigurationManager conf1 = SingletonBase<ConfigurationManager>.Instance;
            if (conf.Equals(conf1) == true)
            {
                this.InstanzEquals = "conf.Equals(conf1) == true";
            }
            else
            {
                this.InstanzEquals = "conf.Equals(conf1) == false";
            }
        }

        #endregion Command Events
    }
}
