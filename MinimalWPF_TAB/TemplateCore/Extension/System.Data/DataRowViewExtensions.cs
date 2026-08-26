// <copyright file="DataRowViewExtensions.cs" company="Lifeprojects.de">
//     Class: DataRowViewExtensions
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>29.09.2020</date>
//
// <summary>Extension Class für DataRowView</summary>
//-----------------------------------------------------------------------

namespace System.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Versioning;

    /// <summary>
    /// Extension methods for ADO.NET DataRowView (DataView / DataTable / DataSet)
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class DataRowViewExtensions
    {
        /// <summary>
        /// Gibt eine Column von einem DataRow im gewünschten Typ zurück
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this">Alltuelle DataRow Zeile</param>
        /// <param name="fieldName">Column</param>
        /// <returns>Ergebnis zur angegebenen Column</returns>
        public static TResult GetAs<TResult>(this DataRowView @this, string fieldName)
        {
            try
            {
                object getAs = null;
                if (typeof(TResult).Name == "Guid")
                {
                    getAs = @this[fieldName] == DBNull.Value ? Guid.Empty : new Guid(@this[fieldName].ToString());
                }
                else if (typeof(TResult).IsEnum == true)
                {
                    if (@this[fieldName].GetType() == typeof(int))
                    {
                        getAs = (TResult)@this[fieldName];
                    }
                    else if (@this[fieldName].GetType() == typeof(string))
                    {
                        getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                    }
                    else
                    {
                        getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                    }
                }
                else
                {
                    getAs = @this[fieldName] == DBNull.Value ? default(TResult) : (TResult)Convert.ChangeType(@this[fieldName], typeof(TResult), CultureInfo.InvariantCulture);
                }

                return (TResult)getAs;
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                return default(TResult);
            }
        }

        /// <summary>
        /// Gibt eine Column von einem DataRow im gewünschten Typ zurück, mit der möglichkeit einen Default-Wert anzugeben
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="this">Alltuelle DataRow Zeile</param>
        /// <param name="fieldName">Column</param>
        /// <param name="defaultValue">Default-Wert</param>
        /// <returns>Ergebnis zur angegebenen Column</returns>
        public static TResult GetAs<TResult>(this DataRowView @this, string fieldName, TResult defaultValue)
        {
            try
            {
                object getAs = null;
                if (@this[fieldName] != DBNull.Value)
                {
                    if (typeof(TResult).Name == "Guid")
                    {
                        getAs = @this[fieldName] == DBNull.Value ? Guid.Empty : new Guid(@this[fieldName].ToString());
                    }
                    else if (typeof(TResult).IsEnum == true)
                    {
                        if (@this[fieldName].GetType() == typeof(int))
                        {
                            getAs = (TResult)@this[fieldName];
                        }
                        else if (@this[fieldName].GetType() == typeof(string))
                        {
                            getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                        }
                        else
                        {
                            getAs = (TResult)Enum.Parse(typeof(TResult), @this[fieldName].ToString(), true);
                        }
                    }
                    else
                    {
                        getAs = @this[fieldName] == DBNull.Value ? default(TResult) : (TResult)Convert.ChangeType(@this[fieldName], typeof(TResult), CultureInfo.InvariantCulture);
                    }

                    return (TResult)getAs;
                }
                else
                {
                    return defaultValue;
                }
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                return default(TResult);
            }
        }

        /// <summary>
        /// Gets the record value casted as byte array.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static byte[] GetBytes(this DataRowView @this, string field)
        {
            return (@this[field] as byte[]);
        }


        /// <summary>
        /// Gets the record value casted as Guid or Guid.Empty.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static Guid GetGuid(this DataRowView @this, string field)
        {
            var value = @this[field];
            return (value is Guid ? (Guid)value : Guid.Empty);
        }

        /// <summary>
        /// Gets the record value casted as DateTime or DateTime.MinValue.
        /// </summary>
        /// <param name = "@this">The data @this.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static DateTime GetDateTime(this DataRowView @this, string field)
        {
            return @this.GetDateTime(field, DateTime.MinValue);
        }

        /// <summary>
        /// Gets the record value casted as DateTime or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static DateTime GetDateTime(this DataRowView @this, string field, DateTime defaultValue)
        {
            var value = @this[field];
            return (value is DateTime ? (DateTime)value : defaultValue);
        }

        /// <summary>
        /// Gets the record value casted as DateTimeOffset (UTC) or DateTime.MinValue.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>The record value</returns>
        public static DateTimeOffset GetDateTimeOffset(this DataRowView @this, string field)
        {
            return new DateTimeOffset(@this.GetDateTime(field), TimeSpan.Zero);
        }

        /// <summary>
        /// Gets the record value casted as DateTimeOffset (UTC) or the specified default value.
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <param name = "defaultValue">The default value.</param>
        /// <returns>The record value</returns>
        public static DateTimeOffset GetDateTimeOffset(this DataRowView @this, string field, DateTimeOffset defaultValue)
        {
            var dt = @this.GetDateTime(field);
            return (dt != DateTime.MinValue ? new DateTimeOffset(dt, TimeSpan.Zero) : defaultValue);
        }

        /// <summary>
        /// Determines whether the record value is DBNull.Value
        /// </summary>
        /// <param name = "@this">The data row.</param>
        /// <param name = "field">The name of the record field.</param>
        /// <returns>
        /// 	<c>true</c> if the value is DBNull.Value; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDBNull(this DataRowView @this, string field)
        {
            var value = @this[field];
            return (value == DBNull.Value);
        }

        public static void AddRange(this DataRowCollection rc, IEnumerable<object[]> tuples)
        {
            foreach (object[] data in tuples)
            {
                rc.Add(tuples);
            }
        }
    }
}
