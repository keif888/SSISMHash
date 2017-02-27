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
    public class SHA384Tests
    {
        const string sqlCEDatabaseName = @".\SHA384Test.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA384BinaryOutput] varbinary(48), [SHA384HexOutput] nvarchar(98), [SHA384BaseOutput] nvarchar(64))";
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
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [SHA384BinaryOutput] varbinary(16), [SHA384HexOutput] varchar(34), [SHA384BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashSHA384Test()
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

            // Add output column SHA384BinaryOutput (SHA384, Binary)
            IDTSOutputColumn100 SHA384BinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA384BinaryOutput", "SHA384 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA384BinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            SHA384BinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA384;
            SHA384BinaryOutput.Name = "SHA384BinaryOutput";
            SHA384BinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA384, MultipleHash.OutputTypeEnumerator.Binary, SHA384BinaryOutput);
            // Add output column SHA384HexOutput (SHA384, HexString)
            IDTSOutputColumn100 SHA384HexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA384HexOutput", "SHA384 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA384HexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            SHA384HexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA384;
            SHA384HexOutput.Name = "SHA384HexOutput";
            SHA384HexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA384, MultipleHash.OutputTypeEnumerator.HexString, SHA384HexOutput);
            // Add output column SHA384BaseOutput (SHA384, Base64String)
            IDTSOutputColumn100 SHA384BaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "SHA384BaseOutput", "SHA384 Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            SHA384BaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            SHA384BaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.SHA384;
            SHA384BaseOutput.Name = "SHA384BaseOutput";
            SHA384BaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA384, MultipleHash.OutputTypeEnumerator.Base64String, SHA384BaseOutput);

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
                            StaticTestUtilities.testValues48("SHA384", sqlData, "NullRow", null, "08b53cabec16f989febb7594542989474cc68bc88f5ef25ef3d3eaa5c587c5bdc54dc88f6c2af8dab2dd496d66c5f4aa", "CLU8q+wW+Yn+u3WUVCmJR0zGi8iPXvJe89PqpcWHxb3FTciPbCr42rLdSW1mxfSq");
                            break;
                        case 2:
                            StaticTestUtilities.testValues48("SHA384", sqlData, "StringData1", "MoreStringData1", "432aad9910a1232a4699f8d998211dd74ace4dc63cc4617e2c02c6d5f582c22f2ce829e81237ec3b2b261c943dd24deb", "QyqtmRChIypGmfjZmCEd10rOTcY8xGF+LALG1fWCwi8s6CnoEjfsOysmHJQ90k3r");
                            break;
                        case 3:
                            StaticTestUtilities.testValues48("SHA384", sqlData, "StringData2", "MoreStringData2", "7e0022b353057d6596a499021300f6f08a82d0dbe36b3f413c0f08b5ba0b740b8fd91d1860e81720487eb624952cd15d", "fgAis1MFfWWWpJkCEwD28IqC0Nvjaz9BPA8ItboLdAuP2R0YYOgXIEh+tiSVLNFd");
                            break;
                        case 4:
                            StaticTestUtilities.testValues48("SHA384", sqlData, "StringData3", "MoreStringData3", "bbeb7ffe8289295e0cd0a3ed7b75ea03b9e7e25116da56569f1e85b2dd30f42903cd0e6b8cf33c3c6690bb1a105b8393", "u+t//oKJKV4M0KPte3XqA7nn4lEW2lZWnx6Fst0w9CkDzQ5rjPM8PGaQuxoQW4OT");
                            break;
                        case 5:
                            StaticTestUtilities.testValues48("SHA384", sqlData, "StringData4", "MoreStringData4", "f0adce5888a63f94b69551b13e612e165eaa25bafa768faf8fc0221bfd906c3eded45ed838f3c095c5a3ce962cc8b32f", "8K3OWIimP5S2lVGxPmEuFl6qJbr6do+vj8AiG/2QbD7e1F7YOPPAlcWjzpYsyLMv");
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
