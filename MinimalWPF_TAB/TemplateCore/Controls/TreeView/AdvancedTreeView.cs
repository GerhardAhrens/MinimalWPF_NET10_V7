namespace System.Windows.Controls
{
    public class AdvancedTreeView : TreeView
    {
        static AdvancedTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AdvancedTreeView), new FrameworkPropertyMetadata(typeof(AdvancedTreeView)));
        }
    }
}
