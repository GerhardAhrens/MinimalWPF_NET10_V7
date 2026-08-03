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
            GeometryGroup geometry = new();

            // Kopf
            geometry.Children.Add(
                new EllipseGeometry(
                    new Point(8, 5),
                    2.5,
                    2.5));

            // Schultern
            geometry.Children.Add(
                Geometry.Parse("M3,14 C3,10.5 13,10.5 13,14"));

            return CreateImage(geometry);
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
