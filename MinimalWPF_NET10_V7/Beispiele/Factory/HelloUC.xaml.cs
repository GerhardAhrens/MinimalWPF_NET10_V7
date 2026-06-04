namespace MinimalWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für HelloUC.xaml
    /// </summary>
    public partial class HelloUC : UserControlBase
    {
        public HelloUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        #region Properties
        public CommandBase QuitCommand { get; private set; }
        public CommandBase HelpCommand { get; private set; }

        public CommandBase InformationCommand { get; private set; }
        public CommandBase SettingsCommand { get; private set; }
        public CommandBase CloseInformationPopupCommand { get; private set; }
        public CommandBase CloseSettingsPopupCommand { get; private set; }

        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        #endregion Properties

        #region Windows Events

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Handle loaded event
        }
        #endregion Windows Events
    }
}
