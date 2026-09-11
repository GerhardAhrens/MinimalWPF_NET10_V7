namespace System.Windows
{
    using System.Runtime.InteropServices;
    using System.Windows.Input;

    /// <summary>
    /// Signallisiert Windows: Bildschirm und System aktiv halten (entspricht AlwaysOn = true)
    /// </summary>
    public static class PowerHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint SetThreadExecutionState(uint esFlags);

        private const uint ES_CONTINUOUS = 0x80000000;
        private const uint ES_DISPLAY_REQUIRED = 0x00000002;
        private const uint ES_SYSTEM_REQUIRED = 0x00000001;

        public static void PreventSleep()
        {
            // Signallisiert Windows: Bildschirm und System aktiv halten (entspricht AlwaysOn = true)
            SetThreadExecutionState(ES_CONTINUOUS | ES_DISPLAY_REQUIRED | ES_SYSTEM_REQUIRED);
        }

        public static void AllowSleep()
        {
            // Setzt den Zustand zurück auf Standard
            SetThreadExecutionState(ES_CONTINUOUS);
        }

        /// <summary>
        /// Behandelt wichtige Ereignisse für das Fenster und ermöglicht so das Umschalten in den 
        /// Vollbildmodus mit F11 sowie das Beenden der Anwendung mit der Escape-Taste.
        /// </summary>
        /// <param name="sender">Window</param>
        /// <param name="e">KeyEventArgs</param>
        public static void WindowKeyDown(Window sender, KeyEventArgs e)
        {
            // F11 schaltet Vollbild um (entspricht ToggleFullScreenKeys)
            if (e.Key == Key.F11)
            {
                if (sender.WindowStyle == WindowStyle.None)
                {
                    sender.WindowStyle = WindowStyle.SingleBorderWindow;
                    sender.WindowState = WindowState.Normal;
                    sender.Topmost = false;
                }
                else
                {
                    sender.WindowStyle = WindowStyle.None;
                    sender.WindowState = WindowState.Maximized;
                    sender.Topmost = true;
                }
            }
            // Escape schließt die App (entspricht EscapeExitsFullScreen)
            else if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

    }
}
