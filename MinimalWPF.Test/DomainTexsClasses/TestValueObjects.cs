namespace MinimalWPF.Test.Sample
{
    using System.Windows;

    public sealed class FirstName : KindOf<string>
    {
        public static DomainResult<FirstName> Create(string value)
        {
            if (value.Equals("Dagober", StringComparison.CurrentCultureIgnoreCase))
            {
                return "Kein Dagobert erlaubt";
            }

            return new FirstName(value);
        }

        FirstName(string value) : base(value) { }
    }

    public sealed class LastName : KindOf<string>
    {
        public static DomainResult<LastName> Create(string value) => new LastName(value);

        LastName(string value) : base(value) { }
    }

    public sealed class EMail : KindOf<string>
    {
        public static DomainResult<EMail> Create(string value)
        {
            if (value.Contains("@") == false)
            {
                return $"die EMail Adresse hat das falsche Format";
            }

            return new EMail(value);
        }

        EMail(string value) : base(value) { }
    }

    public sealed record FullName
    {
        public static DomainResult<FullName> Create(FirstName firstName, LastName lastName)
        {
            if (firstName.Value == "Gustav" && lastName.Value == "Gans")
            {
                return $"{firstName.Value} {lastName.Value} ist auf der Sperrliste";
            }

            return new FullName(firstName, lastName);
        }

        FullName(FirstName firstName, LastName lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public FirstName FirstName { get; }
        public LastName LastName { get; }
    }

    public sealed class Person
    {
        public static DomainResult<Person> Create(FullName fullName, EMail eMail)
        {
            if (!eMail.Value.Contains(fullName.LastName, StringComparison.OrdinalIgnoreCase))
            {
                return "Die EMail Adresse muß den Nachnamen beinhalten";
            }

            return new Person(fullName, eMail);
        }

        Person(FullName fullName, EMail eMail)
        {
            this.FullName = fullName;
            this.EMail = eMail;
        }

        public FullName FullName { get; }
        public EMail EMail { get; }
    }
}
