namespace System.Windows.Controls
{

    /// <summary>
    /// Erweiterte GridViewColumn für das AdvancedListView.
    /// </summary>
    public class AdvancedGridViewColumn : GridViewColumn
    {
        #region AllowSorting

        public static readonly DependencyProperty AllowSortingProperty =
            DependencyProperty.Register(
                nameof(AllowSorting),
                typeof(bool),
                typeof(AdvancedGridViewColumn),
                new PropertyMetadata(true));

        public bool AllowSorting
        {
            get => (bool)GetValue(AllowSortingProperty);
            set => SetValue(AllowSortingProperty, value);
        }

        #endregion

        #region SortMemberPath

        public static readonly DependencyProperty SortMemberPathProperty =
            DependencyProperty.Register(
                nameof(SortMemberPath),
                typeof(string),
                typeof(AdvancedGridViewColumn));

        /// <summary>
        /// Eigenschaft, nach der sortiert werden soll.
        /// Ist sie leer, wird DisplayMemberBinding verwendet.
        /// </summary>
        public string SortMemberPath
        {
            get => (string)GetValue(SortMemberPathProperty);
            set => SetValue(SortMemberPathProperty, value);
        }

        #endregion

        #region ShowFilter

        public static readonly DependencyProperty ShowFilterProperty =
            DependencyProperty.Register(
                nameof(ShowFilter),
                typeof(bool),
                typeof(AdvancedGridViewColumn),
                new PropertyMetadata(true));

        public bool ShowFilter
        {
            get => (bool)GetValue(ShowFilterProperty);
            set => SetValue(ShowFilterProperty, value);
        }

        #endregion

        #region FilterTemplate

        public static readonly DependencyProperty FilterTemplateProperty =
            DependencyProperty.Register(
                nameof(FilterTemplate),
                typeof(DataTemplate),
                typeof(AdvancedGridViewColumn));

        public DataTemplate FilterTemplate
        {
            get => (DataTemplate)GetValue(FilterTemplateProperty);
            set => SetValue(FilterTemplateProperty, value);
        }

        #endregion

        #region FilterWidth

        public static readonly DependencyProperty FilterWidthProperty =
            DependencyProperty.Register(
                nameof(FilterWidth),
                typeof(double),
                typeof(AdvancedGridViewColumn),
                new PropertyMetadata(double.NaN));

        public double FilterWidth
        {
            get => (double)GetValue(FilterWidthProperty);
            set => SetValue(FilterWidthProperty, value);
        }

        #endregion

        #region IsRowNumberColumn

        public static readonly DependencyProperty IsRowNumberColumnProperty =
            DependencyProperty.Register(
                nameof(IsRowNumberColumn),
                typeof(bool),
                typeof(AdvancedGridViewColumn),
                new PropertyMetadata(false));

        public bool IsRowNumberColumn
        {
            get => (bool)GetValue(IsRowNumberColumnProperty);
            set => SetValue(IsRowNumberColumnProperty, value);
        }

        #endregion    }
    }
}
