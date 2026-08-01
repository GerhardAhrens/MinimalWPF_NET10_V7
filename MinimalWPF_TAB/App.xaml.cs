namespace MinimalWPF
{
    using System.Configuration;
    using System.Data;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void ApplicationExit()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.Shutdown(0);
        }

    }
}
