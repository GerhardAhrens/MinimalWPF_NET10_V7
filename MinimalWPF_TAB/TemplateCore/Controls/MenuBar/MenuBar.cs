//-----------------------------------------------------------------------
// <copyright file="MenuBar.cs" company="Lifeprojects.de">
//     Class: MenuBar
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.08.2026</date>
//
// <summary>
// Die Klasse stellt einen Container für Menüs dar, der in einer WPF-Anwendung verwendet werden kann. Sie erbt von ItemsControl
// und bietet zusätzliche Eigenschaften wie ShowHeader, Header, CornerRadius, ShowBorder, ButtonSpacing, ItemPadding und ItemMinWidth.
// Außerdem definiert sie eine angehängte Eigenschaft Dock, die die Ausrichtung der Menüs innerhalb des Containers steuert.
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Controls
{
    public class MenuBar : ItemsControl
    {
        static MenuBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MenuBar), new FrameworkPropertyMetadata(typeof(MenuBar)));
        }

        #region ShowHeader
        public static readonly DependencyProperty ShowHeaderProperty =
            DependencyProperty.Register(
                nameof(ShowHeader),
                typeof(bool),
                typeof(MenuBar),
                new PropertyMetadata(true));

        public bool ShowHeader
        {
            get => (bool)GetValue(ShowHeaderProperty);
            set => SetValue(ShowHeaderProperty, value);
        }
        #endregion ShowHeader

        #region Header

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(string),
                typeof(MenuBar),
                new PropertyMetadata(string.Empty));

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        #endregion

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(MenuBar),
                new PropertyMetadata(new CornerRadius(4)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region ShowBorder

        public static readonly DependencyProperty ShowBorderProperty =
            DependencyProperty.Register(
                nameof(ShowBorder),
                typeof(bool),
                typeof(MenuBar),
                new PropertyMetadata(true));

        public bool ShowBorder
        {
            get => (bool)GetValue(ShowBorderProperty);
            set => SetValue(ShowBorderProperty, value);
        }

        #endregion

        #region ButtonSpacing

        public static readonly DependencyProperty ButtonSpacingProperty =
            DependencyProperty.Register(
                nameof(ButtonSpacing),
                typeof(double),
                typeof(MenuBar),
                new FrameworkPropertyMetadata(
                    4.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure |
                    FrameworkPropertyMetadataOptions.AffectsArrange));

        public double ButtonSpacing
        {
            get => (double)GetValue(ButtonSpacingProperty);
            set => SetValue(ButtonSpacingProperty, value);
        }

        #endregion

        #region ItemPadding

        public static readonly DependencyProperty ItemPaddingProperty =
            DependencyProperty.Register(
                nameof(ItemPadding),
                typeof(Thickness),
                typeof(MenuBar),
                new PropertyMetadata(new Thickness(8, 2, 8, 2)));

        public Thickness ItemPadding
        {
            get => (Thickness)GetValue(ItemPaddingProperty);
            set => SetValue(ItemPaddingProperty, value);
        }

        #endregion

        #region ItemMinWidth

        public static readonly DependencyProperty ItemMinWidthProperty =
            DependencyProperty.Register(
                nameof(ItemMinWidth),
                typeof(double),
                typeof(MenuBar),
                new PropertyMetadata(50.0));

        public double ItemMinWidth
        {
            get => (double)GetValue(ItemMinWidthProperty);
            set => SetValue(ItemMinWidthProperty, value);
        }

        #endregion

        #region Attached Dock

        public static readonly DependencyProperty DockProperty =
            DependencyProperty.RegisterAttached(
                "Dock",
                typeof(MenuBarDock),
                typeof(MenuBar),
                new FrameworkPropertyMetadata(
                    MenuBarDock.Left,
                    FrameworkPropertyMetadataOptions.AffectsParentArrange |
                    FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        public static void SetDock(
            DependencyObject obj,
            MenuBarDock value)
        {
            obj.SetValue(DockProperty, value);
        }

        public static MenuBarDock GetDock(
            DependencyObject obj)
        {
            return (MenuBarDock)obj.GetValue(DockProperty);
        }

        #endregion
    }
}
