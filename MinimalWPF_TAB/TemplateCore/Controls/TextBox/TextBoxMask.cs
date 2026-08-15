namespace System.Windows.Controls
{
    using System.Text;
    using System.Windows.Input;

    public class TextBoxMask : TextBox
    {
        #region Dependency Properties

        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register(
                nameof(Mask),
                typeof(string),
                typeof(TextBoxMask),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnMaskChanged));

        public string Mask
        {
            get => (string)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        public static readonly DependencyProperty RawTextProperty =
            DependencyProperty.Register(
                nameof(RawText),
                typeof(string),
                typeof(TextBoxMask),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnRawTextChanged));

        /// <summary>
        /// Der Inhalt ohne Maskenliterale.
        /// </summary>
        public string RawText
        {
            get => (string)GetValue(RawTextProperty) ?? string.Empty;
            set => SetValue(RawTextProperty, value ?? string.Empty);
        }

        /// <summary>
        /// Alias für RawText.
        /// </summary>
        public string Value
        {
            get => RawText;
            set => RawText = value;
        }

        #endregion

        #region Fields

        private readonly List<MaskPart> _parts = new();

        private bool _internalUpdate;
        private bool _isUndoRedo;

        private readonly Stack<EditState> _undoStack = new();
        private readonly Stack<EditState> _redoStack = new();

        private const int MaxUndoSteps = 100;

        #endregion

        #region Constructor

        /*
        static TextBoxMask()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(TextBoxMask),
                new FrameworkPropertyMetadata(typeof(TextBoxMask)));
        }
        */

        public TextBoxMask()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, OnNewPaste));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Copy, OnNewCopy));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Cut, OnNewCut));

            Loaded += (_, _) =>
            {
                ParseMask();
                NormalizeRawText();
                RefreshText();
            };
        }

        #endregion

        #region Mask Parsing

        private void ParseMask()
        {
            _parts.Clear();

            if (string.IsNullOrEmpty(Mask))
                return;

            bool lowerCase = false;
            bool upperCase = false;
            bool escape = false;

            foreach (char c in Mask)
            {
                if (escape)
                {
                    _parts.Add(MaskPart.Literal(c));
                    escape = false;
                    continue;
                }

                if (c == '\\')
                {
                    escape = true;
                    continue;
                }

                if (c == '<')
                {
                    lowerCase = true;
                    upperCase = false;
                    continue;
                }

                if (c == '>')
                {
                    upperCase = true;
                    lowerCase = false;
                    continue;
                }

                if (c == '|')
                {
                    lowerCase = false;
                    upperCase = false;
                    continue;
                }

                if (IsPlaceholder(c))
                {
                    _parts.Add(
                        MaskPart.Placeholder(
                            c,
                            lowerCase,
                            upperCase));
                }
                else
                {
                    _parts.Add(MaskPart.Literal(c));
                }
            }
        }

        private static bool IsPlaceholder(char c)
        {
            return c switch
            {
                '0' => true,
                '9' => true,
                '#' => true,
                'L' => true,
                '?' => true,
                '&' => true,
                'C' => true,
                'A' => true,
                'a' => true,

                // Benutzerdefinierter Platzhalter
                '_' => true,

                _ => false
            };
        }

        private static bool IsOptional(char c)
        {
            return c switch
            {
                '9' => true,
                '?' => true,
                'C' => true,
                'a' => true,
                _ => false
            };
        }

        #endregion

        #region Character Validation

        private static bool Accepts(char mask, char value)
        {
            return mask switch
            {
                '0' => char.IsDigit(value),

                '9' =>
                    char.IsDigit(value) ||
                    value == ' ',

                '#' =>
                    char.IsDigit(value) ||
                    value == ' ',

                'L' =>
                    char.IsLetter(value),

                '?' =>
                    char.IsLetter(value),

                '&' =>
                    true,

                'C' =>
                    true,

                'A' =>
                    char.IsLetterOrDigit(value),

                'a' =>
                    char.IsLetterOrDigit(value),

                '_' =>
                    true,

                _ =>
                    false
            };
        }

        private static char ConvertCharacter(
            char value,
            bool lowerCase,
            bool upperCase)
        {
            if (lowerCase)
                return char.ToLowerInvariant(value);

            if (upperCase)
                return char.ToUpperInvariant(value);

            return value;
        }

        #endregion

        #region Rendering

        private void RefreshText()
        {
            if (_internalUpdate)
                return;

            ParseMask();

            string oldText = Text;

            int rawCaret =
                CaretIndexToRawIndex(CaretIndex);

            string formatted = FormatRawText(RawText);

            _internalUpdate = true;

            try
            {
                Text = formatted;

                int newCaret = RawIndexToCaretIndex(Math.Min(rawCaret, RawText == null ? 0 :RawText.Length));

                CaretIndex = Math.Min( newCaret, Text.Length);

                SelectionLength = 0;
            }
            finally
            {
                _internalUpdate = false;
            }
        }

        private string FormatRawText(string raw)
        {
            if (_parts.Count == 0)
                return raw ?? string.Empty;

            raw ??= string.Empty;

            var result = new StringBuilder();

            int rawIndex = 0;

            foreach (MaskPart part in _parts)
            {
                if (part.IsLiteral)
                {
                    result.Append(part.Character);
                    continue;
                }

                if (rawIndex < raw.Length)
                {
                    char value = raw[rawIndex];

                    value = ConvertCharacter(
                        value,
                        part.LowerCase,
                        part.UpperCase);

                    result.Append(value);

                    rawIndex++;
                }
                else
                {
                    // Leere Maskenposition anzeigen
                    result.Append('_');
                }
            }

            return result.ToString();
        }

        #endregion

        #region Keyboard Input

        protected override void OnPreviewTextInput(
            TextCompositionEventArgs e)
        {
            if (_internalUpdate)
            {
                base.OnPreviewTextInput(e);
                return;
            }

            e.Handled = true;

            ReplaceSelectionWith(e.Text);
        }

        protected override void OnPreviewKeyDown(
            KeyEventArgs e)
        {
            if (_internalUpdate)
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            bool ctrl =
                (Keyboard.Modifiers & ModifierKeys.Control) != 0;

            bool shift =
                (Keyboard.Modifiers & ModifierKeys.Shift) != 0;

            // Undo
            if (ctrl && e.Key == Key.Z)
            {
                if (shift)
                    Redo();
                else
                    Undo();

                e.Handled = true;
                return;
            }

            // Redo
            if (ctrl && e.Key == Key.Y)
            {
                Redo();

                e.Handled = true;
                return;
            }

            switch (e.Key)
            {
                case Key.Back:

                    if (SelectionLength > 0)
                        DeleteSelection();
                    else
                        Backspace();

                    e.Handled = true;
                    return;

                case Key.Delete:

                    if (SelectionLength > 0)
                        DeleteSelection();
                    else
                        Delete();

                    e.Handled = true;
                    return;

                case Key.Left:

                    MoveCaretLeft(shift);

                    e.Handled = true;
                    return;

                case Key.Right:

                    MoveCaretRight(shift);

                    e.Handled = true;
                    return;

                case Key.Home:

                    if (shift)
                    {
                        int old = CaretIndex;
                        CaretIndex = 0;
                        SelectFrom(old);
                    }
                    else
                    {
                        CaretIndex = 0;
                        SelectionLength = 0;
                    }

                    e.Handled = true;
                    return;

                case Key.End:

                    if (shift)
                    {
                        int old = CaretIndex;
                        CaretIndex = Text.Length;
                        SelectFrom(old);
                    }
                    else
                    {
                        CaretIndex = Text.Length;
                        SelectionLength = 0;
                    }

                    e.Handled = true;
                    return;
            }

            base.OnPreviewKeyDown(e);
        }

        #endregion

        #region Paste / Cut / Copy

        private void OnNewPaste(object sender, ExecutedRoutedEventArgs e)
        {
            if (!Clipboard.ContainsText())
                return;

            string clipboardText = Clipboard.GetText();

            ReplaceSelectionWith(clipboardText);

            e.Handled = true;
        }

        private void OnNewCopy(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectionLength <= 0)
                return;

            string rawText = GetRawTextFromSelection();

            if (!string.IsNullOrEmpty(rawText))
                Clipboard.SetText(rawText);

            e.Handled = true;
        }

        private void OnNewCut(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectionLength <= 0)
                return;

            string rawText = GetRawTextFromSelection();

            if (!string.IsNullOrEmpty(rawText))
                Clipboard.SetText(rawText);

            DeleteSelection();

            e.Handled = true;
        }

        #endregion

        #region Insert

        private void ReplaceSelectionWith(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;

            SaveUndoState();

            int startRaw =
                CaretIndexToRawIndex(SelectionStart);

            int endRaw =
                CaretIndexToRawIndex(
                    SelectionStart + SelectionLength);

            string current =
                RawText ?? string.Empty;

            if (endRaw > startRaw)
            {
                int removeLength =
                    Math.Min(
                        endRaw - startRaw,
                        current.Length - startRaw);

                if (removeLength > 0)
                {
                    current =
                        current.Remove(
                            startRaw,
                            removeLength);
                }
            }

            int rawIndex = startRaw;

            foreach (char inputCharacter in input)
            {
                int maskIndex =
                    GetNextEditableMaskIndex(rawIndex);

                if (maskIndex < 0)
                    break;

                MaskPart part =
                    _parts[maskIndex];

                if (!Accepts(
                        part.Character,
                        inputCharacter))
                {
                    continue;
                }

                char value =
                    ConvertCharacter(
                        inputCharacter,
                        part.LowerCase,
                        part.UpperCase);

                if (rawIndex < current.Length)
                {
                    current =
                        current.Remove(
                            rawIndex,
                            1);

                    current =
                        current.Insert(
                            rawIndex,
                            value.ToString());
                }
                else
                {
                    current += value;
                }

                rawIndex++;
            }

            SetRawTextInternal(current);

            RefreshText();

            CaretIndex =
                RawIndexToCaretIndex(rawIndex);

            SelectionLength = 0;
        }

        #endregion

        #region Delete

        private void Backspace()
        {
            int rawIndex =
                CaretIndexToRawIndex(CaretIndex);

            if (rawIndex <= 0)
                return;

            SaveUndoState();

            rawIndex--;

            string value = RawText;

            if (rawIndex < value.Length)
            {
                value =
                    value.Remove(
                        rawIndex,
                        1);
            }

            SetRawTextInternal(value);

            RefreshText();

            CaretIndex =
                RawIndexToCaretIndex(rawIndex);
        }

        private void Delete()
        {
            int rawIndex =
                CaretIndexToRawIndex(CaretIndex);

            if (rawIndex >= RawText.Length)
                return;

            SaveUndoState();

            string value = RawText;

            value =
                value.Remove(
                    rawIndex,
                    1);

            SetRawTextInternal(value);

            RefreshText();

            CaretIndex =
                RawIndexToCaretIndex(rawIndex);
        }

        private void DeleteSelection()
        {
            int startRaw =
                CaretIndexToRawIndex(
                    SelectionStart);

            int endRaw =
                CaretIndexToRawIndex(
                    SelectionStart +
                    SelectionLength);

            if (endRaw <= startRaw)
                return;

            SaveUndoState();

            string value =
                RawText.Remove(
                    startRaw,
                    endRaw - startRaw);

            SetRawTextInternal(value);

            RefreshText();

            CaretIndex =
                RawIndexToCaretIndex(startRaw);

            SelectionLength = 0;
        }

        #endregion

        #region Cursor

        private void MoveCaretLeft(bool shift)
        {
            int oldCaret = CaretIndex;

            if (CaretIndex <= 0)
                return;

            int raw =
                CaretIndexToRawIndex(
                    CaretIndex);

            raw =
                Math.Max(
                    0,
                    raw - 1);

            CaretIndex =
                RawIndexToCaretIndex(raw);

            if (shift)
                SelectFrom(oldCaret);
            else
                SelectionLength = 0;
        }

        private void MoveCaretRight(bool shift)
        {
            int oldCaret = CaretIndex;

            int raw =
                CaretIndexToRawIndex(
                    CaretIndex);

            if (raw >= RawText.Length)
                return;

            raw++;

            CaretIndex =
                RawIndexToCaretIndex(raw);

            if (shift)
                SelectFrom(oldCaret);
            else
                SelectionLength = 0;
        }

        private void SelectFrom(int anchor)
        {
            int current = CaretIndex;

            if (current >= anchor)
            {
                SelectionStart = anchor;
                SelectionLength =
                    current - anchor;
            }
            else
            {
                SelectionStart = current;
                SelectionLength =
                    anchor - current;
            }
        }

        #endregion

        #region Raw / Visual Mapping

        private int CaretIndexToRawIndex(int caret)
        {
            if (_parts.Count == 0)
                return Math.Min(
                    caret,
                    RawText.Length);

            int visual = 0;
            int raw = 0;

            foreach (MaskPart part in _parts)
            {
                if (visual >= caret)
                    break;

                visual++;

                if (!part.IsLiteral)
                {
                    if (raw < RawText.Length)
                        raw++;
                }
            }

            return Math.Min(raw, RawText == null ? 0 :RawText.Length);
        }

        private int RawIndexToCaretIndex(int rawIndex)
        {
            if (_parts.Count == 0)
                return Math.Min(
                    rawIndex,
                    RawText.Length);

            int visual = 0;
            int raw = 0;

            foreach (MaskPart part in _parts)
            {
                if (part.IsLiteral)
                {
                    /*
                     * Literale werden automatisch übersprungen,
                     * wenn der Cursor davor landet und noch Inhalt
                     * folgt.
                     */
                    if (raw < rawIndex)
                        visual++;

                    continue;
                }

                if (raw >= rawIndex)
                    break;

                raw++;
                visual++;
            }

            return visual;
        }

        private int GetNextEditableMaskIndex(
            int rawIndex)
        {
            int currentRaw = 0;

            for (int i = 0; i < _parts.Count; i++)
            {
                if (_parts[i].IsLiteral)
                    continue;

                if (currentRaw == rawIndex)
                    return i;

                currentRaw++;
            }

            return -1;
        }

        #endregion

        #region Selection / Copy

        private string GetRawTextFromSelection()
        {
            if (SelectionLength <= 0)
                return string.Empty;

            int startRaw =
                CaretIndexToRawIndex(
                    SelectionStart);

            int endRaw =
                CaretIndexToRawIndex(
                    SelectionStart +
                    SelectionLength);

            if (endRaw <= startRaw)
                return string.Empty;

            return RawText.Substring(
                startRaw,
                endRaw - startRaw);
        }

        #endregion

        #region Undo / Redo

        private void SaveUndoState()
        {
            if (_isUndoRedo)
                return;

            _undoStack.Push(
                new EditState(
                    RawText,
                    CaretIndex,
                    SelectionStart,
                    SelectionLength));

            while (_undoStack.Count > MaxUndoSteps)
            {
                RemoveOldest(_undoStack);
            }

            _redoStack.Clear();
        }

        new private void Undo()
        {
            if (_undoStack.Count == 0)
                return;

            EditState current =
                new(
                    RawText,
                    CaretIndex,
                    SelectionStart,
                    SelectionLength);

            EditState previous =
                _undoStack.Pop();

            _redoStack.Push(current);

            RestoreState(previous);
        }

        new private void Redo()
        {
            if (_redoStack.Count == 0)
                return;

            EditState current =
                new(
                    RawText,
                    CaretIndex,
                    SelectionStart,
                    SelectionLength);

            EditState next =
                _redoStack.Pop();

            _undoStack.Push(current);

            RestoreState(next);
        }

        private void RestoreState(EditState state)
        {
            _isUndoRedo = true;

            try
            {
                SetRawTextInternal(state.RawText);

                RefreshText();

                CaretIndex =
                    Math.Min(
                        state.CaretIndex,
                        Text.Length);

                SelectionStart =
                    Math.Min(
                        state.SelectionStart,
                        Text.Length);

                SelectionLength =
                    Math.Min(
                        state.SelectionLength,
                        Text.Length -
                        SelectionStart);
            }
            finally
            {
                _isUndoRedo = false;
            }
        }

        private static void RemoveOldest(
            Stack<EditState> stack)
        {
            if (stack.Count == 0)
                return;

            EditState[] states =
                stack.ToArray();

            stack.Clear();

            for (int i = states.Length - 2;
                 i >= 0;
                 i--)
            {
                stack.Push(states[i]);
            }
        }

        #endregion

        #region RawText

        private void SetRawTextInternal(string value)
        {
            _internalUpdate = true;

            try
            {
                SetCurrentValue(
                    RawTextProperty,
                    value ?? string.Empty);
            }
            finally
            {
                _internalUpdate = false;
            }
        }

        private static void OnRawTextChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBoxMask control)
                return;

            if (control._internalUpdate)
                return;

            control.RefreshText();
        }

        private static void OnMaskChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBoxMask control)
                return;

            control.ParseMask();

            /*
             * Wenn sich die Maske ändert, muss der bisherige
             * RawText-Wert gegen die neue Maske geprüft werden.
             */
            control.NormalizeRawText();

            control.RefreshText();
        }

        #endregion

        #region Normalization

        private void NormalizeRawText()
        {
            if (string.IsNullOrEmpty(RawText))
                return;

            string source = RawText;
            var result = new StringBuilder();

            int rawIndex = 0;

            foreach (char character in source)
            {
                int maskIndex =
                    GetNextEditableMaskIndex(
                        rawIndex);

                if (maskIndex < 0)
                    break;

                MaskPart part =
                    _parts[maskIndex];

                if (!Accepts(
                        part.Character,
                        character))
                {
                    continue;
                }

                result.Append(
                    ConvertCharacter(
                        character,
                        part.LowerCase,
                        part.UpperCase));

                rawIndex++;
            }

            SetRawTextInternal(
                result.ToString());
        }

        #endregion

        #region Mask Validation

        /// <summary>
        /// Gibt zurück, ob der aktuelle Inhalt
        /// alle Pflichtfelder der Maske erfüllt.
        /// </summary>
        public bool IsMaskComplete
        {
            get
            {
                int rawIndex = 0;

                foreach (MaskPart part in _parts)
                {
                    if (part.IsLiteral)
                        continue;

                    if (rawIndex >= RawText.Length)
                    {
                        if (!IsOptional(part.Character))
                            return false;

                        continue;
                    }

                    if (!Accepts(
                            part.Character,
                            RawText[rawIndex]))
                    {
                        return false;
                    }

                    rawIndex++;
                }

                return true;
            }
        }

        #endregion

        #region MaskPart

        private sealed class MaskPart
        {
            public bool IsLiteral { get; private init; }

            public char Character { get; private init; }

            public bool LowerCase { get; private init; }

            public bool UpperCase { get; private init; }

            public static MaskPart Literal(char character)
            {
                return new MaskPart
                {
                    IsLiteral = true,
                    Character = character
                };
            }

            public static MaskPart Placeholder(
                char character,
                bool lowerCase,
                bool upperCase)
            {
                return new MaskPart
                {
                    IsLiteral = false,
                    Character = character,
                    LowerCase = lowerCase,
                    UpperCase = upperCase
                };
            }
        }

        #endregion

        #region EditState

        private sealed class EditState
        {
            public string RawText { get; }

            public int CaretIndex { get; }

            public int SelectionStart { get; }

            public int SelectionLength { get; }

            public EditState(
                string rawText,
                int caretIndex,
                int selectionStart,
                int selectionLength)
            {
                RawText = rawText;
                CaretIndex = caretIndex;
                SelectionStart = selectionStart;
                SelectionLength = selectionLength;
            }
        }

        #endregion
    }
}
