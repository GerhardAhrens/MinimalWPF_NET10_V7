//-----------------------------------------------------------------------
// <copyright file="AdvancedTabItem .cs" company="Lifeprojects.de">
//     Class: AdvancedTabItem 
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>2026 - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.08.2026</date>
//
// <summary>
// Die Klasse stellt eine erweiterte Version des TabItem-Steuerelements in WPF dar. Sie bietet zusätzliche
// Eigenschaften wie HeaderBackground, HeaderForeground, HeaderImage, CloseCommand und CloseCommandParameter,
// um die Darstellung und Funktionalität von Tab-Elementen zu verbessern. Diese Klasse kann in einem TabControl
// verwendet werden, um benutzerdefinierte Registerkarten mit erweiterten Funktionen zu erstellen.
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;

    public class AdvancedTabItem : TabItem
    {
        static AdvancedTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTabItem), new FrameworkPropertyMetadata(typeof(AdvancedTabItem)));
        }

        #region HeaderBackground

        public static readonly DependencyProperty HeaderBackgroundProperty =
            DependencyProperty.Register(
                nameof(HeaderBackground),
                typeof(Brush),
                typeof(AdvancedTabItem),
                new PropertyMetadata(SystemColors.ControlBrush));

        public Brush HeaderBackground
        {
            get => (Brush)GetValue(HeaderBackgroundProperty);
            set => SetValue(HeaderBackgroundProperty, value);
        }

        #endregion

        #region HeaderForeground

        public static readonly DependencyProperty HeaderForegroundProperty =
            DependencyProperty.Register(
                nameof(HeaderForeground),
                typeof(Brush),
                typeof(AdvancedTabItem),
                new PropertyMetadata(SystemColors.ControlTextBrush));

        public Brush HeaderForeground
        {
            get => (Brush)GetValue(HeaderForegroundProperty);
            set => SetValue(HeaderForegroundProperty, value);
        }

        #endregion

        #region HeaderImage

        public static readonly DependencyProperty HeaderImageProperty =
            DependencyProperty.Register(
                nameof(HeaderImage),
                typeof(ImageSource),
                typeof(AdvancedTabItem),
                new PropertyMetadata(null));

        public ImageSource HeaderImage
        {
            get => (ImageSource)GetValue(HeaderImageProperty);
            set => SetValue(HeaderImageProperty, value);
        }

        #endregion

        #region CloseCommand

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(
                nameof(CloseCommand),
                typeof(ICommand),
                typeof(AdvancedTabItem),
                new PropertyMetadata(null));

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }

        #endregion

        #region CloseCommandParameter

        public static readonly DependencyProperty CloseCommandParameterProperty =
            DependencyProperty.Register(
                nameof(CloseCommandParameter),
                typeof(object),
                typeof(AdvancedTabItem),
                new PropertyMetadata(null));

        public object CloseCommandParameter
        {
            get => GetValue(CloseCommandParameterProperty);
            set => SetValue(CloseCommandParameterProperty, value);
        }

        #endregion CloseCommandParameter
    }
}
