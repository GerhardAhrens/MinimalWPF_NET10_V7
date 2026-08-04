//-----------------------------------------------------------------------
// <copyright file="MenuBarSeparator.cs" company="Lifeprojects.de">
//     Class: MenuBarSeparator
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2026</date>
//
// <summary>
// Die Klasse MenuBarSeparator ist ein Steuerelement, das als Trennzeichen in einer Menüleiste verwendet wird.
// </summary>
//-----------------------------------------------------------------------
namespace System.Windows.Controls
{
    public class MenuBarSeparator : Control
    {
        static MenuBarSeparator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuBarSeparator), new FrameworkPropertyMetadata(typeof(MenuBarSeparator)));
        }

        #region Brush

        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register(
                nameof(Brush),
                typeof(Brush),
                typeof(MenuBarSeparator),
                new PropertyMetadata(Brushes.Gray));

        public Brush Brush
        {
            get => (Brush)GetValue(BrushProperty);
            set => SetValue(BrushProperty, value);
        }

        #endregion

        #region Thickness

        public static readonly DependencyProperty LineThicknessProperty =
            DependencyProperty.Register(
                nameof(LineThickness),
                typeof(double),
                typeof(MenuBarSeparator),
                new PropertyMetadata(1.0));

        public double LineThickness
        {
            get => (double)GetValue(LineThicknessProperty);
            set => SetValue(LineThicknessProperty, value);
        }

        #endregion
    }
}
