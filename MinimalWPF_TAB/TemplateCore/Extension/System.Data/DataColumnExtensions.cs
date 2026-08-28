//-----------------------------------------------------------------------
// <copyright file="DataColumnExtensions.cs" company="Lifeprojects.de">
//     Class: DataColumnExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>
// Extension Class für DataColumn und DataColumnCollection
// </summary>
//-----------------------------------------------------------------------

namespace System.Data
{
    using System.Linq;

    public static class DataColumnExtensions
    {
        /// <summary>
        /// Die Methode prüft, ob eine Column Nummerisch ist.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsNumeric(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            var numericTypes = new[] { typeof(Byte), typeof(Decimal), typeof(Double),
           typeof(Int16), typeof(Int32), typeof(Int64), typeof(SByte),
           typeof(Single), typeof(UInt16), typeof(UInt32), typeof(UInt64)};

            return numericTypes.Contains(col.DataType);
        }

        /// <summary>
        /// Die Methode prüft, ob eine Column ein Boolean ist.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsBool(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            var boolTypes = new[] { typeof(bool), typeof(bool?)};

            return boolTypes.Contains(col.DataType);
        }

        /// <summary>
        /// Die Methode prüft, ob eine Column ein String ist.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsString(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            var stringTypes = new[] { typeof(string), typeof(char), typeof(char) };

            return stringTypes.Contains(col.DataType);
        }

        /// <summary>
        /// Die Methode prüft, ob eine Column ein DateTime ist.
        /// </summary>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool IsDateTime(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }


            var stringTypes = new[] { typeof(DateTime), typeof(DateTime?) };

            return stringTypes.Contains(col.DataType);
        }

        /// <summary>
        /// Die Methode gibt den Datentyp einer Column zurück
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="ColumnName">Name der Column</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Type GetColumnDataType(this DataTable tbl, string ColumnName)
        {
            try
            {
                return tbl.Columns[ColumnName].DataType;
            }
            catch (Exception ex)
            {
                throw new Exception($"GetColumnDataType: {ex.Message}");
            }
        }

        /// <summary>
        /// Die Methode gibt den Datentyp einer Column zurück
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="ColumnIndex">Index der Column</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Type GetColumnDataType(this DataTable tbl, int ColumnIndex)
        {
            try
            {
                return tbl.Columns[ColumnIndex].DataType;
            }
            catch (Exception ex)
            {
                throw new Exception("GetColumnDataType: \n" + ex.Message);
            }
        }

        /// <summary>
        /// Die Methode gibt den Wert einer Row, einer Spalte zurück
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbl"></param>
        /// <param name="ColInd">Index der Column</param>
        /// <param name="RowInd">Index der Row</param>
        /// <returns></returns>
        public static T GetColumnValue<T>(this DataTable tbl, int ColInd, int RowInd)
        {
            try
            {
                object column = tbl.Rows[RowInd][ColInd];
                return column == DBNull.Value ? default(T) : (T)Convert.ChangeType(column, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Die Methode gibt den Wert einer Row, für einen Spaltennamen zurück
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbl"></param>
        /// <param name="ColumnName">Name der Spalte</param>
        /// <param name="RowInd">Index der Row</param>
        /// <returns></returns>
        public static T GetColumnValue<T>(this DataTable tbl, string ColumnName, int RowInd)
        {
            try
            {
                object column = tbl.Rows[RowInd][ColumnName];
                return column == DBNull.Value ? default(T) : (T)Convert.ChangeType(column, typeof(T));

            }
            catch
            {
                return default(T);
            }
        }
    }
}