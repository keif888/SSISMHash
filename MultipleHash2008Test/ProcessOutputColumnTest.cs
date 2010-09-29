using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.Threading;
namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for ProcessOutputColumnTest and is intended
    ///to contain all ProcessOutputColumnTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProcessOutputColumnTest
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
        ///A test for CalculateHash
        ///</summary>
        [TestMethod()]
        public void CalculateHashTest()
        {
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
            OutputColumn columnToProcess = new OutputColumn();

            DTSBufferManagerClass bufferManagerClass = new DTSBufferManagerClass();
            PipelineBuffer buffer;
            MainPipeClass mainPipeClass = new MainPipeClass();
            DTP_BUFFCOL bufferColumns = new DTP_BUFFCOL();
            int bufferID;

            bufferColumns.lCodePage = 1205;
            bufferColumns.lLengthOffset = 0;
            bufferColumns.lMaxLength = 128;
            bufferColumns.lOffset = 0;
            bufferColumns.DataType = Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR;
            bufferID = bufferManagerClass.RegisterBufferType(1, ref bufferColumns, 100, (uint)Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSBufferFlags.BUFF_INIT);

            IDTSComponentMetaData100 metaData = mainPipeClass.ComponentMetaDataCollection.New();
            metaData.Description = "This is a test metaData";
            metaData.Name = "Input Buffer";
            metaData.ComponentClassID = typeof(Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSBuffer100).AssemblyQualifiedName;

            buffer = (PipelineBuffer)bufferManagerClass.CreateBuffer(bufferID, metaData);
            ManualResetEvent threadReset = new ManualResetEvent(false);

            PassThreadState state = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false);
            ProcessOutputColumn.CalculateHash(state);
        }

        /// <summary>
        ///A test for ProcessOutputColumn Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void ProcessOutputColumnConstructorTest()
        {
            ProcessOutputColumn_Accessor target = new ProcessOutputColumn_Accessor();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}
