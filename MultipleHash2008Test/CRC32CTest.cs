using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for CRC32CTest and is intended
    ///to contain all CRC32CTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CRC32CTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CRC32C Constructor
        ///</summary>
        [TestMethod()]
        public void CRC32CConstructorTest()
        {
            CRC32C target = new CRC32C();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Polynomial32definition
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void Polynomial32definitionTest()
        {
            CRC32C_Accessor target = new CRC32C_Accessor(); // TODO: Initialize to an appropriate value
            uint[] expected = { 28, 27, 26, 25, 23, 22, 20, 19, 18, 14, 13, 11, 10, 9, 8, 6, 0 };
            uint[] actual;
            actual = target.Polynomial32definition;
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestBuffer()
        {
            CRC32C target = new CRC32C();
            byte[] buffer = {0x01, 0xC0, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x01, 0xFE, 0x60, 0xAC,
                            0x00, 0x00, 0x00, 0x08,
                            0x00, 0x00, 0x00, 0x04,
                            0x00, 0x00, 0x00, 0x09,
                            0x25, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00};
            byte[] expected = { 0xeb, 0x75, 0x4f, 0x66 };
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestBufferOffsetLength()
        {
            CRC32C target = new CRC32C();
            byte[] buffer = {0x01, 0xC0, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x01, 0xFE, 0x60, 0xAC,
                            0x00, 0x00, 0x00, 0x08,
                            0x00, 0x00, 0x00, 0x04,
                            0x00, 0x00, 0x00, 0x09,
                            0x25, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00};
            int ibStart = 0; 
            int cbSize = buffer.Length;
            byte[] expected = { 0xeb, 0x75, 0x4f, 0x66};
            byte[] actual;
            actual = target.ComputeHash(buffer, ibStart, cbSize);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CRC32C expected = new CRC32C();
            CRC32C actual;
            actual = CRC32C.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

    }
}
