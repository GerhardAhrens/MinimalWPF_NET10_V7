//-----------------------------------------------------------------------
// <copyright file="StatusInfoBarItem .cs" company="Lifeprojects.de">
//     Class: StatusInfoBarItem 
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
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Repräsentiert einen Bereich innerhalb der <see cref="StatusInfoBar"/>.
    /// </summary>
    public class StatusInfoBarItem : StatusBarItem
    {
        #region Constructor

        public StatusInfoBarItem(StatusItemType itemType)
        {
            ItemType = itemType;

            Background = StatusInfoBarTheme.Background;
            Foreground = StatusInfoBarTheme.Foreground;

            Margin = StatusInfoBarTheme.ItemMargin;
            Padding = StatusInfoBarTheme.ItemPadding;

            HorizontalContentAlignment = HorizontalAlignment.Left;
            VerticalContentAlignment = VerticalAlignment.Center;

            Visibility = Visibility.Visible;

            CommandParameter = this;
        }

        #endregion

        #region ItemType

        /// <summary>
        /// Typ des Statusfeldes.
        /// </summary>
        public StatusItemType ItemType
        {
            get;
        }

        #endregion

        #region Text

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(StatusInfoBarItem),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Anzuzeigender Text.
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        #endregion

        #region Image

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register(
                nameof(Image),
                typeof(ImageSource),
                typeof(StatusInfoBarItem),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Anzuzeigendes Symbol.
        /// </summary>
        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        #endregion

        #region Command

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(
                nameof(Command),
                typeof(ICommand),
                typeof(StatusInfoBarItem),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Optionaler Command.
        /// </summary>
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        #endregion

        #region CommandParameter

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(
                nameof(CommandParameter),
                typeof(object),
                typeof(StatusInfoBarItem),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Parameter für den Command.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion

        #region Convenience

        /// <summary>
        /// Gibt an, ob ein Command vorhanden ist.
        /// </summary>
        public bool IsClickable => Command != null;

        /// <summary>
        /// Setzt das Item auf die Standarddarstellung zurück.
        /// </summary>
        public virtual void Reset()
        {
            Background = StatusInfoBarTheme.Background;
            Foreground = StatusInfoBarTheme.Foreground;

            Visibility = Visibility.Visible;

            ToolTip = null;
        }

        /// <summary>
        /// Setzt Text und Bild in einem Aufruf.
        /// </summary>
        public void SetContent(
            string text,
            ImageSource image)
        {
            Text = text;
            Image = image;
        }

        /// <summary>
        /// Setzt Text, Bild und Tooltip.
        /// </summary>
        public void SetContent(
            string text,
            ImageSource image,
            string toolTip)
        {
            Text = text;
            Image = image;
            ToolTip = toolTip;
        }

        /// <summary>
        /// Setzt Vorder- und Hintergrundfarbe.
        /// </summary>
        public void SetColors(
            Brush foreground,
            Brush background)
        {
            Foreground = foreground;
            Background = background;
        }

        /// <summary>
        /// Setzt die Sichtbarkeit.
        /// </summary>
        public void Show(bool visible)
        {
            Visibility = visible
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        #endregion
    }
}
