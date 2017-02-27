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
    public class SHA256Tests
    {
        const string sqlCEDatabaseName = @".\SHA256Test.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA256BinaryOutput] varbinary(32), [SHA256HexOutput] nvarchar(68), [SHA256BaseOutput] nvarchar(44))";
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
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA256BinaryOutput] varbinary(16), [SHA256HexOutput] varchar(34), [SHA256BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashSHA256Test()
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

            // Add output column SHA256BinaryOutput (SHA256, Binary)
            IDTSOutputColumn100 SHA256BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA256BinaryOutput", "SHA256 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA256BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            SHA256BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA256;
            SHA256BinaryOutput.Name = "SHA256BinaryOutput";
            SHA256BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA256, MultipleHash.OutputTypeEnumerator.Binary, SHA256BinaryOutput);
            // Add output column SHA256HexOutput (SHA256, HexString)
            IDTSOutputColumn100 SHA256HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA256HexOutput", "SHA256 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA256HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            SHA256HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA256;
            SHA256HexOutput.Name = "SHA256HexOutput";
            SHA256HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA256, MultipleHash.OutputTypeEnumerator.HexString, SHA256HexOutput);
            // Add output column SHA256BaseOutput (SHA256, Base64String)
            IDTSOutputColumn100 SHA256BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA256BaseOutput", "SHA256 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA256BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            SHA256BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA256;
            SHA256BaseOutput.Name = "SHA256BaseOutput";
            SHA256BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA256, MultipleHash.OutputTypeEnumerator.Base64String, SHA256BaseOutput);

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
                            StaticTestUtilities.testValues32("SHA256", sqlData, "NullRow", null, "48c17d6227bdf750df1ef91751f5fa15425b25bc5f965802ca0e95fac5eda7cc", "SMF9Yie991DfHvkXUfX6FUJbJbxfllgCyg6V+sXtp8w=");
                            break;
                        case 2:
                            StaticTestUtilities.testValues32("SHA256", sqlData, "StringData1", "MoreStringData1", "4d511426ecd9e8206ab72db0c8c6f9636b87dbc7c0c7a9fe1164bf86a7913eed", "TVEUJuzZ6CBqty2wyMb5Y2uH28fAx6n+EWS/hqeRPu0=");
                            break;
                        case 3:
                            StaticTestUtilities.testValues32("SHA256", sqlData, "StringData2", "MoreStringData2", "4052c948bac0b3498a71e2ac7ad507b310908de1bdc2443cdcf31601d496bcc9", "QFLJSLrAs0mKceKsetUHsxCQjeG9wkQ83PMWAdSWvMk=");
                            break;
                        case 4:
                            StaticTestUtilities.testValues32("SHA256", sqlData, "StringData3", "MoreStringData3", "c4e88c32401a0ee6c0f73412eb49dace1626118feccfddf9623960f3adcf8a40", "xOiMMkAaDubA9zQS60nazhYmEY/sz935Yjlg863PikA=");
                            break;
                        case 5:
                            StaticTestUtilities.testValues32("SHA256", sqlData, "StringData4", "MoreStringData4", "4eda1ed84bc97ff2640ed39dc11a4da010de9dc74f42e1c8f562e4ff01bc8ebd", "Ttoe2EvJf/JkDtOdwRpNoBDencdPQuHI9WLk/wG8jr0=");
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
