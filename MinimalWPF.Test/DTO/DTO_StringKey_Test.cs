namespace MinimalWPF.Test
{
    using System.Globalization;
    using System.Windows;

    [TestClass]
    public sealed class DTO_StringKey_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void CreateNewDTO()
        {
            DTO dto = new();
            dto.Set("Name", "Max Mustermann");
            dto.Set("Age", 65);
            dto.Set("Birthday", new DateTime(1960, 6, 28));
            dto.Set("IsActive", true);
            dto.Set("Parts", new List<string> { "Part1", "Part2", "Part3" });

            Assert.AreEqual(5, dto.Count);
            var keys = string.Join(";",dto.Keys);
            Assert.AreEqual("NAME;AGE;BIRTHDAY;ISACTIVE;PARTS", keys);
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
