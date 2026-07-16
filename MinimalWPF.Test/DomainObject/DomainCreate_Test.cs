namespace MinimalWPF.Test
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Domain;

    using MinimalWPF.Test.Sample;

    [TestClass]
    public sealed class DomainCreate_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void Create_Successfully()
        {
            /* Person erstellen */
            var nameResult = PersonName.Create("Max", "Mustermann");

            Assert.AreEqual(true,nameResult.Success);

            /* Email erstellen */
            var emailResult = Email.Create("max@test.de");

            Assert.AreEqual(true, emailResult.Success);

            /* Adresse erstellen */
            var addressResult = Address.Create("Hauptstraße 1", "1010", "Entenhausen");

            Assert.AreEqual(true, addressResult.Success);

            /* Customer Objekt erstellen */
            var customerResult = Customer.Create(
                nameResult.Value!,
                emailResult.Value!,
                addressResult.Value!);

            Assert.AreEqual(true, customerResult.Success);

            Customer customer = customerResult.Value!;

            IDomainEvent customObject = customer.DomainEvents.FirstOrDefault();
            Guid customId = ((CustomerCreated)customObject).CustomerId.Value;

            Assert.AreNotEqual(Guid.Empty, customId);
        }

        [TestMethod]
        public void Create_WithError()
        {
            /* Person erstellen */
            var nameResult = PersonName.Create("Max", "Mustermann");

            Assert.AreEqual(true, nameResult.Success);

            /* Email erstellen */
            var emailResult = Email.Create("max-test.de");

            /* Fehler in der Email Adresse */
            Assert.AreNotEqual(true, emailResult.Success);

            /* Adresse erstellen */
            var addressResult = Address.Create("Hauptstraße 1", "1010", "Entenhausen");

            Assert.AreEqual(true, addressResult.Success);

            /* Customer Objekt erstellen */
            var customerResult = Customer.Create(
                nameResult.Value!,
                emailResult.Value!,
                addressResult.Value!);

            Assert.AreEqual(true, customerResult.Success);

            Customer customer = customerResult.Value!;

            IDomainEvent customObject = customer.DomainEvents.FirstOrDefault();
            Guid customId = ((CustomerCreated)customObject).CustomerId.Value;

            Assert.AreNotEqual(Guid.Empty, customId);
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
