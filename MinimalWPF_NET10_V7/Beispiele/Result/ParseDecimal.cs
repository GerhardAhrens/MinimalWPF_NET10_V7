namespace MinimalWPF.Beispiele
{
    public partial class Parsing
    {
        public static XResult<decimal> ParseDecimal(string value)
        {
            try
            {
                if (decimal.TryParse(value, out var n))
                {
                    return n;
                }

                return $"Ungültige Eingabe: {value}";
            }
            catch (Exception ex)
            {
                return $"Fehler beim Parsen: {ex.Message}";
            }
        }

    }
}
