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

        public DataRowView CurrentRow
        {
            get => base.GetValue<DataRowView>();
            set => base.SetValue(value);
        }

        #endregion Properties

        #region Windows Events

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
