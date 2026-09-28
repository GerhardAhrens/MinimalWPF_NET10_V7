namespace System.Windows.Controls
{
    using System.Windows.Media;

    public class SplitButtonSeparator : Border
    {
        public SplitButtonSeparator()
        {
            base.Height = 1;
            base.Margin = new Thickness(8, 6, 8, 6);
            base.HorizontalAlignment = HorizontalAlignment.Stretch;
            base.Background = Brushes.LightGray;
        }
    }
}
