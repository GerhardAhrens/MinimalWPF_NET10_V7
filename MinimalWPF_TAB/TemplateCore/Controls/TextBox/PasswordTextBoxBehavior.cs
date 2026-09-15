namespace System.Windows.Controls
{
    using System.Windows.Input;

    using Microsoft.Xaml.Behaviors;

    public sealed class PasswordTextBoxBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                nameof(Password),
                typeof(string),
                typeof(PasswordTextBoxBehavior),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty MaskCharacterProperty =
            DependencyProperty.Register(
                nameof(MaskCharacter),
                typeof(char),
                typeof(PasswordTextBoxBehavior),
                new PropertyMetadata('*', OnMaskCharacterChanged));

        public char MaskCharacter
        {
            get => (char)GetValue(MaskCharacterProperty);
            set => SetValue(MaskCharacterProperty, value);
        }

        private bool _updating;
        /*
        private int _selectionStart;
        private int _selectionLength;
        */
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            AssociatedObject.TextChanged += OnTextChanged;
            AssociatedObject.VerticalContentAlignment = VerticalAlignment.Center;
            AssociatedObject.VerticalAlignment = VerticalAlignment.Center;

            DataObject.AddPastingHandler(AssociatedObject, OnPaste);
            DataObject.AddCopyingHandler(AssociatedObject, OnCopy);
            DataObject.AddSettingDataHandler(AssociatedObject, OnSettingData);

            UpdateDisplayedText();
        }

        protected override void OnDetaching()
        {
            DataObject.RemovePastingHandler(AssociatedObject, OnPaste);
            DataObject.RemoveCopyingHandler(AssociatedObject, OnCopy);
            DataObject.RemoveSettingDataHandler(AssociatedObject, OnSettingData);

            AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            AssociatedObject.TextChanged -= OnTextChanged;

            base.OnDetaching();
        }

        private static void OnPasswordChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var behavior = (PasswordTextBoxBehavior)d;

            if (!behavior._updating)
            {
                behavior.UpdateDisplayedText();
            }
        }

        private static void OnMaskCharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PasswordTextBoxBehavior)d).UpdateDisplayedText();
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ReplaceSelection(e.Text);
            e.Handled = true;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key == Key.System ? e.SystemKey : e.Key;

            // Clipboard-Aktionen vollständig unterbinden.
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) ||
                Keyboard.Modifiers.HasFlag(ModifierKeys.Windows))
            {
                if (key is Key.C or Key.X or Key.V)
                {
                    e.Handled = true;
                    return;
                }
            }

            // Shift+Insert = Paste
            if (key == Key.Insert && Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                e.Handled = true;
                return;
            }

            // Delete
            if (key == Key.Delete)
            {
                DeleteForward();
                e.Handled = true;
                return;
            }

            // Backspace
            if (key == Key.Back)
            {
                DeleteBackward();
                e.Handled = true;
                return;
            }

            // Home / End / Cursorbewegung dürfen normal funktionieren.
            // Ctrl+A wird verhindert, damit keine sichtbare Selektion
            // des gesamten Passwortes entsteht.
            if (key == Key.A && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                e.Handled = true;
                return;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // Verhindert Änderungen, die nicht über unser Behavior laufen.
            if (_updating)
            {
                return;
            }

            this.UpdateDisplayedText();
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            e.CancelCommand();
        }

        private void OnCopy(object sender, DataObjectCopyingEventArgs e)
        {
            e.CancelCommand();
        }

        private void OnSettingData(object sender, DataObjectSettingDataEventArgs e)
        {
            e.Handled = true;
        }

        private void ReplaceSelection(string text)
        {
            var password = Password ?? string.Empty;

            var start = AssociatedObject.SelectionStart;
            var length = AssociatedObject.SelectionLength;

            Password = password.Remove(start, length).Insert(start, text);

            var newPosition = start + text.Length;

            this.UpdateDisplayedText(newPosition, 0);
        }

        private void DeleteBackward()
        {
            var password = Password ?? string.Empty;
            var start = AssociatedObject.SelectionStart;
            var length = AssociatedObject.SelectionLength;

            if (length > 0)
            {
                Password = password.Remove(start, length);
                this.UpdateDisplayedText(start, 0);
                return;
            }

            if (start <= 0)
                return;

            Password = password.Remove(start - 1, 1);
            this.UpdateDisplayedText(start - 1, 0);
        }

        private void DeleteForward()
        {
            var password = Password ?? string.Empty;
            var start = AssociatedObject.SelectionStart;
            var length = AssociatedObject.SelectionLength;

            if (length > 0)
            {
                Password = password.Remove(start, length);
                this.UpdateDisplayedText(start, 0);
                return;
            }

            if (start >= password.Length)
            {
                return;
            }

            Password = password.Remove(start, 1);
            this.UpdateDisplayedText(start, 0);
        }

        private void UpdateDisplayedText(int? selectionStart = null, int? selectionLength = null)
        {
            if (AssociatedObject == null)
            {
                return;
            }

            _updating = true;

            try
            {
                var password = Password ?? string.Empty;

                AssociatedObject.Text = new string(MaskCharacter, password.Length);

                if (selectionStart.HasValue)
                {
                    AssociatedObject.SelectionStart = Math.Clamp(selectionStart.Value,  0, AssociatedObject.Text.Length);
                    AssociatedObject.SelectionLength = Math.Clamp(selectionLength ?? 0, 0, AssociatedObject.Text.Length - AssociatedObject.SelectionStart);
                }
                else
                {
                    AssociatedObject.SelectionStart = Math.Clamp(AssociatedObject.SelectionStart, 0, AssociatedObject.Text.Length);
                    AssociatedObject.SelectionLength = 0;
                }
            }
            finally
            {
                _updating = false;
            }
        }
    }
}
