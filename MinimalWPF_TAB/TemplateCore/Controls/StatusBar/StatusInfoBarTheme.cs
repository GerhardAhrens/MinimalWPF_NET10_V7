//-----------------------------------------------------------------------
// <copyright file="StatusInfoBarTheme.cs" company="Lifeprojects.de">
//     Class: StatusInfoBarTheme
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.08.2026</date>
//
// <summary>
// Template für eine neue C# Standard-Klasse
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Controls
{
    using System.Windows.Media;

    public static class StatusInfoBarTheme
    {
        public static Brush Background { get; } = Brushes.LightGray;

        public static Brush Foreground { get; } = Brushes.Black;

        public static Brush BorderBrush { get; } = Brushes.DarkGray;

        public static Brush SeparatorBrush { get; } = Brushes.DarkGray;

        public static Thickness ItemMargin { get; } = new Thickness(4, 0, 4, 0);

        public static Thickness SeparatorThickness { get; } =  new Thickness(1, 0, 0, 0);

        public static Thickness ItemPadding { get; } = new Thickness(3, 1, 3, 1);

        public static Thickness ItemBorderPadding => new Thickness(4, 2, 4, 2);

        public const double IconSize = 16;
    }
}
