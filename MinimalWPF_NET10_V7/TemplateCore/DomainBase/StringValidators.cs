namespace System.Windows
{
    using StringValidator = System.Func<string, DomainValidationResult<string>>;

    public static class StringValidators
    {
        public static StringValidator NonEmpty => From(str => str == string.Empty, "Der Inhalt darf nicht leer sein");

        public static StringValidator SingleLine => From(str => 
        str.Contains('\n',StringComparison.OrdinalIgnoreCase) || str.Contains('\r',StringComparison.OrdinalIgnoreCase), "Die Angabe muß Einzeilig sein");

        public static StringValidator NoWhitespace => From(str => !str.Any(c => char.IsWhiteSpace(c)), "darf keine Leerzeichen enthalten");

        public static StringValidator MaxLength(int maxLength) =>
            From(str => str.Length > maxLength, $"darf nicht länger sein als {maxLength} Zeichen");

        public static StringValidator MinLength(int minLength) =>
            From(str => str.Length < minLength, $"darf nicht kürzer sein als {minLength} Zeichen");

        public static StringValidator From(Func<string, bool> isValid, string invalidMessage) =>
            str => isValid(str) ? DomainValidationResult.Success<string>() : DomainValidationResult.Failure(invalidMessage, str);
    }
}
