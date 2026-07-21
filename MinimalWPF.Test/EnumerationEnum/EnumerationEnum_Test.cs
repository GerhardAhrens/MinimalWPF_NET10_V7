namespace MinimalWPF.Test
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [TestClass]
    public sealed class EnumerationEnum_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void CreateAlternateEnum()
        {
            Buttons button = Buttons.Personen;
            Assert.AreEqual(Buttons.Personen, button);
            Assert.AreEqual("Hauptmenü", Buttons.Personen.Category());
            Assert.AreEqual("Personen verwalten",Buttons.Personen.Description());
            Assert.AreEqual("Personen", button.Name);
            Assert.AreEqual(1, button.Value);
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

        public sealed partial class Buttons : EnumerationEnumBase<Buttons>
        {
            private Buttons(int value, string name) : base(value, name) { }
        }

        public sealed partial class Buttons
        {
            [Category("Hauptmenü")]
            [Description("Personen verwalten")]
            public static readonly Buttons Personen = new(1, nameof(Personen));
        }

        public sealed partial class Buttons
        {
            [Category("Hauptmenü")]
            [Description("Adressen verwalten")]
            public static readonly Buttons Adressen = new(2, nameof(Adressen));
        }

        public sealed partial class Buttons
        {
            [Category("Hauptmenü")]
            [Description("Rechnungen verwalten")]
            public static readonly Buttons Rechnungen = new(3, nameof(Rechnungen));
        }
    }
}
