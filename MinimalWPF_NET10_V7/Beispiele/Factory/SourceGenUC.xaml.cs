//-----------------------------------------------------------------------
// <copyright file="SourceGenUC.cs" company="Lifeprojects.de">
//     Class: SourceGenUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>08.06.2026</date>
//
// <summary>
// Source Generator für eine neue UserControl, Window oder Klassen die in den resource abgelegt sind.
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für SourceGenUC.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Member als statisch markieren", Justification = "<Ausstehend>")]
    public partial class SourceGenUC : UserControlBase
    {
        public SourceGenUC(ChangeViewEventArgs args) : base(typeof(SourceGenUC))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.ShowSourceGenCommand = new CommandBase(commandParam => this.OnShowSourceGen(commandParam), () => true);

            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase ShowSourceGenCommand { get; private set; }

        public string ClassName
        {
            get => base.GetValue<string>();
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

        private void OnShowSourceGen(object commandParam)
        {
            // Handle the ShowSourceGen command

            if (string.IsNullOrEmpty(this.ClassName) == true)
            {
                this.Message.Hinweis("Fehler", "Bitte geben Sie einen Klassennamen ein.");
                return;
            }

            if (commandParam != null && commandParam.Equals("Gen_1") == true)
            {
                SourceGenerator.CreateSourceFile("NeuUC", ClassName);
                this.Message.Hinweis("SourceGen","Source Generator wurde ausgeführt. Die erstellte Dateien können über die Zwischenablage eingefügt werden.");
            }
            else if (commandParam != null && commandParam.Equals("Gen_2") == true)
            {
                SourceGenerator.CreateSourceFile("NeuWindow", ClassName);
                this.Message.Hinweis("SourceGen", "Source Generator wurde ausgeführt. Die erstellte Dateien können über die Zwischenablage eingefügt werden.");
            }
            else if (commandParam != null && commandParam.Equals("Gen_3") == true)
            {
                SourceGenerator.CreateSourceFile("NeuEnum", ClassName);
                this.Message.Hinweis("SourceGen", "Source Generator wurde ausgeführt. Die erstellte Dateien können über die Zwischenablage eingefügt werden.");
            }
            else if (commandParam != null && commandParam.Equals("Gen_4") == true)
            {
                SourceGenerator.CreateSourceFile("NeuPublicClass", ClassName);
                this.Message.Hinweis("SourceGen", "Source Generator wurde ausgeführt. Die erstellte Dateien können über die Zwischenablage eingefügt werden.");
            }
            else if (commandParam != null && commandParam.Equals("Gen_5") == true)
            {
                SourceGenerator.CreateSourceFile("NeuStaticExtensionBlock", ClassName);
                this.Message.Hinweis("SourceGen", "Source Generator wurde ausgeführt. Die erstellte Dateien können über die Zwischenablage eingefügt werden.");
            }
            else if (commandParam != null && commandParam.Equals("Gen_6") == true)
            {
                SourceGenerator.CreateSourceFile("NeuStaticExtension", ClassName);
                this.Message.Hinweis("SourceGen", "Source Generator wurde ausgeführt. Die erstellte Dateien können über die Zwischenablage eingefügt werden.");
            }
        }

        #endregion Command Events

    }
}
