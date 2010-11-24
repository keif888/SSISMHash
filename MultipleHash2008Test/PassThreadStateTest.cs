using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.Threading;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for PassThreadStateTest and is intended
    ///to contain all PassThreadStateTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PassThreadStateTest
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
        ///A test for ThreadReset
        ///</summary>
        [TestMethod()]
        public void ThreadResetTest()
        {
            OutputColumn columnToProcess = null; // TODO: Initialize to an appropriate value
            PipelineBuffer buffer = null; // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 metaData = null; // TODO: Initialize to an appropriate value
            ManualResetEvent threadReset = new ManualResetEvent(true); // TODO: Initialize to an appropriate value
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false); // TODO: Initialize to an appropriate value
            ManualResetEvent expected = new ManualResetEvent(true); // TODO: Initialize to an appropriate value
            ManualResetEvent actual;
            target.ThreadReset = expected;
            actual = target.ThreadReset;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for MetaData
        ///</summary>
        [TestMethod()]
        public void MetaDataTest()
        {
            OutputColumn columnToProcess = null; // TODO: Initialize to an appropriate value
            PipelineBuffer buffer = null; // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 metaData = new ComponentMetaDataTestImpl(); // TODO: Initialize to an appropriate value
            ManualResetEvent threadReset = null; // TODO: Initialize to an appropriate value
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false); // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 expected = new ComponentMetaDataTestImpl(); // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 actual;
            target.MetaData = expected;
            actual = target.MetaData;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ColumnToProcess
        ///</summary>
        [TestMethod()]
        public void ColumnToProcessTest()
        {
            OutputColumn columnToProcess = new OutputColumn(); // TODO: Initialize to an appropriate value
            PipelineBuffer buffer = null; // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 metaData = null; // TODO: Initialize to an appropriate value
            ManualResetEvent threadReset = null; // TODO: Initialize to an appropriate value
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false); // TODO: Initialize to an appropriate value
            OutputColumn expected = new OutputColumn(); // TODO: Initialize to an appropriate value
            OutputColumn actual;
            target.ColumnToProcess = expected;
            actual = target.ColumnToProcess;
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Buffer
        ///</summary>
        [TestMethod()]
        public void BufferTest()
        {
            OutputColumn columnToProcess = null; // TODO: Initialize to an appropriate value
            PipelineBuffer buffer = null; // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 metaData = null; // TODO: Initialize to an appropriate value
            ManualResetEvent threadReset = null; // TODO: Initialize to an appropriate value
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false); // TODO: Initialize to an appropriate value
            PipelineBuffer expected = null; // TODO: Initialize to an appropriate value
            PipelineBuffer actual;
            target.Buffer = expected;
            actual = target.Buffer;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.  ie. How do you get a PipelineBuffer without being inside SSIS?");
        }

        /// <summary>
        ///A test for PassThreadState Constructor
        ///</summary>
        [TestMethod()]
        public void PassThreadStateConstructorTest()
        {
            OutputColumn columnToProcess = new OutputColumn();
            PipelineBuffer buffer = null; // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 metaData = new ComponentMetaDataTestImpl();
            ManualResetEvent threadReset = new ManualResetEvent(true);
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, true);
            Assert.IsNotNull(target);
            Assert.AreEqual(columnToProcess, target.ColumnToProcess);
            Assert.AreEqual(metaData, target.MetaData);
            Assert.AreEqual(threadReset, target.ThreadReset);
            Assert.AreEqual(true, target.SafeNullHandling);
        }
    }
}
