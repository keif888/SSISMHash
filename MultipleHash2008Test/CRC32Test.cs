using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

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
            uint polynomial = 0; // TODO: Initialize to an appropriate value
            CRC32 target = new CRC32(polynomial);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CRC32 Constructor
        ///</summary>
        [TestMethod()]
        public void CRC32ConstructorTest1()
        {
            CRC32 target = new CRC32();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTest()
        {
            CRC32 target = new CRC32(); // TODO: Initialize to an appropriate value
            Stream inputStream = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.ComputeHash(inputStream);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTest1()
        {
            CRC32 target = new CRC32(); // TODO: Initialize to an appropriate value
            byte[] buffer = null; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTest2()
        {
            CRC32 target = new CRC32(); // TODO: Initialize to an appropriate value
            byte[] buffer = null; // TODO: Initialize to an appropriate value
            int ibStart = 0; // TODO: Initialize to an appropriate value
            int cbSize = 0; // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.ComputeHash(buffer, ibStart, cbSize);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CRC32 expected = null; // TODO: Initialize to an appropriate value
            CRC32 actual;
            actual = CRC32.Create();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for HashCore
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void HashCoreTest()
        {
            CRC32_Accessor target = new CRC32_Accessor(); // TODO: Initialize to an appropriate value
            byte[] array = null; // TODO: Initialize to an appropriate value
            int ibStart = 0; // TODO: Initialize to an appropriate value
            int cbSize = 0; // TODO: Initialize to an appropriate value
            target.HashCore(array, ibStart, cbSize);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for HashFinal
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void HashFinalTest()
        {
            CRC32_Accessor target = new CRC32_Accessor(); // TODO: Initialize to an appropriate value
            byte[] expected = null; // TODO: Initialize to an appropriate value
            byte[] actual;
            actual = target.HashFinal();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest()
        {
            CRC32 target = new CRC32(); // TODO: Initialize to an appropriate value
            target.Initialize();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for buildCRC32Table
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void buildCRC32TableTest()
        {
            uint polynomial = 0; // TODO: Initialize to an appropriate value
            uint[] expected = null; // TODO: Initialize to an appropriate value
            uint[] actual;
            actual = CRC32_Accessor.buildCRC32Table(polynomial);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
