namespace MinimalWPF.Test.Sample
{
    using System.Windows.Domain;

    public sealed record Address
    {
        public string Street { get; }

        public string ZipCode { get; }

        public string City { get; }

        private Address(string street, string zipCode, string city)
        {
            Street = street;
            ZipCode = zipCode;
            City = city;
        }

        public static DomainResult<Address> Create(string street, string zipCode, string city)
        {
            if (string.IsNullOrEmpty(street) == true)
            {
                return DomainResult<Address>.Fail(CustomerErrors.AdressStreetEmpty);
            }

            if (string.IsNullOrEmpty(zipCode) == true)
            {
                return DomainResult<Address>.Fail(CustomerErrors.AdressZipFalse);
            }

            if (string.IsNullOrEmpty(city) == true)
            {
                return DomainResult<Address>.Fail(CustomerErrors.AdressCityEmpty);
            }

            return DomainResult<Address>.Ok(new Address(street, zipCode,city));
        }

        public override string ToString() => $"{Street}, {ZipCode} {City}";
    }
}
