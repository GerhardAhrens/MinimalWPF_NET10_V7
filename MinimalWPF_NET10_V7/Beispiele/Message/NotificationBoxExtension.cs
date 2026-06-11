//-----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Lifeprojects.de">
//     Class: MainWindow
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.03.2026 18:21:36</date>
//
// <summary>
// WPF Template mit Minimalfunktionen
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{

    public static class NotificationBoxExtension
    {
        public static MessageBoxResult Hinweis(this INotificationBase self, string titel)
        {
            MessageBoxResult result = self.Show(titel);
            return result;
        }
    }
}