namespace System.Windows.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class AdvancedProgressColumn : GridViewColumn
    {
        public static readonly DependencyProperty ProgressBarBackgroundProperty =
            DependencyProperty.Register(nameof(ProgressBarBackground), typeof(Brush), typeof(AdvancedProgressColumn), new PropertyMetadata(Brushes.LightGray));

        public Brush ProgressBarBackground
        {
            get => (Brush)GetValue(ProgressBarBackgroundProperty);
            set => SetValue(ProgressBarBackgroundProperty, value);
        }

        public static readonly DependencyProperty ProgressBarForegroundProperty =
            DependencyProperty.Register(nameof(ProgressBarForeground), typeof(Brush), typeof(AdvancedProgressColumn), new PropertyMetadata(Brushes.DodgerBlue));

        public Brush ProgressBarForeground
        {
            get => (Brush)GetValue(ProgressBarForegroundProperty);
            set => SetValue(ProgressBarForegroundProperty, value);
        }

        public static readonly DependencyProperty PercentageForegroundProperty =
            DependencyProperty.Register(nameof(PercentageForeground), typeof(Brush), typeof(AdvancedProgressColumn), new PropertyMetadata(Brushes.White));

        public Brush PercentageForeground
        {
            get => (Brush)GetValue(PercentageForegroundProperty);
            set => SetValue(PercentageForegroundProperty, value);
        }

        public static readonly DependencyProperty ShowProgressBarProperty =
            DependencyProperty.Register(nameof(ShowProgressBar), typeof(bool), typeof(AdvancedProgressColumn), new PropertyMetadata(true));

        public bool ShowProgressBar
        {
            get => (bool)GetValue(ShowProgressBarProperty);
            set => SetValue(ShowProgressBarProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(AdvancedProgressColumn), new PropertyMetadata(0.0));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(AdvancedProgressColumn), new PropertyMetadata(100.0));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(AdvancedProgressColumn), new PropertyMetadata(0.0));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private BindingBase _valueBinding;

        public BindingBase ValueBinding
        {
            get => _valueBinding;
            set
            {
                _valueBinding = value;
                this.CreateCellTemplate();
            }
        }

        public AdvancedProgressColumn()
        {
            this.CreateCellTemplate();
        }

        private void CreateCellTemplate()
        {
            var gridFactory = new FrameworkElementFactory(typeof(Grid));

            var progressFactory = new FrameworkElementFactory(typeof(ProgressBar));

            progressFactory.Name = "progressBar";

            progressFactory.SetBinding(FrameworkElement.WidthProperty, new Binding(nameof(Width))
                {
                    Source = this,
                    Converter = new ProgressColumnWidthConverter()
                });

            progressFactory.SetValue(FrameworkElement.HeightProperty, 28.0);
            progressFactory.SetValue(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Left);
            progressFactory.SetValue(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center);
            progressFactory.SetBinding(ProgressBar.MinimumProperty, new Binding(nameof(Minimum)) { Source = this });
            progressFactory.SetBinding(ProgressBar.MaximumProperty, new Binding(nameof(Maximum)) { Source = this });
            progressFactory.SetBinding(ProgressBar.BackgroundProperty, new Binding(nameof(ProgressBarBackground))
                {
                    Source = this
                });

            progressFactory.SetBinding(ProgressBar.ForegroundProperty, new Binding(nameof(ProgressBarForeground))
                {
                    Source = this
                });

            if (_valueBinding != null)
            {
                progressFactory.SetBinding(ProgressBar.ValueProperty, _valueBinding);
            }
            else
            {
                progressFactory.SetBinding(ProgressBar.ValueProperty, new Binding(nameof(Value))
                    {
                        Source = this
                    });
            }

            gridFactory.AppendChild(progressFactory);

            var textFactory = new FrameworkElementFactory(typeof(TextBlock));
            textFactory.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            textFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            textFactory.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            textFactory.SetValue(UIElement.IsHitTestVisibleProperty, false);
            textFactory.SetBinding(TextBlock.ForegroundProperty, new Binding(nameof(PercentageForeground))
                {
                    Source = this
                });

            textFactory.SetBinding(UIElement.VisibilityProperty, new Binding(nameof(ShowProgressBar))
                {
                    Source = this,
                    Converter = new BooleanToVisibilityConverter()
                });

            var percentageBinding = new MultiBinding
            {
                Converter = new ProgressPercentageLVConverter()
            };

            percentageBinding.Bindings.Add(
                new Binding("Value")
                {
                    ElementName = "progressBar"
                });

            percentageBinding.Bindings.Add(
                new Binding("Minimum")
                {
                    ElementName = "progressBar"
                });

            percentageBinding.Bindings.Add(
                new Binding("Maximum")
                {
                    ElementName = "progressBar"
                });

            textFactory.SetBinding(TextBlock.TextProperty, percentageBinding);

            gridFactory.AppendChild(textFactory);

            CellTemplate = new DataTemplate
            {
                VisualTree = gridFactory
            };
        }
    }

    public sealed class ProgressPercentageLVConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                values[0] == DependencyProperty.UnsetValue ||
                values[1] == DependencyProperty.UnsetValue ||
                values[2] == DependencyProperty.UnsetValue)
            {
                return string.Empty;
            }

            try
            {
                double value = System.Convert.ToDouble(values[0], culture);

                double minimum = System.Convert.ToDouble(values[1], culture);

                double maximum = System.Convert.ToDouble(values[2], culture);

                if (!double.IsFinite(value) ||
                    !double.IsFinite(minimum) ||
                    !double.IsFinite(maximum) ||
                    maximum <= minimum)
                {
                    return string.Empty;
                }

                double percentage = (value - minimum) / (maximum - minimum) * 100.0;

                percentage = Math.Clamp(percentage, 0.0, 100.0);

                return $"{Math.Round(percentage):0}%";
            }
            catch (FormatException)
            {
                return string.Empty;
            }
            catch (InvalidCastException)
            {
                return string.Empty;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public sealed class ProgressColumnWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && double.IsFinite(width) && width > 0)
            {
                return Math.Max(0, width - 8);
            }

            return 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

