namespace System.Windows.Controls
{

    using System.Windows.Data;
    using System.Windows.Media;

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

        #region TextAlignment
        public static readonly DependencyProperty TextAlignmentProperty =
            DependencyProperty.Register(
                nameof(TextAlignment),
                typeof(TextAlignment),
                typeof(AdvancedGridViewColumn),
                new PropertyMetadata(TextAlignment.Left));

        public TextAlignment TextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }
        #endregion TextAlignment

        #region CellStyle
        public static readonly DependencyProperty CellStyleProperty =
            DependencyProperty.Register(
                nameof(CellStyle),
                typeof(Style),
                typeof(AdvancedGridViewColumn));

        public Style CellStyle
        {
            get => (Style)GetValue(CellStyleProperty);
            set => SetValue(CellStyleProperty, value);
        }
        #endregion CellStyle

        #region StringFormat
        public static readonly DependencyProperty StringFormatProperty =
            DependencyProperty.Register(
                nameof(StringFormat),
                typeof(string),
                typeof(AdvancedGridViewColumn));

        public string StringFormat
        {
            get => (string)GetValue(StringFormatProperty);
            set => SetValue(StringFormatProperty, value);
        }
        #endregion StringFormat

        #region FilterMemberPath
        public static readonly DependencyProperty FilterMemberPathProperty =
            DependencyProperty.Register(
                nameof(FilterMemberPath),
                typeof(string),
                typeof(AdvancedGridViewColumn));

        public string FilterMemberPath
        {
            get => (string)GetValue(FilterMemberPathProperty);
            set => SetValue(FilterMemberPathProperty, value);
        }

        internal string EffectiveFilterMemberPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FilterMemberPath))
                    return FilterMemberPath;

                return EffectiveSortMemberPath;
            }
        }
        #endregion FilterMemberPath

        internal string EffectiveSortMemberPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SortMemberPath))
                    return SortMemberPath;

                if (DisplayMemberBinding is Binding binding)
                    return binding.Path?.Path;

                return null;
            }
        }

        internal void CreateDefaultCellTemplate()
        {
            if (DisplayMemberBinding is not Binding binding)
                return;

            var gridFactory = new FrameworkElementFactory(typeof(Grid));

            var textFactory = new FrameworkElementFactory(typeof(TextBlock));

            var newBinding = new Binding
            {
                Path = binding.Path,
                Mode = binding.Mode,
                Converter = binding.Converter,
                ConverterCulture = binding.ConverterCulture,
                ConverterParameter = binding.ConverterParameter,
                FallbackValue = binding.FallbackValue,
                TargetNullValue = binding.TargetNullValue,
                UpdateSourceTrigger = binding.UpdateSourceTrigger,
                StringFormat = StringFormat
            };

            textFactory.SetBinding(TextBlock.TextProperty, newBinding);

            textFactory.SetValue(FrameworkElement.WidthProperty, Math.Max(0, Width - 15));
            //textFactory.SetValue(TextBlock.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
            textFactory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment);
            textFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            textFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);

            if (CellStyle != null)
            {
                textFactory.SetValue(FrameworkElement.StyleProperty, CellStyle);
            }

            gridFactory.AppendChild(textFactory);

            CellTemplate = new DataTemplate
            {
                VisualTree = gridFactory
            };

            DisplayMemberBinding = null;
        }
    }
}
