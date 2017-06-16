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
    public class UnitTestCRC32
    {
        const string sqlCEDatabaseName = @".\CRC32Test.sdf";
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
        ///A test for CalculateHash
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashCRC32Test()
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
            IDTSOutputColumn100 CRC32BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BinaryOutput", "CRC32 Hash of the input");
            CRC32BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            CRC32BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.CRC32;
            CRC32BinaryOutput.Name = "BinaryOutput";
            CRC32BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32, MultipleHash.OutputTypeEnumerator.Binary, CRC32BinaryOutput);
            // Add output column CRC32HexOutput (CRC32, HexString)
            IDTSOutputColumn100 CRC32HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "HexOutput", "CRC32 Hash of the input");
            CRC32HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            CRC32HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.CRC32;
            CRC32HexOutput.Name = "HexOutput";
            CRC32HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32, MultipleHash.OutputTypeEnumerator.HexString, CRC32HexOutput);
            // Add output column CRC32BaseOutput (CRC32, Base64String)
            IDTSOutputColumn100 CRC32BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BaseOutput", "CRC32 Hash of the input");
            CRC32BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            CRC32BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.CRC32;
            CRC32BaseOutput.Name = "BaseOutput";
            CRC32BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32, MultipleHash.OutputTypeEnumerator.Base64String, CRC32BaseOutput);

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
                            StaticTestUtilities.testValues4("CRC32", sqlData, "NullRow", null, "b72e9896", "ty6Ylg==");
                            break;
                        case 2:
                            StaticTestUtilities.testValues4("CRC32", sqlData, "StringData1", "MoreStringData1", "4fa69232", "T6aSMg==");
                            break;
                        case 3:
                            StaticTestUtilities.testValues4("CRC32", sqlData, "StringData2", "MoreStringData2", "12988cea", "EpiM6g==");
                            break;
                        case 4:
                            StaticTestUtilities.testValues4("CRC32", sqlData, "StringData3", "MoreStringData3", "631f9b57", "Yx+bVw==");
                            break;
                        case 5:
                            StaticTestUtilities.testValues4("CRC32", sqlData, "StringData4", "MoreStringData4", "f8bed23b", "+L7SOw==");
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


        /// <summary>
        ///A test for CRC32 Constructor
        ///</summary>
        [TestMethod()]
        public void CRC32ConstructorTest()
        {
            CRC32 target = new CRC32();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CRC32 expected = new CRC32();
            CRC32 actual;
            actual = CRC32.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestJustBuffer()
        {
            CRC32 target = new CRC32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz");
            byte[] expected = { 0xbd, 0x50, 0x27, 0x4c };
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
