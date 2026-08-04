//-----------------------------------------------------------------------
// <copyright file="ArtikellisteUC.cs" company="Lifeprojects.de">
//     Class: ArtikellisteUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.ComponentModel;
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using MinimalWPF.Core;

    /// <summary>
    /// Interaktionslogik für Artikelliste.xaml
    /// </summary>
    public partial class ArtikellisteUC : UserControlBase
    {
        public ArtikellisteUC(ChangeViewEventArgs args) : base(typeof(ArtikellisteUC))
        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.CurrentCtorArgs = args;

            this.GoBackCommand = new CommandBase(commandParam => this.OnGoBack(commandParam), () => true);
            this.SelectDataRowCommand = new CommandBase(commandParam => this.OnSelectDataRow(commandParam), () => true);
            this.SelectDataRowClickCommand = new CommandBase(commandParam => this.OnSelectDataRowClick(commandParam), () => true);
            this.DataContext = this;
        }

        #region Properties
        public CommandBase GoBackCommand { get; private set; }
        public CommandBase SelectDataRowCommand { get; private set; }
        public CommandBase SelectDataRowClickCommand { get; private set; }


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

        public ID Id
        {
            get => base.GetValue<ID>();
            set => base.SetValue(value);
        }

        private ChangeViewEventArgs CurrentCtorArgs { get; set; }
        private MessageBase Message { get; } = new MessageBase();

        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
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

            if (App.EventAgg.IsSubscription<StatusEvent>() == true)
            {
                await App.EventAgg.PublishAsync(new StatusEvent("Bereit"));
            }
        }
        #endregion Windows Events

        #region Command Events
        private async void OnGoBack(object commandParam)
        {
            if (commandParam != null && commandParam is CommandButtons button)
            {
                if (button == CommandButtons.GoBack)
                {
                    ChangeViewEventArgs args = new();
                    args.MenuButton = this.CurrentCtorArgs.FromPage;
                    args.FromPage = this.CurrentCtorArgs.MenuButton;
                    if (App.EventAgg.IsSubscription<ChangeViewEventArgs>() == true)
                    {
                        await App.EventAgg.PublishAsync(args);
                    }
                }
            }
        }

        private void OnSelectDataRow(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {
                this.Id = Convert.ToInt32(rowView["A"]);
            }
        }

        private void OnSelectDataRowClick(object commandParam)
        {
            if (commandParam is DataRowView rowView)
            {
                this.Message.Hinweis("Information", $"Artikelnummer: {this.Id}");
            }
        }

        #endregion Command Events

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
