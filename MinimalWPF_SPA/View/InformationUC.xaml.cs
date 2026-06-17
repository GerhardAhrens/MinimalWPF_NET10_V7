namespace MinimalWPF.View
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für InformationUC.xaml
    /// </summary>
    public partial class InformationUC : UserControlBase
    {
        public InformationUC()
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                this.WindowTitel = LocalizationValue.Get("WindowsTitelZeile");
                this.ApplikationVersion = base.ApplicationVersion.ToString();
                this.LaufzeitVersion = base.RuntimeVersion;
                this.WinVersion = base.WindowsVersion;
            }

            this.DataContext = this;
        }

        #region Properties
        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string ApplikationVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string LaufzeitVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string WinVersion
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        #endregion Properties

        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion WindowEventHandler
    }
}
