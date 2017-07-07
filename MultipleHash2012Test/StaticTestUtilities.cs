using Martin.SQLServer.Dts.Tests;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace MultipleHash2012Test
{
    static public class StaticTestUtilities
    {
        #region Byte Stuff

        public static byte[] GetStringToBytes(string value)
        {
            SoapHexBinary shb = SoapHexBinary.Parse(value);
            return shb.Value;
        }

        public static void testByteValues(byte[] expected, byte[] actual, String message)
        {
            Assert.AreEqual(expected.Length, actual.Length, String.Format("{0} Length", message));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], String.Format("{0} Array Item {1}", message, i));
            }
        }

        #endregion

        public static void CreatePath(MainPipe dataFlowTask, IDTSOutput100 fromOutput, IDTSComponentMetaData100 toComponent, CManagedComponentWrapper toInstance)
        {
            // Create the path from source to destination.
            IDTSPath100 path = dataFlowTask.PathCollection.New();
            path.AttachPathAndPropagateNotifications(fromOutput, toComponent.InputCollection[0]);

            // Get the destination's default input and virtual input.
            IDTSInput100 input = toComponent.InputCollection[0];
            IDTSVirtualInput100 vInput = input.GetVirtualInput();

            // Iterate through the virtual input column collection.
            foreach (IDTSVirtualInputColumn100 vColumn in vInput.VirtualInputColumnCollection)
            {
                // Find external column by name
                IDTSExternalMetadataColumn100 externalColumn = null;
                foreach (IDTSExternalMetadataColumn100 column in input.ExternalMetadataColumnCollection)
                {
                    if (String.Compare(column.Name, vColumn.Name, true) == 0)
                    {
                        externalColumn = column;
                        break;
                    }
                }
                if (externalColumn != null)
                {
                    // Select column, and retain new input column
                    IDTSInputColumn100 inputColumn = toInstance.SetUsageType(input.ID, vInput, vColumn.LineageID, DTSUsageType.UT_READONLY);
                    // Map input column to external column
                    toInstance.MapInputColumn(input.ID, inputColumn.ID, externalColumn.ID);
                }
            }
        }

        public static void CreateSQLCEComponent(Microsoft.SqlServer.Dts.Runtime.Package package, MainPipe dataFlowTask, string sqlCEDatabaseName, string sqlCEPassword, String tableName, out ConnectionManager sqlCECM, out IDTSComponentMetaData100 sqlCETarget, out CManagedComponentWrapper sqlCEInstance)
        {
            // Add SQL CE Connection
            sqlCECM = package.Connections.Add("SQLMOBILE");
            sqlCECM.ConnectionString = connectionString(sqlCEDatabaseName, sqlCEPassword);
            sqlCECM.Name = "SQLCE Destination " + tableName;

            sqlCETarget = dataFlowTask.ComponentMetaDataCollection.New();
            sqlCETarget.ComponentClassID = typeof(Microsoft.SqlServer.Dts.Pipeline.SqlCEDestinationAdapter).AssemblyQualifiedName;
            sqlCEInstance = sqlCETarget.Instantiate();
            sqlCEInstance.ProvideComponentProperties();
            sqlCETarget.Name = "SQLCE Target " + tableName;
            sqlCETarget.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(sqlCECM);
            sqlCETarget.RuntimeConnectionCollection[0].ConnectionManagerID = sqlCECM.ID;

            sqlCETarget.CustomPropertyCollection["Table Name"].Value = tableName;
            sqlCEInstance.AcquireConnections(null);
            sqlCEInstance.ReinitializeMetaData();
            sqlCEInstance.ReleaseConnections();
        }

        public static string connectionString(string sqlCEDatabaseName, string sqlCEPassword)
        {
            return String.Format("DataSource=\"{0}\"; Password='{1}'", sqlCEDatabaseName, sqlCEPassword);
        }


        public static void testValues4(String hashName, SqlCeDataReader sqlData, String string0, String string1, String bytes, String base64)
        {
            byte[] byteResult = null;
            byte[] byteExpected = null;
            Assert.AreEqual(string0, sqlData.GetString(0), String.Format("StringData <> {0}", string0));
            if (String.IsNullOrEmpty(string1))
            {
                Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
            }
            else
            {
                Assert.AreEqual(string1, sqlData.GetString(1), String.Format("AccountName <> {0}", string1));
            }
            byteResult = new byte[4];
            sqlData.GetBytes(5, 0, byteResult, 0, 4);
            byteExpected = StaticTestUtilities.GetStringToBytes(bytes);
            StaticTestUtilities.testByteValues(byteExpected, byteResult, String.Format("{0} Hash as Binary", hashName));
            Assert.AreEqual(String.Format("0x{0}", bytes), sqlData.GetString(6).ToLower(), String.Format("{0} Hash as Hex String", hashName));
            Assert.AreEqual(base64, sqlData.GetString(7), String.Format("{0} as Base 64", hashName));
        }

        public static void testValues8(String hashName, SqlCeDataReader sqlData, String string0, String string1, String bytes, String base64)
        {
            byte[] byteResult = null;
            byte[] byteExpected = null;
            Assert.AreEqual(string0, sqlData.GetString(0), String.Format("StringData <> {0}", string0));
            if (String.IsNullOrEmpty(string1))
            {
                Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
            }
            else
            {
                Assert.AreEqual(string1, sqlData.GetString(1), String.Format("AccountName <> {0}", string1));
            }
            byteResult = new byte[8];
            sqlData.GetBytes(5, 0, byteResult, 0, 8);
            byteExpected = StaticTestUtilities.GetStringToBytes(bytes);
            StaticTestUtilities.testByteValues(byteExpected, byteResult, String.Format("{0} Hash as Binary", hashName));
            Assert.AreEqual(String.Format("0x{0}", bytes), sqlData.GetString(6).ToLower(), String.Format("{0} Hash as Hex String", hashName));
            Assert.AreEqual(base64, sqlData.GetString(7), String.Format("{0} as Base 64", hashName));
        }

        public static void testValues16(String hashName, SqlCeDataReader sqlData, String string0, String string1, String bytes, String base64)
        {
            byte[] byteResult = null;
            byte[] byteExpected = null;
            Assert.AreEqual(string0, sqlData.GetString(0), String.Format("StringData <> {0}", string0));
            if (String.IsNullOrEmpty(string1))
            {
                Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
            }
            else
            {
                Assert.AreEqual(string1, sqlData.GetString(1), String.Format("AccountName <> {0}", string1));
            }
            byteResult = new byte[16];
            sqlData.GetBytes(5, 0, byteResult, 0, 16);
            byteExpected = StaticTestUtilities.GetStringToBytes(bytes);
            StaticTestUtilities.testByteValues(byteExpected, byteResult, String.Format("{0} Hash as Binary", hashName));
            Assert.AreEqual(String.Format("0x{0}", bytes), sqlData.GetString(6).ToLower(), String.Format("{0} Hash as Hex String", hashName));
            Assert.AreEqual(base64, sqlData.GetString(7), String.Format("{0} as Base 64", hashName));
        }

        public static void testValues20(String hashName, SqlCeDataReader sqlData, String string0, String string1, String bytes, String base64)
        {
            byte[] byteResult = null;
            byte[] byteExpected = null;
            Assert.AreEqual(string0, sqlData.GetString(0), String.Format("StringData <> {0}", string0));
            if (String.IsNullOrEmpty(string1))
            {
                Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
            }
            else
            {
                Assert.AreEqual(string1, sqlData.GetString(1), String.Format("AccountName <> {0}", string1));
            }
            byteResult = new byte[20];
            sqlData.GetBytes(5, 0, byteResult, 0, 20);
            byteExpected = StaticTestUtilities.GetStringToBytes(bytes);
            StaticTestUtilities.testByteValues(byteExpected, byteResult, String.Format("{0} Hash as Binary", hashName));
            Assert.AreEqual(String.Format("0x{0}", bytes), sqlData.GetString(6).ToLower(), String.Format("{0} Hash as Hex String", hashName));
            Assert.AreEqual(base64, sqlData.GetString(7), String.Format("{0} as Base 64", hashName));
        }

        public static void testValues32(String hashName, SqlCeDataReader sqlData, String string0, String string1, String bytes, String base64)
        {
            byte[] byteResult = null;
            byte[] byteExpected = null;
            Assert.AreEqual(string0, sqlData.GetString(0), String.Format("StringData <> {0}", string0));
            if (String.IsNullOrEmpty(string1))
            {
                Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
            }
            else
            {
                Assert.AreEqual(string1, sqlData.GetString(1), String.Format("AccountName <> {0}", string1));
            }
            byteResult = new byte[32];
            sqlData.GetBytes(5, 0, byteResult, 0, 32);
            byteExpected = StaticTestUtilities.GetStringToBytes(bytes);
            StaticTestUtilities.testByteValues(byteExpected, byteResult, String.Format("{0} Hash as Binary", hashName));
            Assert.AreEqual(String.Format("0x{0}", bytes), sqlData.GetString(6).ToLower(), String.Format("{0} Hash as Hex String", hashName));
            Assert.AreEqual(base64, sqlData.GetString(7), String.Format("{0} as Base 64", hashName));
        }

        public static void testValues48(String hashName, SqlCeDataReader sqlData, String string0, String string1, String bytes, String base64)
        {
            byte[] byteResult = null;
            byte[] byteExpected = null;
            Assert.AreEqual(string0, sqlData.GetString(0), String.Format("StringData <> {0}", string0));
            if (String.IsNullOrEmpty(string1))
            {
                Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
            }
            else
            {
                Assert.AreEqual(string1, sqlData.GetString(1), String.Format("AccountName <> {0}", string1));
            }
            byteResult = new byte[48];
            sqlData.GetBytes(5, 0, byteResult, 0, 48);
            byteExpected = StaticTestUtilities.GetStringToBytes(bytes);
            StaticTestUtilities.testByteValues(byteExpected, byteResult, String.Format("{0} Hash as Binary", hashName));
            Assert.AreEqual(String.Format("0x{0}", bytes), sqlData.GetString(6).ToLower(), String.Format("{0} Hash as Hex String", hashName));
            Assert.AreEqual(base64, sqlData.GetString(7), String.Format("{0} as Base 64", hashName));
        }

        public static void testValues64(String hashName, SqlCeDataReader sqlData, String string0, String string1, String bytes, String base64)
        {
            byte[] byteResult = null;
            byte[] byteExpected = null;
            Assert.AreEqual(string0, sqlData.GetString(0), String.Format("StringData <> {0}", string0));
            if (String.IsNullOrEmpty(string1))
            {
                Assert.IsTrue(sqlData.IsDBNull(1), "2nd Column is NOT null");
            }
            else
            {
                Assert.AreEqual(string1, sqlData.GetString(1), String.Format("AccountName <> {0}", string1));
            }
            byteResult = new byte[64];
            sqlData.GetBytes(5, 0, byteResult, 0, 64);
            byteExpected = StaticTestUtilities.GetStringToBytes(bytes);
            StaticTestUtilities.testByteValues(byteExpected, byteResult, String.Format("{0} Hash as Binary", hashName));
            Assert.AreEqual(String.Format("0x{0}", bytes), sqlData.GetString(6).ToLower(), String.Format("{0} Hash as Hex String", hashName));
            Assert.AreEqual(base64, sqlData.GetString(7), String.Format("{0} as Base 64", hashName));
        }

        public static void BuildSSISPackage(out Microsoft.SqlServer.Dts.Runtime.Package package, out IDTSComponentMetaData100 multipleHash, out CManagedComponentWrapper multipleHashInstance, out String lineageString, out MainPipe dataFlowTask)
        {
            Microsoft.SqlServer.Dts.Runtime.Application app;
            BuildSSISPackage(out package, out multipleHash, out multipleHashInstance, out lineageString, out dataFlowTask, out app);
        }
        public static void BuildSSISPackage(out Microsoft.SqlServer.Dts.Runtime.Package package, out IDTSComponentMetaData100 multipleHash, out CManagedComponentWrapper multipleHashInstance, out String lineageString, out MainPipe dataFlowTask, out Microsoft.SqlServer.Dts.Runtime.Application app)
        {
            package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            dataFlowTask = thMainPipe.InnerObject as MainPipe;
            ComponentEventHandler events = new ComponentEventHandler();
            dataFlowTask.Events = DtsConvert.GetExtendedInterface(events as IDTSComponentEvents);

            // Create a flat file source
            ConnectionManager flatFileConnectionManager = package.Connections.Add("FLATFILE");
            flatFileConnectionManager.Properties["Format"].SetValue(flatFileConnectionManager, "Delimited");
            flatFileConnectionManager.Properties["Name"].SetValue(flatFileConnectionManager, "Flat File Connection");
            flatFileConnectionManager.Properties["ConnectionString"].SetValue(flatFileConnectionManager, @".\TextDataToBeHashed.txt");
            flatFileConnectionManager.Properties["ColumnNamesInFirstDataRow"].SetValue(flatFileConnectionManager, false);
            flatFileConnectionManager.Properties["HeaderRowDelimiter"].SetValue(flatFileConnectionManager, "\r\n");
            flatFileConnectionManager.Properties["TextQualifier"].SetValue(flatFileConnectionManager, "\"");
            flatFileConnectionManager.Properties["DataRowsToSkip"].SetValue(flatFileConnectionManager, 0);
            flatFileConnectionManager.Properties["Unicode"].SetValue(flatFileConnectionManager, false);
            flatFileConnectionManager.Properties["CodePage"].SetValue(flatFileConnectionManager, 1252);

            // Create the columns in the flat file
            IDTSConnectionManagerFlatFile100 flatFileConnection = flatFileConnectionManager.InnerObject as IDTSConnectionManagerFlatFile100;
            IDTSConnectionManagerFlatFileColumn100 StringDataColumn = flatFileConnection.Columns.Add();
            StringDataColumn.ColumnDelimiter = ",";
            StringDataColumn.ColumnType = "Delimited";
            StringDataColumn.DataType = DataType.DT_STR;
            StringDataColumn.DataPrecision = 0;
            StringDataColumn.DataScale = 0;
            StringDataColumn.MaximumWidth = 255;
            ((IDTSName100)StringDataColumn).Name = "StringData";
            IDTSConnectionManagerFlatFileColumn100 MoreStringColumn = flatFileConnection.Columns.Add();
            MoreStringColumn.ColumnDelimiter = ",";
            MoreStringColumn.ColumnType = "Delimited";
            MoreStringColumn.DataType = DataType.DT_STR;
            MoreStringColumn.DataPrecision = 0;
            MoreStringColumn.DataScale = 0;
            MoreStringColumn.MaximumWidth = 255;
            ((IDTSName100)MoreStringColumn).Name = "MoreString";
            IDTSConnectionManagerFlatFileColumn100 DateColumn = flatFileConnection.Columns.Add();
            DateColumn.ColumnDelimiter = ",";
            DateColumn.ColumnType = "Delimited";
            DateColumn.DataType = DataType.DT_DATE;
            DateColumn.DataPrecision = 0;
            DateColumn.DataScale = 0;
            DateColumn.MaximumWidth = 0;
            ((IDTSName100)DateColumn).Name = "DateColumn";
            IDTSConnectionManagerFlatFileColumn100 IntegerColumn = flatFileConnection.Columns.Add();
            IntegerColumn.ColumnDelimiter = ",";
            IntegerColumn.ColumnType = "Delimited";
            IntegerColumn.DataType = DataType.DT_I4;
            IntegerColumn.DataPrecision = 0;
            IntegerColumn.DataScale = 0;
            IntegerColumn.MaximumWidth = 0;
            ((IDTSName100)IntegerColumn).Name = "IntegerColumn";
            IDTSConnectionManagerFlatFileColumn100 NumericColumn = flatFileConnection.Columns.Add();
            NumericColumn.ColumnDelimiter = "\r\n";
            NumericColumn.ColumnType = "Delimited";
            NumericColumn.DataType = DataType.DT_NUMERIC;
            NumericColumn.DataPrecision = 15;
            NumericColumn.DataScale = 2;
            NumericColumn.MaximumWidth = 0;
            ((IDTSName100)NumericColumn).Name = "NumericColumn";

            app = new Microsoft.SqlServer.Dts.Runtime.Application();

            IDTSComponentMetaData100 flatFileSource = dataFlowTask.ComponentMetaDataCollection.New();
            flatFileSource.ComponentClassID = app.PipelineComponentInfos["Flat File Source"].CreationName;
            // Get the design time instance of the Flat File Source Component
            var flatFileSourceInstance = flatFileSource.Instantiate();
            flatFileSourceInstance.ProvideComponentProperties();

            flatFileSource.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(flatFileConnectionManager);
            flatFileSource.RuntimeConnectionCollection[0].ConnectionManagerID = flatFileConnectionManager.ID;

            // Reinitialize the metadata.
            flatFileSourceInstance.AcquireConnections(null);
            flatFileSourceInstance.ReinitializeMetaData();
            flatFileSourceInstance.ReleaseConnections();
            flatFileSource.CustomPropertyCollection["RetainNulls"].Value = true;



            //[MD5BinaryOutput] varbinary(16), [MD5HexOutput] varchar(34), [MD5BaseOutput] varchar(24))";
            multipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            multipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            multipleHashInstance = multipleHash.Instantiate();

            multipleHashInstance.ProvideComponentProperties();
            multipleHash.Name = "Multiple Hash Test";
            multipleHashInstance.ReinitializeMetaData();

            // Create the path from source to destination.
            StaticTestUtilities.CreatePath(dataFlowTask, flatFileSource.OutputCollection[0], multipleHash, multipleHashInstance);

            // Select the input columns.
            IDTSInput100 multipleHashInput = multipleHash.InputCollection[0];
            IDTSVirtualInput100 multipleHashvInput = multipleHashInput.GetVirtualInput();
            foreach (IDTSVirtualInputColumn100 vColumn in multipleHashvInput.VirtualInputColumnCollection)
            {
                multipleHashInstance.SetUsageType(multipleHashInput.ID, multipleHashvInput, vColumn.LineageID, DTSUsageType.UT_READONLY);
            }

            // Add the output columns
            // Generate the Lineage String
            lineageString = String.Empty;
            foreach (IDTSInputColumn100 inputColumn in multipleHashInput.InputColumnCollection)
            {
                if (lineageString == String.Empty)
                {
                    lineageString = String.Format("#{0}", inputColumn.LineageID);
                }
                else
                {
                    lineageString = String.Format("{0},#{1}", lineageString, inputColumn.LineageID);
                }
            }
        }

    }
}
