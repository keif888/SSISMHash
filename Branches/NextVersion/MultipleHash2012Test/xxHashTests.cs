using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Martin.SQLServer.Dts;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.SqlServerCe;
using System.Data;
using System.Diagnostics;
using Martin.SQLServer.Dts.Tests;
using System.IO;

namespace MultipleHash2012Test
{
    [TestClass]
    public class xxHashTests
    {
        const string sqlCEDatabaseName = @".\xxHashTest.sdf";
        const string sqlCEPassword = "MartinSource";
        SqlCeEngine sqlCEEngine = null;

        [TestInitialize]
        public void SetupSQLCEDatabase()
        {
            // Discard the previous iteration of this test database.
            if (File.Exists(sqlCEDatabaseName))
            {
                File.Delete(sqlCEDatabaseName);
            }

            // Connect to SQL CE, and create the new database
            sqlCEEngine = new SqlCeEngine(connectionString());
            sqlCEEngine.CreateDatabase();

            // Connect to the sucker
            SqlCeConnection connection = new SqlCeConnection(connectionString());
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // Create the table with the test results.
            String tableCreate = "CREATE TABLE [TestRecordsxxHash] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [xxHashBinaryOutput] varbinary(8), [xxHashHexOutput] nvarchar(128), [xxHashBaseOutput] nvarchar(128))";
            SqlCeCommand command = new SqlCeCommand(tableCreate, connection);
            command.ExecuteNonQuery();

            // Diconnect from the SQL CE database
            connection.Close();
            sqlCEEngine.Dispose();
        }

        [TestMethod]
        public void InitialiseTest()
        {
            xxHash actual = new Martin.SQLServer.Dts.xxHash();
            actual.Initialize();
        }

        /// <summary>
        /// Test that a byte value results in the correct output hash value.
        /// </summary>
        [TestMethod]
        public void xxHashTestComputeHashBase()
        {
            xxHash target = new Martin.SQLServer.Dts.xxHash();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("ThisIsATestValueForxxHash64");
            byte[] expected = GetStringToBytes("f8d1c99eda0d8577");  // https://asecuritysite.com/encryption/xxHash  BUT, reverse the hash bytes.
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        /// Test that a byte value starting from the start and ending at the end of the byte value results in the correct output hash value
        /// </summary>
        [TestMethod]
        public void xxHashTestComputeHashOffsetZero()
        {
            xxHash target = new Martin.SQLServer.Dts.xxHash();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("ThisIsATestValueForxxHash64");
            byte[] expected = GetStringToBytes("f8d1c99eda0d8577");  // https://asecuritysite.com/encryption/xxHash  BUT, reverse the hash bytes.
            byte[] actual;
            actual = target.ComputeHash(buffer,0, buffer.Length);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        /// Test that a byte value that starts 10 bytes in, and ends at the end results in the correct output hash value
        /// </summary>
        [TestMethod]
        public void xxHashTestComputeHashOffsetTen()
        {
            xxHash target = new Martin.SQLServer.Dts.xxHash();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("0123456789ThisIsATestValueForxxHash64");
            byte[] expected = GetStringToBytes("f8d1c99eda0d8577");  // https://asecuritysite.com/encryption/xxHash  BUT, reverse the hash bytes.
            byte[] actual;
            actual = target.ComputeHash(buffer, 10, buffer.Length - 10);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        /// Test that a byte value that starts 10 bytes in, and ends 5 bytes in results in the correct output hash value
        /// </summary>
        [TestMethod]
        public void xxHashTestComputeHashOffsetTenPartial()
        {
            xxHash target = new Martin.SQLServer.Dts.xxHash();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("0123456789ThisIsATestValueForxxHash6401234");
            byte[] expected = GetStringToBytes("f8d1c99eda0d8577");  // https://asecuritysite.com/encryption/xxHash  BUT, reverse the hash bytes.
            byte[] actual;
            actual = target.ComputeHash(buffer, 10, buffer.Length - 15);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }


        /// <summary>
        ///A test for CalculateHash
        ///StringData,MoreStringData,2012-01-04,18,19.05
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [MD5BinaryOutput] varbinary(16), [MD5HexOutput] varchar(34), [MD5BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void xxHashWithinSSISTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;
            ComponentEventHandler events = new ComponentEventHandler();
            dataFlowTask.Events = DtsConvert.GetExtendedInterface(events as IDTSComponentEvents);

            // Create a flat file source
            ConnectionManager flatFileConnectionManager = package.Connections.Add("FLATFILE");
            flatFileConnectionManager.Properties["Format"].SetValue(flatFileConnectionManager, "Delimited");
            flatFileConnectionManager.Properties["Name"].SetValue(flatFileConnectionManager, "Flat File Connection");
            flatFileConnectionManager.Properties["ConnectionString"].SetValue(flatFileConnectionManager, @".\TextDataToBeHashed.txt");
            flatFileConnectionManager.Properties["ColumnNamesInFirstDataRow"].SetValue(flatFileConnectionManager, false);
            flatFileConnectionManager.Properties["HeaderRowDelimiter"].SetValue(flatFileConnectionManager, "\r\n");
            flatFileConnectionManager.Properties["TextQualifier"].SetValue(flatFileConnectionManager, "\"");
            flatFileConnectionManager.Properties["DataRowsToSkip"].SetValue(flatFileConnectionManager, 0);
            flatFileConnectionManager.Properties["Unicode"].SetValue(flatFileConnectionManager, false);
            flatFileConnectionManager.Properties["CodePage"].SetValue(flatFileConnectionManager, 1252);

            // Create the columns in the flat file
            IDTSConnectionManagerFlatFile100 flatFileConnection = flatFileConnectionManager.InnerObject as IDTSConnectionManagerFlatFile100;
            IDTSConnectionManagerFlatFileColumn100 StringDataColumn = flatFileConnection.Columns.Add();
            StringDataColumn.ColumnDelimiter = ",";
            StringDataColumn.ColumnType = "Delimited";
            StringDataColumn.DataType = DataType.DT_STR;
            StringDataColumn.DataPrecision = 0;
            StringDataColumn.DataScale = 0;
            StringDataColumn.MaximumWidth = 255;
            ((IDTSName100)StringDataColumn).Name = "StringData";
            IDTSConnectionManagerFlatFileColumn100 MoreStringColumn = flatFileConnection.Columns.Add();
            MoreStringColumn.ColumnDelimiter = ",";
            MoreStringColumn.ColumnType = "Delimited";
            MoreStringColumn.DataType = DataType.DT_STR;
            MoreStringColumn.DataPrecision = 0;
            MoreStringColumn.DataScale = 0;
            MoreStringColumn.MaximumWidth = 255;
            ((IDTSName100)MoreStringColumn).Name = "MoreString";
            IDTSConnectionManagerFlatFileColumn100 DateColumn = flatFileConnection.Columns.Add();
            DateColumn.ColumnDelimiter = ",";
            DateColumn.ColumnType = "Delimited";
            DateColumn.DataType = DataType.DT_DATE;
            DateColumn.DataPrecision = 0;
            DateColumn.DataScale = 0;
            DateColumn.MaximumWidth = 0;
            ((IDTSName100)DateColumn).Name = "DateColumn";
            IDTSConnectionManagerFlatFileColumn100 IntegerColumn = flatFileConnection.Columns.Add();
            IntegerColumn.ColumnDelimiter = ",";
            IntegerColumn.ColumnType = "Delimited";
            IntegerColumn.DataType = DataType.DT_I4;
            IntegerColumn.DataPrecision = 0;
            IntegerColumn.DataScale = 0;
            IntegerColumn.MaximumWidth = 0;
            ((IDTSName100)IntegerColumn).Name = "IntegerColumn";
            IDTSConnectionManagerFlatFileColumn100 NumericColumn = flatFileConnection.Columns.Add();
            NumericColumn.ColumnDelimiter = "\r\n";
            NumericColumn.ColumnType = "Delimited";
            NumericColumn.DataType = DataType.DT_NUMERIC;
            NumericColumn.DataPrecision = 15;
            NumericColumn.DataScale = 2;
            NumericColumn.MaximumWidth = 0;
            ((IDTSName100)NumericColumn).Name = "NumericColumn";

            var app = new Microsoft.SqlServer.Dts.Runtime.Application();

            IDTSComponentMetaData100 flatFileSource = dataFlowTask.ComponentMetaDataCollection.New();
            flatFileSource.ComponentClassID = app.PipelineComponentInfos["Flat File Source"].CreationName;
            // Get the design time instance of the Flat File Source Component
            var flatFileSourceInstance = flatFileSource.Instantiate();
            flatFileSourceInstance.ProvideComponentProperties();

            flatFileSource.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(flatFileConnectionManager);
            flatFileSource.RuntimeConnectionCollection[0].ConnectionManagerID = flatFileConnectionManager.ID;

            // Reinitialize the metadata.
            flatFileSourceInstance.AcquireConnections(null);
            flatFileSourceInstance.ReinitializeMetaData();
            flatFileSourceInstance.ReleaseConnections();
            flatFileSource.CustomPropertyCollection["RetainNulls"].Value = true;



            //[MD5BinaryOutput] varbinary(16), [MD5HexOutput] varchar(34), [MD5BaseOutput] varchar(24))";
            IDTSComponentMetaData100 multipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            multipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper multipleHashInstance = multipleHash.Instantiate();

            multipleHashInstance.ProvideComponentProperties();
            multipleHash.Name = "Multiple Hash Test";
            multipleHashInstance.ReinitializeMetaData();

            // Create the path from source to destination.
            CreatePath(dataFlowTask, flatFileSource.OutputCollection[0], multipleHash, multipleHashInstance);

            // Select the input columns.
            IDTSInput100 multipleHashInput = multipleHash.InputCollection[0];
            IDTSVirtualInput100 multipleHashvInput = multipleHashInput.GetVirtualInput();
            foreach (IDTSVirtualInputColumn100 vColumn in multipleHashvInput.VirtualInputColumnCollection)
            {
                multipleHashInstance.SetUsageType(multipleHashInput.ID, multipleHashvInput, vColumn.LineageID, DTSUsageType.UT_READONLY);
            }

            // Add the output columns
            // Generate the Lineage String
            String lineageString = String.Empty;
            foreach (IDTSInputColumn100 inputColumn in multipleHashInput.InputColumnCollection)
            {
                if (lineageString == String.Empty)
                {
                    lineageString = String.Format("#{0}", inputColumn.LineageID);
                }
                else
                {
                    lineageString = String.Format("{0},#{1}", lineageString, inputColumn.LineageID);
                }
            }

            int outputID = multipleHash.OutputCollection[0].ID;
            int outputColumnPos = multipleHash.OutputCollection[0].OutputColumnCollection.Count;

            // Add output column xxHashBinaryOutput (xxHash, Binary)
            IDTSOutputColumn100 xxHashBinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "xxHashBinaryOutput", "xxHash Hash of the input");
            xxHashBinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            xxHashBinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.xxHash;
            xxHashBinaryOutput.Name = "xxHashBinaryOutput";
            xxHashBinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.xxHash, MultipleHash.OutputTypeEnumerator.Binary, xxHashBinaryOutput);
            // Add output column xxHashHexOutput (xxHash, HexString)
            IDTSOutputColumn100 xxHashHexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "xxHashHexOutput", "xxHash Hash of the input");
            xxHashHexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            xxHashHexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.xxHash;
            xxHashHexOutput.Name = "xxHashHexOutput";
            xxHashHexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.xxHash, MultipleHash.OutputTypeEnumerator.HexString, xxHashHexOutput);
            // Add output column xxHashBaseOutput (xxHash, Base64String)
            IDTSOutputColumn100 xxHashBaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "xxHashBaseOutput", "xxHash Hash of the input");
            xxHashBaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            xxHashBaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.xxHash;
            xxHashBaseOutput.Name = "xxHashBaseOutput";
            xxHashBaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.xxHash, MultipleHash.OutputTypeEnumerator.Base64String, xxHashBaseOutput);

            // Add SQL CE Destination

            // Add SQL CE Connection
            ConnectionManager sqlCECM = null;
            IDTSComponentMetaData100 sqlCETarget = null;
            CManagedComponentWrapper sqlCEInstance = null;
            CreateSQLCEComponent(package, dataFlowTask, "TestRecordsxxHash", out sqlCECM, out sqlCETarget, out sqlCEInstance);
            CreatePath(dataFlowTask, multipleHash.OutputCollection[0], sqlCETarget, sqlCEInstance);

            app.SaveToXml(@"d:\test\test.dtsx", package, null);

            // Create a package events handler, to catch the output when running.
            PackageEventHandler packageEvents = new PackageEventHandler();

            // Execute the package
            Microsoft.SqlServer.Dts.Runtime.DTSExecResult result = package.Execute(null, null, packageEvents as IDTSEvents, null, null);
            foreach (String message in packageEvents.eventMessages)
            {
                Debug.WriteLine(message);
            }
            // Make sure the package worked.
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.DTSExecResult.Success, result, "Execution Failed");

            // Connect to the SQLCE database
            SqlCeConnection connection = new SqlCeConnection(connectionString());
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCeCommand sqlCommand = new SqlCeCommand("SELECT * FROM [TestRecordsxxHash] ORDER BY [StringData]", connection);
            SqlCeDataReader sqlData = sqlCommand.ExecuteReader(CommandBehavior.Default);
            int rowCount = 0;
            byte[] byteResult = null;
            byte[] byteExpected = null;
            while (sqlData.Read())
            {
                rowCount++;
                switch (rowCount)
                {
                    case 1:
                        Assert.AreEqual("NullRow", sqlData.GetString(0), "StringData <> NullRow");
                        Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
                        byteResult = new byte[8];
                        sqlData.GetBytes(5, 0, byteResult, 0, 8);
                        byteExpected = GetStringToBytes("63d75a5b2781fb7f");
                        testByteValues(byteExpected, byteResult, "xxHash Hash as Binary");
                        Assert.AreEqual("0x63d75a5b2781fb7f", sqlData.GetString(6).ToLower(), "xxHash Hash as Hex String");
                        Assert.AreEqual("Y9daWyeB+38=", sqlData.GetString(7), "xxHash as Base 64");
                        break;
                    case 2:
                        Assert.AreEqual("StringData1", sqlData.GetString(0), "StringData <> StringData1");
                        Assert.AreEqual("MoreStringData1", sqlData.GetString(1), "AccountName <> MoreStringData1");
                        byteResult = new byte[8];
                        sqlData.GetBytes(5, 0, byteResult, 0, 8);
                        byteExpected = GetStringToBytes("e5f10f243a28ead1");
                        testByteValues(byteExpected, byteResult, "xxHash Hash as Binary");
                        Assert.AreEqual("0xe5f10f243a28ead1", sqlData.GetString(6).ToLower(), "xxHash Hash as Hex String");
                        Assert.AreEqual("5fEPJDoo6tE=", sqlData.GetString(7), "xxHash as Base 64");
                        break;
                    case 3:
                        Assert.AreEqual("StringData2", sqlData.GetString(0), "StringData <> StringData2");
                        Assert.AreEqual("MoreStringData2", sqlData.GetString(1), "AccountName <> MoreStringData2");
                        byteResult = new byte[8];
                        sqlData.GetBytes(5, 0, byteResult, 0, 8);
                        byteExpected = GetStringToBytes("191f4ed7e4958618");
                        testByteValues(byteExpected, byteResult, "xxHash Hash as Binary");
                        Assert.AreEqual("0x191f4ed7e4958618", sqlData.GetString(6).ToLower(), "xxHash Hash as Hex String");
                        Assert.AreEqual("GR9O1+SVhhg=", sqlData.GetString(7), "xxHash as Base 64");
                        break;
                    case 4:
                        Assert.AreEqual("StringData3", sqlData.GetString(0), "StringData <> StringData3");
                        Assert.AreEqual("MoreStringData3", sqlData.GetString(1), "AccountName <> MoreStringData3");
                        byteResult = new byte[8];
                        sqlData.GetBytes(5, 0, byteResult, 0, 8);
                        byteExpected = GetStringToBytes("fb80f19f00ce45b7");
                        testByteValues(byteExpected, byteResult, "xxHash Hash as Binary");
                        Assert.AreEqual("0xfb80f19f00ce45b7", sqlData.GetString(6).ToLower(), "xxHash Hash as Hex String");
                        Assert.AreEqual("+4DxnwDORbc=", sqlData.GetString(7), "xxHash as Base 64");
                        break;
                    case 5:
                        Assert.AreEqual("StringData4", sqlData.GetString(0), "StringData <> StringData4");
                        Assert.AreEqual("MoreStringData4", sqlData.GetString(1), "AccountName <> MoreStringData4");
                        byteResult = new byte[8];
                        sqlData.GetBytes(5, 0, byteResult, 0, 8);
                        byteExpected = GetStringToBytes("c056c66a492d32fc");
                        testByteValues(byteExpected, byteResult, "xxHash Hash as Binary");
                        Assert.AreEqual("0xc056c66a492d32fc", sqlData.GetString(6).ToLower(), "xxHash Hash as Hex String");
                        Assert.AreEqual("wFbGakktMvw=", sqlData.GetString(7), "xxHash as Base 64");
                        break;
                    default:
                        Assert.Fail(string.Format("Account has to many records AccountCode {0}, AccountName {1}", sqlData.GetInt32(1), sqlData.GetString(2)));
                        break;
                }
            }
            Assert.AreEqual(5, rowCount, "Rows in TestRecords");
            connection.Close();
            sqlCEEngine.Dispose();

        }

        #region Private Functions

        private void CreatePath(MainPipe dataFlowTask, IDTSOutput100 fromOutput, IDTSComponentMetaData100 toComponent, CManagedComponentWrapper toInstance)
        {
            // Create the path from source to destination.
            IDTSPath100 path = dataFlowTask.PathCollection.New();
            path.AttachPathAndPropagateNotifications(fromOutput, toComponent.InputCollection[0]);

            // Get the destination's default input and virtual input.
            IDTSInput100 input = toComponent.InputCollection[0];
            IDTSVirtualInput100 vInput = input.GetVirtualInput();

            // Iterate through the virtual input column collection.
            foreach (IDTSVirtualInputColumn100 vColumn in vInput.VirtualInputColumnCollection)
            {
                // Find external column by name
                IDTSExternalMetadataColumn100 externalColumn = null;
                foreach (IDTSExternalMetadataColumn100 column in input.ExternalMetadataColumnCollection)
                {
                    if (String.Compare(column.Name, vColumn.Name, true) == 0)
                    {
                        externalColumn = column;
                        break;
                    }
                }
                if (externalColumn != null)
                {
                    // Select column, and retain new input column
                    IDTSInputColumn100 inputColumn = toInstance.SetUsageType(input.ID, vInput, vColumn.LineageID, DTSUsageType.UT_READONLY);
                    // Map input column to external column
                    toInstance.MapInputColumn(input.ID, inputColumn.ID, externalColumn.ID);
                }
            }
        }

        private void CreateSQLCEComponent(Microsoft.SqlServer.Dts.Runtime.Package package, MainPipe dataFlowTask, String tableName, out ConnectionManager sqlCECM, out IDTSComponentMetaData100 sqlCETarget, out CManagedComponentWrapper sqlCEInstance)
        {
            // Add SQL CE Connection
            sqlCECM = package.Connections.Add("SQLMOBILE");
            sqlCECM.ConnectionString = connectionString();
            sqlCECM.Name = "SQLCE Destination " + tableName;

            sqlCETarget = dataFlowTask.ComponentMetaDataCollection.New();
            sqlCETarget.ComponentClassID = typeof(Microsoft.SqlServer.Dts.Pipeline.SqlCEDestinationAdapter).AssemblyQualifiedName;
            sqlCEInstance = sqlCETarget.Instantiate();
            sqlCEInstance.ProvideComponentProperties();
            sqlCETarget.Name = "SQLCE Target " + tableName;
            sqlCETarget.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(sqlCECM);
            sqlCETarget.RuntimeConnectionCollection[0].ConnectionManagerID = sqlCECM.ID;

            sqlCETarget.CustomPropertyCollection["Table Name"].Value = tableName;
            sqlCEInstance.AcquireConnections(null);
            sqlCEInstance.ReinitializeMetaData();
            sqlCEInstance.ReleaseConnections();
        }

        private static string connectionString()
        {
            return String.Format("DataSource=\"{0}\"; Password='{1}'", sqlCEDatabaseName, sqlCEPassword);
        }

        #endregion


        #region Byte Stuff

        public byte[] GetStringToBytes(string value)
        {
            SoapHexBinary shb = SoapHexBinary.Parse(value);
            return shb.Value;
        }

        public void testByteValues(byte[] expected, byte[] actual, String message)
        {
            Assert.AreEqual(expected.Length, actual.Length, String.Format("{0} Length", message));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], String.Format("{0} Array Item {1}", message, i));
            }
        }

        #endregion
    }
}
