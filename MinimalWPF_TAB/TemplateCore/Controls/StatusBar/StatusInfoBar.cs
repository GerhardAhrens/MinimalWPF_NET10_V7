//-----------------------------------------------------------------------
// <copyright file="StatusInfoBar .cs" company="Lifeprojects.de">
//     Class: StatusInfoBar 
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
    using System.Windows;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Statusleiste mit fünf festen Bereichen.
    /// </summary>
    /// <summary>
    /// Statusleiste mit fünf fest definierten Bereichen.
    /// </summary>
    public class StatusInfoBar : UserControl
    {
        #region Constructor

        public StatusInfoBar()
        {
            Background = StatusInfoBarTheme.Background;
            Foreground = StatusInfoBarTheme.Foreground;

            Account = CreateStatusItem(
                StatusItemType.Account,
                "Gast",
                "Nicht angemeldet",
                StatusInfoBarImages.Account);

            Datasource = CreateStatusItem(
                StatusItemType.Datasource,
                "Keine Datenquelle",
                "Datasource",
                StatusInfoBarImages.Database);

            Rights = CreateStatusItem(
                StatusItemType.Rights,
                "Keine Rechte",
                "Benutzerrechte",
                StatusInfoBarImages.Shield);

            Notification = CreateStatusItem(
                StatusItemType.Notification,
                "Bereit",
                "Status",
                StatusInfoBarImages.Notification);

            Date = CreateStatusItem(
                StatusItemType.Date,
                System.DateTime.Now.ToString("dd.MM.yyyy HH:mm"),
                "Datum",
                StatusInfoBarImages.Calendar);

            Content = CreateLayout();
        }

        #endregion

        #region Properties

        public StatusInfoBarItem Account { get; }

        public StatusInfoBarItem Datasource { get; }

        public StatusInfoBarItem Rights { get; }

        public StatusInfoBarItem Notification { get; }

        public StatusInfoBarItem Date { get; }

        #endregion

        #region Layout
        private UIElement CreateLayout()
        {
            Grid grid = new();

            grid.Background = Background;

            grid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(
                        1,
                        GridUnitType.Star)
                });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });

            AddItem(grid, Account, 0);
            AddItem(grid, Datasource, 1);
            AddItem(grid, Rights, 2);
            AddItem(grid, Notification, 3);
            AddItem(grid, Date, 4);

            return grid;
        }

        private void AddItem(Grid grid, StatusInfoBarItem item, int column)
        {
            Grid.SetColumn(item, column);

            grid.Children.Add(item);
        }
        #endregion Layout

        #region CreateStatusItem

        private StatusInfoBarItem CreateStatusItem(StatusItemType type, string text, string toolTip, ImageSource image)
        {
            StatusInfoBarItem item = new(type)
            {
                Text = text,
                ToolTip = toolTip,
                Image = image
            };

            item.Content = CreateBorder(item);

            if (type == StatusItemType.Notification)
            {
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                item.Width = double.NaN;
            }

            return item;
        }

        private UIElement CreateBorder(StatusInfoBarItem item)
        {
            Border border = new()
            {
                Padding = StatusInfoBarTheme.ItemBorderPadding,
                BorderBrush = StatusInfoBarTheme.SeparatorBrush,
                BorderThickness = item.ItemType == StatusItemType.Account ? new Thickness(0) : StatusInfoBarTheme.SeparatorThickness
            };

            border.Child = CreateContent(item);

            return border;
        }
        #endregion

        #region CreateContent

        private FrameworkElement CreateContent(StatusInfoBarItem item)
        {
            Button button = new Button
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(0),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Center,
            };

            button.Focusable = false;
            button.Background = Brushes.Transparent;
            button.BorderBrush = Brushes.Transparent;

            button.SetBinding(Button.CommandProperty, new Binding(nameof(StatusInfoBarItem.Command))
                {
                    Source = item
                });

            button.SetBinding(Button.CommandParameterProperty, new Binding(nameof(StatusInfoBarItem.CommandParameter))
                {
                    Source = item
                });

            Grid grid = new();

            grid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });

            grid.ColumnDefinitions.Add(
                new ColumnDefinition()
                {
                    Width = new GridLength(
                        1,
                        GridUnitType.Star)
                });

            Image img = new()
            {
                Width = StatusInfoBarTheme.IconSize,
                Height = StatusInfoBarTheme.IconSize,
                Margin = new Thickness(0, 0, 4, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            img.SetBinding(
                Image.SourceProperty,
                new Binding(nameof(StatusInfoBarItem.Image))
                {
                    Source = item
                });

            Grid.SetColumn(img, 0);

            if (item.ItemType == StatusItemType.Notification)
            {
                item.HorizontalAlignment =  HorizontalAlignment.Stretch;
            }
            else
            {
                item.HorizontalAlignment = HorizontalAlignment.Left;
            }

            TextBlock tb = new()
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Left,
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.CharacterEllipsis
            };

            tb.SetBinding(
                TextBlock.TextProperty,
                new Binding(nameof(StatusInfoBarItem.Text))
                {
                    Source = item
                });

            tb.SetBinding(
                TextBlock.ForegroundProperty,
                new Binding(nameof(StatusInfoBarItem.Foreground))
                {
                    Source = item
                });

            Grid.SetColumn(tb, 1);

            grid.Children.Add(img);
            grid.Children.Add(tb);

            button.Content = grid;

            return button;
        }

        #endregion

        #region Convenience

        /// <summary>
        /// Aktualisiert den Accountbereich.
        /// </summary>
        public void SetAccount(
            string text,
            ImageSource image = null,
            string toolTip = null)
        {
            Account.Text = text;

            if (image != null)
                Account.Image = image;

            if (toolTip != null)
                Account.ToolTip = toolTip;
        }

        /// <summary>
        /// Aktualisiert den Datenquellenbereich.
        /// </summary>
        public void SetDatasource(
            string text,
            ImageSource image = null,
            string toolTip = null)
        {
            Datasource.Text = text;

            if (image != null)
                Datasource.Image = image;

            if (toolTip != null)
                Datasource.ToolTip = toolTip;
        }

        /// <summary>
        /// Aktualisiert den Rechtebereich.
        /// </summary>
        public void SetRights(
            string text,
            ImageSource image = null,
            string toolTip = null)
        {
            Rights.Text = text;

            if (image != null)
                Rights.Image = image;

            if (toolTip != null)
                Rights.ToolTip = toolTip;
        }

        /// <summary>
        /// Aktualisiert den Statusbereich.
        /// </summary>
        public void SetNotification(string text, Brush foreground = null, Brush background = null, ImageSource image = null, string toolTip = null)
        {
            Notification.Text = text;

            if (foreground != null)
                Notification.Foreground = foreground;

            if (background != null)
                Notification.Background = background;

            if (image != null)
                Notification.Image = image;

            if (toolTip != null)
                Notification.ToolTip = toolTip;
        }

        /// <summary>
        /// Aktualisiert den Datumsbereich.
        /// </summary>
        public void SetDate(DateTime date)
        {
            Date.Text = date.ToString("dd.MM.yyyy HH:mm");
        }

        /// <summary>
        /// Setzt alle Bereiche auf ihre Standarddarstellung zurück.
        /// </summary>
        public void Reset()
        {
            Account.Reset();
            Datasource.Reset();
            Rights.Reset();
            Notification.Reset();
            Date.Reset();

            Account.SetContent("Gast", StatusInfoBarImages.Account, "Nicht angemeldet");

            Datasource.SetContent("Keine Datenquelle", StatusInfoBarImages.Database, "Datasource");

            Rights.SetContent("Keine Rechte", StatusInfoBarImages.Shield, "Benutzerrechte");

            Notification.SetContent("Bereit", StatusInfoBarImages.Notification, "Status");

            Date.SetContent(System.DateTime.Now.ToString("dd.MM.yyyy HH:mm"), StatusInfoBarImages.Calendar, "Datum");
        }

        #endregion

    }
}
