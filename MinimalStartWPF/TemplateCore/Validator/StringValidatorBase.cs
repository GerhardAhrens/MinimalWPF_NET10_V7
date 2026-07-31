namespace System.Windows
{
    public abstract class StringValidatorBase : IValidator<string>
    {
        public ValidationResult Validate(string value)
        {
            var result = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value))
            {
                result.AddError("Der Wert darf nicht leer sein.");
                return result;
            }

            ValidateCore(value, result);

            return result;
        }

        protected abstract void ValidateCore(string value, ValidationResult result);
    }
}
