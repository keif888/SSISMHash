using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for DataFlowElementTest and is intended
    ///to contain all DataFlowElementTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataFlowElementTest
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
        ///A test for DataFlowElement Constructor
        ///</summary>
        [TestMethod()]
        public void DataFlowElementConstructorTest2Value()
        {
            string name = "name"; // TODO: Initialize to an appropriate value
            IDTSExternalMetadataColumn100 ic = new ExternalMetadataColumnTestImpl();
            ic.Name = "name";
            string expected = "Name: name\nData type: DT_NULL\nLength: 0\nScale: 0\nPrecision: 0\nCode page: 0";
            DataFlowElement target = new DataFlowElement(name, ic);
            Assert.AreEqual(name, target.Name);
            Assert.AreEqual(ic, target.Tag);
            Assert.AreEqual(expected, target.ToolTip);
        }

        /// <summary>
        ///A test for DataFlowElement Constructor
        ///</summary>
        [TestMethod()]
        public void DataFlowElementConstructorTest1Value()
        {
            string name = "name";
            DataFlowElement target = new DataFlowElement(name);
            Assert.AreEqual(name, target.Name);
        }

        /// <summary>
        ///A test for DataFlowElement Constructor
        ///</summary>
        [TestMethod()]
        public void DataFlowElementConstructorTestDefault()
        {
            DataFlowElement target = new DataFlowElement();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        public void CloneTest()
        {
            DataFlowElement target = new DataFlowElement("Hello");
            DataFlowElement actual;
            actual = target.Clone();
            Assert.IsNotNull(actual);
            Assert.AreEqual("Hello", actual.Name);
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            DataFlowElement target = new DataFlowElement("Hello");
            int expected = "Hello".GetHashCode();
            int actual;
            actual = target.GetHashCode();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            DataFlowElement target = new DataFlowElement("Hello There");
            string expected = "Hello There";
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            DataFlowElement target = new DataFlowElement("No Name");
            string expected = "New Name";
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Tag
        ///</summary>
        [TestMethod()]
        public void TagTest()
        {
            IDTSExternalMetadataColumn100 ic = new ExternalMetadataColumnTestImpl();
            DataFlowElement target = new DataFlowElement("Name Me Please", ic);
            object actual;
            actual = target.Tag;
            Assert.IsNotNull(actual);
            Assert.AreEqual(ic.Name, (actual as IDTSExternalMetadataColumn100).Name);
        }

        /// <summary>
        ///A test for ToolTip
        ///</summary>
        [TestMethod()]
        public void ToolTipTest()
        {
            IDTSExternalMetadataColumn100 ic = new ExternalMetadataColumnTestImpl();
            ic.Name = "name";
            string expected = "Name: name\nData type: DT_NULL\nLength: 0\nScale: 0\nPrecision: 0\nCode page: 0";
            DataFlowElement target = new DataFlowElement("New Name", ic); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ToolTip;
            Assert.AreEqual(expected, actual);
        }
    }
}
