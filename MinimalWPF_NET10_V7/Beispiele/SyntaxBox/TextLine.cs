namespace MinimalWPF.Beispiele
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Sichtbare Instanzfelder nicht deklarieren", Justification = "<Ausstehend>")]
    public class TextLine
    {
        public int LineNumber;
        /// <summary>
        /// The start index of the line relative to the entire text.
        /// </summary>
        public int StartIndex;
        public string Text;
        public int EndIndex => StartIndex + Text?.Length ?? 0;

    }
}
