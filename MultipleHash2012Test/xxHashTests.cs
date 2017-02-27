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

        [ClassCleanup]
        public static void RemoveSQLCEDatabase()
        {
            // Discard the previous iteration of this test database.
            if (File.Exists(sqlCEDatabaseName))
            {
                File.Delete(sqlCEDatabaseName);
            }
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
        /// </summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void xxHashWithinSSISTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package;
            IDTSComponentMetaData100 multipleHash;
            CManagedComponentWrapper multipleHashInstance;
            String lineageString;
            MainPipe dataFlowTask;
            StaticTestUtilities.BuildSSISPackage(out package, out multipleHash, out multipleHashInstance, out lineageString, out dataFlowTask);

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
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlCeCommand sqlCommand = new SqlCeCommand("SELECT * FROM [TestRecordsxxHash] ORDER BY [StringData]", connection);
                SqlCeDataReader sqlData = sqlCommand.ExecuteReader(CommandBehavior.Default);
                int rowCount = 0;
                while (sqlData.Read())
                {
                    rowCount++;
                    switch (rowCount)
                    {
                        case 1:
                            StaticTestUtilities.testValues8("xxHash", sqlData, "NullRow", null, "63d75a5b2781fb7f", "Y9daWyeB+38=");
                            break;
                        case 2:
                            StaticTestUtilities.testValues8("xxHash", sqlData, "StringData1", "MoreStringData1", "e5f10f243a28ead1", "5fEPJDoo6tE=");
                            break;
                        case 3:
                            StaticTestUtilities.testValues8("xxHash", sqlData, "StringData2", "MoreStringData2", "191f4ed7e4958618", "GR9O1+SVhhg=");
                            break;
                        case 4:
                            StaticTestUtilities.testValues8("xxHash", sqlData, "StringData3", "MoreStringData3", "fb80f19f00ce45b7", "+4DxnwDORbc=");
                            break;
                        case 5:
                            StaticTestUtilities.testValues8("xxHash", sqlData, "StringData4", "MoreStringData4", "c056c66a492d32fc", "wFbGakktMvw=");
                            break;
                        default:
                            Assert.Fail(string.Format("Account has to many records AccountCode {0}, AccountName {1}", sqlData.GetInt32(1), sqlData.GetString(2)));
                            break;
                    }
                }
                Assert.AreEqual(5, rowCount, "Rows in TestRecords");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
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
