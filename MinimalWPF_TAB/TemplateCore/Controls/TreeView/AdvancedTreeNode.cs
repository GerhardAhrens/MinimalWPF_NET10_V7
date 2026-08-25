namespace System.Windows.Controls
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
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
        private bool _isFilterVisible = true;
        private ObservableCollection<AdvancedTreeMenuItem> _contextMenuItems;

        public AdvancedTreeNode()
        {
            this.Children = new ObservableCollection<AdvancedTreeNode>();

            this._filteredChildren = new ObservableCollection<AdvancedTreeNode>();

            this.FilteredChildren = new ReadOnlyObservableCollection<AdvancedTreeNode>(this._filteredChildren);

            this.ContextMenuItems.CollectionChanged += ContextMenuItems_CollectionChanged;
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
                this.OnPropertyChanged();
            }
        }

        public bool IsFilterVisible
        {
            get => this._isFilterVisible;
            private set
            {
                if (this._isFilterVisible == value)
                {
                    return;
                }

                this._isFilterVisible = value;
                this.OnPropertyChanged();
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

        public ObservableCollection<AdvancedTreeMenuItem> ContextMenuItems
        {
            get
            {
                return _contextMenuItems ??= new ObservableCollection<AdvancedTreeMenuItem>();
            }
        }

        public bool HasContextMenu => ContextMenuItems.Count > 0;

        private void ContextMenuItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(HasContextMenu));
        }

        internal bool ApplyFilter(string filter, Func<AdvancedTreeNode, string, bool> predicate)
        {
            _filteredChildren.Clear();


            // Kein Filter:
            // komplette Hierarchie anzeigen.
            if (string.IsNullOrWhiteSpace(filter))
            {
                IsFilterVisible = true;

                foreach (var child in Children)
                {
                    child.ApplyFilter(string.Empty, predicate);

                    _filteredChildren.Add(child);
                }

                return false;
            }


            // Prüfen, ob diese Node selbst dem Filter entspricht.
            bool nodeMatches = predicate(this, filter);


            // ---------------------------------------------------------
            // Die Node selbst ist ein Treffer.
            //
            // In diesem Fall sollen alle Kinder sichtbar bleiben.
            // ---------------------------------------------------------
            if (nodeMatches)
            {
                IsFilterVisible = true;

                foreach (var child in Children)
                {
                    child.ApplyFilter(string.Empty, predicate);

                    _filteredChildren.Add(child);
                }

                // Die Treffer-Node wird geöffnet, damit ihre
                // untergeordneten Nodes unmittelbar sichtbar sind.
                if (Children.Count > 0)
                {
                    IsExpanded = true;
                }

                return true;
            }


            // ---------------------------------------------------------
            // Die Node selbst ist kein Treffer.
            // Deshalb nur die Kinder untersuchen.
            // ---------------------------------------------------------
            foreach (var child in Children)
            {
                bool childMatches = child.ApplyFilter(filter, predicate);

                if (childMatches)
                {
                    _filteredChildren.Add(child);
                }
            }


            // Die Node bleibt sichtbar, wenn sich irgendwo
            // darunter ein Treffer befindet.
            bool visible = _filteredChildren.Count > 0;

            IsFilterVisible = visible;


            // Elternknoten eines Treffers automatisch öffnen.
            if (visible)
            {
                IsExpanded = true;
            }


            return visible;
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
