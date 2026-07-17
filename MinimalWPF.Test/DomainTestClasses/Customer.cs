namespace MinimalWPF.Test.Sample
{

    using System.Windows.Domain;

    public sealed class Customer : AuditableAggregateRoot<EntityId<Customer>>, IAuditable, ISoftDelete
    {
        public PersonName Fullname { get; private set; }

        public Email Email { get; private set; }

        public Address Address { get; private set; }

        public bool Active { get; private set; }

        // ISoftDelete
        public bool IsDeleted { get; private set; }

        public DateTime? DeletedOn { get; private set; }
        public string DeletedFrom { get; private set; }
        private Customer(EntityId<Customer> id, PersonName fullName, Email email, Address address) : base(id)
        {
            Fullname = fullName;
            Email = email;
            Address = address;

            Active = true;
        }

        public static DomainResult<Customer> Create(PersonName fullName, Email email, Address address)
        {
            var customer = new Customer(EntityId.New<Customer>(), fullName, email, address);

            customer.Raise(new CustomerCreated(customer.Id));
            customer.CreatedOn = DateTime.UtcNow;
            customer.CreatedFrom = Environment.UserName;
            return DomainResult<Customer>.Ok(customer);
        }

        public DomainResult RenamePerson(string firstName, string lastName)
        {
            var nameResult = PersonName.Create(firstName, lastName);

            if (nameResult.Success == false)
            {
                return DomainResult.Fail(nameResult.Errors);
            }

            if (Fullname == nameResult.Value)
            {
                return DomainResult.Ok();
            }

            Fullname = nameResult.Value!;
            base.ModifiedOn = DateTime.UtcNow;
            base.ModifiedFrom = Environment.UserName;
            Raise(new CustomerRenamed(Id, Fullname));

            return DomainResult.Ok();
        }

        public DomainResult ChangeEmail(string email)
        {
            var emailResult = Email.Create(email);
            if (emailResult.Success == false)
            {
                return DomainResult.Fail(emailResult.Errors);
            }

            if (Email == emailResult.Value)
            {
                return DomainResult.Ok();
            }

            Email = emailResult.Value;

            base.ModifiedOn = DateTime.UtcNow;
            base.ModifiedFrom = Environment.UserName;

            Raise(new CustomerEmailChanged(Id, Email));

            return DomainResult.Ok();
        }

        public DomainResult ChangeAddress(string street, string zipCode, string city)
        {
            var addressResult = Address.Create(street, zipCode, city);
            if (addressResult.Success == false)
            {
                return DomainResult.Fail(addressResult.Errors);
            }

            Address = addressResult.Value;

            base.ModifiedOn = DateTime.UtcNow;
            base.ModifiedFrom = Environment.UserName;

            Raise(new CustomerMoved(Id, Address));

            return DomainResult.Ok();
        }

        public DomainResult Delete()
        {
            if (IsDeleted == true)
            {
                return DomainResult.Fail(CustomerErrors.AlreadyDeleted);
            }

            this.IsDeleted = true;

            this.DeletedOn = DateTime.UtcNow;
            this.DeletedFrom = Environment.UserName;

            Raise(new CustomerDeleted(Id, this));

            return DomainResult.Ok();
        }
    }
}
