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

    /// <summary>
    /// Interaktionslogik für TabArtikelDetail.xaml
    /// </summary>
    public partial class TabArtikelDetail : UserControlBase
    {
        public TabArtikelDetail(DataRowView rowView) : base(typeof(TabArtikelDetail))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.CurrentRow = rowView;
            this.DataContext = this;
        }

        #region Properties

        public ID ArtikelId
        {
            get => base.GetValue<ID>();
            set => base.SetValue(value);
        }

        public string ArtikelBezeichnung
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public decimal ArtikelPreis
        {
            get => base.GetValue<decimal>();
            set => base.SetValue(value);
        }

        public DataRowView CurrentRow { get; set; }
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ArtikelId = this.CurrentRow.Row.Field<int>("A");
            this.ArtikelBezeichnung = this.CurrentRow.Row.Field<string>("B");
            this.ArtikelPreis = this.CurrentRow.Row.Field<decimal>("C");
        }
        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
