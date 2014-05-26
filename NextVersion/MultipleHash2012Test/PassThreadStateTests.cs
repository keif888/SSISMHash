using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System.Threading;
using MultipleHash2012Test.SSISImplementations;


namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class PassThreadStateTests
    {
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
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false, false); // TODO: Initialize to an appropriate value
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
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false, false); // TODO: Initialize to an appropriate value
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
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false, false); // TODO: Initialize to an appropriate value
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
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, false, false); // TODO: Initialize to an appropriate value
            PipelineBuffer expected = null; // TODO: Initialize to an appropriate value
            PipelineBuffer actual;
            target.Buffer = expected;
            actual = target.Buffer;
            Assert.AreEqual(expected, actual);
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
            PassThreadState target = new PassThreadState(columnToProcess, buffer, metaData, threadReset, true, false);
            Assert.IsNotNull(target);
            Assert.AreEqual(columnToProcess, target.ColumnToProcess);
            Assert.AreEqual(metaData, target.MetaData);
            Assert.AreEqual(threadReset, target.ThreadReset);
            Assert.AreEqual(true, target.SafeNullHandling);
            Assert.AreEqual(false, target.MillisecondHandling);
        }        
    }
}
