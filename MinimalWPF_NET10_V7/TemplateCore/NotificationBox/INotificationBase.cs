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
    }
}