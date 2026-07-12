namespace MinimalWPF.Test
{
    using System.Globalization;
    using System.Windows;

    using MinimalWPF.Beispiele;

    [TestClass]
    public sealed class Validator_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void Validator_Registry()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new IbanValidator(new[] { "DE", "AT", "CH", "FR", "NL" }));

            Assert.AreEqual(1, registry.Count);
            Assert.AreEqual(typeof(IbanValidator).Name, registry.Names[0]);
        }

        [TestMethod]
        public void Validator_Iban_True_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new IbanValidator(new[] { "DE"}));

            if (registry.TryGet<IbanValidator>(out var ibanValidator) == true)
            {
                ValidationResult result = ibanValidator.Validate("DE89 3704 0044 0532 0130 00");
                if (result.IsValid == true)
                {
                    /* IBAN OK */
                    Assert.IsTrue(result.IsValid == true);
                }
            }
        }

        [TestMethod]
        public void Validator_Iban_False_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new IbanValidator(new[] { "DE" }));

            if (registry.TryGet<IbanValidator>(out var ibanValidator) == true)
            {
                ValidationResult result = ibanValidator.Validate("AT88 7777 6666 5555 4444 039");
                if (result.IsValid == false)
                {
                    /* IBAN Fehler */
                    Assert.IsFalse(result.IsValid == true);
                    Assert.AreEqual(2, result.Errors.Count);
                }
            }
        }

        [TestMethod]
        public void Validator_URL_True_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new UrlValidator());

            if (registry.TryGet<UrlValidator>(out var urlValidator) == true)
            {
                ValidationResult result = urlValidator.Validate("www.google.de");
                if (result.IsValid == true)
                {
                    /* URL Ok */
                    Assert.IsTrue(result.IsValid == true);
                }
            }
        }

        [TestMethod]
        public void Validator_EMail_False_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new EmailValidator());

            if (registry.TryGet<EmailValidator>(out var emailValidator) == true)
            {
                ValidationResult result = emailValidator.Validate("developer@lifeprojectsde");
                if (result.IsValid == false)
                {
                    /* URL Fehler */
                    Assert.IsFalse(result.IsValid == true);
                    Assert.AreEqual(1, result.Errors.Count);
                }
            }
        }

        [TestMethod]
        public void Validator_EMail_True_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new EmailValidator());

            if (registry.TryGet<EmailValidator>(out var emailValidator) == true)
            {
                ValidationResult result = emailValidator.Validate("developer@lifeprojects.de");
                if (result.IsValid == true)
                {
                    /* URL Ok */
                    Assert.IsTrue(result.IsValid == true);
                }
            }
        }

        [TestMethod]
        public void Validator_GermanPhone_False_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new GermanPhoneNumberValidator());

            if (registry.TryGet<GermanPhoneNumberValidator>(out var germanPhoneValidator) == true)
            {
                ValidationResult result = germanPhoneValidator.Validate("+88-0621/332288");
                if (result.IsValid == false)
                {
                    /* URL Fehler */
                    Assert.IsFalse(result.IsValid == true);
                    Assert.AreEqual(2, result.Errors.Count);
                }
            }
        }

        [TestMethod]
        public void Validator_RegisterMany_True_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new UrlValidator());
            registry.Register(new ZipCodeValidator());

            if (registry.TryGet<ZipCodeValidator>(out var zipValidator) == true)
            {
                ValidationResult result = zipValidator.Validate("68165");
                if (result.IsValid == true)
                {
                    /* ZIP Ok */
                    Assert.IsTrue(result.IsValid == true);
                }
            }
        }

        [TestMethod]
        public void Validator_RegisterMany_False_Test()
        {
            var registry = new ValidatorRegistry();
            registry.Register(new UrlValidator());
            registry.Register(new ZipCodeValidator());

            if (registry.TryGet<ZipCodeValidator>(out var zipValidator) == true)
            {
                ValidationResult result = zipValidator.Validate("168165");
                if (result.IsValid == false)
                {
                    /* ZIP Fehler */
                    Assert.IsFalse(result.IsValid == true);
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
