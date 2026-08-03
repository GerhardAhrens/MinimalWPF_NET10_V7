namespace MinimalWPF
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EventAggregator EventAgg { get; } = new();

        public static void ApplicationExit()
        {
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Application.Current.Shutdown(0);
        }

    }
}
