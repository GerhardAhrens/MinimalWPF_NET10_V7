//-----------------------------------------------------------------------
// <copyright file="TabArtikelDetail.cs" company="Lifeprojects.de">
//     Class: TabArtikelDetail
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.08.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Interaktionslogik für TabArtikelDetail.xaml
    /// </summary>
    public partial class TabArtikelDetail : UserControlBase
    {
        public TabArtikelDetail(DataRow rowView, DataRowAction rowAction = DataRowAction.Change) : base(typeof(TabArtikelDetail))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "LostFocus", this.OnLostFocus);
            this.CurrentRow = rowView;
            this.RowAction = rowAction;
            this.Background = System.Windows.Media.Brushes.LightBlue;
        }

        public TabArtikelDetail(DataTable originalTable, DataRowAction rowAction = DataRowAction.Add) : base(typeof(TabArtikelDetail))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "LostFocus", this.OnLostFocus);
            this.OriginalTable = originalTable;
            this.RowAction = rowAction;
            this.Background = System.Windows.Media.Brushes.LightBlue;
        }

        #region Properties

        public DataRow CurrentRow
        {
            get => base.GetValue<DataRow>();
            set => base.SetValue(value);
        }

        public DataRowAction RowAction { get; set; }

        private DataTable OriginalTable { get; set; }
        private DataRow CopyRow { get; set; }
        private MessageBase Message { get; } = new MessageBase();

        #endregion Properties

        #region Windows Events

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.RowAction == DataRowAction.Add)
            {
                this.CurrentRow = this.OriginalTable.NewRow();
                this.CurrentRow.SetField("A", 0);
                this.CurrentRow.SetField("B", string.Empty);
                this.CurrentRow.SetField("C", 0.0m);
                this.CurrentRow.SetField("Warengruppe", "Schreibwaren");
                this.CurrentRow.SetField("Anzahl", 0);
            }
            else if (this.RowAction == DataRowAction.Change)
            {
                this.ArtikelNummer.IsReadOnly = true;
                this.ArtikelNummer.Background = Brushes.LightYellow;
            }
            else if (this.RowAction == DataRowAction.ChangeOriginal)
            {
                DataRow newRow = this.CurrentRow.Table.NewRow();
                newRow = this.CurrentRow.CloneRow();
                this.CurrentRow = newRow;
            }
            else if (this.RowAction == DataRowAction.Nothing)
            {
                this.ArtikelNummer.IsReadOnly = true;
                this.ArtikelNummer.Background = Brushes.LightYellow;
                this.ArtikelBezeichnung.IsReadOnly = true;
                this.ArtikelBezeichnung.Background = Brushes.LightYellow;
                this.Artikelpreis.IsReadOnly = true;
                this.Artikelpreis.Background = Brushes.LightYellow;
                this.Warengruppe.IsReadOnly = true;
                this.Warengruppe.Background = Brushes.LightYellow;
                this.AnzahlProPackung.IsReadOnly = true;
                this.AnzahlProPackung.Background = Brushes.LightYellow;
            }

            this.DataContext = this;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {

        }
        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
