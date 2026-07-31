namespace System.Windows
{
    public sealed class ValidationResult
    {
        private readonly List<string> _errors = new();

        public bool IsValid => this._errors.Count == 0;

        public IReadOnlyList<string> Errors => this._errors;

        public void AddError(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                this._errors.Add(error);
            }
        }

        public static ValidationResult Success() => new();

        public static ValidationResult Fail(string error)
        {
            var result = new ValidationResult();
            result.AddError(error);
            return result;
        }
    }
}
