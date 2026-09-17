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

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewTextInput += OnPreviewTextInput;
            this.AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            this.AssociatedObject.TextChanged += OnTextChanged;
            this.AssociatedObject.VerticalContentAlignment = VerticalAlignment.Center;
            this.AssociatedObject.VerticalAlignment = VerticalAlignment.Center;

            DataObject.AddPastingHandler(this.AssociatedObject, OnPaste);
            DataObject.AddCopyingHandler(this.AssociatedObject, OnCopy);
            DataObject.AddSettingDataHandler(this.AssociatedObject, OnSettingData);

            UpdateDisplayedText();
        }

        protected override void OnDetaching()
        {
            DataObject.RemovePastingHandler(this.AssociatedObject, OnPaste);
            DataObject.RemoveCopyingHandler(this.AssociatedObject, OnCopy);
            DataObject.RemoveSettingDataHandler(this.AssociatedObject, OnSettingData);

            this.AssociatedObject.PreviewTextInput -= OnPreviewTextInput;
            this.AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            this.AssociatedObject.TextChanged -= OnTextChanged;

            base.OnDetaching();
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
            if (AssociatedObject.MaxLength > 0 && AssociatedObject.Text.Length > AssociatedObject.MaxLength)
            {
                e.Handled = true;
                return;
            }

            var key = e.Key == Key.System ? e.SystemKey : e.Key;

            // Clipboard-Aktionen vollständig unterbinden.
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control) || Keyboard.Modifiers.HasFlag(ModifierKeys.Windows))
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
                this.DeleteForward();
                e.Handled = true;
                return;
            }

            // Backspace
            if (key == Key.Back)
            {
                this.DeleteBackward();
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
            if (this._updating == true)
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
            var password = this.Password ?? string.Empty;

            var start = this.AssociatedObject.SelectionStart;
            var length = this.AssociatedObject.SelectionLength;

            this.Password = password.Remove(start, length).Insert(start, text);

            var newPosition = start + text.Length;

            this.UpdateDisplayedText(newPosition, 0);
        }

        private void DeleteBackward()
        {
            var password = this.Password ?? string.Empty;
            var start = this.AssociatedObject.SelectionStart;
            var length = this.AssociatedObject.SelectionLength;

            if (length > 0)
            {
                this.Password = password.Remove(start, length);
                this.UpdateDisplayedText(start, 0);
                return;
            }

            if (start <= 0)
                return;

            this.Password = password.Remove(start - 1, 1);
            this.UpdateDisplayedText(start - 1, 0);
        }

        private void DeleteForward()
        {
            var password = this.Password ?? string.Empty;
            var start = this.AssociatedObject.SelectionStart;
            var length = this.AssociatedObject.SelectionLength;

            if (length > 0)
            {
                this.Password = password.Remove(start, length);
                this.UpdateDisplayedText(start, 0);
                return;
            }

            if (start >= password.Length)
            {
                return;
            }

            this.Password = password.Remove(start, 1);
            this.UpdateDisplayedText(start, 0);
        }

        private void UpdateDisplayedText(int? selectionStart = null, int? selectionLength = null)
        {
            if (this.AssociatedObject == null)
            {
                return;
            }

            _updating = true;

            try
            {
                var password = this.Password ?? string.Empty;

                this.AssociatedObject.Text = new string(this.MaskCharacter, password.Length);

                if (selectionStart.HasValue)
                {
                    this.AssociatedObject.SelectionStart = Math.Clamp(selectionStart.Value,  0, this.AssociatedObject.Text.Length);
                    this.AssociatedObject.SelectionLength = Math.Clamp(selectionLength ?? 0, 0, this.AssociatedObject.Text.Length - this.AssociatedObject.SelectionStart);
                }
                else
                {
                    this.AssociatedObject.SelectionStart = Math.Clamp(this.AssociatedObject.SelectionStart, 0, this.AssociatedObject.Text.Length);
                    this.AssociatedObject.SelectionLength = 0;
                }
            }
            finally
            {
                _updating = false;
            }
        }
    }
}
