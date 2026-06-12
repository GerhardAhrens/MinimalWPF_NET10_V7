namespace MinimalWPF.Beispiele
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für SettingsPopupUC.xaml
    /// </summary>
    public partial class SettingsPopupUC : UserControl
    {
        public SettingsPopupUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.DataContext = this;
        }

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion WindowEventHandler
    }
}
