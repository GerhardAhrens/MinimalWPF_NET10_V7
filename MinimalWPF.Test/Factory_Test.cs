namespace MinimalWPF.Test
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    [STATestClass()]
    [TestClass]
    public sealed class Factory_Test : BaseTest
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
            Factory.RegisterTransient<ViewId>(ViewId.Dashboard, () => new DashboardControl(service));
            Factory.RegisterSingleton<WindowId>(WindowId.Login, () => new LoginWindow());
            Factory.RegisterSingleton<NormalClassId>(NormalClassId.SingletonClass, () => new SingletonClass());

            Assert.AreEqual(3, Factory.Count);
            string names = string.Join(',', Factory.Names);
            Assert.AreEqual("Dashboard,Login,SingletonClass", names);
        }

        [TestMethod]
        public void FactoryRegister_Test()
        {
            DataService service = new();
            Factory.RegisterTransient<ViewId>(ViewId.Dashboard, () => new DashboardControl(service));
            Factory.RegisterSingleton<WindowId>(WindowId.Login, () => new LoginWindow());
            Factory.RegisterSingleton<NormalClassId>(NormalClassId.SingletonClass, () => new SingletonClass());

            Assert.AreEqual(3, Factory.Count);
            Assert.AreEqual("Login", Factory.Names.Where(name => name == "Login").FirstOrDefault());
            Assert.AreEqual("Dashboard", Factory.Names.Where(name => name == "Dashboard").FirstOrDefault());
            Assert.AreEqual("SingletonClass", Factory.Names.Where(name => name == "SingletonClass").FirstOrDefault());

            Assert.AreEqual(service.Id, (Factory.Get<DashboardControl, ViewId>(ViewId.Dashboard).Service).Id);
        }

        [TestMethod]
        public void FactoryGet_Test()
        {
            DataService service = new();
            Factory.RegisterTransient<ViewId>(ViewId.Dashboard, () => new DashboardControl(service));
            Factory.RegisterSingleton<WindowId>(WindowId.Login, () => new LoginWindow());
            Factory.RegisterSingleton<NormalClassId>(NormalClassId.SingletonClass, () => new SingletonClass());

            var dashboard = Factory.Get<UserControl, ViewId>(ViewId.Dashboard);
            var logWindow = Factory.Get<LoginWindow, WindowId>(WindowId.Login);
            var normalClass = Factory.Get<SingletonClass, NormalClassId>(NormalClassId.SingletonClass);
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

    internal class DataService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    internal class DashboardControl : UserControl
    {
        public DataService Service { get; }

        public DashboardControl(DataService service)
        {
            this.Service = service;

            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt | Service: {service.Id}");
        }
    }

    internal class LoginWindow : Window
    {
        public LoginWindow()
        {
            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt");
        }
    }

    internal class SingletonClass : SingletonBase<SingletonClass>
    {
        public SingletonClass()
        {
            Debug.WriteLine($"Die Instanz {this.GetType().Name} erstellt");
        }
    }

    internal enum ViewId
    {
        Dashboard,
    }

    internal enum WindowId
    {
        Login,
    }

    internal enum NormalClassId
    {
        SingletonClass,
    }
}
