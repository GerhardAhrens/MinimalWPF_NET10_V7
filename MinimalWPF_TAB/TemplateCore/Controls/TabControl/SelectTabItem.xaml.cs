namespace System.Windows.Controls
{
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;


    /// <summary>
    /// Interaktionslogik für SelectTabItem.xaml
    /// </summary>
    public partial class SelectTabItem : Window
    {
        /* API Importe (GetWindowLong, SetWindowLong) */
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        public SelectTabItem(Dictionary<int, string>  tabItems)
        {
            this.InitializeComponent();

            this.SourceInitialized += (s, e) => 
            {
                IntPtr hwnd = new WindowInteropHelper(this).Handle;
                // API Aufruf zum Entfernen des Systemmenüs
                _ = SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            };

            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.NoResize;

            WeakEventManager<Window, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<Window, CancelEventArgs>.AddHandler(this, "Closing", this.OnWindowClosing);


            if (tabItems != null)
            {
                this.OpenTabItems = tabItems;
            }
            else 
            {
                this.Result = new DialogValueResult<int> 
                { 
                    Accepted = false, 
                    ResultValue = -1 
                };
            }
        }

        public DialogValueResult<int> Result { get; private set; }
        public Dictionary<int, string> OpenTabItems { get; set; }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {

        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
                return;
            }

            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                this.Result = new DialogValueResult<int>
                {
                    Accepted = false,
                    ResultValue = -1
                };
                return;
            }

            base.OnPreviewKeyDown(e);
        }

        private void TabItemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        #region Aufruf WIN 32 API
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        #endregion Aufruf WIN 32 API
    }
}
