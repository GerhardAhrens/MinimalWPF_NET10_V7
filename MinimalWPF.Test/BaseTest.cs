namespace MinimalWPF.Test
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class AssemblyHelper
    {
        public static Func<Assembly> GetEntryAssembly = () => Assembly.GetEntryAssembly();
        public static Func<Assembly> GetRemoteAssembly = () => Assembly.GetExecutingAssembly();
    }

    [TestClass]
    public abstract class BaseTest
    {
        private readonly Assembly testAssembly = null;

        public BaseTest()
        {
            /* Preparing test start */
            this.testAssembly = Assembly.GetEntryAssembly();
            this.TestAssembly = this.testAssembly;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
        }

        public CultureInfo GetCurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }

        public DirectoryInfo GetAssemblyPath
        {
            get
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                return new DirectoryInfo(assemblyPath);
            }
        }

        public string GetAssemblyFullname
        {
            get
            {
                return Assembly.GetEntryAssembly().Location;
            }
        }

        public string GetAssemblyName
        {
            get
            {
                return Path.GetFileName(Assembly.GetEntryAssembly().Location);
            }
        }

        public Assembly TestAssembly { get; private set; }

        public Assembly BaseAssembly { get; private set; }

        public TestContext TestContext { get; set; }

        public string Class
        {
            get { return this.TestContext.FullyQualifiedTestClassName; }
        }

        public string Method
        {
            get { return this.TestContext.TestName; }
        }

        protected virtual void Trace(object message, params object[] args)
        {
            System.Diagnostics.Trace.WriteLine(string.Format(message.ToString(), args));
        }

        protected virtual void Trace(object message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }

        protected virtual void Trace(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }

        protected virtual void Trace(int message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }

        protected virtual void DebugLine(char symbol = '-', int length = 30)
        {
            System.Diagnostics.Trace.WriteLine(new string(symbol, length));
        }
    }
}
