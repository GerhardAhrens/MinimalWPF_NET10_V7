namespace System.Windows
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public enum CheckBoxSkins
    {
        DefaultSquare,
        DefaultEllipse,
        EllipseSkin1,
    }

    public class FlatCheckBox : CheckBox
    {
        public static readonly DependencyProperty SkinsProperty = DependencyProperty.Register("Skins" , typeof(CheckBoxSkins), typeof(FlatCheckBox), new PropertyMetadata(CheckBoxSkins.DefaultSquare));
        public static readonly DependencyProperty UnCheckedColorProperty = DependencyProperty.Register("UnCheckedColor" , typeof(Brush), typeof(FlatCheckBox));
        public static readonly DependencyProperty CheckedColorProperty = DependencyProperty.Register("CheckedColor" , typeof(Brush), typeof(FlatCheckBox));

        static FlatCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlatCheckBox), new FrameworkPropertyMetadata(typeof(FlatCheckBox)));
        }

        public CheckBoxSkins Skins
        {
            get { return (CheckBoxSkins)GetValue(SkinsProperty); }
            set { SetValue(SkinsProperty, value); }
        }


        public Brush UnCheckedColor
        {
            get { return (Brush)GetValue(UnCheckedColorProperty); }
            set { SetValue(UnCheckedColorProperty, value); }
        }

        public Brush CheckedColor
        {
            get { return (Brush)GetValue(CheckedColorProperty); }
            set { SetValue(CheckedColorProperty, value); }
        }
    }
}
