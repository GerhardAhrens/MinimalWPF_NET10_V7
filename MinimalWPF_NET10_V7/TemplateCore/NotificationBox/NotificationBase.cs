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
// Message Basis zur Kapselung der MessageBox Funktionalität
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    using System.Diagnostics;
    using System.Runtime.Versioning;

    [DebuggerStepThrough]
    [Serializable]
    [SupportedOSPlatform("windows")]
    public class NotificationBase : INotificationBase
    {
        public MessageBoxResult Show(string messageBoxText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult Show(string messageBoxText, string caption)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult Show(Window owner, string messageBoxText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Owner = owner
            };

            return msgData.ShowMessageBox();
        }
    }
}
 