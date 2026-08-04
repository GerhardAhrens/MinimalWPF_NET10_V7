namespace MinimalWPF
{
    using System.ComponentModel;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using MinimalWPF.Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            this.InitializeComponent();
            WeakEventManager<WindowBase, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<WindowBase, CancelEventArgs>.AddHandler(this, "Closing", this.OnWindowClosing);

            this.SetVectorIcon("IconApplicationLogo", 64);
            this.WindowTitel = LocalizationValue.Get("WindowsTitelZeile");

            this.SelectDataRowCommand = new CommandBase(commandParam => this.OnSelectDataRow(commandParam), () => true);
            this.SelectDataRowClickCommand = new CommandBase(commandParam => this.OnSelectDataRowClick(commandParam), () => true);
            this.ContextMenuClickCommand = new CommandBase(commandParam => this.OnContextMenuOpening(commandParam), () => true);
            this.DeleteCommand = new CommandBase(commandParam => this.OnDeleteCommand(commandParam), () => true);
            this.EditCommand = new CommandBase(commandParam => this.OnEditCommand(commandParam), () => true);
            this.StatusBarCommand = new CommandBase(commandParam => this.OnStatusBarCommand(commandParam), () => true);
            this.MenuNeuCommand = new CommandBase(commandParam => this.OnMenuNeuCommand(commandParam), () => true);

            this.RegisterFactory();

            this.DataContext = this;
        }

        #region Properties
        public CommandBase MenuNeuCommand { get; private set; }
        public CommandBase SelectDataRowCommand { get; private set; }
        public CommandBase SelectDataRowClickCommand { get; private set; }
        public CommandBase ContextMenuClickCommand { get; private set; }
        public CommandBase DeleteCommand { get; private set; }
        public CommandBase EditCommand { get; private set; }
        public CommandBase StatusBarCommand { get; private set; }

        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public System.Windows.Controls.UserControl WorkContent
        {
            get { return base.GetValue<System.Windows.Controls.UserControl>(); }
            set { base.SetValue(value); }
        }

        public ICollectionView DataSource
        {
            get => base.GetValue<ICollectionView>();
            set => base.SetValue(value);
        }

        public DataRowView SelectedDataRow
        {
            get => base.GetValue<DataRowView>();
            set => base.SetValue(value);
        }

        public string Id
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private MessageBase Message { get; } = new MessageBase();
        #endregion Properties


        #region Windows Events
        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            App.EventAgg.Subscribe<StatusEvent>(async (evt, ct) => this.OnUpdateStatusBar(evt));

            this.ConfigurationStatusInfoBar();

            DataTable dt = LadeArtikel();
            this.DataSource = CollectionViewSource.GetDefaultView(dt);
            if (this.DataSource != null)
            {
                try
                {
                    this.DataSource.MoveCurrentToFirst();
                    int maxCount = this.DataSource.Cast<DataRowView>().Count();
                }
                catch (Exception ex)
                {
                    string errorText = ex.Message;
                    throw;
                }

                if (App.EventAgg.IsSubscription<StatusEvent>() == true)
                {
                    await App.EventAgg.PublishAsync(new StatusEvent("Bereit; Aktikeldaten geladen"));
                }

                /*
                 * Alle Zellen einfärben, unabhängig von der Spalte
                lvwMain.CellStyleProvider = request =>
                {
                    return new CellStyleInfo
                    {
                        Background = Brushes.Yellow,
                        Foreground = Brushes.Red
                    };
                };
                */

                /*
                 * Nur die Spalte "C" "Preis"
                lvwMain.CellStyleProvider = request =>
                {
                    if (request.Column.SortMemberPath == "C")
                    {
                        return new CellStyleInfo
                        {
                            Background = Brushes.Yellow,
                            Foreground = Brushes.Red
                        };
                    }

                    return null;
                };
                */

                /*
                 * Nur eine Zeile
                lvwMain.CellStyleProvider = request =>
                {
                    if (request.Item is DataRowView row)
                    {
                        if ((string)row["B"] == "Bleistift")
                        {
                            return new CellStyleInfo
                            {
                                Background = Brushes.LightBlue
                            };
                        }
                    }

                    return null;
                };
                */

                /*
                 * Nur eine Zelle
                lvwMain.CellStyleProvider = request =>
                {
                    if (request.Column.SortMemberPath == "C")
                    {
                        if (request.Value is decimal preis && preis > 2)
                        {
                            return new CellStyleInfo
                            {
                                Foreground = Brushes.Red,
                                FontWeight = FontWeights.Bold
                            };
                        }
                    }

                    return null;
                };
                */
            }

        }

        private void OnUpdateStatusBar(StatusEvent evt)
        {
            StatusBar.SetNotification(evt.Notification);
        }

        private void ConfigurationStatusInfoBar()
        {
            #region Test Visibility
            StatusBar.Rights.Show(false);
            //StatusBar.Date.Show(true);
            StatusBar.Datasource.Show(false);
            #endregion Test Visibility

            #region Lange Text in Notification
            //StatusBar.SetNotification("Dies ist eine sehr lange Meldung welche den gesamten freien Platz innerhalb der StatusInfoBar ausfüllen sollte. Danach muss der Text automatisch mit Ellipsis abgeschnitten werden.");
            #endregion Lange Text in Notification

            #region Test Text
            //StatusBar.Rights.Text = "Benutzerrechte";
            //StatusBar.Date.Text = System.DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            //StatusBar.Datasource.Text = "Datenquelle";
            #endregion Test Text

            #region Test Farben
            //StatusBar.Account.SetColors(Brushes.Green, Brushes.AliceBlue);
            #endregion Test Farben

            #region Command
            StatusBar.Account.Command = StatusBarCommand;
            #endregion Command

            #region Auto Timer
            //StatusBar.AutoUpdateDateTime = true;
            #endregion Auto Timer
        }

        private void OnCloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnQuit()
        {
            this.Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;


            MessageBoxResult msgYN;
            if (this.Tag != null)
            {
                msgYN = this.Message.AppExitMessage(this.Tag.ToString());
            }
            else
            {
                msgYN = this.Message.AppExitMessage();
            }

            if (msgYN == MessageBoxResult.Yes)
            {
                App.ApplicationExit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        #endregion Windows Events

        private void OnSelectDataRow(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {
                this.Id = rowView["A"].ToString();
            }
        }

        private void OnSelectDataRowClick(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {
                string id = rowView["A"].ToString();
                this.Message.Hinweis("Information",$"Artikelnummer: {id}" );
            }
        }

        private void OnContextMenuOpening(object commandParam)
        {
            if (commandParam is ContextMenuCommandArgs args)
            {
                DataRowView rowView = (DataRowView)args.SelectedItem;
                string id = rowView["A"].ToString();

                foreach (MenuItem item in args.ContextMenu.Items)
                {
                    if (item.Name == "ctxDelete")
                    {
                        item.IsEnabled = true;
                    }
                }
            }
        }
        
        private void OnDeleteCommand(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {
                string id = rowView["A"].ToString();
                this.Message.Hinweis("Löschen", $"Artikelnummer: {id}");
            }
        }

        private void OnEditCommand(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {
                string id = rowView["A"].ToString();
                this.Message.Hinweis("Bearbeiten", $"Artikelnummer: {id}");
            }
        }

        private void OnMenuNeuCommand(object commandParam)
        {
            if (commandParam is Button button)
            {
                this.Message.Hinweis("Menu", $"Klick auf Menu Neu => {button.Content}");
            }
        }

        #region Event Aggregator Handler
        private void OnStatusBarCommand(object commandParam)
        {
            if (commandParam is StatusInfoBarItem item)
            {
                string accountText = item.Text;
                this.Message.Hinweis("StatusBar", $"Klick auf StatusBar Account => {accountText}");
            }
        }

        private async void ChangeControl(ChangeViewEventArgs commandParam)
        {
            try
            {
                this.Dispatcher.Invoke(() => Mouse.OverrideCursor = Cursors.Wait);

                if (commandParam != null && commandParam.MenuButton is CommandButtons button)
                {
                    if (button == CommandButtons.AppQuit)
                    {
                        this.OnQuit();
                    }
                    else if (button.In(CommandButtons.Home, CommandButtons.GoBack))
                    {

                        if (App.EventAgg.IsSubscription<WindowsTitelEvent>() == true)
                        {
                            await App.EventAgg.PublishAsync(new WindowsTitelEvent(button.ToDescription()));
                        }

                        this.WorkContent = null;
                        this.WorkContent = (UserControl)Factory.Get<UserControlBase, CommandButtons>((CommandButtons)commandParam.MenuButton, commandParam);
                    }
                }

                this.Dispatcher.Invoke(() => Mouse.OverrideCursor = null);
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                App.ErrorMessage(ex, $"Fehler in {this.GetType().Name}");
            }
        }

        #endregion Event Aggregator Handler

        /// <summary>
        /// Dialog aus UserControls werden hier für die Factory registriert 😊
        /// </summary>
        private void RegisterFactory()
        {
        }


        private static DataTable LadeArtikel()
        {
            DataTable table = new("Artikel");

            table.Columns.Add("A", typeof(int));         // Key
            table.Columns.Add("B", typeof(string));      // Artikelname
            table.Columns.Add("C", typeof(decimal));     // Preis

            table.PrimaryKey = new[] { table.Columns["A"] };

            table.Rows.Add(2001, "Kugelschreiber", 1.99m);
            table.Rows.Add(2002, "Bleistift", 0.79m);
            table.Rows.Add(2003, "Radiergummi", 1.29m);
            table.Rows.Add(2004, "Notizblock", 3.49m);
            table.Rows.Add(2005, "Ordner", 4.99m);
            table.Rows.Add(2006, "Locher", 8.95m);
            table.Rows.Add(2007, "Tacker", 12.50m);
            table.Rows.Add(2008, "Lineal", 2.19m);
            table.Rows.Add(2009, "Schere", 6.75m);
            table.Rows.Add(2010, "Marker", 2.99m);

            return table;
        }
    }
}