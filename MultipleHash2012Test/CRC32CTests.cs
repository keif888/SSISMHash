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
    public class CRC32CTests
    {
        const string sqlCEDatabaseName = @".\CRC32CTest.sdf";
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
        ///A test for CRC32C Constructor
        ///</summary>
        [TestMethod()]
        public void CRC32CConstructorTest()
        {
            CRC32C target = new CRC32C();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestBuffer()
        {
            CRC32C target = new CRC32C();
            byte[] buffer = {0x01, 0xC0, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x01, 0xFE, 0x60, 0xAC,
                            0x00, 0x00, 0x00, 0x08,
                            0x00, 0x00, 0x00, 0x04,
                            0x00, 0x00, 0x00, 0x09,
                            0x25, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00};
            byte[] expected = { 0xeb, 0x75, 0x4f, 0x66 };
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
        public void ComputeHashTestBufferOffsetLength()
        {
            CRC32C target = new CRC32C();
            byte[] buffer = {0x01, 0xC0, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x01, 0xFE, 0x60, 0xAC,
                            0x00, 0x00, 0x00, 0x08,
                            0x00, 0x00, 0x00, 0x04,
                            0x00, 0x00, 0x00, 0x09,
                            0x25, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00};
            int ibStart = 0;
            int cbSize = buffer.Length;
            byte[] expected = { 0xeb, 0x75, 0x4f, 0x66 };
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
            CRC32C expected = new CRC32C();
            CRC32C actual;
            actual = CRC32C.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for CalculateHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashCRC32CTest()
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
            IDTSOutputColumn100 CRC32CBinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BinaryOutput", "CRC32C Hash of the input");
            CRC32CBinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            CRC32CBinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.CRC32C;
            CRC32CBinaryOutput.Name = "BinaryOutput";
            CRC32CBinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.Binary, CRC32CBinaryOutput);
            // Add output column CRC32CHexOutput (CRC32C, HexString)
            IDTSOutputColumn100 CRC32CHexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "HexOutput", "CRC32C Hash of the input");
            CRC32CHexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            CRC32CHexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.CRC32C;
            CRC32CHexOutput.Name = "HexOutput";
            CRC32CHexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.HexString, CRC32CHexOutput);
            // Add output column CRC32CBaseOutput (CRC32C, Base64String)
            IDTSOutputColumn100 CRC32CBaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BaseOutput", "CRC32C Hash of the input");
            CRC32CBaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            CRC32CBaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.CRC32C;
            CRC32CBaseOutput.Name = "BaseOutput";
            CRC32CBaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.Base64String, CRC32CBaseOutput);

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
                            StaticTestUtilities.testValues4("CRC32C", sqlData, "NullRow", null, "e19030fc", "4ZAw/A==");
                            break;
                        case 2:
                            StaticTestUtilities.testValues4("CRC32C", sqlData, "StringData1", "MoreStringData1", "89a94f9f", "ialPnw==");
                            break;
                        case 3:
                            StaticTestUtilities.testValues4("CRC32C", sqlData, "StringData2", "MoreStringData2", "d2907643", "0pB2Qw==");
                            break;
                        case 4:
                            StaticTestUtilities.testValues4("CRC32C", sqlData, "StringData3", "MoreStringData3", "7f987478", "f5h0eA==");
                            break;
                        case 5:
                            StaticTestUtilities.testValues4("CRC32C", sqlData, "StringData4", "MoreStringData4", "ae3d0f61", "rj0PYQ==");
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
