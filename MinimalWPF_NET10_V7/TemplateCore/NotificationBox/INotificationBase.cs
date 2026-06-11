//-----------------------------------------------------------------------
// <copyright file="INotificationBase.cs" company="Lifeprojects.de">
//     Class: INotificationBase
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.03.2026 18:21:36</date>
//
// <summary>
// Interface zur MassageBox Base
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{
    public interface INotificationBase
    {
        MessageBoxResult Show(string messageBoxText);
        MessageBoxResult Show(string messageBoxText, string caption);
        MessageBoxResult Show(Window owner, string messageBoxText);
        MessageBoxResult Show(Window owner, string messageBoxText, string caption);
        MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button);
        MessageBoxResult Show(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
        MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText);
        MessageBoxResult ShowOK(string messageBoxText, string caption, string okButtonText, MessageBoxImage icon);
        MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText);
        MessageBoxResult ShowOKCancel(string messageBoxText, string caption, string okButtonText, string cancelButtonText, MessageBoxImage icon);
        MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText);
        MessageBoxResult ShowYesNo(string messageBoxText, string caption, string yesButtonText, string noButtonText, MessageBoxImage icon);
        MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText);
        MessageBoxResult ShowYesNoCancel(string messageBoxText, string caption, string yesButtonText, string noButtonText, string cancelButtonText, MessageBoxImage icon);
    }
}