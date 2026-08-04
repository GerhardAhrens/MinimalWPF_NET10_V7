//-----------------------------------------------------------------------
// <copyright file="StatusItemType.cs" company="Lifeprojects.de">
//     Class: StatusItemType
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2026</date>
//
// <summary>
// Die Klasse StatusItemType ist eine Enumeration, die verschiedene Typen von Status-Elementen in einer Statusleiste definiert.
// Sie wird verwendet, um den Typ eines Status-Elements zu kennzeichnen, z.B. Konto, Datenquelle, Rechte, Benachrichtigung oder Datum.
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Controls
{
    public enum StatusItemType
    {
        Account,
        Datasource,
        Rights,
        Notification,
        Date
    }
}
