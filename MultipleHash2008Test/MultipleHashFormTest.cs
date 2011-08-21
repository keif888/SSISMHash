using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for MultipleHashFormTest and is intended
    ///to contain all MultipleHashFormTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultipleHashFormTest
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
        ///A test for GetHashName
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void GetHashNameTest()
        {
            Assert.AreEqual("MD5", MultipleHashForm_Accessor.GetHashName(MultipleHash.HashTypeEnumerator.MD5));
            Assert.AreEqual("RipeMD160", MultipleHashForm_Accessor.GetHashName(MultipleHash.HashTypeEnumerator.RipeMD160));
            Assert.AreEqual("SHA1", MultipleHashForm_Accessor.GetHashName(MultipleHash.HashTypeEnumerator.SHA1));
            Assert.AreEqual("SHA256", MultipleHashForm_Accessor.GetHashName(MultipleHash.HashTypeEnumerator.SHA256));
            Assert.AreEqual("SHA384", MultipleHashForm_Accessor.GetHashName(MultipleHash.HashTypeEnumerator.SHA384));
            Assert.AreEqual("SHA512", MultipleHashForm_Accessor.GetHashName(MultipleHash.HashTypeEnumerator.SHA512));
            Assert.AreEqual("None", MultipleHashForm_Accessor.GetHashName(MultipleHash.HashTypeEnumerator.None));
        }

        /// <summary>
        ///A test for GetHashEnum
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void GetHashEnumTest()
        {
            Assert.AreEqual(MultipleHashForm_Accessor.GetHashEnum("MD5"), MultipleHash.HashTypeEnumerator.MD5);
            Assert.AreEqual(MultipleHashForm_Accessor.GetHashEnum("RipeMD160"), MultipleHash.HashTypeEnumerator.RipeMD160);
            Assert.AreEqual(MultipleHashForm_Accessor.GetHashEnum("SHA1"), MultipleHash.HashTypeEnumerator.SHA1);
            Assert.AreEqual(MultipleHashForm_Accessor.GetHashEnum("SHA256"), MultipleHash.HashTypeEnumerator.SHA256);
            Assert.AreEqual(MultipleHashForm_Accessor.GetHashEnum("SHA384"), MultipleHash.HashTypeEnumerator.SHA384);
            Assert.AreEqual(MultipleHashForm_Accessor.GetHashEnum("SHA512"), MultipleHash.HashTypeEnumerator.SHA512);
            Assert.AreEqual(MultipleHashForm_Accessor.GetHashEnum("None"), MultipleHash.HashTypeEnumerator.None);
        }
    }
}
