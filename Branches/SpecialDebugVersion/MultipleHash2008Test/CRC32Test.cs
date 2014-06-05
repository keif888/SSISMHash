using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for CRC32Test and is intended
    ///to contain all CRC32Test Unit Tests
    ///</summary>
    [TestClass()]
    public class CRC32Test
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
        ///A test for CRC32 Constructor
        ///</summary>
        [TestMethod()]
        public void CRC32ConstructorTest()
        {
            CRC32 target = new CRC32();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Polynomial32definition
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void Polynomial32definitionTest()
        {
            CRC32_Accessor target = new CRC32_Accessor(); // TODO: Initialize to an appropriate value
            uint[] expected = { 26, 23, 22, 16, 12, 11, 10, 8, 7, 5, 4, 2, 1, 0 };
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
        public void ComputeHashTestJustBuffer()
        {
            CRC32 target = new CRC32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz");
            byte[] expected = {0xbd, 0x50, 0x27, 0x4c};
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
        public void ComputeHashTestBufferIndexAndLength()
        {
            CRC32 target = new CRC32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz");
            int ibStart = 0; 
            int cbSize = buffer.Length;
            byte[] expected = { 0xbd, 0x50, 0x27, 0x4c };
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
            CRC32 expected = new CRC32();
            CRC32 actual;
            actual = CRC32.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
