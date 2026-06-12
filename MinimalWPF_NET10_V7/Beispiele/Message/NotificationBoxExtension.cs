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
        public static MessageBoxResult Hinweis(this INotificationBase self, string message)
        {
            MessageBoxResult result = self.Show(message);
            return result;
        }

        public static MessageBoxResult Hinweis(this INotificationBase self, string titel, string message)
        {
            MessageBoxResult result = self.Show(titel, message);
            return result;
        }

        public static MessageBoxResult StopMessage(this INotificationBase self, string titel, string message)
        {
            MessageBoxResult result = self.Show(titel, message, MessageBoxButton.OK, MessageBoxImage.Stop);
            return result;
        }

        public static MessageBoxResult Information(this INotificationBase self, string titel, string message)
        {
            MessageBoxResult result = self.Show(titel, message, MessageBoxButton.OK, MessageBoxImage.Information);
            return result;
        }

        public static MessageBoxResult Question(this INotificationBase self, string titel, string message)
        {
            MessageBoxResult result = self.ShowYesNo(titel, message,"Ja","Nein");
            return result;
        }

        public static MessageBoxResult QuestionCancel(this INotificationBase self, string titel, string message)
        {
            MessageBoxResult result = self.Show(titel, message, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            return result;
        }

        public static MessageBoxResult QuestionAbortRetryIgnore(this INotificationBase self, string titel, string message)
        {
            MessageBoxResult result = self.ShowAbortRetryIgnore(titel, message, "_Abrechen","_Wiederholen","_Ignoriren", MessageBoxImage.Question);
            return result;
        }

        public static MessageBoxResult Warning(this INotificationBase self, string titel, string message)
        {
            MessageBoxResult result = self.Show(titel, message, MessageBoxButton.OK, MessageBoxImage.Warning);
            return result;
        }

    }
}