using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for FNV1a32Test and is intended
    ///to contain all FNV1a32Test Unit Tests
    ///</summary>
    [TestClass()]
    public class FNV1a32Test
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
        ///A test for FNV1a32 Constructor
        ///</summary>
        [TestMethod()]
        public void FNV1a32ConstructorTest()
        {
            FNV1a32 target = new FNV1a32();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTest()
        {
            FNV1a32 target = new FNV1a32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("curds and whey\n");
            byte[] expected = { 0x0c, 0x47, 0xa1, 0x19 }; // 0x19a1470cUL 
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
            FNV1a32 target = new FNV1a32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("chongo was here!\n");
            int ibStart = 0;
            int cbSize = buffer.Length;
            byte[] expected = { 0xd5, 0x30, 0x99, 0xd4 }; // 0xd49930d5UL  
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
            FNV1a32 expected = new FNV1a32();
            FNV1a32 actual;
            actual = FNV1a32.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for HashCore
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void HashCoreTest()
        {
            FNV1a32_Accessor target = new FNV1a32_Accessor();
            byte[] array = ASCIIEncoding.ASCII.GetBytes("chongo was here!\n");
            int ibStart = 0;
            int cbSize = array.Length;
            target.HashCore(array, ibStart, cbSize);
            Assert.AreEqual((UInt32)0xd49930d5, target.FNVHash);
        }

        /// <summary>
        ///A test for HashFinal
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void HashFinalTest()
        {
            FNV1a32_Accessor target = new FNV1a32_Accessor(); // TODO: Initialize to an appropriate value
            byte[] array = ASCIIEncoding.ASCII.GetBytes("chongo was here!\n");
            int ibStart = 0;
            int cbSize = array.Length;
            target.HashCore(array, ibStart, cbSize);
            byte[] expected = { 0xd5, 0x30, 0x99, 0xd4 }; // 0xd49930d5UL  
            byte[] actual;
            actual = target.HashFinal();
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest()
        {
            FNV1a32 target = new FNV1a32();
            target.Initialize();
            Assert.AreEqual(32, target.HashSize);
        }
    }
}
