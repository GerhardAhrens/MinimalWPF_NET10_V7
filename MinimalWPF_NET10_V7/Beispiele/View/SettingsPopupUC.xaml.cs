namespace MinimalWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für SettingsPopupUC.xaml
    /// </summary>
    public partial class SettingsPopupUC : UserControlBase
    {
        public SettingsPopupUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        #region Properties
        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private ApplicationSettings Settings { get; set; }

        #endregion Properties

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.WindowTitel = LocalizationValue.Get("WindowsTitelZeile");
        }
        #endregion WindowEventHandler
    }
}
