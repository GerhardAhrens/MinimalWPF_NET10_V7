namespace System.Windows.Controls
{
    using System.Windows;
    using System.Windows.Input;

    public static class TextBoxIntegerBehavior
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(TextBoxIntegerBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        public static void SetIsEnabled(DependencyObject element, bool value) => element.SetValue(IsEnabledProperty, value);

        public static bool GetIsEnabled(DependencyObject element) => (bool)element.GetValue(IsEnabledProperty);

        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.RegisterAttached("AllowNegative", typeof(bool), typeof(TextBoxIntegerBehavior), new PropertyMetadata(false));

        public static void SetAllowNegative(DependencyObject element, bool value) => element.SetValue(AllowNegativeProperty, value);

        public static bool GetAllowNegative(DependencyObject element) => (bool)element.GetValue(AllowNegativeProperty);


        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox textBox)
                return;

            if ((bool)e.NewValue == true)
            {
                // Text nach rechts ausrichten
                textBox.TextAlignment = TextAlignment.Right;
                textBox.PreviewTextInput += OnPreviewTextInput;
                DataObject.AddPastingHandler(textBox, OnPaste);
            }
            else
            {
                // Text nach links ausrichten
                textBox.TextAlignment = TextAlignment.Left;
                textBox.PreviewTextInput -= OnPreviewTextInput;
                DataObject.RemovePastingHandler(textBox, OnPaste);
            }
        }


        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is not TextBox textBox)
                return;

            e.Handled = !IsValidInput(textBox, e.Text);
        }


        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (sender is not TextBox textBox)
                return;

            if (!e.DataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            var pastedText = e.DataObject.GetData(DataFormats.Text) as string;

            if (pastedText == null || !IsValidInput(textBox, pastedText))
            {
                e.CancelCommand();
            }
        }


        private static bool IsValidInput(TextBox textBox, string input)
        {
            var selectionStart = textBox.SelectionStart;
            var selectionLength = textBox.SelectionLength;

            var newText = textBox.Text
                .Remove(selectionStart, selectionLength)
                .Insert(selectionStart, input);

            // Leere Eingabe erlauben.
            if (string.IsNullOrEmpty(newText))
                return true;

            // Minus ist nur erlaubt, wenn AllowNegative aktiviert ist.
            if (!GetAllowNegative(textBox) && newText.Contains('-'))
                return false;

            // Wenn negative Werte erlaubt sind:
            // '-' darf nur am Anfang und maximal einmal vorkommen.
            if (GetAllowNegative(textBox))
            {
                if (newText.Count(c => c == '-') > 1)
                    return false;

                if (newText.Contains('-') && !newText.StartsWith("-"))
                    return false;
            }

            return int.TryParse(newText, out _);
        }
    }
}
