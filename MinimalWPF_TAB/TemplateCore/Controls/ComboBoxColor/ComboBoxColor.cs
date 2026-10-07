namespace System.Windows.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Input;
    using System.Windows.Media;

    public class ComboBoxColor : ComboBox
    {
        private static SolidColorBrush CreateBrush(string hex)
        {
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));
        }

        private static readonly IReadOnlyList<ColorOption> ColorList = CreateColorList();

        private bool _synchronizingSelection;

        static ComboBoxColor()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxColor), new FrameworkPropertyMetadata(typeof(ComboBoxColor)));
        }

        public ComboBoxColor()
        {
            ItemsSource = ColorList;
            SelectedIndex = 0;
        }

        // Ausgewählter Brush
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(Brush),
                typeof(ComboBoxColor),
                new FrameworkPropertyMetadata(CreateBrush("#FF1E90FF"), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        public Brush SelectedColor
        {
            get => (Brush)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }

        // Hex-Code der aktuellen Auswahl
        private static readonly DependencyPropertyKey
            SelectedColorHexPropertyKey =
                DependencyProperty.RegisterReadOnly(
                    nameof(SelectedColorHex),
                    typeof(string),
                    typeof(ComboBoxColor),
                    new FrameworkPropertyMetadata("#FF1E90FF"));

        public static readonly DependencyProperty SelectedColorHexProperty = SelectedColorHexPropertyKey.DependencyProperty;

        public string SelectedColorHex => (string)GetValue(SelectedColorHexProperty);

        // ICommand für Auswahländerungen
        public static readonly DependencyProperty SelectedChangeCommandProperty =
            DependencyProperty.Register(
                nameof(SelectedChangeCommand),
                typeof(ICommand),
                typeof(ComboBoxColor),
                new PropertyMetadata(null));

        public ICommand SelectedChangeCommand
        {
            get => (ICommand)GetValue(SelectedChangeCommandProperty);
            set => SetValue(SelectedChangeCommandProperty, value);
        }

        // Optionale Bereitstellung der unveränderlichen Farbliste
        public static IReadOnlyList<ColorOption> AvailableColors => ColorList;

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not ComboBoxColor control ||
                control._synchronizingSelection)
                return;

            if (e.NewValue is not Brush brush)
                return;

            var option = ColorList.FirstOrDefault(
                item => BrushesMatch(item.Brush, brush));

            control._synchronizingSelection = true;

            try
            {
                control.SelectedItem = option;

                if (option != null)
                    control.SetValue(SelectedColorHexPropertyKey, option.HexCode);
            }
            finally
            {
                control._synchronizingSelection = false;
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (SelectedItem is not ColorOption option)
                return;

            _synchronizingSelection = true;

            try
            {
                SetCurrentValue(SelectedColorProperty, option.Brush);
                SetValue(SelectedColorHexPropertyKey, option.HexCode);
            }
            finally
            {
                _synchronizingSelection = false;
            }

            // Bei einer tatsächlichen Auswahländerung den Command ausführen.
            if (e.AddedItems.Contains(option))
            {
                ICommand command = SelectedChangeCommand;

                if (command?.CanExecute(option) == true)
                    command.Execute(option);
            }
        }

        private static bool BrushesMatch(Brush a, Brush b)
        {
            if (ReferenceEquals(a, b))
                return true;

            return a is SolidColorBrush first && b is SolidColorBrush second && first.Color == second.Color;
        }

        private static IReadOnlyList<ColorOption> CreateColorList()
        {
            var colors = typeof(Brushes)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(property =>
                    property.PropertyType == typeof(SolidColorBrush) &&
                    property.GetIndexParameters().Length == 0)
                .Select(property => new
                {
                    Name = property.Name,
                    Brush = property.GetValue(null) as SolidColorBrush
                })
                .Where(item => item.Brush != null)
                .Select(item => new ColorOption(
                    item.Name,
                    $"#{item.Brush!.Color.A:X2}" +
                    $"{item.Brush.Color.R:X2}" +
                    $"{item.Brush.Color.G:X2}" +
                    $"{item.Brush.Color.B:X2}",
                    item.Brush))
                .OrderBy(color => color.Name, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return Array.AsReadOnly(colors);
        }
    }

    public sealed class ColorOption
    {
        public string Name { get; }
        public string HexCode { get; }
        public SolidColorBrush Brush { get; }

        public ColorOption(string name, string hexCode, SolidColorBrush brush)
        {
            this.Name = name;
            this.HexCode = hexCode;
            this.Brush = brush;
        }
    }
}
