namespace System.Windows.Domain
{
    using System;
    using System.Text.RegularExpressions;

    public static class Guard
    {
        public static void NotNull<T>(T value, string parameterName)
        {
            ArgumentNullException.ThrowIfNull(value, parameterName);
        }

        public static void NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} darf nicht leer sein.", parameterName);
            }
        }

        public static void MaxLength(string value, int maxLength, string parameterName)
        {
            if (value.Length > maxLength)
            {
                throw new ArgumentException($"{parameterName} darf maximal {maxLength} Zeichen besitzen.", parameterName);
            }
        }

        public static void Positive(decimal value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Wert muss größer 0 sein.");
            }
        }

        public static void Positive(int value, string parameterName)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName, "Wert muss größer 0 sein.");
            }
        }

        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static void Email(string value, string parameterName)
        {
            NotNullOrWhiteSpace(value, parameterName);

            if (EmailRegex.IsMatch(value) == false)
            {
                throw new ArgumentException("Ungültige E-Mail-Adresse.", parameterName);
            }
        }
    }
}
