//-----------------------------------------------------------------------
// <copyright file="AppStartWindow.cs" company="Lifeprojects.de">
//     Class: AppStartWindow
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.07.2026</date>
//
// <summary>
// Template für eine neues Window
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Interaktionslogik für AppStartWindow.xaml
    /// </summary>
    public partial class AppStartWindow : WindowBase
    {
        public AppStartWindow()
        {
            this.InitializeComponent();
            WeakEventManager<WindowBase, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            WeakEventManager<WindowBase, CancelEventArgs>.AddHandler(this, "Closing", this.OnWindowClosing);

            this.QuitCommand = new CommandBase(commandParam => this.OnQuit(commandParam), () => true);

            this.SetVectorIcon("IconDatabase_User", 64);

            this.ShowInTaskbar = true;
            this.WindowTitel = "Login Anwendung";
            this.DataContext = this;
        }

        #region Properties
        public CommandBase QuitCommand { get; private set; }

        public string WindowTitel
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

		#endregion Properties
		
        #region WindowEventHandler
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void OnCloseApplication(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
        }
        #endregion WindowEventHandler

        #region Command Events
        private async void OnQuit(object commandParam)
        {
            if (commandParam != null && commandParam.ToString() == "EXIT")
            {
                DialogResult = false;
                this.Close();
            }
            else if (commandParam != null && commandParam.ToString() == "START")
            {
                DialogResult = true;
            }
        }

        #endregion Command Events
    }
}
