namespace MinimalWPF.Beispiele
{
    using System;
    using System.Windows;

    public class UrlValidator : IValidator<string>
    {
        public ValidationResult Validate(string value)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value) == true)
            {
                result.AddError("Url darf nicht leer sein.");
                return result;
            }

            if (value.Contains("http://") == false || value.Contains("https://") == false)
            {
                value = $"https://{value}";
            }

            bool ok = Uri.TryCreate(value, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (ok == false)
            {
                result.AddError("Ungültige Web-URL.");
            }

            return result;
        }
    }
}
