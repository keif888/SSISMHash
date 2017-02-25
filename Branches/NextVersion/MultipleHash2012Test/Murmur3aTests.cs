using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.SqlServerCe;
using System.Data;
using System.Diagnostics;
using Martin.SQLServer.Dts.Tests;
using System.IO;
using MultipleHash2012Test;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class Murmur3aTests
    {
        const string sqlCEDatabaseName = @".\Murmur3aHashTest.sdf";
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
            String tableCreate = "CREATE TABLE [TestRecordsMurmur3a] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [Murmur3aBinaryOutput] varbinary(16), [Murmur3aHexOutput] nvarchar(34), [Murmur3aBaseOutput] nvarchar(24))";
            SqlCeCommand command = new SqlCeCommand(tableCreate, connection);
            command.ExecuteNonQuery();

            // Diconnect from the SQL CE database
            connection.Close();
            sqlCEEngine.Dispose();
        }


        [TestMethod()]
        public void Murmur3aTest()
        {
            Murmur3a target = new Murmur3a();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("ThisIsATestValueForMurmurHash3");
            byte[] expected = StaticTestUtilities.GetStringToBytes("bbcac44bfcae82a84a46adad35cecf90");  // http://murmurhash.shorelabs.com/  but you need to use the x64/128 bit implementation, AND do a BigEndian on it.
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod()]
        public void InitializeTest()
        {
            Murmur3a target = new Murmur3a();
            target.Initialize();  // Rely on an exception to indicate test failure.
            //Assert.AreEqual(0, target.seed);  // seed is private :-(
        }

        [TestMethod()]
        public void CreateTest()
        {
            Murmur3a expected = new Murmur3a(); // TODO: Initialize to an appropriate value
            Murmur3a actual;
            actual = Murmur3a.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for CalculateHash
        ///StringData,MoreStringData,2012-01-04,18,19.05
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [MD5BinaryOutput] varbinary(16), [MD5HexOutput] varchar(34), [MD5BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void CalculateHashMurmurHash3aTest()
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

            // Add output column Murmur3aBinaryOutput (Murmur3a, Binary)
            IDTSOutputColumn100 Murmur3aBinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "Murmur3aBinaryOutput", "Murmur3a Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            Murmur3aBinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            Murmur3aBinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.MurmurHash3a;
            Murmur3aBinaryOutput.Name = "Murmur3aBinaryOutput";
            Murmur3aBinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            // Add output column Murmur3aHexOutput (Murmur3a, HexString)
            IDTSOutputColumn100 Murmur3aHexOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "Murmur3aHexOutput", "Murmur3a Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            Murmur3aHexOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.HexString;
            Murmur3aHexOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.MurmurHash3a;
            Murmur3aHexOutput.Name = "Murmur3aHexOutput";
            Murmur3aHexOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MurmurHash3a, MultipleHash.OutputTypeEnumerator.HexString, Murmur3aHexOutput);
            // Add output column Murmur3aBaseOutput (Murmur3a, Base64String)
            IDTSOutputColumn100 Murmur3aBaseOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "Murmur3aBaseOutput", "Murmur3a Hash of the input"); //multipleHash.OutputCollection[0].OutputColumnCollection.New();
            Murmur3aBaseOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Base64String;
            Murmur3aBaseOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.MurmurHash3a;
            Murmur3aBaseOutput.Name = "Murmur3aBaseOutput";
            Murmur3aBaseOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MurmurHash3a, MultipleHash.OutputTypeEnumerator.Base64String, Murmur3aBaseOutput);

            // Add SQL CE Connection
            ConnectionManager sqlCECM = null;
            IDTSComponentMetaData100 sqlCETarget = null;
            CManagedComponentWrapper sqlCEInstance = null;
            StaticTestUtilities.CreateSQLCEComponent(package, dataFlowTask, sqlCEDatabaseName, sqlCEPassword, "TestRecordsMurmur3a", out sqlCECM, out sqlCETarget, out sqlCEInstance);
            StaticTestUtilities.CreatePath(dataFlowTask, multipleHash.OutputCollection[0], sqlCETarget, sqlCEInstance);

            // Uncomment the following line if you want to save the DTSX package.
            // Although you will have to add app to the BuildSSISPackage as an out to make this work.
            // app.SaveToXml(@".\test.dtsx", package, null);

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

            SqlCeCommand sqlCommand = new SqlCeCommand("SELECT * FROM [TestRecordsMurmur3a] ORDER BY [StringData]", connection);
            SqlCeDataReader sqlData = sqlCommand.ExecuteReader(CommandBehavior.Default);
            int rowCount = 0;
            while (sqlData.Read())
            {
                rowCount++;
                switch (rowCount)
                {
                    case 1:
                        StaticTestUtilities.testValues16("Murmur3a", sqlData, "NullRow", null, "a6b1e7adea05d62eee4a69b75bb6fa0f", "prHnreoF1i7uSmm3W7b6Dw==");
                        break;
                    case 2:
                        StaticTestUtilities.testValues16("Murmur3a", sqlData, "StringData1", "MoreStringData1", "dd8bc7d17663e60762a651a0cf9c4587", "3YvH0XZj5gdiplGgz5xFhw==");
                        break;
                    case 3:
                        StaticTestUtilities.testValues16("Murmur3a", sqlData, "StringData2", "MoreStringData2", "a9a7e8b837da471597612542cb991d60", "qafouDfaRxWXYSVCy5kdYA==");
                        break;
                    case 4:
                        StaticTestUtilities.testValues16("Murmur3a", sqlData, "StringData3", "MoreStringData3", "c7b26b54eae2b2b3bbc6517442f87850", "x7JrVOrisrO7xlF0Qvh4UA==");
                        break;
                    case 5:
                        StaticTestUtilities.testValues16("Murmur3a", sqlData, "StringData4", "MoreStringData4", "52e3a27aa7a40b6bf9f7e12ffdbdab6d", "UuOieqekC2v59+Ev/b2rbQ==");
                        break;
                    default:
                        Assert.Fail(string.Format("Account has to many records AccountCode {0}, AccountName {1}", sqlData.GetInt32(1), sqlData.GetString(2)));
                        break;
                }
            }
            Assert.AreEqual(5, rowCount, "Rows in TestRecords");
            connection.Close();
            sqlCEEngine.Dispose();

        }




    }
}
