namespace MinimalWPF.Beispiele
{
    using System;
    using System.Globalization;
    using System.Windows;

    public class ZipCodeValidator : IValidator<string>
    {
        public ValidationResult Validate(string value)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value) == true || value.ToString(CultureInfo.CurrentCulture).Length != 5)
            {
                result.AddError("Die Postleitzahl darf nicht leer sein, bzw. muss 5 stellig sein.");
                return result;
            }


            return result;
        }
    }
}
