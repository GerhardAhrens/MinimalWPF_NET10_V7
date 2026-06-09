//-----------------------------------------------------------------------
// <copyright file="LocalizationUC.cs" company="Lifeprojects.de">
//     Class: LocalizationUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.06.2026</date>
//
// <summary>
// UserControl zur Demonstration der Lokalisierung von Texten
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Beispiel
{
    using System.Windows;
    using System.Windows.Controls;

    using MinimalWPF.Beispiele;

    /// <summary>
    /// Interaktionslogik für LocalizationUC.xaml
    /// </summary>
    public partial class LocalizationUC : UserControlBase
    {
        public LocalizationUC(ChangeViewEventArgs args) : base(typeof(LocalizationUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }

        public IEnumerable<string> LocalizationKeys
        {
            get => base.GetValue<IEnumerable<string>>();
            set => base.SetValue(value);
        }

        public string SelectedLocalizationValue
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

            this.LocalizationKeys = LocalizationValue.Keys; 
        }

        private void cbKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox cb)
            {
                object val = ((object[])e.AddedItems)[0];
                string content = LocalizationValue.Get(val.ToString());

                this.SelectedLocalizationValue = LocalizationValue.Get(val.ToString(), "zusätzlicher Parameter", "und einem zweiten Parameter");
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
