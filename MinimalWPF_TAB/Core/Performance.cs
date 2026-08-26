//-----------------------------------------------------------------------
// <copyright file="ArtikellisteUC.cs" company="Lifeprojects.de">
//     Class: ArtikellisteUC
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.Diagnostics;

    public static class Performance
    {
        /// <summary>
        /// Zeit der ausführung einer Aktion messen
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <example>
        /// TimeSpan t = Performance.Measure(() =>
        /// {
        ///     Thread.Sleep(500);
        /// });
        /// 
        /// Console.WriteLine(t.TotalMilliseconds);
        /// </example>
        public static TimeSpan Measure(Action action)
        {
            var sw = Stopwatch.StartNew();

            action();

            sw.Stop();

            return sw.Elapsed;
        }

        /// <summary>
        /// Zeit der ausführung einer Funktion messen und das Ergebnis zurückgeben
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        /// <example>
        /// int sum = Performance.Measure(() =>
        /// {
        ///      return Enumerable.Range(1, 1000000).Sum();
        /// }, out TimeSpan duration);
        /// 
        /// Console.WriteLine(sum);
        /// Console.WriteLine(duration.TotalMilliseconds);
        /// </example>
        public static T Measure<T>(Func<T> func, out TimeSpan duration)
        {
            var sw = Stopwatch.StartNew();

            T result = func();

            sw.Stop();

            duration = sw.Elapsed;

            return result;
        }
    }
}
