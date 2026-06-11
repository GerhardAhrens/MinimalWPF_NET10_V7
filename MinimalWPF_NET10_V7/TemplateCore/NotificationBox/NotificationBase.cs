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

        public MessageBoxResult Show(Window owner, string messageBoxText, string caption)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Owner = owner
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = button
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = button,
                Image = icon
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.YesNoCancel,
                OkButtonCaption = okButtonText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText, MessageBoxImage icon)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.OK,
                Image = icon,
                OkButtonCaption = okButtonText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.OKCancel,
                OkButtonCaption = okButtonText,
                CancelButtonCaption = cancelButtonText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText, MessageBoxImage icon)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.OKCancel,
                Image = icon,
                OkButtonCaption = okButtonText,
                CancelButtonCaption = cancelButtonText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.YesNo,
                YesButtonCaption = yesButtonText,
                NoButtonCaption = noButtonText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText, MessageBoxImage icon)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.YesNo,
                Image = icon,
                YesButtonCaption = yesButtonText,
                NoButtonCaption = noButtonText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.YesNoCancel,
                YesButtonCaption = yesButtonText,
                NoButtonCaption = noButtonText,
                CancelButtonCaption = cancelButtonText
            };

            return msgData.ShowMessageBox();
        }

        public MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText, MessageBoxImage icon)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = MessageBoxButton.YesNoCancel,
                Image = icon,
                YesButtonCaption = yesButtonText,
                NoButtonCaption = noButtonText,
                CancelButtonCaption = cancelButtonText
            };

            return msgData.ShowMessageBox();
        }
    }
}
 