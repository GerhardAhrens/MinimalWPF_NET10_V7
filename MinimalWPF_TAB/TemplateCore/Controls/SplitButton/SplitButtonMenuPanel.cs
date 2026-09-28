namespace System.Windows.Controls
{
    public class SplitButtonMenuPanel : StackPanel
    {
        static SplitButtonMenuPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SplitButtonMenuPanel),
                new System.Windows.FrameworkPropertyMetadata(
                    typeof(SplitButtonMenuPanel)));
        }

        public SplitButtonMenuPanel()
        {
            this.Orientation = Orientation.Vertical;
        }
    }
}
