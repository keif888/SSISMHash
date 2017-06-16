using System;
using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.SqlServerCe;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace MultipleHash2012Test
{
    [TestClass]
    public class MD5Tests
    {
        const string sqlCEDatabaseName = @".\MD5Test.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [MD5BinaryOutput] varbinary(16), [MD5HexOutput] nvarchar(34), [MD5BaseOutput] nvarchar(24))";
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
        ///StringData,MoreStringData,2012-01-04,18,19.05
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [MD5BinaryOutput] varbinary(16), [MD5HexOutput] varchar(34), [MD5BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashMD5Test()
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

            // Add output column MD5BinaryOutput (MD5, Binary)
            IDTSOutputColumn100 MD5BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "MD5BinaryOutput", "MD5 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            MD5BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            MD5BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.MD5;
            MD5BinaryOutput.Name = "MD5BinaryOutput";
            MD5BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            // Add output column MD5HexOutput (MD5, HexString)
            IDTSOutputColumn100 MD5HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "MD5HexOutput", "MD5 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            MD5HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            MD5HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.MD5;
            MD5HexOutput.Name = "MD5HexOutput";
            MD5HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MD5, MultipleHash.OutputTypeEnumerator.HexString, MD5HexOutput);
            // Add output column MD5BaseOutput (MD5, Base64String)
            IDTSOutputColumn100 MD5BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "MD5BaseOutput", "MD5 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            MD5BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            MD5BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.MD5;
            MD5BaseOutput.Name = "MD5BaseOutput";
            MD5BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MD5, MultipleHash.OutputTypeEnumerator.Base64String, MD5BaseOutput);

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
                            StaticTestUtilities.testValues16("MD5", sqlData, "NullRow", null, "ad0ff38c612ba6550f7f991d8d451557", "rQ/zjGErplUPf5kdjUUVVw==");
                            break;
                        case 2:
                            StaticTestUtilities.testValues16("MD5", sqlData, "StringData1", "MoreStringData1", "35ec7260ec3b96b84e026111f8d7c966", "NexyYOw7lrhOAmER+NfJZg==");
                            break;
                        case 3:
                            StaticTestUtilities.testValues16("MD5", sqlData, "StringData2", "MoreStringData2", "85070590507e30f622e85b3de2fd1be7", "hQcFkFB+MPYi6Fs94v0b5w==");
                            break;
                        case 4:
                            StaticTestUtilities.testValues16("MD5", sqlData, "StringData3", "MoreStringData3", "56c4813f94449bae1db11116a983a515", "VsSBP5REm64dsREWqYOlFQ==");
                            break;
                        case 5:
                            StaticTestUtilities.testValues16("MD5", sqlData, "StringData4", "MoreStringData4", "687502290576828a03b30658121389c2", "aHUCKQV2gooDswZYEhOJwg==");
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
