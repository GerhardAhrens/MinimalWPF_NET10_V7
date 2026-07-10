namespace MinimalWPF.Beispiele
{
    using System.Globalization;
    using System.Numerics;
    using System.Windows;

    public sealed class IbanValidator : IValidator<string>
    {
        private readonly HashSet<string> _allowedCountries;

        public IbanValidator(IEnumerable<string> allowedCountries)
        {
            _allowedCountries = new HashSet<string>(allowedCountries.Select(c => c.ToUpperInvariant()));
        }

        public ValidationResult Validate(string value)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value))
            {
                result.AddError("IBAN darf nicht leer sein.");
                return result;
            }

            value = value.Replace(" ", "").ToUpperInvariant();

            if (value.Length < 15 || value.Length > 34)
            {
                result.AddError($"Ungültige IBAN-Länge => '{value.Length}'.");
            }

            var country = value[..2];

            if (!_allowedCountries.Contains(country))
            {
                result.AddError($"Land '{country}' wird nicht unterstützt.");
            }

            string moved = value[4..] + value[..4];
            var numeric = string.Empty;
            foreach (char c in moved)
            {
                if (char.IsDigit(c))
                {
                    numeric += c;
                }
                else if (char.IsLetter(c))
                {
                    numeric += (c - 'A' + 10);
                }
                else
                {
                    result.AddError($"Ungültiges Zeichen in der IBAN => '{c}'.");
                    return result;
                }
            }

            var number = BigInteger.Parse(numeric,CultureInfo.CurrentCulture);
            if (number % 97 != 1)
            {
                result.AddError("IBAN-Prüfsumme ist ungültig.");
            }

            return result;
        }
    }
}
