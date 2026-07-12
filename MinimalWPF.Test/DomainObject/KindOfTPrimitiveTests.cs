namespace MinimalWPF.Test
{
    using System.Globalization;
    using System.Windows;

    using static Mono.CompilerServices.SymbolWriter.CodeBlockEntry;

    [TestClass]
    public sealed class KindOfTPrimitiveTests : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void Kann_als_Basistyp_verwendet_werden()
        {
            FirstName firstName = new FirstName("Donald");

            string firstNameStr = firstName;
            Assert.AreEqual(firstName, firstNameStr);
        }

        [TestMethod]
        public void Kann_nicht_als_anderer_Typ_mit_demselben_Basistyp_verwendet_werden()
        {
            FirstName firstName = new FirstName("Donald");
            Assert.IsNotInstanceOfType(firstName, typeof(LastName));
        }


        [TestMethod]
        public void Ctor_Wenn_der_zugrunde_liegende_Wert_null_ist_wird_eine_ArgumentNullException_geworfen()
        {
            Action act = () => new FirstName(null);
            Assert.Throws<ArgumentNullException>(act);
        }

        [TestMethod]
        public void Ist_gleich_wenn_die_Typen_übereinstimmen_und_der_zugrundeliegende_Wert_derselbe_ist_dann_wird_true()
        {
            FirstName name1 = new FirstName("Donald");
            FirstName name2 = new FirstName("Donald");

            var isEqual = name1.Equals(name2);
            Assert.IsTrue(isEqual);
        }

        [TestMethod]
        public void Ist_gleich_wenn_die_Typen_übereinstimmen_und_der_zugrundeliegende_Wert_derselbe_ist_dann_wird_false()
        {
            FirstName name1 = new FirstName("Donald");
            FirstName name2 = new FirstName("Dagobert");

            var isEqual = name1.Equals(name2);
            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        public void Ist_gleich_wenn_die_Typen_identisch_sind_und_die_zugrunde_liegenden_Werte_unterschiedlich_sind_False()
        {
            FirstName donald = new FirstName("Donald");
            FirstName dagobert = new FirstName("Dagobert");

            var isEqual = donald.Equals(dagobert);
            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        public void Ist_gleich_wenn_die_Typen_unterschiedlich_sind_und_der_zugrundeliegende_Wert_derselbe_ist_dann_wird_false()
        {
            FirstName donald = new FirstName("Donald");
            LastName duck = new LastName("Duck");

            var isEqual = donald.Equals(duck);
            Assert.IsFalse(isEqual);
        }

        [TestMethod]
        public void GetHashcode()
        {
            FirstName donald = new FirstName("Donald");
            FirstName dagobert = new FirstName("Dagobert");

            Assert.AreNotEqual(donald, dagobert);
        }

        [TestMethod]
        public void Leer3()
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

        private class FirstName : KindOf<string, FirstName>
        {
            public FirstName(string value) : base(value) { }
        }

        private class LastName : KindOf<string, LastName>
        {
            public LastName(string value) : base(value) { }
        }

    }
}
