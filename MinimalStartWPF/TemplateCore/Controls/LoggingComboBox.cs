namespace System.Windows
{
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class LoggingComboBox : ComboBox
    {
        private const string NOLOG = "Kein Logger aktiv";
        private const string DEBUGLOG = "Debug Informationen";
        private const string INFOLOG = "Allgemeine Informationen";
        private const string WARNINGLOG = "Warnungen";
        private const string ERRORLOG = "Fehler";
        private const string CRITICALLOG = "Kritische Fehler";

        static LoggingComboBox()
        {
            // Überschreibt die Metadaten der bestehenden SelectedValue-Property
            SelectedValueProperty.OverrideMetadata(typeof(LoggingComboBox), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSelectedValuePropertyChanged)));
        }

        public LoggingComboBox()
        {
            // Die ComboBox fest mit Zoom-Werten füllen
            this.Items.Add(NOLOG);
            this.Items.Add(DEBUGLOG); /* 10 */
            this.Items.Add(INFOLOG); /* 20 */
            this.Items.Add(WARNINGLOG); /* 30 */
            this.Items.Add(ERRORLOG); /* 40 */
            this.Items.Add(CRITICALLOG); /* 50 */

            // Standardmäßig nichts oder den ersten Wert auswählen (optional)
            this.SelectedIndex = 0;
            this.Width = 100;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.Height = 20;
            this.IsEditable = true;
            this.IsReadOnly = true; // Verhindert die Eingabe von benutzerdefiniertem Text
            this.FontWeight = FontWeights.Bold;
            this.DropDownOpened += (s, e) => { this.Foreground = Brushes.Black; };
        }

        // Registrierung der umbenannten Dependency Property (ResultSelection)
        public static readonly DependencyProperty ResultSelectionProperty =
            DependencyProperty.Register(
                nameof(ResultSelection),
                typeof(double),
                typeof(LoggingComboBox),
                new FrameworkPropertyMetadata(
                    100.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnResultSelectionChanged)));

        public double ResultSelection
        {
            get => (double)GetValue(ResultSelectionProperty);
            set => SetValue(ResultSelectionProperty, value);
        }

        private static void OnResultSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = (LoggingComboBox)d;
            double newValue = (double)e.NewValue;

            newValue = (double)e.NewValue == 0 ? 100 : (double)e.NewValue;

            comboBox.UpdateSelectionByExternalValue(newValue);
        }

        // EVENT A: Wird ausgelöst, wenn sich die interne Auswahl ändert
        private static void OnSelectedValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = (LoggingComboBox)d;
            comboBox.UpdateVisualsAndStateBySelection(e.NewValue);
        }

        private void UpdateVisualsAndStateBySelection(object newVal)
        {
            // 1. Absicherung: Wenn der neue Wert null ist (nichts ausgewählt), ist das Ergebnis false
            if (newVal == null)
            {
                this.Foreground = Brushes.Green;
                this.ResultSelection = 0;
                return;
            }

            // 2. Variante A: Prüfung, wenn die ComboBox Text/Strings enthält
            if (newVal is string selectedText)
            {
                if (selectedText == NOLOG)
                {
                    this.Foreground = Brushes.Green; 
                    this.ResultSelection = 0;
                    return;
                }
                else if (selectedText == DEBUGLOG)
                {
                    this.Foreground = Brushes.Black;
                    this.ResultSelection = 10;
                    return;
                }
                else if (selectedText == INFOLOG)
                {
                    this.Foreground = Brushes.Black;
                    this.ResultSelection = 20;
                    return;
                }
                else if (selectedText == WARNINGLOG)
                {
                    this.Foreground = Brushes.Black;
                    this.ResultSelection = 30;
                    return;
                }
                else if (selectedText == ERRORLOG)
                {
                    this.Foreground = Brushes.Red;
                    this.ResultSelection = 40;
                    return;
                }
                else if (selectedText == CRITICALLOG)
                {
                    this.Foreground = Brushes.DarkRed;
                    this.ResultSelection = 50;
                    return;
                }
            }

            // Standard-Rückfallwert, falls kein Datentyp oben zutrifft
            this.Foreground = Brushes.Green;
            this.ResultSelection = 0;
        }

        private void UpdateSelectionByExternalValue(double externalValue)
        {
            string targetText = externalValue.ToString(CultureInfo.CurrentCulture);

            foreach (var item in this.Items)
            {
                string itemText = string.Empty;

                if (item is ComboBoxItem comboItem)
                {
                    itemText = comboItem.Content?.ToString();
                }
                else
                {
                    itemText = item?.ToString();
                }

                if (itemText != null && itemText.Trim().Equals(targetText, StringComparison.OrdinalIgnoreCase))
                {
                    if (this.SelectedItem != item)
                    {
                        this.SelectedItem = item;
                    }

                    return;
                }
            }
        }
    }
}
