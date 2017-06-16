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
    public class FNV1a64Tests
    {
        const string sqlCEDatabaseName = @".\FNV1a64Test.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [BinaryOutput] varbinary(8), [HexOutput] nvarchar(18), [BaseOutput] nvarchar(16))";
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
        ///A test for FNV1a64 Constructor
        ///</summary>
        [TestMethod()]
        public void FNV1a64ConstructorTest()
        {
            FNV1a64 target = new FNV1a64();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTest()
        {
            FNV1a64 target = new FNV1a64();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("curds and whey\n");
            byte[] expected = { 0xec, 0x85, 0x13, 0xfe, 0xcc, 0x44, 0x0b, 0x1a }; // 0x1a0b44ccfe1385ecULL  
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
            FNV1a64 target = new FNV1a64(); // TODO: Initialize to an appropriate value
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("chongo was here!\n");
            int ibStart = 0;
            int cbSize = buffer.Length;
            byte[] expected = { 0x15, 0xf9, 0xf5, 0xef, 0x40, 0x09, 0x81, 0x46 }; // 0x46810940eff5f915ULL  
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
            FNV1a64 expected = new FNV1a64(); // TODO: Initialize to an appropriate value
            FNV1a64 actual;
            actual = FNV1a64.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest()
        {
            FNV1a64 target = new FNV1a64();
            target.Initialize();
            Assert.AreEqual(64, target.HashSize);
        }

        /// <summary>
        ///A test for CalculateHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashFNV1a64Test()
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

            // Add output column CRC32BinaryOutput (CRC32, Binary)
            IDTSOutputColumn100 FNV1a64BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BinaryOutput", "FNV1a64 Hash of the input");
            FNV1a64BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            FNV1a64BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.FNV1a64;
            FNV1a64BinaryOutput.Name = "BinaryOutput";
            FNV1a64BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a64, MultipleHash.OutputTypeEnumerator.Binary, FNV1a64BinaryOutput);
            // Add output column FNV1a64HexOutput (FNV1a64, HexString)
            IDTSOutputColumn100 FNV1a64HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "HexOutput", "FNV1a64 Hash of the input");
            FNV1a64HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            FNV1a64HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.FNV1a64;
            FNV1a64HexOutput.Name = "HexOutput";
            FNV1a64HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a64, MultipleHash.OutputTypeEnumerator.HexString, FNV1a64HexOutput);
            // Add output column FNV1a64BaseOutput (FNV1a64, Base64String)
            IDTSOutputColumn100 FNV1a64BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BaseOutput", "FNV1a64 Hash of the input");
            FNV1a64BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            FNV1a64BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.FNV1a64;
            FNV1a64BaseOutput.Name = "BaseOutput";
            FNV1a64BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a64, MultipleHash.OutputTypeEnumerator.Base64String, FNV1a64BaseOutput);

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
                            StaticTestUtilities.testValues8("FNV1a64", sqlData, "NullRow", null, "758396eaa4bce12f", "dYOW6qS84S8=");
                            break;
                        case 2:
                            StaticTestUtilities.testValues8("FNV1a64", sqlData, "StringData1", "MoreStringData1", "3269fd2c40e86d04", "Mmn9LEDobQQ=");
                            break;
                        case 3:
                            StaticTestUtilities.testValues8("FNV1a64", sqlData, "StringData2", "MoreStringData2", "eb95ee14223bee87", "65XuFCI77oc=");
                            break;
                        case 4:
                            StaticTestUtilities.testValues8("FNV1a64", sqlData, "StringData3", "MoreStringData3", "e8976e5edde8b3bc", "6JduXt3os7w=");
                            break;
                        case 5:
                            StaticTestUtilities.testValues8("FNV1a64", sqlData, "StringData4", "MoreStringData4", "a39fa068f937ff57", "o5+gaPk3/1c=");
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
