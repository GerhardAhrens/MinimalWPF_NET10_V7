namespace System.Windows.Controls
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Media;

    public class AdvancedTreeNode : INotifyPropertyChanged
    {
        private Guid _id = Guid.Empty;
        private string _text = string.Empty;
        private bool _isExpanded;
        private bool _isSelected;
        private DrawingImage _openImage;
        private DrawingImage _expandedImage;
        private readonly ObservableCollection<AdvancedTreeNode> _filteredChildren;

        public AdvancedTreeNode()
        {
            this.Children = new ObservableCollection<AdvancedTreeNode>();

            this._filteredChildren = new ObservableCollection<AdvancedTreeNode>();

            this.FilteredChildren = new ReadOnlyObservableCollection<AdvancedTreeNode>(this._filteredChildren);
        }

        public AdvancedTreeNode(Guid id, string text) : this()
        {
            this.Id = id;
            this.Text = text;
        }

        /// <summary>
        /// Id der TreeNode.
        /// </summary>
        public Guid Id
        {
            get => this._id;
            set
            {
                if (this._id == value)
                {
                    return;
                }

                this._id = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Text der TreeNode.
        /// </summary>
        public string Text
        {
            get => this._text;
            set
            {
                if (_text == value)
                {
                    return;
                }

                this._text = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Symbol für die Node.
        /// </summary>
        public DrawingImage OpenImage
        {
            get => this._openImage;
            set
            {
                if (ReferenceEquals(this._openImage, value))
                {
                    return;
                }

                this._openImage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Symbol für eine geöffnete Node.
        /// Wenn kein Symbol angegeben ist, wird Image verwendet.
        /// </summary>
        public DrawingImage ExpandedImage
        {
            get => this._expandedImage;
            set
            {
                if (ReferenceEquals(this._expandedImage, value))
                {
                    return;
                }

                this._expandedImage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gibt an, ob die Node geöffnet ist.
        /// </summary>
        public bool IsExpanded
        {
            get => this._isExpanded;
            set
            {
                if (this._isExpanded == value)
                {
                    return;
                }

                this._isExpanded = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gibt an, ob die Node ausgewählt ist.
        /// </summary>
        public bool IsSelected
        {
            get => this._isSelected;
            set
            {
                if (this._isSelected == value)
                {
                    return;
                }

                this._isSelected = value;
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

        public ReadOnlyObservableCollection<AdvancedTreeNode> FilteredChildren
        {
            get;
        }

        internal bool ApplyFilter(string filter, Func<AdvancedTreeNode, string, bool> predicate)
        {
            _filteredChildren.Clear();


            // Kein Filter:
            // alle Original-Kinder anzeigen.
            if (string.IsNullOrWhiteSpace(filter))
            {
                foreach (var child in Children)
                {
                    child.ApplyFilter(filter, predicate);

                    _filteredChildren.Add(child);
                }

                return true;
            }


            // Prüfen, ob diese Node selbst passt.
            bool nodeMatches = predicate(this, filter);


            // Kinder prüfen.
            foreach (var child in Children)
            {
                bool childMatches = child.ApplyFilter(filter, predicate);

                if (childMatches)
                {
                    _filteredChildren.Add(child);
                }
            }


            // Node bleibt sichtbar, wenn sie selbst
            // oder mindestens ein Kind passt.
            return nodeMatches || _filteredChildren.Count > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return this.Text;
        }
    }
}
