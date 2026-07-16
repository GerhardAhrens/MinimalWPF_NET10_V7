namespace MinimalWPF.Test.Sample
{
    using System.Text.RegularExpressions;
    using System.Windows.Domain;

    public sealed record Email
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static DomainResult<Email> Create(string value)
        {
            if (string.IsNullOrEmpty(value) == true)
            {
                return DomainResult<Email>.Fail(CustomerErrors.InvalidEmail);
            }

            if (EmailRegex.IsMatch(value) == false)
            {
                return DomainResult<Email>.Fail(CustomerErrors.InvalidEmail);
            }

            return DomainResult<Email>.Ok(new Email(value));
        }
    }
}
