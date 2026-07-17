namespace MinimalWPF.Test
{
    using System.Globalization;
    using System.Windows.Domain;

    using MinimalWPF.Test.Sample;

    [TestClass]
    public sealed class DomainWork_Test : BaseTest
    {

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public DomainWork_Test()
        {
            /* Person erstellen */
            var nameResult = PersonName.Create("Max", "Mustermann");

            /* Email erstellen */
            var emailResult = Email.Create("max@test.de");

            /* Adresse erstellen */
            var addressResult = Address.Create("Hauptstraße 1", "1010", "Entenhausen");

            /* Customer Objekt erstellen */
            var customerResult = Customer.Create(
                nameResult.Value!,
                emailResult.Value!,
                addressResult.Value!);

            this.Customer = customerResult.Value!;
        }

        private Customer Customer { get; }

        [TestMethod]
        public void Customer_Rename()
        {
            PersonName beforRename = this.Customer.Fullname;
            
            this.Customer.RenamePerson("Dagobert", "Duck");
            
            PersonName afterRename = this.Customer.Fullname;
            
            Assert.AreNotEqual(beforRename, afterRename);

            this.Customer_AllActions();
        }

        [TestMethod]
        public void Customer_ChangeEmail()
        {
            Email beforChange = this.Customer.Email;

            this.Customer.ChangeEmail("dagobert.duck@entenhausen.eh");

            Email afterChange = this.Customer.Email;

            Assert.AreNotEqual(beforChange, afterChange);

            this.Customer_AllActions();
        }

        [TestMethod]
        public void Customer_ChangeAddress()
        {
            Address beforChange = this.Customer.Address;

            this.Customer.ChangeAddress("Talerstrasse 1", "1010", "Entenhausen");

            Address afterChange = this.Customer.Address;

            Assert.AreNotEqual(beforChange, afterChange);

            this.Customer_AllActions();
        }

        [TestMethod]
        public void Customer_Delete()
        {
            Customer beforChange = this.Customer;

            this.Customer.Delete();

            Assert.AreEqual(true, this.Customer.IsDeleted);

            this.Customer_AllActions();
        }


        [TestMethod]
        public void Customer_AllActions()
        {
            foreach (var domainEvent in this.Customer.DomainEvents)
            {
                switch (domainEvent)
                {
                    case CustomerCreated e:
                        Assert.AreNotEqual(Guid.Empty, e.CustomerId);

                        break;

                    case CustomerRenamed e:
                        Assert.AreEqual("Dagobert", e.Fullname.FirstName);
                        Assert.AreEqual("Duck", e.Fullname.LastName);

                        break;

                    case CustomerEmailChanged e:
                        Assert.AreEqual("dagobert.duck@entenhausen.eh", e.Email.Value);

                        break;

                    case CustomerMoved e:
                        Assert.AreEqual("Talerstrasse 1", e.Address.Street);
                        Assert.AreEqual("1010", e.Address.ZipCode);
                        Assert.AreEqual("Entenhausen", e.Address.City);

                        break;

                    case CustomerDeleted e:
                        Assert.AreEqual(true,e.Customer.IsDeleted);
                        Assert.AreNotEqual(Guid.Empty,e.CustomerId);
                        Assert.AreEqual("Max Mustermann", e.Customer.Fullname.ToString());

                        break;
                }
            }
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }
}
