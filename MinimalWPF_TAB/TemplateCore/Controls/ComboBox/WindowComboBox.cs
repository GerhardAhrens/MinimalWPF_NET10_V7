namespace System.Windows.Controls
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    public class WindowComboBox : ComboBox
    {
        private TextBox _editableTextBox;
        private ICollectionView _filterView;
        private bool _isUpdatingText;
        private bool _isSubstringSearchActive;
        private object _selectedItemBeforeSearch;

        static WindowComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowComboBox), new FrameworkPropertyMetadata(typeof(WindowComboBox)));
        }

        #region PlaceholderText

        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register(
                nameof(PlaceholderText),
                typeof(string),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(string.Empty));

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        #endregion

        #region IsSubstringSearchEnabled

        public static readonly DependencyProperty IsSubstringSearchEnabledProperty =
            DependencyProperty.Register(
                nameof(IsSubstringSearchEnabled),
                typeof(bool),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(false, OnSearchPropertyChanged));

        public bool IsSubstringSearchEnabled
        {
            get => (bool)GetValue(IsSubstringSearchEnabledProperty);
            set => SetValue(IsSubstringSearchEnabledProperty, value);
        }

        #endregion

        #region MinimumSearchLength

        public static readonly DependencyProperty MinimumSearchLengthProperty =
            DependencyProperty.Register(
                nameof(MinimumSearchLength),
                typeof(int),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(1));

        public int MinimumSearchLength
        {
            get => (int)GetValue(MinimumSearchLengthProperty);
            set => SetValue(MinimumSearchLengthProperty, value);
        }

        #endregion

        #region SearchText

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(
                nameof(SearchText),
                typeof(string),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SearchText
        {
            get => (string)GetValue(SearchTextProperty);
            set => SetValue(SearchTextProperty, value);
        }

        #endregion

        #region CornerRadius

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(WindowComboBox),
                new FrameworkPropertyMetadata(new CornerRadius(6)));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion

        #region Template

        public override void OnApplyTemplate()
        {
            if (_editableTextBox != null)
            {
                _editableTextBox.TextChanged -=
                    EditableTextBox_TextChanged;

                _editableTextBox.PreviewKeyDown -=
                    EditableTextBox_PreviewKeyDown;
            }

            base.OnApplyTemplate();

            _editableTextBox =
                GetTemplateChild("PART_EditableTextBox") as TextBox;

            if (_editableTextBox != null)
            {
                _editableTextBox.TextChanged +=
                    EditableTextBox_TextChanged;

                _editableTextBox.PreviewKeyDown +=
                    EditableTextBox_PreviewKeyDown;
            }
        }

        #endregion

        #region ItemsSource

        protected override void OnItemsSourceChanged(
            IEnumerable oldValue,
            IEnumerable newValue)
        {
            if (_filterView != null)
            {
                _filterView.Filter = null;
                _filterView = null;
            }

            base.OnItemsSourceChanged(oldValue, newValue);

            if (newValue != null)
            {
                _filterView =
                    CollectionViewSource.GetDefaultView(newValue);
            }
        }

        #endregion

        #region TextChanged

        private void EditableTextBox_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            if (_isUpdatingText)
                return;

            if (!IsSubstringSearchEnabled)
                return;

            string text = _editableTextBox?.Text ?? string.Empty;

            SetCurrentValue(
                SearchTextProperty,
                text);

            ProcessSearchText(text);
        }

        #endregion

        #region Search

        private void ProcessSearchText(string text)
        {
            if (!IsSubstringSearchEnabled)
                return;

            if (text.Length == 0)
            {
                EndSubstringSearch(
                    restoreSelection: false);

                return;
            }

            if (text.Length < MinimumSearchLength)
            {
                if (!_isSubstringSearchActive)
                {
                    BeginSubstringSearch();
                }

                ClearFilter();

                return;
            }

            if (!_isSubstringSearchActive)
            {
                BeginSubstringSearch();
            }

            ApplyFilter(text);
        }

        private void BeginSubstringSearch()
        {
            if (_isSubstringSearchActive)
                return;

            _isSubstringSearchActive = true;

            _selectedItemBeforeSearch = SelectedItem;

            /*
             * Die aktuelle Auswahl darf während der Suche
             * nicht den eingegebenen Text überschreiben.
             */
            SetCurrentValue(
                SelectedItemProperty,
                null);

            IsDropDownOpen = true;
        }

        private void ApplyFilter(string searchText)
        {
            if (_filterView == null)
                return;

            _filterView.Filter = item =>
                FilterItem(item, searchText);

            _filterView.Refresh();

            IsDropDownOpen = true;
        }

        private bool FilterItem(
            object item,
            string searchText)
        {
            if (item == null)
                return false;

            string value = GetSearchText(item);

            if (string.IsNullOrEmpty(value))
                return false;

            return value.IndexOf(
                       searchText,
                       StringComparison.CurrentCultureIgnoreCase)
                   >= 0;
        }

        private string GetSearchText(object item)
        {
            string path = TextSearch.GetTextPath(this);

            if (string.IsNullOrWhiteSpace(path))
            {
                path = DisplayMemberPath;
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                return item.ToString() ?? string.Empty;
            }

            object value = GetPropertyValue(item, path);

            return value?.ToString() ?? string.Empty;
        }

        private static object GetPropertyValue(object source, string propertyPath)
        {
            object current = source;

            foreach (string propertyName in propertyPath.Split('.'))
            {
                if (current == null)
                    return null;

                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(current)[propertyName];

                if (descriptor == null)
                    return null;

                current = descriptor.GetValue(current);
            }

            return current;
        }

        private void ClearFilter()
        {
            if (_filterView == null)
                return;

            _filterView.Filter = null;
            _filterView.Refresh();

            IsDropDownOpen = true;
        }

        #endregion

        #region Keyboard

        private void EditableTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!IsSubstringSearchEnabled)
                return;

            switch (e.Key)
            {
                case Key.Enter:

                    CommitCurrentSearchResult();

                    e.Handled = true;
                    break;

                case Key.Escape:

                    CancelSubstringSearch();

                    e.Handled = true;
                    break;

                case Key.Down:

                    IsDropDownOpen = true;
                    MoveSelection(1);

                    e.Handled = true;
                    break;

                case Key.Up:

                    IsDropDownOpen = true;
                    MoveSelection(-1);

                    e.Handled = true;
                    break;
            }
        }

        private void MoveSelection(int direction)
        {
            if (_filterView == null)
                return;

            if (!_filterView.MoveCurrentToPosition(_filterView.CurrentPosition + direction))
            {
                return;
            }

            /*
             * Während der Suche wird nur die aktuelle
             * Trefferposition geändert.
             *
             * SelectedItem bleibt zunächst null.
             */
        }

        #endregion

        #region Commit / Cancel

        private void CommitCurrentSearchResult()
        {
            if (!_isSubstringSearchActive)
                return;

            if (_filterView == null)
            {
                EndSubstringSearch(false);
                return;
            }

            object item = _filterView.CurrentItem;

            if (item == null)
            {
                if (!_filterView.MoveCurrentToFirst())
                {
                    EndSubstringSearch(false);
                    return;
                }

                item = _filterView.CurrentItem;
            }

            if (item != null)
            {
                SetCurrentValue(
                    SelectedItemProperty,
                    item);
            }

            EndSubstringSearch(
                restoreSelection: false);
        }

        private void CancelSubstringSearch()
        {
            if (!_isSubstringSearchActive)
                return;

            EndSubstringSearch(
                restoreSelection: true);
        }

        private void EndSubstringSearch(
            bool restoreSelection)
        {
            if (!_isSubstringSearchActive)
            {
                ClearFilter();
                return;
            }

            _isSubstringSearchActive = false;

            ClearFilter();

            if (restoreSelection &&
                _selectedItemBeforeSearch != null)
            {
                SetCurrentValue(
                    SelectedItemProperty,
                    _selectedItemBeforeSearch);
            }

            _selectedItemBeforeSearch = null;

            if (restoreSelection)
            {
                UpdateEditableTextFromSelection();
            }
        }

        private void UpdateEditableTextFromSelection()
        {
            if (_editableTextBox == null)
                return;

            string text = string.Empty;

            if (SelectedItem != null)
            {
                text = GetSearchText(SelectedItem);
            }

            try
            {
                _isUpdatingText = true;

                _editableTextBox.Text = text;
                _editableTextBox.CaretIndex =
                    _editableTextBox.Text.Length;
            }
            finally
            {
                _isUpdatingText = false;
            }
        }

        #endregion

        #region Search Properties

        private static void OnSearchPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var comboBox = (WindowComboBox)d;

            if (!(bool)e.NewValue)
            {
                comboBox.EndSubstringSearch(
                    restoreSelection: false);
            }
        }

        #endregion
    }
}
