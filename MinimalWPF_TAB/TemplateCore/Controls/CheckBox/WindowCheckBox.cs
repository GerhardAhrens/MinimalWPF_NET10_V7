namespace System.Windows.Controls
{
    using System.Windows;

    public class WindowCheckBox : CheckBox
    {
        static WindowCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowCheckBox), new FrameworkPropertyMetadata(typeof(WindowCheckBox)));
        }
    }
}
