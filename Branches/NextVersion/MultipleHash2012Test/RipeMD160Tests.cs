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
    public class RipeMD160Tests
    {
        const string sqlCEDatabaseName = @".\RipeMD160Test.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [RipeMD160BinaryOutput] varbinary(20), [RipeMD160HexOutput] nvarchar(42), [RipeMD160BaseOutput] nvarchar(28))";
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
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [RipeMD160BinaryOutput] varbinary(16), [RipeMD160HexOutput] varchar(34), [RipeMD160BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashRipeMD160Test()
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

            // Add output column RipeMD160BinaryOutput (RipeMD160, Binary)
            IDTSOutputColumn100 RipeMD160BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "RipeMD160BinaryOutput", "RipeMD160 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            RipeMD160BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            RipeMD160BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.RipeMD160;
            RipeMD160BinaryOutput.Name = "RipeMD160BinaryOutput";
            RipeMD160BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.RipeMD160, MultipleHash.OutputTypeEnumerator.Binary, RipeMD160BinaryOutput);
            // Add output column RipeMD160HexOutput (RipeMD160, HexString)
            IDTSOutputColumn100 RipeMD160HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "RipeMD160HexOutput", "RipeMD160 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            RipeMD160HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            RipeMD160HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.RipeMD160;
            RipeMD160HexOutput.Name = "RipeMD160HexOutput";
            RipeMD160HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.RipeMD160, MultipleHash.OutputTypeEnumerator.HexString, RipeMD160HexOutput);
            // Add output column RipeMD160BaseOutput (RipeMD160, Base64String)
            IDTSOutputColumn100 RipeMD160BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "RipeMD160BaseOutput", "RipeMD160 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            RipeMD160BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            RipeMD160BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.RipeMD160;
            RipeMD160BaseOutput.Name = "RipeMD160BaseOutput";
            RipeMD160BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.RipeMD160, MultipleHash.OutputTypeEnumerator.Base64String, RipeMD160BaseOutput);

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
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCeCommand sqlCommand = new SqlCeCommand("SELECT * FROM [TestRecords] ORDER BY [StringData]", connection);
            try
            {
                SqlCeDataReader sqlData = sqlCommand.ExecuteReader(CommandBehavior.Default);
                int rowCount = 0;
                while (sqlData.Read())
                {
                    rowCount++;
                    switch (rowCount)
                    {
                        case 1:
                            StaticTestUtilities.testValues20("RipeMD160", sqlData, "NullRow", null, "83c5a4d2b3d607c4dc49829011a16cc9415ac865", "g8Wk0rPWB8TcSYKQEaFsyUFayGU=");
                            break;
                        case 2:
                            StaticTestUtilities.testValues20("RipeMD160", sqlData, "StringData1", "MoreStringData1", "95878786667e4fb8baf5e93e3d7560ead8939551", "lYeHhmZ+T7i69ek+PXVg6tiTlVE=");
                            break;
                        case 3:
                            StaticTestUtilities.testValues20("RipeMD160", sqlData, "StringData2", "MoreStringData2", "616a8ed8b2210e16da0dcfa9a126e7e0bc305b39", "YWqO2LIhDhbaDc+poSbn4LwwWzk=");
                            break;
                        case 4:
                            StaticTestUtilities.testValues20("RipeMD160", sqlData, "StringData3", "MoreStringData3", "c3a0013e3c3fc7d4e8e078ae165cb54d6ec91023", "w6ABPjw/x9To4HiuFly1TW7JECM=");
                            break;
                        case 5:
                            StaticTestUtilities.testValues20("RipeMD160", sqlData, "StringData4", "MoreStringData4", "d570b9cd0e00f44afb85d6c5e1ccaccc17280fd3", "1XC5zQ4A9Er7hdbF4cyszBcoD9M=");
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
