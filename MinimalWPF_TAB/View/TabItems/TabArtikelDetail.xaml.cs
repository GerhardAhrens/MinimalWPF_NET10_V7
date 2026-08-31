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
        public TabArtikelDetail(DataRow rowView) : base(typeof(TabArtikelDetail))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
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
            if (this.CurrentRow.RowState == DataRowState.Added)
            {
                this.CurrentRow.SetField("A", 0);
                this.CurrentRow.SetField("B", string.Empty);
                this.CurrentRow.SetField("C", 0.0m);
            }

            this.DataContext = this;
        }

        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
