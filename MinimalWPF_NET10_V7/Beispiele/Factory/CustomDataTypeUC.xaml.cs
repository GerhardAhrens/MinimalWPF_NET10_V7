//-----------------------------------------------------------------------
// <copyright file="CustomDataTypeUC.cs" company="Lifeprojects.de">
//     Class: CustomDataTypeUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>16.06.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Beispiel
{
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Resources;

    using MinimalWPF.Beispiele;

    /// <summary>
    /// Interaktionslogik für CustomDataTypeUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class CustomDataTypeUC : UserControlBase
    {
        public CustomDataTypeUC(ChangeViewEventArgs args) : base(typeof(CustomDataTypeUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            
            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.CustomDataTypeCommand = new CommandBase(commandParam => this.OnCustomDataType(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase CustomDataTypeCommand { get; private set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            string outCodeCS = string.Empty;
            string className = "DemoCustomDataType";
            Uri uriCS = new Uri($"pack://application:,,,/Resources/Source/{className}.cs.source", UriKind.RelativeOrAbsolute);
            StreamResourceInfo sri = Application.GetResourceStream(uriCS);
            using StreamReader reader = new StreamReader(sri.Stream);
            outCodeCS = reader.ReadToEnd();

            this.TxtSyntaxBox.Text = outCodeCS;

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

        private void OnCustomDataType(object commandParam)
        {
            if (commandParam != null && commandParam.Equals("1") == true)
            {

            }
        }
        #endregion Command Events

    }
}
