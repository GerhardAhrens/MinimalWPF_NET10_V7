namespace System.Windows
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class FPInputDialog : Window, INotifyPropertyChanged
    {
        private string message;
        private string inputText;

        public FPInputDialog()
        {
            this.InitializeComponent();
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        public string InputText
        {
            get
            {
                return inputText;
            }
            set
            {
                inputText = value;
                OnPropertyChanged();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
