//-----------------------------------------------------------------------
// <copyright file="StatusInfoBarImages.cs" company="Lifeprojects.de">
//     Class: StatusInfoBarImages
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

    /// <summary>
    /// Enthält alle Standardbilder der StatusInfoBar.
    /// Alle DrawingImages werden einmalig erzeugt und eingefroren.
    /// </summary>
    public static class StatusInfoBarImages
    {
        public static DrawingImage Account { get; }
        public static DrawingImage Database { get; }
        public static DrawingImage Shield { get; }
        public static DrawingImage Notification { get; }
        public static DrawingImage Calendar { get; }

        static StatusInfoBarImages()
        {
            Account = CreateAccount();
            Database = CreateDatabase();
            Shield = CreateShield();
            Notification = CreateNotification();
            Calendar = CreateCalendar();
        }

        #region Account

        private static DrawingImage CreateAccount()
        {
            DrawingGroup group = new DrawingGroup();

            group.Transform = new ScaleTransform(0.25, 0.25);

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FF1388B1")),
                    null,
                    Geometry.Parse(
                        "F1M120.578,100.353C120.578,100.353 100.573,144.788 86.587,144.788 72.601,144.788 52.595,100.353 52.595,100.353 21.672,107.24 0,124.458 0,152.792L0,216.682 173.172,216.682 173.172,152.792C173.172,124.458,151.502,107.241,120.578,100.353z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFF0BF7C")),
                    null,
                    Geometry.Parse(
                        "F1M123.225,44.475C123.225,69.038 106.82,88.949 86.585,88.949 66.352,88.949 49.948,69.038 49.948,44.475 49.948,19.912 66.352,0 86.585,0 106.82,0 123.225,19.912 123.225,44.475z")));

            group.Freeze();

            DrawingImage image = new DrawingImage(group);

            image.Freeze();

            return image;
        }

        #endregion

        #region Database

        private static DrawingImage CreateDatabase()
        {
            GeometryGroup geometry = new();

            geometry.Children.Add(
                Geometry.Parse(
                    "M2,4 " +
                    "A6,2 0 0 1 14,4 " +
                    "L14,12 " +
                    "A6,2 0 0 1 2,12 Z"));

            geometry.Children.Add(
                Geometry.Parse("M2,8 A6,2 0 0 0 14,8"));

            return CreateImage(geometry);
        }

        #endregion

        #region Shield

        private static DrawingImage CreateShield()
        {
            Geometry geometry =
                Geometry.Parse(
                    "M8,2 " +
                    "L13,4 " +
                    "V8 " +
                    "C13,11 10.8,13.5 8,14 " +
                    "C5.2,13.5 3,11 3,8 " +
                    "V4 Z");

            return CreateImage(geometry);
        }

        #endregion

        #region Notification

        private static DrawingImage CreateNotification()
        {
            GeometryGroup geometry = new();

            geometry.Children.Add(
                new EllipseGeometry(
                    new Point(8, 8),
                    6,
                    6));

            geometry.Children.Add(
                Geometry.Parse("M8,6 L8,10"));

            geometry.Children.Add(
                new EllipseGeometry(
                    new Point(8, 12),
                    0.6,
                    0.6));

            return CreateImage(geometry);
        }

        #endregion

        #region Calendar

        private static DrawingImage CreateCalendar()
        {
            GeometryGroup geometry = new();

            geometry.Children.Add(
                Geometry.Parse(
                    "M2,3 L14,3 L14,14 L2,14 Z"));

            geometry.Children.Add(
                Geometry.Parse("M2,6 L14,6"));

            geometry.Children.Add(
                Geometry.Parse("M5,2 L5,5"));

            geometry.Children.Add(
                Geometry.Parse("M11,2 L11,5"));

            return CreateImage(geometry);
        }

        #endregion

        #region Helper

        private static DrawingImage CreateImage(Geometry geometry)
        {
            geometry.Freeze();

            Pen pen = new Pen(Brushes.Black, 1.2);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            pen.LineJoin = PenLineJoin.Round;
            pen.Freeze();

            GeometryDrawing drawing = new()
            {
                Geometry = geometry,
                Pen = pen,
                Brush = null
            };

            drawing.Freeze();

            DrawingImage image = new(drawing);
            image.Freeze();

            return image;
        }

        private static DrawingImage CreateImage(GeometryGroup geometry)
        {
            geometry.Freeze();

            Pen pen = new Pen(Brushes.Black, 1.2);
            pen.StartLineCap = PenLineCap.Round;
            pen.EndLineCap = PenLineCap.Round;
            pen.LineJoin = PenLineJoin.Round;
            pen.Freeze();

            GeometryDrawing drawing = new()
            {
                Geometry = geometry,
                Pen = pen,
                Brush = null
            };

            drawing.Freeze();

            DrawingImage image = new(drawing);
            image.Freeze();

            return image;
        }

        #endregion
    }
}
