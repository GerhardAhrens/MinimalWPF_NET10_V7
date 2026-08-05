namespace System.Windows.Controls
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Input;

    public static class TextBoxDecimalExtensions
    {
        private static readonly CultureInfo GermanCulture = new CultureInfo("de-DE");

        public static readonly DependencyProperty IsDecimalOnlyProperty =
            DependencyProperty.RegisterAttached(
                "IsDecimalOnly",
                typeof(bool),
                typeof(TextBoxDecimalExtensions),
                new PropertyMetadata(false, OnIsDecimalOnlyChanged));

        public static bool GetIsDecimalOnly(DependencyObject obj) => (bool)obj.GetValue(IsDecimalOnlyProperty);
        public static void SetIsDecimalOnly(DependencyObject obj, bool value) => obj.SetValue(IsDecimalOnlyProperty, value);

        private static void OnIsDecimalOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                bool isEnabled = (bool)e.NewValue;

                if (isEnabled)
                {
                    textBox.TextAlignment = TextAlignment.Right;

                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    textBox.LostFocus += TextBox_LostFocus; // Neu: Event für Fokusverlust registrieren
                    DataObject.AddPastingHandler(textBox, TextBox_Pasting);
                }
                else
                {
                    textBox.ClearValue(TextBox.TextAlignmentProperty);

                    textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                    textBox.LostFocus -= TextBox_LostFocus;
                    DataObject.RemovePastingHandler(textBox, TextBox_Pasting);
                }
            }
        }

        // Neu: Wenn das Feld leer oder unvollständig ist, weise eine "0" oder "0,00" zu
        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string text = textBox.Text.Trim();

                // Wenn das Feld komplett leer ist oder nur ein Minus/Komma enthält
                if (string.IsNullOrEmpty(text) || text == "-" || text == ",")
                {
                    textBox.Text = "0";

                    // Erzwingt das Update der Datenbindung (Binding) an das ViewModel/die DataRow
                    var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                    binding?.UpdateSource();
                }
                // Optional: Formatiert eine gültige Zahl direkt schön auf zwei Nachkommastellen (z.B. "5" wird zu "5,00")
                else if (decimal.TryParse(text, NumberStyles.Any, GermanCulture, out decimal value))
                {
                    textBox.Text = value.ToString("F2", GermanCulture);

                    var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                    binding?.UpdateSource();
                }
            }
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string neuerText = GetProposedText(textBox, e.Text);

                if (!IsValidDecimal(neuerText))
                {
                    e.Handled = true;
                }
            }
        }

        private static void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is TextBox textBox && e.DataObject.GetDataPresent(typeof(string)))
            {
                string eingefuegterText = (string)e.DataObject.GetData(typeof(string));
                string neuerText = GetProposedText(textBox, eingefuegterText);

                if (!IsValidDecimal(neuerText))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static string GetProposedText(TextBox textBox, string neueEingabe)
        {
            string text = textBox.Text;
            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            return text.Remove(selectionStart, selectionLength).Insert(selectionStart, neueEingabe);
        }

        private static bool IsValidDecimal(string text)
        {
            if (string.IsNullOrEmpty(text)) return true;
            if (text == "-") return true;

            string pattern = @"^-?\d*(,\d{0,2})?$";
            return Regex.IsMatch(text, pattern);
        }
    }
}
