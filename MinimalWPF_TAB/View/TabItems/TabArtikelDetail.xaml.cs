//-----------------------------------------------------------------------
// <copyright file="TabArtikelDetail.cs" company="Lifeprojects.de">
//     Class: TabArtikelDetail
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>GERHARD-G6\gerha - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.08.2026</date>
//
// <summary>
// Template für eine neues UserControl
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.View
{
    using System.Data;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Interaktionslogik für TabArtikelDetail.xaml
    /// </summary>
    public partial class TabArtikelDetail : UserControlBase
    {
        public TabArtikelDetail(DataRowView rowView) : base(typeof(TabArtikelDetail))

        {
            this.InitializeComponent();
            WeakEventManager<UserControl, RoutedEventArgs>.AddHandler(this, "Loaded", this.OnLoaded);
            this.CurrentRow = rowView;
            this.Background = System.Windows.Media.Brushes.LightBlue;
            this.DataContext = this;
        }

        #region Properties

        public DataRowView CurrentRow
        {
            get => base.GetValue<DataRowView>();
            set => base.SetValue(value);
        }

        public Dictionary<int,string> ImageSource
        {
            get => base.GetValue<Dictionary<int, string>>();
            set => base.SetValue(value);
        }

        public int SelectedImage
        {
            get => base.GetValue<int>();
            set => base.SetValue(value);
        }

        public string PhonNumber
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string PhonNumberRaw
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string Kennzeichen
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        public string KennzeichenRaw
        {
            get => base.GetValue<string>();
            set => base.SetValue(value);
        }

        private MessageBase Message { get; } = new MessageBase();

        #endregion Properties

        #region Windows Events

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.ImageSource = new Dictionary<int, string>
                {
                    { 1, "M 0,0 L 100,0 L 100,100 L 0,100 Z" },
                    { 2, "M 50,0 L 100,100 L 0,100 Z" },
                    { 3,
                        @"
                        <DrawingImage
                            xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
                            <DrawingImage.Drawing>
                                <GeometryDrawing Brush=""Red"">
                                    <GeometryDrawing.Geometry>
                                        <PathGeometry Figures=""M 0,0 L 100,0 L 50,100 Z"" />
                                    </GeometryDrawing.Geometry>
                                </GeometryDrawing>
                            </DrawingImage.Drawing>
                        </DrawingImage>"
                    },
                    { 4, "M12,12.14C11.09,10.77 10.14,9.78 9.14,9.19C8.14,8.59 7.27,8.38 6.5,8.55C5.77,8.73 5.14,9.14 4.64,9.8C4.2,10.39 4,11.06 4,11.81V12C4,12.78 4.11,13.58 4.36,14.39C4.45,14.64 4.5,14.64 4.55,14.39C4.67,13.77 4.96,13.31 5.41,13.03C5.87,12.75 6.47,12.76 7.22,13.05C7.97,13.35 8.7,14 9.42,14.95C10.7,16.67 12.2,17.72 13.92,18.09C16.14,18.41 17.81,17.7 18.94,16C19.25,15.39 19.5,14.86 19.64,14.39C19.73,14.08 19.69,14.05 19.5,14.3C19.03,14.92 18.4,15.33 17.6,15.5C16.8,15.7 15.89,15.5 14.86,15C13.83,14.43 12.88,13.5 12,12.14M16.97,8.16C15.41,5.81 13.72,4.5 11.91,4.17C10.47,3.95 8.91,4.45 7.22,5.67C7,5.83 6.9,5.91 6.91,5.93C6.93,5.95 7.06,5.89 7.31,5.77C9.81,4.55 12.22,5.83 14.53,9.61C15.03,10.45 15.55,11.11 16.1,11.58C16.65,12.05 17.16,12.33 17.65,12.42C18.13,12.5 18.57,12.5 18.96,12.38C19.35,12.25 19.7,12.05 20,11.77C20,11.17 19.91,10.5 19.69,9.8C19.19,9.92 18.74,9.88 18.35,9.68C17.96,9.5 17.5,8.97 16.97,8.16M12,2C14.75,2 17.1,3 19.05,4.95C21,6.9 22,9.25 22,12C22,14.75 21,17.1 19.05,19.05C17.1,21 14.75,22 12,22C9.25,22 6.9,21 4.95,19.05C3,17.1 2,14.75 2,12C2,9.25 3,6.9 4.95,4.95C6.9,3 9.25,2 12,2Z" },
                    { 5, "M17.9,17.39C17.64,16.59 16.89,16 16,16H15V13A1,1 0 0,0 14,12H8V10H10A1,1 0 0,0 11,9V7H13A2,2 0 0,0 15,5V4.59C17.93,5.77 20,8.64 20,12C20,14.08 19.2,15.97 17.9,17.39M11,19.93C7.05,19.44 4,16.08 4,12C4,11.38 4.08,10.78 4.21,10.21L9,15V16A2,2 0 0,0 11,18M12,2A10,10 0 0,0 2,12A10,10 0 0,0 12,22A10,10 0 0,0 22,12A10,10 0 0,0 12,2Z" },
                };

            this.SelectedImage = 4;
        }

        #endregion Windows Events

        #region Command Events
        #endregion Command Events

        private void OnTestTextBoxMask(object sender, RoutedEventArgs e)
        {
            Message.Hinweis("Test TextBoxMask", $"Ergebnis: {this.PhonNumberRaw}");
        }
    }
}
