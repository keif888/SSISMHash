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


        [TestMethod()]
        public void ProvideComponentPropertiesTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            metaDataMultipleHash.OutputCollection.New();
            metaDataMultipleHash.OutputCollection.New();
            instance.ProvideComponentProperties();
            Assert.AreEqual(3, metaDataMultipleHash.CustomPropertyCollection.Count, "Custom Properites Not Added");
            Assert.AreEqual(Utility.MultipleThreadPropName, metaDataMultipleHash.CustomPropertyCollection[0].Name, "MultipleThreadPropName is wrong");
            Assert.AreEqual(Utility.SafeNullHandlingPropName, metaDataMultipleHash.CustomPropertyCollection[1].Name, "SafeNullHandlingPropName is wrong");
            Assert.AreEqual("Input", metaDataMultipleHash.InputCollection[0].Name, "Input is not named correctly");
            Assert.AreEqual(1, metaDataMultipleHash.OutputCollection.Count, "Extra Outputs were not removed");
            Assert.AreEqual("HashedOutput", metaDataMultipleHash.OutputCollection[0].Name, "Output is not named correctly");
            Assert.AreEqual("Hashed rows are directed to this output.", metaDataMultipleHash.OutputCollection[0].Description, "Description is incorrect");
            Assert.AreEqual(metaDataMultipleHash.InputCollection[0].ID, metaDataMultipleHash.OutputCollection[0].SynchronousInputID, "SynchronousInputID is wrong");
        }


        /// <summary>
        ///A test for PerformUpgrade
        ///Ensure that the LineageID's are changed to #nn format.
        ///This test will fail if run in UnitTest mode, because the assembly isn't in the GAC!
        ///</summary>
        [TestMethod()]
        public void PerformUpgradeTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();
            instance.ReinitializeMetaData();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            // Set the LineageID value to Old format
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "1,2,3";
            Assert.AreEqual(0, metaDataMultipleHash.Version, "Version failed to match on initial create");
            // Don't ask why, but MS set the version to 0 on create...

            string packageXML, package2XML;
            package.SaveToXML(out packageXML, null);
            package = new Microsoft.SqlServer.Dts.Runtime.Package();
            // Force the Upgrade to run the 1st time.
            package.LoadFromXML(packageXML, null);
            // We have to save it, so that the meta data will be shown as changed.  Don't ask, it's an SSIS thing.
            package.SaveToXML(out package2XML, null);
            package = new Microsoft.SqlServer.Dts.Runtime.Package();
            // Load again, so that we can check that the version number is now 4!
            package.LoadFromXML(package2XML, null);

            exec = package.Executables[0];
            thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            dataFlowTask = thMainPipe.InnerObject as MainPipe;
            metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection[0];
            Assert.AreEqual(4, metaDataMultipleHash.Version, "Version failed to match on reload");
            Assert.AreEqual("#1,#2,#3", metaDataMultipleHash.OutputCollection[0].OutputColumnCollection[0].CustomPropertyCollection[Utility.InputColumnLineagePropName].Value as String, "LineageID's not updated");
        }


        [TestMethod()]
        public void ValidateToManyMultiThreadTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            IDTSCustomProperty100 multiThread = metaDataMultipleHash.CustomPropertyCollection.New();
            multiThread.Description = "Select the number of threads to use";
            multiThread.Name = Utility.MultipleThreadPropName;
            multiThread.ContainsID = false;
            multiThread.EncryptionRequired = false;
            multiThread.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            multiThread.TypeConverter = typeof(Martin.SQLServer.Dts.MultipleHash.MultipleThread).AssemblyQualifiedName;
            multiThread.Value = Martin.SQLServer.Dts.MultipleHash.MultipleThread.None;

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult =  package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "There are to many Multi Thread Properties Defined.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataToManyMultiThreadTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            IDTSCustomProperty100 multiThread = metaDataMultipleHash.CustomPropertyCollection.New();
            multiThread.Description = "Select the number of threads to use";
            multiThread.Name = Utility.MultipleThreadPropName;
            multiThread.ContainsID = false;
            multiThread.EncryptionRequired = false;
            multiThread.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            multiThread.TypeConverter = typeof(Martin.SQLServer.Dts.MultipleHash.MultipleThread).AssemblyQualifiedName;
            multiThread.Value = Martin.SQLServer.Dts.MultipleHash.MultipleThread.None;

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateToManySafeNullHandlingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            IDTSCustomProperty100 safeNullHandlingProperty = metaDataMultipleHash.CustomPropertyCollection.New();
            safeNullHandlingProperty.Description = "Select True to force Nulls and Empty Strings to be detected in Hash, False for earlier version compatability.";
            safeNullHandlingProperty.Name = Utility.SafeNullHandlingPropName;
            safeNullHandlingProperty.ContainsID = false;
            safeNullHandlingProperty.EncryptionRequired = false;
            safeNullHandlingProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            safeNullHandlingProperty.TypeConverter = typeof(Martin.SQLServer.Dts.MultipleHash.SafeNullHandling).AssemblyQualifiedName;
            safeNullHandlingProperty.Value = Martin.SQLServer.Dts.MultipleHash.SafeNullHandling.True;
            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The count of SafeNullHandling properties is not valid.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataToManySafeNullHandlingTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            IDTSCustomProperty100 safeNullHandlingProperty = metaDataMultipleHash.CustomPropertyCollection.New();
            safeNullHandlingProperty.Description = "Select True to force Nulls and Empty Strings to be detected in Hash, False for earlier version compatability.";
            safeNullHandlingProperty.Name = Utility.SafeNullHandlingPropName;
            safeNullHandlingProperty.ContainsID = false;
            safeNullHandlingProperty.EncryptionRequired = false;
            safeNullHandlingProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            safeNullHandlingProperty.TypeConverter = typeof(Martin.SQLServer.Dts.MultipleHash.SafeNullHandling).AssemblyQualifiedName;
            safeNullHandlingProperty.Value = Martin.SQLServer.Dts.MultipleHash.SafeNullHandling.True;
            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateMissingSafeNullHandlingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection.RemoveObjectByID(metaDataMultipleHash.CustomPropertyCollection[Utility.SafeNullHandlingPropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The count of SafeNullHandling properties is not valid.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataMissingSafeNullHandlingTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection.RemoveObjectByID(metaDataMultipleHash.CustomPropertyCollection[Utility.SafeNullHandlingPropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateMissingMultiThreadTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection.RemoveObjectByID(metaDataMultipleHash.CustomPropertyCollection[Utility.MultipleThreadPropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The MultiThread Property is missing.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataMissingMultiThreadTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection.RemoveObjectByID(metaDataMultipleHash.CustomPropertyCollection[Utility.MultipleThreadPropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }


        [TestMethod()]
        public void ValidateMultiThreadInvalidTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection[Utility.MultipleThreadPropName].Value = 99;

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The property MultiThread is set to an invalid value.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataMultiThreadInvalidTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection[Utility.MultipleThreadPropName].Value = 99;

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateSafeNullInvalidTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection[Utility.SafeNullHandlingPropName].Value = 99;

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The property SafeNullHandling is set to an invalid value.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataSafeNullInvalidTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.CustomPropertyCollection[Utility.SafeNullHandlingPropName].Value = 99;

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateMissingOutputTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.OutputCollection.RemoveObjectByIndex(0);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "There should be exactly one output. The metadata of this component is corrupt.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataMissingOutputTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.OutputCollection.RemoveObjectByIndex(0);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateToManyOutputsTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.OutputCollection.New();

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "There should be exactly one output. The metadata of this component is corrupt.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataToManyOutputsTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.OutputCollection.New();

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateMissingInputTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.InputCollection.RemoveObjectByIndex(0);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "There should be exactly one input.  The metadata of this component is corrupt.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataMissingInputTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.InputCollection.RemoveObjectByIndex(0);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }

        [TestMethod()]
        public void ValidateToManyInputsTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.InputCollection.New();

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "There should be exactly one input.  The metadata of this component is corrupt.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataToManyInputsTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            metaDataMultipleHash.InputCollection.New();

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, validateResult);
        }


        [TestMethod()]
        public void ValidateToManyCustomPropertiesOnOutputTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 2;
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output properties HashType and InputColumnLineageIDs are missing (or there are extra properties) on column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataToManyCustomPropertiesOnOutputTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 2;
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output properties HashType and InputColumnLineageIDs are missing (or there are extra properties) on column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }


        [TestMethod()]
        public void ValidateWrongOutputColumnTypeTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            // Set an invalid data type...
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 2;

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
            foundError = false;
            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 3, 0, 0, 0);
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
            foundError = false;
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 3, 0, 0, 0);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 2;
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataWrongOutputColumnTypeTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            // Set an invalid data type...
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }


        [TestMethod()]
        public void ValidateHashTypeMissingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection.RemoveObjectByID(outputColumn.CustomPropertyCollection[Utility.HashTypePropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);

            foundError = false;
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);

            foundError = false;
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataHashTypeMissingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection.RemoveObjectByID(outputColumn.CustomPropertyCollection[Utility.HashTypePropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);

            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#5";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);

            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#5";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }

        [TestMethod()]
        public void ValidateInputColumnLineageIDsMissingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection.RemoveObjectByID(outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
            foundError = false;
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
            foundError = false;
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataInputColumnLineageIDsMissingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection.RemoveObjectByID(outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].ID);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);
            validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);
            validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }

        [TestMethod()]
        public void ValidateBothPropertiesMissingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection.RemoveObjectByID(outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].ID);
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output properties HashType and InputColumnLineageIDs are missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataBothPropertiesMissingTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection.RemoveObjectByID(outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].ID);
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output properties HashType and InputColumnLineageIDs are missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }


        [TestMethod()]
        public void ValidateBothPropertiesWrongNamesTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "More Garbage";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataBothPropertiesWrongNamesTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "More Garbage";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }

        [TestMethod()]
        public void ValidatePropertyZeroNamedWrongTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            //outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "More Garbage";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
            foundError = false;
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = Utility.HashTypePropName;
            //outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataPropertyZeroNamedWrongTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1,#2,#3";
            //outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = "More Garbage";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);
            // This still fails as the LineageID's are bogus...
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);
            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property HashType is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Name = "Garbage";
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Name = Utility.HashTypePropName;
            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs is missing from column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }

        [TestMethod()]
        public void ValidateBadLineageTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#5";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs has invalid lineage id's for column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
            foundError = false;

            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#5";

            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The output property InputColumnLineageIDs has invalid lineage id's for column OutputCol1.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataBadLineageTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#5";

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            Assert.AreEqual(string.Empty, outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value);
            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#5";

            validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            Assert.AreEqual(string.Empty, outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value);
        }

        [TestMethod()]
        public void ValidateBadHashTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 10;
            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 3, 0, 0, 0);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
            foundError = false;

            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 10;
            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 3, 0, 0, 0);

            validateResult = package.Validate(null, null, null, null);

            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Failure, validateResult);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(true, foundError);
        }

        [TestMethod()]
        public void ReinitializeMetaDataBadHashTest()
        {
            Boolean foundError = false;
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;

            IDTSComponentMetaData100 metaDataMultipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            metaDataMultipleHash.Name = "Multiple Hash Test";
            metaDataMultipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper instance = metaDataMultipleHash.Instantiate();
            instance.ProvideComponentProperties();

            // Create the input columns and their LineageID's
            IDTSInputColumn100 inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 1;
            inputColumn.Name = "Column 1";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 2;
            inputColumn.Name = "Column 2";
            inputColumn = metaDataMultipleHash.InputCollection[0].InputColumnCollection.New();
            inputColumn.LineageID = 3;
            inputColumn.Name = "Column 3";

            // Create a new output column
            IDTSOutputColumn100 outputColumn = instance.InsertOutputColumnAt(metaDataMultipleHash.OutputCollection[0].ID, 0, "OutputCol1", "Output Column 1");
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 4;
            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 3, 0, 0, 0);

            Microsoft.SqlServer.Dts.Runtime.DTSExecResult validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
            foundError = false;

            outputColumn.CustomPropertyCollection.RemoveAll();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = "#1";
            outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = 4;
            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 3, 0, 0, 0);

            validateResult = package.Validate(null, null, null, null);
            instance.ReinitializeMetaData();
            validateResult = package.Validate(null, null, null, null);

            foreach (Microsoft.SqlServer.Dts.Runtime.DtsError errorResult in package.Errors)
            {
                if (errorResult.Description == "The HashType and Column Data Types do not match.")
                {
                    foundError = true;
                    break;
                }
            }
            Assert.AreEqual(false, foundError);
        }

        /// <summary>
        ///A test for AddInputLineageIDsProperty
        ///</summary>
        [TestMethod()]
        
        public void AddInputLineageIDsPropertyTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor(); // TODO: Initialize to an appropriate value
            IDTSOutput100 output = new OutputTestImpl();
            IDTSOutputColumn100 outputColumn = output.OutputColumnCollection.New();
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            Assert.AreEqual(Utility.InputColumnLineagePropName, outputColumn.CustomPropertyCollection[0].Name);
            Assert.AreEqual("Enter the Lineage ID's that will be used to calculate the hash for this output column.", outputColumn.CustomPropertyCollection[0].Description);
            Assert.AreEqual(true, outputColumn.CustomPropertyCollection[0].ContainsID);
            Assert.AreEqual(false, outputColumn.CustomPropertyCollection[0].EncryptionRequired);
            Assert.AreEqual(DTSCustomPropertyExpressionType.CPET_NONE, outputColumn.CustomPropertyCollection[0].ExpressionType);
            Assert.AreEqual(string.Empty, outputColumn.CustomPropertyCollection[0].Value);
        }



        /// <summary>
        ///A test for AddMultipleThreadProperty
        ///</summary>
        [TestMethod()]
        
        public void AddMultipleThreadPropertyTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor(); // TODO: Initialize to an appropriate value
            IDTSComponentMetaData100 metaData = new ComponentMetaDataTestImpl();
            MultipleHash_Accessor.AddMultipleThreadProperty(metaData);
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
        
        public void AddHashTypePropertyTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor();
            IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
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
        
        public void ValidateDataTypeTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor(); 
            IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
            MultipleHash_Accessor.AddHashTypeProperty(outputColumn);
            MultipleHash_Accessor.AddInputLineageIDsProperty(outputColumn);
            int customPropertyIndex = 0;
            bool expected = true;
            bool actual;
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5;
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.RipeMD160, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.RipeMD160;
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA1, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA1;
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA256, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA256;
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA384, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA384;
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512;
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            expected = false;
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            expected = false;
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.RipeMD160, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.RipeMD160;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA1, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA1;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA256, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA256;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA384, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA384;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
            expected = false;
            Utility.SetOutputColumnDataType(Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.SHA512, outputColumn);
            outputColumn.CustomPropertyCollection[0].Value = Martin.SQLServer.Dts.MultipleHash_Accessor.HashTypeEnumerator.MD5;
            outputColumn.SetDataTypeProperties(DataType.DT_BOOL, 0, 0, 0, 0);
            actual = MultipleHash_Accessor.ValidateDataType(outputColumn, customPropertyIndex);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for FixColumnList
        ///</summary>
        [TestMethod()]
        
        public void FixColumnListTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor();
            string inputLineageIDs = "#1,#2,#3";
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
            expected = "#1,#2";
            // Test with 2 input columns, and to many input linage ids
            actual = target.FixColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual);
            inputColumn = inputColumns.New();
            inputColumn.LineageID = 3;
            expected = "#1,#2,#3";
            // Test with 3 input columns, and correct number of linage ids
            actual = target.FixColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual);
            inputLineageIDs = string.Empty;
            inputColumn = inputColumns.New();
            inputColumn.LineageID = 3;
            expected = "#1";
            inputLineageIDs = "#4,#1";
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
        
        public void ValidateColumnListTest()
        {
            MultipleHash_Accessor target = new MultipleHash_Accessor();
            string inputLineageIDs = string.Empty;
            IDTSInputColumnCollection100 inputColumns = new InputColumnCollectionTestImpl(); 
            bool expected = false;
            bool actual;
            actual = target.ValidateColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual, "No input columns or lineageid");
            inputLineageIDs = "#1";
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
            inputLineageIDs = "#1,#3";
            expected = false;
            actual = target.ValidateColumnList(inputLineageIDs, inputColumns);
            Assert.AreEqual(expected, actual, "2 input, 2 lineage one incorrect");
        }

        /// <summary>
        ///A test for AddSafeNullHandlingProperty
        ///</summary>
        [TestMethod()]
        public void AddSafeNullHandlingPropertyTest()
        {
            IDTSComponentMetaData100 metaData = new ComponentMetaDataTestImpl();
            MultipleHash_Accessor.AddSafeNullHandlingProperty(metaData);
            Assert.AreEqual("Select True to force Nulls and Empty Strings to be detected in Hash, False for earlier version compatability.", metaData.CustomPropertyCollection[0].Description);
            Assert.AreEqual(Utility.SafeNullHandlingPropName, metaData.CustomPropertyCollection[0].Name);
            Assert.AreEqual(false, metaData.CustomPropertyCollection[0].ContainsID);
            Assert.AreEqual(false, metaData.CustomPropertyCollection[0].EncryptionRequired);
            Assert.AreEqual(DTSCustomPropertyExpressionType.CPET_NONE, metaData.CustomPropertyCollection[0].ExpressionType);
            Assert.AreEqual(typeof(Martin.SQLServer.Dts.MultipleHash.SafeNullHandling).AssemblyQualifiedName, metaData.CustomPropertyCollection[0].TypeConverter);
            Assert.AreEqual(Martin.SQLServer.Dts.MultipleHash.SafeNullHandling.True, metaData.CustomPropertyCollection[0].Value);
        }
    }
}
