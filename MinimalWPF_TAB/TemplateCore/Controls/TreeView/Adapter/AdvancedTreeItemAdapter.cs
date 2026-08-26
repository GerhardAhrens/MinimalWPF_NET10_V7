namespace System.Windows.Controls
{
    public class AdvancedTreeItemAdapter<T> : IAdvancedTreeItemAdapter
    {
        private readonly Func<T, string> _textSelector;


        public AdvancedTreeItemAdapter(Func<T, string> textSelector)
        {
            this._textSelector = textSelector ?? throw new ArgumentNullException(nameof(textSelector));
        }


        public AdvancedTreeNode Convert(object item)
        {
            if (item is not T value)
            {
                throw new ArgumentException($"Das Element ist nicht vom erwarteten Typ '{typeof(T).Name}'.", nameof(item));
            }


            var node = new AdvancedTreeNode(Guid.CreateVersion7(), this._textSelector(value));


            node.SourceItem = value;

            return node;
        }
    }
}
