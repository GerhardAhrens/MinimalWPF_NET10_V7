namespace MinimalWPF.Test.Sample
{

    using System.Windows.Domain;

    public sealed record PersonName
    {
        public string FirstName { get; }

        public string LastName { get; }

        public PersonName(string firstName, string lastName)
        {
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void Deconstruct(out string firstName, out string lastName)
        {
            firstName = FirstName;
            lastName = LastName;
        }

        public static DomainResult<PersonName> Create(string firstName, string lastName)
        {
            Guard.NotNullOrWhiteSpace(firstName, nameof(firstName));
            Guard.NotNullOrWhiteSpace(lastName, nameof(lastName));
            return DomainResult<PersonName>.Ok(new PersonName(firstName, lastName));
        }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
