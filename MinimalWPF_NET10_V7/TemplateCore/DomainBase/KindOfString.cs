namespace System.Windows
{
    public class KindOfString : KindOf<string>
    {
        public KindOfString(string value, Func<string, DomainValidationResult<string>> validate, Func<string, string> canonicalize)
            : base(
                validate(value).Match(
                    () => canonicalize(value),
                    error => throw new ArgumentException(string.Join("\n", error.ErrorMessage, $"Original Wert: {error.OriginalValue}"), nameof(value))))
        {
        }
    }
}
