namespace MinimalWPF.Beispiele
{
    using System.Windows;

    public sealed class GermanPhoneNumberValidator : StringValidatorBase
    {
        protected override void ValidateCore(string value, ValidationResult result)
        {
            // Leerzeichen, Trennzeichen und Klammern entfernen
            var phone = value
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("/", "")
                .Replace("(", "")
                .Replace(")", "");

            if (phone.StartsWith("+49", StringComparison.OrdinalIgnoreCase) == false || phone.StartsWith("0049", StringComparison.OrdinalIgnoreCase) == false)
            {
                result.AddError($"Falsche Länderkennung => {phone}.");
                return;
            }


            // +49 in 0 umwandeln
            if (phone.StartsWith("+49", StringComparison.OrdinalIgnoreCase) == true)
            {
                phone = "0" + phone[3..];
            }

            // 0049 in 0 umwandeln
            if (phone.StartsWith("0049", StringComparison.OrdinalIgnoreCase) == true)
            {
                phone = "0" + phone[4..];
            }

            // Muss mit 0 beginnen
            if (phone.StartsWith('0') == false)
            {
                result.AddError("Die Telefonnummer muss mit einer deutschen Vorwahl beginnen.");
            }

            // Nur Ziffern erlaubt
            if (phone.All(char.IsDigit) == false)
            {
                result.AddError("Die Telefonnummer enthält ungültige Zeichen.");
            }

            // Plausibilitätsprüfung
            if (phone.Length < 5 || phone.Length > 15)
            {
                result.AddError("Die Telefonnummer hat eine ungültige Länge.");
            }
        }
    }
}
