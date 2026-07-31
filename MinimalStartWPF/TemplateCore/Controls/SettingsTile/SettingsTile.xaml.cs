//-----------------------------------------------------------------------
// <copyright file="SettingsTile.cs" company="Lifeprojects.de">
//     Class: SettingsTile
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>22.07.2026</date>
//
// <summary>
// Template für eine neues Standard UserControl
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaktionslogik für SettingsTile.xaml
    /// </summary>
    public partial class SettingsTile : UserControlBase
    {
        public SettingsTile() : base(typeof(SettingsTile))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);

            this.DataContext = this;
        }

        #region Properties
        #endregion Properties

        #region Windows Events

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion Windows Events

        #region Command Events
        #endregion Command Events

    }
}
