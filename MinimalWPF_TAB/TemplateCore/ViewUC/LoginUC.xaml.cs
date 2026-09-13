namespace System.Windows
{
    using MinimalWPF.Core;

    /// <summary>
    /// Interaktionslogik für LoginUC.xaml
    /// </summary>
    public partial class LoginUC : UserControlBase
    {
        public LoginUC(ChangeViewEventArgs args)
        {
            this.InitializeComponent();
            this.CurrentCtorArgs = args;
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
    }
}
