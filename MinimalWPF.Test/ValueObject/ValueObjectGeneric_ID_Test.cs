//-----------------------------------------------------------------------
// <copyright file="CDT_Base64.cs" company="Lifeprojects.de">
//     Class: CDT_Base64
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>02.05.2025 11:12:02</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace MinimalWPF.Test
{
    using System;
    using System.Windows;
    using System.Globalization;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ValueObjectGeneric_ID_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CDT_Base64"/> class.
        /// </summary>
        public ValueObjectGeneric_ID_Test()
        {
        }

        [TestMethod]
        public void CreateNewID_Guid()
        {
            ID<Guid> id = Guid.Empty;
            Assert.IsTrue(id.Status == ID<Guid>.IDStatus.New);
        }

        [TestMethod]
        public void CreateNewID_Int()
        {
            ID<int> id = 0;
            Assert.AreEqual(-1,id.Value);
            Assert.IsTrue(id.Status == ID<int>.IDStatus.New);
        }

        [TestMethod]
        public void CreateEditID_Guid()
        {
            ID<Guid> id = Guid.NewGuid();
            Assert.IsTrue(id.Status == ID<Guid>.IDStatus.Edit);
        }

        [TestMethod]
        public void CreateEditID_Int()
        {
            ID<int> id = 100;
            Assert.AreEqual(100, id.Value);
            Assert.IsTrue(id.Status == ID<int>.IDStatus.Edit);
        }

        [TestMethod]
        public void GuidID_IsEqual()
        {
            ID<Guid> id1 = Guid.CreateVersion7();
            ID<Guid> id2 = id1;

            Assert.AreEqual(id1, id2);
        }

        [TestMethod]
        public void GuidID_IsNotEqual()
        {
            ID<Guid> id1 = Guid.CreateVersion7();
            ID<Guid> id2 = Guid.CreateVersion7();

            Assert.AreNotEqual(id1, id2);
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
