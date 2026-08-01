namespace MinimalWPF
{
    using System.ComponentModel;
    using System.Data;
    using System.Windows;
    using System.Windows.Data;

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
            this.DataContext = this;
        }

        #region Properties
        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
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

        private MessageBase Message { get; } = new MessageBase();
        #endregion Properties


        #region Windows Events
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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

            }

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