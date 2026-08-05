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
                    // Text nach rechts ausrichten
                    textBox.TextAlignment = TextAlignment.Right;

                    // Events für Eingabebeschränkung und Formatierung
                    textBox.PreviewTextInput += TextBox_PreviewTextInput;
                    textBox.LostFocus += TextBox_LostFocus;
                    DataObject.AddPastingHandler(textBox, TextBox_Pasting);

                    // Neu: Events für automatische Markierung bei Fokus
                    textBox.GotKeyboardFocus += TextBox_GotKeyboardFocus;
                    textBox.PreviewMouseLeftButtonDown += TextBox_PreviewMouseLeftButtonDown;
                }
                else
                {
                    textBox.ClearValue(TextBox.TextAlignmentProperty);

                    textBox.PreviewTextInput -= TextBox_PreviewTextInput;
                    textBox.LostFocus -= TextBox_LostFocus;
                    DataObject.RemovePastingHandler(textBox, TextBox_Pasting);

                    textBox.GotKeyboardFocus -= TextBox_GotKeyboardFocus;
                    textBox.PreviewMouseLeftButtonDown -= TextBox_PreviewMouseLeftButtonDown;
                }
            }
        }

        // Neu: Markiert den Text, wenn der Fokus per Tastatur (z. B. Tab-Taste) kommt
        private static void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        // Neu: Markiert den Text bei einem Mausklick und verhindert das Aufheben durch das Standard-Click-Verhalten
        private static void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (!textBox.IsKeyboardFocusWithin)
                {
                    textBox.Focus();
                    e.Handled = true; // Verhindert, dass der Cursor an die Klick-Position springt
                }
            }
        }

        // Wenn das Feld leer ist, weise "0" zu und formatiere gültige Zahlen auf 2 Nachkommastellen
        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string text = textBox.Text.Trim();

                if (string.IsNullOrEmpty(text) || text == "-" || text == ",")
                {
                    textBox.Text = "0,00"; // Direkt als schönes Standardformat

                    var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                    binding?.UpdateSource();
                }
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
