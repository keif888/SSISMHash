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
    public class SHA1Tests
    {
        const string sqlCEDatabaseName = @".\SHA1Test.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA1BinaryOutput] varbinary(20), [SHA1HexOutput] nvarchar(42), [SHA1BaseOutput] nvarchar(28))";
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
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA1BinaryOutput] varbinary(16), [SHA1HexOutput] varchar(34), [SHA1BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashSHA1Test()
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

            // Add output column SHA1BinaryOutput (SHA1, Binary)
            IDTSOutputColumn100 SHA1BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA1BinaryOutput", "SHA1 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA1BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            SHA1BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA1;
            SHA1BinaryOutput.Name = "SHA1BinaryOutput";
            SHA1BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA1, MultipleHash.OutputTypeEnumerator.Binary, SHA1BinaryOutput);
            // Add output column SHA1HexOutput (SHA1, HexString)
            IDTSOutputColumn100 SHA1HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA1HexOutput", "SHA1 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA1HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            SHA1HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA1;
            SHA1HexOutput.Name = "SHA1HexOutput";
            SHA1HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA1, MultipleHash.OutputTypeEnumerator.HexString, SHA1HexOutput);
            // Add output column SHA1BaseOutput (SHA1, Base64String)
            IDTSOutputColumn100 SHA1BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA1BaseOutput", "SHA1 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA1BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            SHA1BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA1;
            SHA1BaseOutput.Name = "SHA1BaseOutput";
            SHA1BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA1, MultipleHash.OutputTypeEnumerator.Base64String, SHA1BaseOutput);

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
                            StaticTestUtilities.testValues20("SHA1", sqlData, "NullRow", null, "7404459d2254bfcd2d55cddf90fb29c751012310", "dARFnSJUv80tVc3fkPspx1EBIxA=");
                            break;
                        case 2:
                            StaticTestUtilities.testValues20("SHA1", sqlData, "StringData1", "MoreStringData1", "a007b9d8891448bf08ab2e87dc36cc4fadc7ec94", "oAe52IkUSL8Iqy6H3DbMT63H7JQ=");
                            break;
                        case 3:
                            StaticTestUtilities.testValues20("SHA1", sqlData, "StringData2", "MoreStringData2", "b7e5cdd69e366cabd2ecbabe23e2e270cf1d908e", "t+XN1p42bKvS7Lq+I+LicM8dkI4=");
                            break;
                        case 4:
                            StaticTestUtilities.testValues20("SHA1", sqlData, "StringData3", "MoreStringData3", "4b135b551bcb7383c6a3c3151db51ab5862a0a65", "SxNbVRvLc4PGo8MVHbUatYYqCmU=");
                            break;
                        case 5:
                            StaticTestUtilities.testValues20("SHA1", sqlData, "StringData4", "MoreStringData4", "053d62760fea89724505e4f15fbc7d299cf6423f", "BT1idg/qiXJFBeTxX7x9KZz2Qj8=");
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
