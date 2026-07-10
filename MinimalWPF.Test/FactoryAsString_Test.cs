namespace MinimalWPF.Test
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    [STATestClass]
    public sealed class FactoryAsString_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void FactoryMeta_Test()
        {
            DataService service = new();
            Factory.RegisterTransient("Dashboard", (param) => new DashboardControl(service));
            Factory.RegisterSingleton("Login", () => new LoginWindow());
            Factory.RegisterSingleton("SingletonClass", () => new SingletonClass());

            Assert.AreEqual(3, Factory.Count);
            string names = string.Join(',', Factory.Names.Order());
            Assert.AreEqual("Dashboard,Login,SingletonClass", names);
        }

        [TestMethod]
        public void FactoryRegister_Test()
        {
            DataService service = new();
            Factory.RegisterTransient("Dashboard", (param) => new DashboardControl(service));
            Factory.RegisterSingleton("Login", () => new LoginWindow());
            Factory.RegisterSingleton("SingletonClass", () => new SingletonClass());

            Assert.AreEqual(3, Factory.Count);
            Assert.AreEqual("Login", Factory.Names.Where(name => name == "Login").FirstOrDefault());
            Assert.AreEqual("Dashboard", Factory.Names.Where(name => name == "Dashboard").FirstOrDefault());
            Assert.AreEqual("SingletonClass", Factory.Names.Where(name => name == "SingletonClass").FirstOrDefault());

            Assert.AreEqual(service.Id, (Factory.Get<DashboardControl>("Dashboard").Service).Id);
        }

        [TestMethod]
        public void FactoryGet_Test()
        {
            DataService service = new();
            Factory.RegisterTransient("Dashboard", (param) => new DashboardControl(service));
            Factory.RegisterSingleton("Login", () => new LoginWindow());
            Factory.RegisterSingleton("SingletonClass", () => new SingletonClass());

            var dashboard = Factory.Get<UserControl>("Dashboard");
            var logWindow = Factory.Get<LoginWindow>("Login");
            var normalClass = Factory.Get<SingletonClass>("SingletonClass");
            Assert.AreEqual(typeof(DashboardControl), dashboard.GetType());
            Assert.AreEqual(typeof(LoginWindow), logWindow.GetType());
            Assert.AreEqual(typeof(SingletonClass), normalClass.GetType());
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

    internal class DataServiceB
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    internal class DashboardControlB : UserControl
    {
        public DataService Service { get; }

        public DashboardControlB(DataService service)
        {
            this.Service = service;

            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt | Service: {service.Id}");
        }
    }

    internal class LoginWindowB : Window
    {
        public LoginWindowB()
        {
            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt");
        }
    }

    internal class SingletonClassB : SingletonBase<SingletonClassB>
    {
        public SingletonClassB()
        {
            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt");
        }
    }

    public sealed record ButtonId(string Name);

    public static partial class Buttons
    {
        public static readonly ButtonId Dashboard = new("Dashboard");
        public static readonly ButtonId Login = new("Login");
        public static readonly ButtonId SingletonClass = new("SingletonClass");
    }

    /*
     * Festlegung in XAML
     * CommandParameter="{x:Static local:Buttons.Dashboard}"
     */

    /*
     * Das ist sogar typsicherer als Strings.
     * und im Command:
     * private void Execute(object? parameter)
     * {
     *      if (parameter is ButtonId button)
     *      {
     *          
     *      }
     * }
     */

}
