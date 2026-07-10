namespace MinimalWPF.Beispiele
{
    using System;
    using System.Windows;

    public class EmailValidator : IValidator<string>
    {
        public ValidationResult Validate(string value)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value) == true)
            {
                result.AddError("Email Adresse darf nicht leer sein.");
                return result;
            }


            return result;
        }
    }
}
