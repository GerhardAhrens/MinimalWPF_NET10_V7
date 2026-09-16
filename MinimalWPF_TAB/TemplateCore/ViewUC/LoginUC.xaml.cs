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
        private List<ApplicationAccount> Accounts { get; set; } = new List<ApplicationAccount>();
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
            try
            {
                if (this.Accounts.Count == 0)
                {
                    ApplicationAccount account = new ApplicationAccount();
                }
            }
            catch (Exception ex)
            {
                App.ErrorMessage(ex, $"{this.Name}:");
                App.ApplicationExit();
            }
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

    public class ApplicationAccount
    {
        public ApplicationAccount()
        {
            this.AccountId = Guid.CreateVersion7();
            this.CreatedBy = Environment.UserName;
            this.CreatedOn = DateTime.Now;
        }

        public Guid AccountId { get; set; }
        public string Displayname { get; set; }
        public string Benutzername { get; set; }
        public string Password { get; set; }
        public bool HasPin { get; set; }
        public string Pin { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

    }
}
