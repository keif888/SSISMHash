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
    public class SHA512Tests
    {
        const string sqlCEDatabaseName = @".\SHA512Test.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA512BinaryOutput] varbinary(64), [SHA512HexOutput] nvarchar(136), [SHA512BaseOutput] nvarchar(88))";
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
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA512BinaryOutput] varbinary(16), [SHA512HexOutput] varchar(34), [SHA512BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashSHA512Test()
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

            // Add output column SHA512BinaryOutput (SHA512, Binary)
            IDTSOutputColumn100 SHA512BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA512BinaryOutput", "SHA512 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA512BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            SHA512BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA512;
            SHA512BinaryOutput.Name = "SHA512BinaryOutput";
            SHA512BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA512, MultipleHash.OutputTypeEnumerator.Binary, SHA512BinaryOutput);
            // Add output column SHA512HexOutput (SHA512, HexString)
            IDTSOutputColumn100 SHA512HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA512HexOutput", "SHA512 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA512HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            SHA512HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA512;
            SHA512HexOutput.Name = "SHA512HexOutput";
            SHA512HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA512, MultipleHash.OutputTypeEnumerator.HexString, SHA512HexOutput);
            // Add output column SHA512BaseOutput (SHA512, Base64String)
            IDTSOutputColumn100 SHA512BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA512BaseOutput", "SHA512 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA512BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            SHA512BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA512;
            SHA512BaseOutput.Name = "SHA512BaseOutput";
            SHA512BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA512, MultipleHash.OutputTypeEnumerator.Base64String, SHA512BaseOutput);

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
                            StaticTestUtilities.testValues64("SHA512", sqlData, "NullRow", null, "50c86f4f943e637eb2c42db429731031e210b4d9f5b50abc874e89273405ce75d2a409f85964c54420ec0de3d8626850f850c8a8591c50c692e44e17cd305849", "UMhvT5Q+Y36yxC20KXMQMeIQtNn1tQq8h06JJzQFznXSpAn4WWTFRCDsDePYYmhQ+FDIqFkcUMaS5E4XzTBYSQ==");
                            break;
                        case 2:
                            StaticTestUtilities.testValues64("SHA512", sqlData, "StringData1", "MoreStringData1", "0ef1f38cf79cba4bdceb303ceed5372cb1236b04f4860b705345e5e6a922303dc6a4aa9646c2649bc437790d3fd4b936672659423b86a52ec95a4eb50ba791a3", "DvHzjPecukvc6zA87tU3LLEjawT0hgtwU0Xl5qkiMD3GpKqWRsJkm8Q3eQ0/1Lk2ZyZZQjuGpS7JWk61C6eRow==");
                            break;
                        case 3:
                            StaticTestUtilities.testValues64("SHA512", sqlData, "StringData2", "MoreStringData2", "3c6257a9044773472704a838cacc6ef738b6bd8e580011c20a2fed85a2c710aad16b825858f779857f6b3fedf1e4821c3970aaccfbef5cb7406b0800dfbc1029", "PGJXqQRHc0cnBKg4ysxu9zi2vY5YABHCCi/thaLHEKrRa4JYWPd5hX9rP+3x5IIcOXCqzPvvXLdAawgA37wQKQ==");
                            break;
                        case 4:
                            StaticTestUtilities.testValues64("SHA512", sqlData, "StringData3", "MoreStringData3", "b7695273172e0fda459c3e165ae7a971e0ea592beda60c35fb93e320aad8e4d8dbaeacd2f0f9d660ede1179add67fd057ce2301c7f9eca665abd770ced38d872", "t2lScxcuD9pFnD4WWuepceDqWSvtpgw1+5PjIKrY5NjbrqzS8PnWYO3hF5rdZ/0FfOIwHH+eymZavXcM7TjYcg==");
                            break;
                        case 5:
                            StaticTestUtilities.testValues64("SHA512", sqlData, "StringData4", "MoreStringData4", "52924dd94fe06c5b00f3659cd368be6cb39730c95836228a48eca9aa3447958027fb6d25c75af03b8ac19b035a3c72ee5a26a0a6672b89d4d9dd94ce16727f04", "UpJN2U/gbFsA82Wc02i+bLOXMMlYNiKKSOypqjRHlYAn+20lx1rwO4rBmwNaPHLuWiagpmcridTZ3ZTOFnJ/BA==");
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
