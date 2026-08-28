//-----------------------------------------------------------------------
// <copyright file="DataRowCollectionExtensions.cs" company="Lifeprojects.de">
//     Class: DataRowCollectionExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>Extension Class für DataRow</summary>
//-----------------------------------------------------------------------

namespace System.Data
{
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public static class DataRowCollectionExtensions
    {
        /// <summary>
        /// Eine Erweiterungsmethode für DataTable, die die erste Zeile zurück gibt.
        /// </summary>
        /// <param name="this">Übergebene DataTable</param>
        /// <returns>Gibt die erste Row der DataTable zurück</returns>
        public static DataRow FirstRow(this DataRowCollection @this)
        {
            return @this[0];
        }

        /// <summary>Eine Erweiterungsmethode für DataTable, die die letzte Zeile zurück gibt.</summary>
        /// <param name="this">Übergebene DataTable</param>
        /// <returns>Gibt die letzte Row der DataTable zurück</returns>
        public static DataRow LastRow(this DataRowCollection @this)
        {
            return @this[@this.Count - 1];
        }

        /// <summary>Eine Erweiterungsmethode für DataTable, die eine DataRow auf Basis eines Row Index zurück gibt</summary>
        /// <param name="this">Übergebene DataTable</param>
        /// <returns>Gibt eine DataRow auf Basis des Row Index zurück</returns>
        public static DataRow RowByIndex(this DataRowCollection @this, int index)
        {
            return @this[index];
        }
    }
}