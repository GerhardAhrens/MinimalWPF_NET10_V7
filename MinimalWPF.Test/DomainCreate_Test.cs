namespace MinimalWPF.Test
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;

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
            DomainResult<Person> personResult = Create.From(Person.Create,
                            Create.From(FullName.Create,
                                FirstName.Create("Donald"),
                                LastName.Create("Duck")),
                            EMail.Create("donald.duck@entenhausen.eh"));

            switch (personResult)
            {
                case DomainResult<Person>.Ok ok:
                    Assert.AreEqual(typeof(Person),ok.Item.GetType());
                    break;

                case DomainResult<Person>.Failure failure:
                    foreach (var error in failure.Errors)
                    {
                        Console.WriteLine(error.Message);
                    }
                    break;
            }
        }

        [TestMethod]
        public void Create_WithError()
        {
            DomainResult<Person> personResult = Create.From(Person.Create,
                            Create.From(FullName.Create,
                                FirstName.Create("Gustav"),
                                LastName.Create("Gans")),
                            EMail.Create("gustav.gans@entenhauseneh"));

            switch (personResult)
            {
                case DomainResult<Person>.Ok ok:
                    Assert.AreEqual(typeof(Person), ok.Item.GetType());
                    break;

                case DomainResult<Person>.Failure failure:
                    foreach (var error in failure.GetErrors())
                    {
                        foreach (var childerror in error.ChildErrors)
                        {
                            Assert.AreEqual("Gustav Gans ist auf der Sperrliste", childerror.Message);
                        }
                    }
                    break;
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
