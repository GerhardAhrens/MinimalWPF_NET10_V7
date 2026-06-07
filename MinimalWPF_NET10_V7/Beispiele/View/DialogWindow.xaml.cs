namespace MinimalWPF.Beispiele
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Interaktionslogik für DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : WindowBase
    {
        public DialogWindow(string param)
        {
            this.InitializeComponent();
            WeakEventManager<WindowBase, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<WindowBase, CancelEventArgs>.AddHandler(this, "Closing", this.OnWindowClosing);
            this.WindowTitel = LocalizationValue.Get("DialogWindowsTitelZeile");
            this.DataContext = this;
            this.DemoText = param;
        }

        public DialogWindow(string param,string name, int age)
        {
            this.InitializeComponent();
            WeakEventManager<WindowBase, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<WindowBase, CancelEventArgs>.AddHandler(this, "Closing", this.OnWindowClosing);
            this.WindowTitel = LocalizationValue.Get("DialogWindowsTitelZeile");
            this.DataContext = this;
            this.DemoText = $"{param}\nName: {name}, Age: {age}";
        }

        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string DemoText
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnCloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (this.Owner != null)
            {
                this.DialogResult = false;
            }
        }
        #endregion WindowEventHandler
    }
}
