namespace System.Windows.Controls
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    /// <summary>
    /// Behavior für eine WPF TextBox, die ausschließlich Integer-Werte akzeptiert.
    ///
    /// Eigenschaften:
    /// - AllowNegative: Erlaubt negative Zahlen.
    /// - Minimum: Optionaler Minimalwert.
    /// - Maximum: Optionaler Maximalwert.
    ///
    /// TextBox.MaxLength kann weiterhin verwendet werden.
    /// </summary>
    public sealed class IntegerTextBoxBehavior : Behavior<TextBox>
    {
        #region AllowNegative

        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register(
                nameof(AllowNegative),
                typeof(bool),
                typeof(IntegerTextBoxBehavior),
                new PropertyMetadata(false));

        public bool AllowNegative
        {
            get => (bool)GetValue(AllowNegativeProperty);
            set => SetValue(AllowNegativeProperty, value);
        }

        #endregion

        #region Minimum

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(
                nameof(Minimum),
                typeof(int?),
                typeof(IntegerTextBoxBehavior),
                new PropertyMetadata(null, OnRangePropertyChanged));

        public int? Minimum
        {
            get => (int?)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        #endregion

        #region Maximum

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(
                nameof(Maximum),
                typeof(int?),
                typeof(IntegerTextBoxBehavior),
                new PropertyMetadata(null, OnRangePropertyChanged));

        public int? Maximum
        {
            get => (int?)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        #endregion

        #region Attach / Detach

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
        }

        protected override void OnDetaching()
        {
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);

            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;

            base.OnDetaching();
        }

        #endregion

        #region Text Input

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = AssociatedObject;

            string proposedText = GetTextAfterReplacement(textBox, e.Text);

            e.Handled = !IsValidInput(proposedText);
        }

        #endregion

        #region Key Handling

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (AssociatedObject.MaxLength > 0 && AssociatedObject.Text.Length > AssociatedObject.MaxLength)
            {
                e.Handled = true;
                return;
            }

            var textBox = AssociatedObject;

            // Minus explizit behandeln, da "-" nicht über PreviewTextInput
            // auf allen Tastaturlayouts zuverlässig behandelt wird.
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                if (!AllowNegative)
                {
                    e.Handled = true;
                    return;
                }

                string proposedText = GetTextAfterReplacement(textBox, "-");

                e.Handled = !IsValidInput(proposedText);
            }
        }

        #endregion

        #region Paste

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(DataFormats.Text))
            {
                e.CancelCommand();
                return;
            }

            string pastedText = e.SourceDataObject.GetData(DataFormats.Text) as string;

            if (pastedText == null)
            {
                e.CancelCommand();
                return;
            }

            var textBox = AssociatedObject;

            string proposedText = GetTextAfterReplacement(textBox, pastedText);

            if (!IsValidInput(proposedText))
            {
                e.CancelCommand();
            }
        }

        #endregion

        #region Validation

        private bool IsValidInput(string text)
        {
            // Leere Eingabe muss möglich sein.
            // Dadurch bleiben Löschen und Rücktaste vollständig nutzbar.
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            // Zwischenzustand bei negativer Eingabe:
            // "-" darf zunächst stehen, damit "-123" getippt werden kann.
            if (AllowNegative && text == "-")
            {
                return true;
            }

            // Negative Zahlen nicht erlaubt
            if (!AllowNegative && text.Contains("-"))
            {
                return false;
            }

            // Nur ASCII-Ziffern erlauben
            foreach (char c in text)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            // Integer-Bereich prüfen
            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
            {
                return false;
            }

            // Negative Werte verhindern
            if (!AllowNegative && value < 0)
                return false;

            // Minimum
            if (Minimum.HasValue && value < Minimum.Value)
            {
                return false;
            }

            // Maximum
            if (Maximum.HasValue && value > Maximum.Value)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Helper

        /// <summary>
        /// Ermittelt den Text, der entstehen würde, wenn die aktuelle
        /// Selektion durch replacement ersetzt wird.
        /// </summary>
        private static string GetTextAfterReplacement(TextBox textBox, string replacement)
        {
            string text = textBox.Text ?? string.Empty;

            int selectionStart = textBox.SelectionStart;
            int selectionLength = textBox.SelectionLength;

            if (selectionStart < 0)
            {
                selectionStart = 0;
            }

            if (selectionStart > text.Length)
            {
                selectionStart = text.Length;
            }

            if (selectionLength < 0)
            {
                selectionLength = 0;
            }

            if (selectionStart + selectionLength > text.Length)
            {
                selectionLength = text.Length - selectionStart;
            }

            return text.Remove(selectionStart, selectionLength).Insert(selectionStart, replacement ?? string.Empty);
        }

        private static void OnRangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (IntegerTextBoxBehavior)d;

            if (behavior.Minimum.HasValue && behavior.Maximum.HasValue && behavior.Minimum.Value > behavior.Maximum.Value)
            {
                throw new ArgumentException("Minimum darf nicht größer als Maximum sein.");
            }
        }

        #endregion
    }
}
