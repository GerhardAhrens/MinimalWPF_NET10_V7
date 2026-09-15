namespace System.Windows
{
    using System.Reflection;
    using System.Windows.Controls;

    using MinimalWPF;
    using MinimalWPF.Core;

    /// <summary>
    /// Interaktionslogik für LoginUC.xaml
    /// </summary>
    public partial class LoginUC : UserControlBase
    {
        private readonly string programDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        private readonly string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

        public LoginUC(ChangeViewEventArgs args)
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CreateAccountCommand = new CommandBase(commandParam => this.OnCreateAccount(commandParam), () => true);
            this.CancelLoginCommand = new CommandBase(commandParam => this.OnCancelLogin(commandParam), () => true);

            this.CurrentCtorArgs = args;
        }

        #region Properties
        public CommandBase CreateAccountCommand { get; private set; }
        public CommandBase CancelLoginCommand { get; private set; }

        public string LoginTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string Displayname
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string Benutzername
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string Password
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string PasswordRepeat
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string Pin
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private string FullAccountName { get; set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
        private LoginState LoginStep { get; set; } = LoginState.None;
        #endregion Properties

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this) == false)
            {
                this.FullAccountName = System.IO.Path.Combine(this.programDataPath, this.assemblyName, "Accounts.json");
                if (System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(this.FullAccountName)) == false)
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.FullAccountName));
                }

                if (System.IO.File.Exists(this.FullAccountName) == false)
                {
                    this.LoginStep = LoginState.CreateFirstAccount;
                    this.GridNeuesKonto.Visibility = Visibility.Visible;
                }
                else
                {
                    this.LoginStep = LoginState.LoginUsername;
                }
            }
        }

        private async void OnCreateAccount(object commandParam)
        {

        }

        private async void OnCancelLogin(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.AppQuit)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = button;

                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private enum LoginState
        {
            None,
            CreateAccount,
            CreateFirstAccount,
            LoginUsername,
            LoginPin,
        }
    }
}
