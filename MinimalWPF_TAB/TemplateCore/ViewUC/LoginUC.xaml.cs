namespace System.Windows
{
    using System.Data;
    using System.Reflection;
    using System.Windows.Controls;
    using System.Windows.Input;

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
            this.LoginCommand = new CommandBase(commandParam => this.OnLogin(commandParam), () => true);
            this.InputTextCommand = new CommandBase(commandParam => this.OnInputText(commandParam), () => true);
            this.CancelLoginCommand = new CommandBase(commandParam => this.OnCancelLogin(commandParam), () => true);

            this.CurrentCtorArgs = args;
        }

        private void UserControlBase_PreviewKeyDown(object sender, Input.KeyEventArgs e)
        {
            bool isCtrlPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Control);
            bool isAltPressed = Keyboard.Modifiers.HasFlag(ModifierKeys.Alt);
            Key gedrueckteTaste = (e.Key == Key.System) ? e.SystemKey : e.Key;

            if (gedrueckteTaste == Key.I && isAltPressed)
            {
                this.InputTextCommand.Execute();
                e.Handled = true; // Verhindert, dass die TextBox das Event weiterverarbeitet
            }
        }

        private void OnInputText(object commandParam)
        {
        }

        #region Properties
        public CommandBase CreateAccountCommand { get; private set; }
        public CommandBase LoginCommand { get; private set; }
        public CommandBase InputTextCommand { get; private set; }
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

        public List<ApplicationAccount> AccountSource
        {
            get => base.GetValue<List<ApplicationAccount>>();
            set => base.SetValue(value);
        }

        public ApplicationAccount CurrentAccount
        {
            get => base.GetValue<ApplicationAccount>();
            set => base.SetValue(value);
        }

        private int MaxTryLogin { get; set; } = 3;
        private string FullAccountName { get; set; }
        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();
        private LoginState LoginStep { get; set; } = LoginState.None;
        private List<ApplicationAccount> Accounts { get; set; } = new List<ApplicationAccount>();
        #endregion Properties

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Erzwingt, dass das UserControl den Fokus bekommt
            this.Focus();

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
                    var jsonStorage = new JsonListSerializer<ApplicationAccount>();
                    this.Accounts = jsonStorage.Load(this.FullAccountName);

                    this.AccountSource = this.Accounts.Where(w => w.HasPin == true).ToList();
                    if (this.Accounts.Count == 1)
                    {
                        this.CurrentAccount = this.Accounts.First();
                        if (this.CurrentAccount.HasPin == true)
                        {
                            this.LoginStep = LoginState.LoginPin;
                            this.GridLoginPin.Visibility = Visibility.Visible;
                            this.Displayname = this.CurrentAccount.Displayname;
                        }
                        else
                        {
                            this.LoginStep = LoginState.LoginUsername;
                            this.GridLoginPassword.Visibility = Visibility.Visible;
                        }
                    }
                    else if (this.Accounts.Count > 1)
                    {
                    }
                    else
                    {
                        this.LoginStep = LoginState.CreateFirstAccount;
                        this.GridNeuesKonto.Visibility = Visibility.Visible;
                    }
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
                    account.Displayname = this.Displayname;
                    account.Benutzername = this.Benutzername;
                    account.Password = this.Password;
                    account.HasPin = !string.IsNullOrEmpty(this.Pin);
                    account.Pin = this.Pin;
                    this.Accounts.Add(account);
                    var jsonStorage = new JsonListSerializer<ApplicationAccount>();
                    jsonStorage.Version = 1;
                    jsonStorage.Save(this.FullAccountName, this.Accounts);

                    if (account.HasPin == true)
                    {
                    }
                    else
                    {
                        this.GridLoginPassword.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                App.ErrorMessage(ex, $"{this.Name}:");
                App.ApplicationExit();
            }
        }

        private async void OnLogin(object commandParam)
        {
            try
            {
                if (this.CurrentAccount.HasPin == false)
                {
                    if (string.IsNullOrEmpty(this.Benutzername) == true || string.IsNullOrEmpty(this.Password) == true)
                    {
                        this.Message.Warning("Login", "Für die Anmeldung muß ein Benutzername und ein Passwort eingegeben werden.");
                        return;
                    }

                    int countAccount = this.Accounts.Count(a => a.Benutzername == this.Benutzername && a.Password == this.Password && a.CreatedBy == Environment.UserName);
                    if (countAccount == 0)
                    {
                        this.MaxTryLogin--;
                        this.Message.Warning("Login", $"Benutzername oder Passwort ist falsch. Sie haben noch {this.MaxTryLogin} Versuche.");
                        if (this.MaxTryLogin <= 0)
                        {
                            this.Message.Warning("Login", $"Sie haben die maximale Anzahl an Login-Versuchen erreicht. Das Programm wird beendet.");
                            App.ApplicationExit();
                        }
                    }
                    else
                    {
                        int countAccountPW = this.Accounts.Count(a => a.Benutzername == this.Benutzername && a.Password == this.Password);
                        if (countAccountPW > 0)
                        {
                            ChangeViewEventArgs args = new();
                            args.MenuButton = CommandButtons.Home;
                            args.FromPage = CommandButtons.Login;
                            if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                            {
                                await App.EventAgg.PublishAsync(args);
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(this.Pin) == true)
                    {
                        this.Message.Warning("Login", "Für die Anmeldung muß eine gültige Pin eingegeben werden.");
                        return;
                    }

                    int countAccount = this.Accounts.Count(a => a.Pin == this.Pin && a.CreatedBy == Environment.UserName);
                    if (countAccount > 0)
                    {
                        ChangeViewEventArgs args = new();
                        args.MenuButton = CommandButtons.Home;
                        args.FromPage = CommandButtons.Login;
                        if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                        {
                            await App.EventAgg.PublishAsync(args);
                        }
                    }
                    else
                    {
                        this.MaxTryLogin--;
                        this.Message.Warning("Login", $"Die eingegebene Pin ist falsch. Sie haben noch {this.MaxTryLogin} Versuche.");
                        this.Pin = string.Empty;
                        if (this.MaxTryLogin <= 0)
                        {
                            this.Message.Warning("Login", $"Sie haben die maximale Anzahl an Login-Versuchen erreicht. Das Programm wird beendet.");
                            App.ApplicationExit(true);
                        }
                    }
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
