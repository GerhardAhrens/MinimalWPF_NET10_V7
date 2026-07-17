namespace MinimalWPF.Test
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
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

        [TestMethod]
        public void Create_Successfully()
        {
        }

        [TestMethod]
        public void Create_WithError()
        {
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
