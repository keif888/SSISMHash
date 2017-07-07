using Microsoft.VisualStudio.TestTools.UnitTesting;
using Martin.SQLServer.Dts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using System.Data.SqlServerCe;
using System.Data;
using System.Diagnostics;
using System.IO;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace MultipleHash2012Test
{
    [TestClass()]
    public class UnitTestDataFlowComponentUI
    {

        const string sqlCEDatabaseName = @".\UnitTestDataFlowComponent.sdf";
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

        [TestMethod()]
        [DeploymentItem(@"TextDataToBeHashed.txt")]
        public void GetTooltipStringTest()
        {
            string expected = string.Empty;
            string result = string.Empty;

            Microsoft.SqlServer.Dts.Runtime.Package package;
            IDTSComponentMetaData100 multipleHash;
            CManagedComponentWrapper multipleHashInstance;
            String lineageString;
            MainPipe dataFlowTask;
            // Microsoft.SqlServer.Dts.Runtime.Application app;
            StaticTestUtilities.BuildSSISPackage(out package, out multipleHash, out multipleHashInstance, out lineageString, out dataFlowTask/*, out app */);

            result = DataFlowComponentUI.GetTooltipString(multipleHash.InputCollection[0].InputColumnCollection[0]);
            expected = "Name: StringData\nData type: DT_STR\nLength: 255\nScale: 0\nPrecision: 0\nCode page: 1252";
            Assert.AreEqual(expected, result, "Checking IDTSInputColumn");

            int outputID = multipleHash.OutputCollection[0].ID;
            int outputColumnPos = multipleHash.OutputCollection[0].OutputColumnCollection.Count;

            // Add output column CRC32BinaryOutput (CRC32, Binary)
            IDTSOutputColumn100 CRC32CBinaryOutput = multipleHashInstance.InsertOutputColumnAt(outputID, outputColumnPos++, "BinaryOutput", "CRC32C Hash of the input");
            CRC32CBinaryOutput.CustomPropertyCollection[Utility.OutputColumnOutputTypePropName].Value = MultipleHash.OutputTypeEnumerator.Binary;
            CRC32CBinaryOutput.CustomPropertyCollection[Utility.HashTypePropName].Value = MultipleHash.HashTypeEnumerator.CRC32C;
            CRC32CBinaryOutput.Name = "BinaryOutput";
            CRC32CBinaryOutput.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = lineageString;
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.Binary, CRC32CBinaryOutput);
            result = DataFlowComponentUI.GetTooltipString(CRC32CBinaryOutput);
            expected = "Name: BinaryOutput\nData type: DT_BYTES\nLength: 4\nScale: 0\nPrecision: 0\nCode page: 0";
            Assert.AreEqual(expected, result, "Check IDTSOutputColumn100");
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
            // StaticTestUtilities.CreatePath(dataFlowTask, multipleHash.OutputCollection[0], sqlCETarget, sqlCEInstance);

            // Create the path from source to destination.
            IDTSPath100 path = dataFlowTask.PathCollection.New();
            path.AttachPathAndPropagateNotifications(multipleHash.OutputCollection[0], sqlCETarget.InputCollection[0]);

            // Get the destination's default input and virtual input.
            IDTSInput100 input = sqlCETarget.InputCollection[0];
            IDTSVirtualInput100 vInput = input.GetVirtualInput();

            IDTSVirtualInputColumn100 vColumn = vInput.VirtualInputColumnCollection[0];

            result = DataFlowComponentUI.GetTooltipString(vColumn);
            expected = "Name: StringData\nData type: DT_STR\nLength: 255\nScale: 0\nPrecision: 0\nCode page: 1252\nSource: Flat File Source";
            Assert.AreEqual(expected, result, "Check IDTSVirtualInputColumn100");

            result = DataFlowComponentUI.GetTooltipString(sqlCETarget.InputCollection[0].ExternalMetadataColumnCollection[0]);
            expected = "Name: StringData\nData type: DT_WSTR\nLength: 255\nScale: 0\nPrecision: 0\nCode page: 0";
            Assert.AreEqual(expected, result, "Check IDTSExternalMetadataColumn");

            result = DataFlowComponentUI.GetTooltipString("String");
            expected = String.Empty;
            Assert.AreEqual(expected, result, "Check String Object");
        }
    }
}