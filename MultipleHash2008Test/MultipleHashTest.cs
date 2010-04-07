using System;
using Martin.SQLServer.Dts;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for MultipleHashTest and is intended
    ///to contain all MultipleHashTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultipleHashTest
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


        /*
         * Still haven't worked out how to initialise the meta data (ComponentMetaData) without DTExec...
         * 
         */
        /// <summary>
        ///A test for PerformUpgrade
        ///</summary>
        [TestMethod()]
        public void PerformUpgradeTest()
        {
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
            // This test doesn't work so it's "invalidated" above.
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;
            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash.MultipleThread).AssemblyQualifiedName;
            // CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            // instance.ProvideComponentProperties();
            MultipleHash target = new MultipleHash();
            // target.ComponentMetaData // Gets a NULL
            target.ReinitializeMetaData();
            target.ProvideComponentProperties();
            int pipelineVersion = 1;
            target.PerformUpgrade(pipelineVersion);
            Assert.AreEqual(2, target.ComponentMetaData.PipelineVersion);
        }

        /// <summary>
        ///A test for AddInputLineageIDsProperty
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void AddInputLineageIDsPropertyTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor(); // TODO: Initialize to an appropriate value
            IDTSOutput100 output = new OutputTestImpl();
            IDTSOutputColumn100 outputColumn = output.OutputColumnCollection.New();
            target.AddInputLineageIDsProperty(outputColumn);
            Assert.AreEqual(Utility.InputColumnLineagePropName, outputColumn.CustomPropertyCollection[0].Name);
            Assert.AreEqual("Enter the Lineage ID's that will be used to calculate the hash for this output column.", outputColumn.CustomPropertyCollection[0].Description);
            Assert.AreEqual(false, outputColumn.CustomPropertyCollection[0].ContainsID);
            Assert.AreEqual(false, outputColumn.CustomPropertyCollection[0].EncryptionRequired);
            Assert.AreEqual(DTSCustomPropertyExpressionType.CPET_NONE, outputColumn.CustomPropertyCollection[0].ExpressionType);
            Assert.AreEqual(string.Empty, outputColumn.CustomPropertyCollection[0].Value);
        }



        /// <summary>
        ///A test for AddMultipleThreadProperty
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void AddMultipleThreadPropertyTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor(); // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 metaData = new ComponentMetaDataTestImpl();
            target.AddMultipleThreadProperty(metaData);
            Assert.AreEqual(Utility.MultipleThreadPropName, metaData.CustomPropertyCollection[0].Name);
            Assert.AreEqual("Select the number of threads to use", metaData.CustomPropertyCollection[0].Description);
            Assert.AreEqual(false, metaData.CustomPropertyCollection[0].ContainsID );
            Assert.AreEqual(false, metaData.CustomPropertyCollection[0].EncryptionRequired );
            Assert.AreEqual(DTSCustomPropertyExpressionType.CPET_NONE, metaData.CustomPropertyCollection[0].ExpressionType );
            Assert.AreEqual(typeof(Martin.SQLServer.Dts.MultipleHash.MultipleThread).AssemblyQualifiedName, metaData.CustomPropertyCollection[0].TypeConverter);
            Assert.AreEqual(Martin.SQLServer.Dts.MultipleHash_Accessor.MultipleThread.None, metaData.CustomPropertyCollection[0].Value);
        }

        /// <summary>
        ///A test for AddHashTypeProperty
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void AddHashTypePropertyTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor();
            IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
            target.AddHashTypeProperty(outputColumn);
            Assert.AreEqual(Utility.HashTypePropName, outputColumn.CustomPropertyCollection[0].Name); 
            Assert.AreEqual("Select the Hash Type that will be used for this output column.", outputColumn.CustomPropertyCollection[0].Description);
            Assert.AreEqual(false, outputColumn.CustomPropertyCollection[0].ContainsID );
            Assert.AreEqual(false, outputColumn.CustomPropertyCollection[0].EncryptionRequired);
            Assert.AreEqual(DTSCustomPropertyExpressionType.CPET_NONE, outputColumn.CustomPropertyCollection[0].ExpressionType);
            Assert.AreEqual(typeof(Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator).AssemblyQualifiedName, outputColumn.CustomPropertyCollection[0].TypeConverter);
            Assert.AreEqual(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.None, outputColumn.CustomPropertyCollection[0].Value);
        }

        /// <summary>
        ///A test for ValidateDataType
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void ValidateDataTypeTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor(); 
            IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
            target.AddHashTypeProperty(outputColumn);
            target.AddInputLineageIDsProperty(outputColumn);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5;
            int customPropertyIndex = 0;
            bool expected = true;
            bool actual;
            actual = target.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.RipeMD160, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.RipeMD160;
            actual = target.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA1, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA1;
            actual = target.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA256, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA256;
            actual = target.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA384, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA384;
            actual = target.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512;
            actual = target.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            expected = false;
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5;
            actual = target.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FixColumnList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void FixColumnListTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor();
            string inputLineageIDs = "1,2,3";
            IDTSInputColumnCollection100 inputColumns = new InputColumnCollectionTestImpl(); 
            string expected = string.Empty;
            string actual;
            // Test with no input columns
            actual = target.FixColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual);
            IDTSInputColumn100 inputColumn =  inputColumns.New();
            inputColumn.LineageID = 1;
            inputColumn = inputColumns.New();
            inputColumn.LineageID = 2;
            expected = "1,2";
            // Test with 2 input columns, and to many input linage ids
            actual = target.FixColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual);
            inputColumn = inputColumns.New();
            inputColumn.LineageID = 3;
            expected = "1,2,3";
            // Test with 3 input columns, and correct number of linage ids
            actual = target.FixColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual);
            inputLineageIDs = string.Empty;
            inputColumn = inputColumns.New();
            inputColumn.LineageID = 3;
            expected = "1";
            inputLineageIDs = "4,1";
            // Test with 3 input columns, and 1 correct and 1 incorrect input.
            actual = target.FixColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual);
            // Test with no linageids
            inputLineageIDs = string.Empty;
            expected = string.Empty;
            actual = target.FixColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ValidateColumnList
        ///</summary>
        [TestMethod()]
        [DeploymentItem("MultipleHash2008.dll")]
        public void ValidateColumnListTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor();
            string inputLineageIDs = string.Empty;
            IDTSInputColumnCollection100 inputColumns = new InputColumnCollectionTestImpl(); 
            bool expected = false;
            bool actual;
            actual = target.ValidateColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual, "No input columns or lineageid");
            inputLineageIDs = "1";
            expected = false;
            actual = target.ValidateColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual, "0 input, 1 lineage incorrect");
            IDTSInputColumn100 inputColumn = inputColumns.New();
            inputColumn.LineageID = 1;
            expected = true;
            actual = target.ValidateColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual, "1 input, 1 lineage");
            inputColumn = inputColumns.New();
            inputColumn.LineageID = 2;
            actual = target.ValidateColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual, "2 input, 1 lineage");
            inputLineageIDs = "1,3";
            expected = false;
            actual = target.ValidateColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual, "2 input, 2 lineage one incorrect");
        }
    }
}
