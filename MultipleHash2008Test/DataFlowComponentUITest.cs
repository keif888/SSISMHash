using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.SqlServer.Dts.Pipeline.Design;
using System.Windows.Forms;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for DataFlowComponentUITest and is intended
    ///to contain all DataFlowComponentUITest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataFlowComponentUITest
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
        ///A test for FormatTooltipText
        ///</summary>
        [TestMethod()]
        public void FormatTooltipTextTest()
        {
            string name = "name";
            string dataType = "dataType";
            string length = "length";
            string scale = "scale";
            string precision = "precision";
            string codePage = "codePage";
            string expected = "Name: name\nData type: dataType\nLength: length\nScale: scale\nPrecision: precision\nCode page: codePage";
            string actual;
            
            actual = DataFlowComponentUI.FormatTooltipText(name, dataType, length, scale, precision, codePage);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FormatTooltipText
        ///</summary>
        [TestMethod()]
        public void FormatTooltipTextTestWithComponent()
        {
            string name = "name";
            string dataType = "dataType";
            string length = "length";
            string scale = "scale";
            string precision = "precision";
            string codePage = "codePage";
            string sourceComponent = "sourceComponent";
            string expected = "Name: name\nData type: dataType\nLength: length\nScale: scale\nPrecision: precision\nCode page: codePage\nSource: sourceComponent";
            string actual;

            actual = DataFlowComponentUI.FormatTooltipText(name, dataType, length, scale, precision, codePage, sourceComponent);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for GetTooltipString
        ///</summary>
        [TestMethod()]
        public void GetTooltipStringTestInputColumn()
        {
            IDTSInputColumn100 ic = new InputColumnTestImpl();
            ic.Name = "name";
            string expected = "Name: name\nData type: DT_NULL\nLength: 0\nScale: 0\nPrecision: 0\nCode page: 0";
            string actual = DataFlowComponentUI.GetTooltipString(ic);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetTooltipString
        ///</summary>
        [TestMethod()]
        public void GetTooltipStringTestOutputColumn()
        {
            IDTSOutputColumn100 ic = new OutputColumnTestImpl();
            ic.Name = "name";
            string expected = "Name: name\nData type: DT_NULL\nLength: 0\nScale: 0\nPrecision: 0\nCode page: 0";
            string actual = DataFlowComponentUI.GetTooltipString(ic);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetTooltipString
        ///</summary>
        [TestMethod()]
        public void GetTooltipStringTestExternalMetaColumn()
        {
            IDTSExternalMetadataColumn100 ic = new ExternalMetadataColumnTestImpl();
            ic.Name = "name";
            string expected = "Name: name\nData type: DT_NULL\nLength: 0\nScale: 0\nPrecision: 0\nCode page: 0";
            string actual = DataFlowComponentUI.GetTooltipString(ic);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetTooltipString
        ///</summary>
        [TestMethod()]
        public void GetTooltipStringTestVirtualInputColumn()
        {
            IDTSVirtualInputColumn100 ic = new VirtualInputColumnTestImpl();
            ic.Name = "name";
            string expected = "Name: name\nData type: DT_NULL\nLength: 0\nScale: 0\nPrecision: 0\nCode page: 0\nSource: LocalSourceComponent";
            string actual = DataFlowComponentUI.GetTooltipString(ic);
            Assert.AreEqual(expected, actual);
        }


        

    }
}
