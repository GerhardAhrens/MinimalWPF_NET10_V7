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
    using System.Windows.Media;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für TabArtikelDetail.xaml
    /// </summary>
    public partial class TabArtikelDetail : UserControlBase
    {
        public TabArtikelDetail(DataRow rowView) : base(typeof(TabArtikelDetail))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "LostFocus", this.OnLostFocus);
            this.CurrentRow = rowView;
            this.Background = System.Windows.Media.Brushes.LightBlue;
        }

        #region Properties

        public DataRow CurrentRow
        {
            get => base.GetValue<DataRow>();
            set => base.SetValue(value);
        }

        private MessageBase Message { get; } = new MessageBase();

        #endregion Properties

        #region Windows Events

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (this.CurrentRow.RowState == DataRowState.Detached)
            {
                this.CurrentRow.SetField("A", 0);
                this.CurrentRow.SetField("B", string.Empty);
                this.CurrentRow.SetField("C", 0.0m);
                this.CurrentRow.SetField("Warengruppe", "Schreibwaren");
            }
            else if (this.CurrentRow.RowState == DataRowState.Unchanged)
            {
                this.ArtikelNummer.IsReadOnly = true;
                this.ArtikelNummer.Background = Brushes.LightYellow;
            }
            else if (this.CurrentRow.RowState == DataRowState.Added)
            {
                this.CurrentRow.AcceptChanges();
                this.CurrentRow.Table.AcceptChanges();
                this.CurrentRow.SetModified();
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
