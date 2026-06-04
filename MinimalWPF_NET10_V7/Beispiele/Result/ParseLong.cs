namespace MinimalWPF.Beispiele
{
    using System.Windows;

    public class Parsing
    {
        public static Result<long> ParseLong(string value)
        {
            if (long.TryParse(value, out var n))
            {
                return Result<long>.Success(n);
            }

            return Result<long>.Fail($"Ungültige Eingabe: {value}");
        }

    }
}
