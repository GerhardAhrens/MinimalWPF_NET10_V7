
namespace System.Windows
{
    using System.Diagnostics;
    using System.Runtime.Versioning;

    [DebuggerStepThrough]
    [Serializable]
    [SupportedOSPlatform("windows")]
    public class NotificationBox
    {
        /// <summary>
        /// Zeigt ein Meldungsfeld mit einer Meldung an und gibt ein Ergebnis zurück.
        /// </summary>
        /// <param name="messageBoxText">Ein System.String, der den anzuzeigenden Text angibt.</param>
        /// <returns>Ein Wert vom Typ „System.Windows.MessageBoxResult“, der angibt, welche Schaltfläche im Meldungsfeld vom Benutzer angeklickt wurde.</returns>
        public static MessageBoxResult Show(string messageBoxText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText
            };

            return msgData.ShowMessageBox();
        }

        /// <summary>
        /// Zeigt ein Meldungsfeld an, das eine Meldung und eine Titelzeile enthält und ein Ergebnis zurückgibt.
        /// </summary>
        /// <param name="messageBoxText">Ein System.String, der den anzuzeigenden Text angibt.</param>
        /// <param name="caption">Ein System.String, der die anzuzeigende Beschriftung der Titelleiste angibt.</param>
        /// <returns>Ein Wert vom Typ „System.Windows.MessageBoxResult“, der angibt, welche Schaltfläche im Meldungsfeld vom Benutzer angeklickt wurde.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption
            };

            return msgData.ShowMessageBox();
        }

        /// <summary>
        /// Zeigt vor dem angegebenen Fenster ein Meldungsfeld an. Das Meldungsfeld zeigt eine Meldung an und gibt ein Ergebnis zurück.
        /// </summary>
        /// <param name="owner">A System.Windows.Window that represents the owner window of the message box.</param>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Owner = owner
            };

            return msgData.ShowMessageBox();
        }

        /// <summary>
        /// Displays a message box in front of the specified window. The message box displays a message and title bar caption; and it returns a result.
        /// </summary>
        /// <param name="owner">A System.Windows.Window that represents the owner window of the message box.</param>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(Window owner, string messageBoxText, string caption)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Owner = owner
            };

            return msgData.ShowMessageBox();
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, and button; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="button">A System.Windows.MessageBoxButton value that specifies which button or buttons to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button)
        {
            var msgData = new MessageBoxData()
            {
                Message = messageBoxText,
                Caption = caption,
                Buttons = button
            };

            return msgData.ShowMessageBox();
        }

        /// <summary>
        /// Displays a message box that has a message, title bar caption, button, and icon; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="button">A System.Windows.MessageBoxButton value that specifies which button or buttons to display.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
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

        /// <summary>
        /// Displays a message box that has a message, title bar caption, and OK button with a custom System.String value for the button's text; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText)
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

        /// <summary>
        /// Displays a message box that has a message, title bar caption, OK button with a custom System.String value for the button's text, and icon; and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText, MessageBoxImage icon)
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

        /// <summary>
        /// Displays a message box that has a message, caption, and OK/Cancel buttons with custom System.String values for the buttons' text;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText)
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

        /// <summary>
        /// Displays a message box that has a message, caption, OK/Cancel buttons with custom System.String values for the buttons' text, and icon;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="okButtonText">A System.String that specifies the text to display within the OK button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText, MessageBoxImage icon)
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

        /// <summary>
        /// Displays a message box that has a message, caption, and Yes/No buttons with custom System.String values for the buttons' text;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText)
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

        /// <summary>
        /// Displays a message box that has a message, caption, Yes/No buttons with custom System.String values for the buttons' text, and icon;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText, MessageBoxImage icon)
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

        /// <summary>
        /// Displays a message box that has a message, caption, and Yes/No/Cancel buttons with custom System.String values for the buttons' text;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText)
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

        /// <summary>
        /// Displays a message box that has a message, caption, Yes/No/Cancel buttons with custom System.String values for the buttons' text, and icon;
        /// and that returns a result.
        /// </summary>
        /// <param name="messageBoxText">A System.String that specifies the text to display.</param>
        /// <param name="caption">A System.String that specifies the title bar caption to display.</param>
        /// <param name="yesButtonText">A System.String that specifies the text to display within the Yes button.</param>
        /// <param name="noButtonText">A System.String that specifies the text to display within the No button.</param>
        /// <param name="cancelButtonText">A System.String that specifies the text to display within the Cancel button.</param>
        /// <param name="icon">A System.Windows.MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A System.Windows.MessageBoxResult value that specifies which message box button is clicked by the user.</returns>
        public static MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText, MessageBoxImage icon)
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
