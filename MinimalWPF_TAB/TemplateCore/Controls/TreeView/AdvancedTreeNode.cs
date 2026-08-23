namespace System.Windows.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    public class AdvancedTreeNode : INotifyPropertyChanged
    {
        private string _text = string.Empty;
        private bool _isExpanded;
        private bool _isSelected;
        private DrawingImage _image;
        private DrawingImage _expandedImage;

        public AdvancedTreeNode()
        {
            Children = new ObservableCollection<AdvancedTreeNode>();
        }

        public AdvancedTreeNode(string text)
            : this()
        {
            Text = text;
        }

        /// <summary>
        /// Text der TreeNode.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value)
                    return;

                _text = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Symbol für die Node.
        /// </summary>
        public DrawingImage Image
        {
            get => _image;
            set
            {
                if (ReferenceEquals(_image, value))
                    return;

                _image = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Symbol für eine geöffnete Node.
        /// Wenn kein Symbol angegeben ist, wird Image verwendet.
        /// </summary>
        public DrawingImage ExpandedImage
        {
            get => _expandedImage;
            set
            {
                if (ReferenceEquals(_expandedImage, value))
                    return;

                _expandedImage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gibt an, ob die Node geöffnet ist.
        /// </summary>
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value)
                    return;

                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gibt an, ob die Node ausgewählt ist.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Untergeordnete Nodes.
        /// </summary>
        public ObservableCollection<AdvancedTreeNode> Children
        {
            get;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
