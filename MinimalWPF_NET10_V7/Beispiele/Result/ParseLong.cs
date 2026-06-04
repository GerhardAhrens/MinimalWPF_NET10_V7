namespace MinimalWPF.Beispiele
{
    using System.Windows;

    public class Parsing
    {
        public static Result<long> ParseLong(string value)
        {
            try
            {
                if (long.TryParse(value, out var n))
                {
                    return Result<long>.Success(n);
                }

                return Result<long>.Fail($"Ungültige Eingabe: {value}");
            }
            catch (Exception ex)
            {
                return Result<long>.Fail($"Fehler beim Parsen: {ex.Message}");  
            }
        }

    }
}
