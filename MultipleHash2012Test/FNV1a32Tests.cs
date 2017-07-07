using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Martin.SQLServer.Dts;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.SqlServerCe;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace MultipleHash2012Test
{
    [TestClass()]
    public class FNV1a32Tests
    {

        const string sqlCEDatabaseName = @".\FNV1a32Test.sdf";
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
            sqlCEEngine = new SqlCeEngine(StaticTestUtilities.connectionString(sqlCEDatabaseName, sqlCEPassword));
            sqlCEEngine.CreateDatabase();

            // Connect to the sucker
            SqlCeConnection connection = new SqlCeConnection(StaticTestUtilities.connectionString(sqlCEDatabaseName, sqlCEPassword));
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            // Create the table with the test results.
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [BinaryOutput] varbinary(4), [HexOutput] nvarchar(10), [BaseOutput] nvarchar(8))";
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


        /// <summary>
        ///A test for FNV1a32 Constructor
        ///</summary>
        [TestMethod()]
        public void FNV1a32ConstructorTest()
        {
            FNV1a32 target = new FNV1a32();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTest()
        {
            FNV1a32 target = new FNV1a32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("curds and whey\n");
            byte[] expected = { 0x0c, 0x47, 0xa1, 0x19 }; // 0x19a1470cUL 
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestBufferIndexAndLength()
        {
            FNV1a32 target = new FNV1a32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("chongo was here!\n");
            int ibStart = 0;
            int cbSize = buffer.Length;
            byte[] expected = { 0xd5, 0x30, 0x99, 0xd4 }; // 0xd49930d5UL  
            byte[] actual;
            actual = target.ComputeHash(buffer, ibStart, cbSize);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            FNV1a32 expected = new FNV1a32();
            FNV1a32 actual;
            actual = FNV1a32.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest()
        {
            FNV1a32 target = new FNV1a32();
            target.Initialize();
            Assert.AreEqual(32, target.HashSize);
        }

        /// <summary>
        ///A test for CalculateHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashFNV1a32Test()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package;
            IDTSComponentMetaData100 multipleHash;
            CManagedComponentWrapper multipleHashInstance;
            String lineageString;
            MainPipe dataFlowTask;
            // Microsoft.SqlServer.Dts.Runtime.Application app;
            StaticTestUtilities.BuildSSISPackage(out package, out multipleHash, out multipleHashInstance, out lineageString, out dataFlowTask/*, out app */);

            int outputID = multipleHash.OutputCollection[0].ID;
            int outputColumnPos = multipleHash.OutputCollection[0].OutputColumnCollection.Count;

            // Add output column FNV1a32BinaryOutput (FNV1a32, Binary)
            IDTSOutputColumn100 FNV1a32BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BinaryOutput", "FNV1a32 Hash of the input");
            FNV1a32BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            FNV1a32BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.FNV1a32;
            FNV1a32BinaryOutput.Name = "BinaryOutput";
            FNV1a32BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a32, MultipleHash.OutputTypeEnumerator.Binary, FNV1a32BinaryOutput);
            // Add output column FNV1a32HexOutput (FNV1a32, HexString)
            IDTSOutputColumn100 FNV1a32HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "HexOutput", "FNV1a32 Hash of the input");
            FNV1a32HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            FNV1a32HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.FNV1a32;
            FNV1a32HexOutput.Name = "HexOutput";
            FNV1a32HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a32, MultipleHash.OutputTypeEnumerator.HexString, FNV1a32HexOutput);
            // Add output column FNV1a32BaseOutput (FNV1a32, Base64String)
            IDTSOutputColumn100 FNV1a32BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BaseOutput", "FNV1a32 Hash of the input");
            FNV1a32BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            FNV1a32BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.FNV1a32;
            FNV1a32BaseOutput.Name = "BaseOutput";
            FNV1a32BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a32, MultipleHash.OutputTypeEnumerator.Base64String, FNV1a32BaseOutput);

            // Add SQL CE Destination

            // Add SQL CE Connection
            ConnectionManager sqlCECM = null;
            IDTSComponentMetaData100 sqlCETarget = null;
            CManagedComponentWrapper sqlCEInstance = null;
            StaticTestUtilities.CreateSQLCEComponent(package, dataFlowTask, sqlCEDatabaseName, sqlCEPassword, "TestRecords", out sqlCECM, out sqlCETarget, out sqlCEInstance);
            StaticTestUtilities.CreatePath(dataFlowTask, multipleHash.OutputCollection[0], sqlCETarget, sqlCEInstance);


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
            SqlCeConnection connection = new SqlCeConnection(StaticTestUtilities.connectionString(sqlCEDatabaseName, sqlCEPassword));
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlCeCommand sqlCommand = new SqlCeCommand("SELECT * FROM [TestRecords] ORDER BY [StringData]", connection);
                SqlCeDataReader sqlData = sqlCommand.ExecuteReader(CommandBehavior.Default);
                int rowCount = 0;
                while (sqlData.Read())
                {
                    rowCount++;
                    switch (rowCount)
                    {
                        case 1:
                            StaticTestUtilities.testValues4("FNV1a32", sqlData, "NullRow", null, "f5fd7107", "9f1xBw==");
                            break;
                        case 2:
                            StaticTestUtilities.testValues4("FNV1a32", sqlData, "StringData1", "MoreStringData1", "3299e04f", "MpngTw==");
                            break;
                        case 3:
                            StaticTestUtilities.testValues4("FNV1a32", sqlData, "StringData2", "MoreStringData2", "abad79a7", "q615pw==");
                            break;
                        case 4:
                            StaticTestUtilities.testValues4("FNV1a32", sqlData, "StringData3", "MoreStringData3", "a8fec3da", "qP7D2g==");
                            break;
                        case 5:
                            StaticTestUtilities.testValues4("FNV1a32", sqlData, "StringData4", "MoreStringData4", "6369e97e", "Y2npfg==");
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
    }
}
