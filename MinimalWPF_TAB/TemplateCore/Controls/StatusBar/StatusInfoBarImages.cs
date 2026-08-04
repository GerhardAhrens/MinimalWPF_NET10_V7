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
// Die Klasse StatusInfoBarImages enthält alle Standardbilder der StatusInfoBar. Alle DrawingImages werden einmalig erzeugt und eingefroren.
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
            DrawingGroup group = new DrawingGroup();

            group.Transform = new ScaleTransform(0.25, 0.25);

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFE7BE29")),
                    null,
                    Geometry.Parse(
                        "F1M154,46C154,53.731,147.732,60,140,60L14,60C6.268,60,0,53.731,0,46L0,14C0,6.268,6.268,0,14,0L140,0C147.732,0,154,6.268,154,14L154,46z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFE7BE29")),
                    null,
                    Geometry.Parse(
                        "F1M154,126C154,133.731,147.732,140,140,140L14,140C6.268,140,0,133.731,0,126L0,94C0,86.268,6.268,80,14,80L140,80C147.732,160,154,86.268,154,94L154,126z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFE7BE29")),
                    null,
                    Geometry.Parse(
                        "F1M154,206C154,213.731,147.732,220,140,220L14,220C6.268,220,0,213.731,0,206L0,174C0,166.268,6.268,160,14,160L140,160C147.732,160,154,166.268,154,174L154,206z")));

            group.Freeze();

            DrawingImage image = new DrawingImage(group);
            image.Freeze();

            return image;
        }

        #endregion

        #region Shield

        private static DrawingImage CreateShield()
        {
            DrawingGroup group = new DrawingGroup();

            group.Transform = new ScaleTransform(0.25, 0.25);

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FF79919C")),
                    null,
                    Geometry.Parse(
                        "F1M97.385,0L0,41.345 5.592,134.661C9.885,188.959 97.385,232.979 97.385,232.979 97.385,232.979 185.517,192.621 189.179,134.661L194.77,41.345 97.385,0z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFF9F7EF")),
                    null,
                    Geometry.Parse(
                        "F1M171.211,133.585C169.031,168.075 122.959,199.36 97.719,212.718 72.486,198.382 26.242,165.733 23.548,133.396L18.728,52.949 97.385,19.555 176.042,52.949 171.211,133.585z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FF1388B1")),
                    null,
                    Geometry.Parse(
                        "F1M35.643,133.498L153.943,56.604 97.385,32.592 31.213,60.685 35.518,132.524C35.546,132.847,35.601,133.174,35.643,133.498z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FF1388B1")),
                    null,
                    Geometry.Parse(
                        "F1M45.375,154.595C57.003,170.322 77.292,186.49 97.912,198.926 128.291,181.666 157.802,155.513 159.232,132.867L162.491,78.47 45.375,154.595z")));

            group.Freeze();

            DrawingImage image = new DrawingImage(group);
            image.Freeze();

            return image;
        }

        #endregion

        #region Notification

        private static DrawingImage CreateNotification()
        {
            DrawingGroup group = new DrawingGroup();

            group.Transform = new ScaleTransform(0.25, 0.25);

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FF3EA8F4")),
                    null,
                    Geometry.Parse(
                        "M116.206,0C52.13,0 0,33.918 0,75.609 0,94.813 11.161,113.151 31.428,127.245L32.699,128.127 32.715,129.676C32.901,147.965 29.446,170.274 14.368,184.356 29.195,179.377 50.579,168.754 68.292,146.403L69.522,144.85 71.434,145.374C85.639,149.252 100.702,151.221 116.206,151.221 180.279,151.221 232.406,117.301 232.406,75.609 232.406,33.918 180.279,0 116.206,0z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFFFFFFF")),
                    null,
                    Geometry.Parse(
                        "M185.675,98.404L51.674,98.404 51.674,83.522 185.675,83.522 185.675,98.404z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#FFFFFFFF")),
                    null,
                    Geometry.Parse(
                        "M185.675,64.404L51.674,64.404 51.674,49.522 185.675,49.522 185.675,64.404z")));

            group.Freeze();

            DrawingImage image = new DrawingImage(group);
            image.Freeze();

            return image;
        }

        #endregion

        #region Calendar

        private static DrawingImage CreateCalendar()
        {
            DrawingGroup group = new DrawingGroup();

            group.Transform = new ScaleTransform(0.25, 0.25);

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7F7F7F")),
                    null,
                    Geometry.Parse(
                        "F1M0,77.778L0,220.278C0,227.833,6.125,233.958,13.68,233.958L208.945,233.958C216.5,233.958,222.625,227.833,222.625,220.278L222.625,77.778 0,77.778z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF1F1F1")),
                    null,
                    Geometry.Parse(
                        "F1M13.68,227.958C9.445,227.958,6,224.513,6,220.278L6,83.778 216.625,83.778 216.625,220.278C216.625,224.513,213.18,227.958,208.945,227.958L13.68,227.958z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF323232")),
                    null,
                    Geometry.Parse(
                        "F1M49.567,128.884C51.308,129.036 52.822,129.112 54.109,129.112 58.878,129.112 62.531,128.11 65.067,126.102 67.603,124.096 69.533,120.652 70.859,115.769L88.63,115.769 88.63,196.109 68.417,196.109 68.417,144.499 49.567,144.499 49.567,128.884z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF323232")),
                    null,
                    Geometry.Parse(
                        "F1M129.226,169.821C129.717,173.152 131.033,175.811 133.173,177.797 135.311,179.786 137.913,180.779 140.979,180.779 144.424,180.779 147.263,179.681 149.496,177.486 151.729,175.292 152.845,172.47 152.845,169.026 152.845,165.771 151.775,162.98 149.639,160.651 147.499,158.324 144.857,157.16 141.716,157.16 137.401,157.16 133.881,158.788 131.155,162.042L113.386,162.042 118.041,115.769 169.765,115.769 168.005,132.631 133.199,132.631 131.837,145.52C137.099,142.152 141.981,140.467 146.485,140.467 154.132,140.467 160.473,143.042 165.507,148.189 170.539,153.336 173.058,159.771 173.058,167.493 173.058,176.35 169.972,183.589 163.804,189.211 157.634,194.831 149.816,197.642 140.354,197.642 131.686,197.642 124.627,195.295 119.177,190.601 113.727,185.909 110.678,179.493 110.035,171.354L129.226,169.821z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7F7F7F")),
                    null,
                    Geometry.Parse(
                        "F1M111.313,0C106.696,0,102.953,3.743,102.953,8.361L102.953,41.8C102.953,46.417 106.696,50.16 111.313,50.16 115.93,50.16 119.672,46.417 119.672,41.8L119.672,8.361C119.672,3.743,115.93,0,111.313,0z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7F7F7F")),
                    null,
                    Geometry.Parse(
                        "F1M54.313,0C49.696,0,45.953,3.743,45.953,8.361L45.953,41.8C45.953,46.417 49.696,50.16 54.313,50.16 58.929,50.16 62.672,46.417 62.672,41.8L62.672,8.361C62.672,3.743,58.929,0,54.313,0z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEC1B23")),
                    null,
                    Geometry.Parse(
                        "F1M214.646,24.72L183.512,24.72 183.512,41.8C183.512,50.181 176.693,57 168.313,57 159.932,57 153.113,50.181 153.113,41.8L153.113,24.72 126.512,24.72 126.512,41.8C126.512,50.181 119.693,57 111.313,57 102.931,57 96.113,50.181 96.113,41.8L96.113,24.72 69.512,24.72 69.512,41.8C69.512,50.181 62.694,57 54.313,57 45.931,57 39.113,50.181 39.113,41.8L39.113,24.72 7.98,24.72C3.572,24.72,0,28.293,0,32.7L0,69.506 222.625,69.506 222.625,32.7C222.625,28.293,219.053,24.72,214.646,24.72z")));

            group.Children.Add(
                new GeometryDrawing(
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF7F7F7F")),
                    null,
                    Geometry.Parse(
                        "F1M168.313,0C163.695,0,159.953,3.743,159.953,8.361L159.953,41.8C159.953,46.417 163.695,50.16 168.313,50.16 172.93,50.16 176.672,46.417 176.672,41.8L176.672,8.361C176.672,3.743,172.93,0,168.313,0z")));

            group.Freeze();

            DrawingImage image = new DrawingImage(group);
            image.Freeze();

            return image;
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
