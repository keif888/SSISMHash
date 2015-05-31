using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;
using MultipleHash2012Test;
using MultipleHash2012Test.SSISImplementations;
using System.Data.SqlServerCe;
using System.IO;
using System.Data;
using System.Diagnostics;
using Microsoft.SqlServer.Dts.Runtime;


namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class UtilityTests
    {

        const string sqlCEDatabaseName = @".\UtilityTest.sdf";
        const string sqlCEPassword = "MartinSource";
        SqlCeEngine sqlCEEngine = null;

        private static string connectionString()
        {
            return String.Format("DataSource=\"{0}\"; Password='{1}'", sqlCEDatabaseName, sqlCEPassword);
        }

        [TestInitialize]
        public void SetupSQLCEDatabase()
        {
            if (File.Exists(sqlCEDatabaseName))
            {
                File.Delete(sqlCEDatabaseName);
            }

            sqlCEEngine = new SqlCeEngine(connectionString());
            sqlCEEngine.CreateDatabase();

            SqlCeConnection connection = new SqlCeConnection(connectionString());
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            //StringData,MoreStringData,2012-01-04,18,19.05

            String tableCreate = "CREATE TABLE [TestRecords] ([StringData] nvarchar(255), [MoreString] nvarchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [MD5BinaryOutput] varbinary(16), [MD5HexOutput] nvarchar(34), [MD5BaseOutput] nvarchar(24))";
            SqlCeCommand command = new SqlCeCommand(tableCreate, connection);
            command.ExecuteNonQuery();

//            tableCreate = "CREATE TABLE [MoreTestRecords] ([binarycolumn] varbinary(20), )";

            connection.Close();
            sqlCEEngine.Dispose();
        }

        /// <summary>
        ///A test for MultipleThreadPropName
        ///</summary>
        [TestMethod()]
        public void MultipleThreadPropNameTest()
        {
            string actual;
            actual = Utility.MultipleThreadPropName;
            Assert.IsTrue(actual == "MultipleThreads", "Invalid return value from test");
        }

        /// <summary>
        ///A test for InputColumnLineagePropName
        ///</summary>
        [TestMethod()]
        public void InputColumnLineagePropNameTest()
        {
            string actual;
            actual = Utility.InputColumnLineagePropName;
            Assert.IsTrue(actual == "InputColumnLineageIDs", "Invalid return value from test");
        }

        /// <summary>
        ///A test for HashTypePropName
        ///</summary>
        [TestMethod()]
        public void HashTypePropNameTest()
        {
            string actual;
            actual = Utility.HashTypePropName;
            Assert.IsTrue(actual == "HashType", "Invalid return value from test");
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestLong()
        {
            long value = 1234567890123456789;
            byte[] expected = new byte[8] { 21, 129, 233, 125, 244, 16, 34, 17 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestFloat()
        {
            float value = 1234567890.1234567890F;
            byte[] expected = new byte[4] { 6, 44, 147, 78 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestShort()
        {
            short value = 12345;
            byte[] expected = new byte[2] { 57, 48 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestSbyte()
        {
            sbyte value = 123;
            byte[] expected = new byte[1] { 123 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestUint()
        {
            uint value = 1234567890;
            byte[] expected = new byte[4] { 210, 2, 150, 73 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestUlong()
        {
            ulong value = 12345678901234567890;
            byte[] expected = new byte[8] { 210, 10, 31, 235, 140, 169, 84, 171 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestDouble()
        {
            double value = 1234567890.1234567890F;
            byte[] expected = new byte[8] { 0, 0, 0, 192, 128, 101, 210, 65 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestUshort()
        {
            ushort value = 1234;
            byte[] expected = new byte[2] { 210, 4 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestDecimal()
        {
            Decimal value = new Decimal();
            value = 1234567890.1234567890M;
            byte[] expected = new byte[16] { 210, 10, 31, 235, 140, 169, 84, 171, 0, 0, 0, 0, 0, 0, 10, 0 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestBool()
        {
            bool value = true;
            byte[] expected = new byte[1] { 1 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestInt()
        {
            int value = 1234567890;
            byte[] expected = new byte[4] { 210, 2, 150, 73 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestDateTime()
        {
            DateTime value = new DateTime(1234567890);
            byte[] expected = new byte[28] { 27, 48, 48, 48, 49, 45, 48, 49, 45, 48, 49, 32, 48, 48, 58, 48, 50, 58, 48, 51, 46, 52, 53, 54, 55, 56, 57, 48 };
            byte[] actual;
            actual = Utility.ToArray(value, true);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestDateTimeFalse()
        {
            DateTime value = new DateTime(1234567890);
            byte[] expected = new byte[21] { 20, 48, 48, 48, 49, 45, 48, 49, 45, 48, 49, 32, 48, 48, 58, 48, 50, 58, 48, 51, 90 };
            byte[] actual;
            actual = Utility.ToArray(value, false);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }


        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestGuid()
        {
            Guid value = new Guid();
            byte[] expected = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestByte()
        {
            byte value = 123;
            byte[] expected = new byte[1] { 123 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for ToArray
        ///</summary>
        [TestMethod()]
        public void ToArrayTestByteArray()
        {
            TimeSpan value = new TimeSpan(1234567890);
            byte[] expected = new byte[17] { 16, 48, 48, 58, 48, 50, 58, 48, 51, 46, 52, 53, 54, 55, 56, 57, 48 };
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        ///// <summary>
        /////A test for SetOutputColumnDataType
        /////</summary>
        //[TestMethod()]
        //public void SetOutputColumnDataTypeTest()
        //{
        //    MultipleHash.HashTypeEnumerator propertyValue = new MultipleHash.HashTypeEnumerator();
        //    propertyValue = MultipleHash.HashTypeEnumerator.MD5;
        //    IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
        //    Utility.SetOutputColumnDataType(propertyValue, outputColumn);
        //    Assert.AreEqual(outputColumn.CodePage, 0);
        //    Assert.AreEqual(outputColumn.DataType, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES);
        //    Assert.AreEqual(outputColumn.Length, 16);
        //    Assert.AreEqual(outputColumn.Precision, 0);
        //    Assert.AreEqual(outputColumn.Scale, 0);
        //}

        /// <summary>
        ///A test for GetNumberOfProcessorCores
        ///</summary>
        [TestMethod()]
        public void GetNumberOfProcessorCoresTest()
        {
            int expected = 8; // This has to be changed for every machine the test is run on!
            int actual;
            actual = Utility.GetNumberOfProcessorCores();
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestBool()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] { 0, 1 };
            bool value = true;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestDateTime()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[29] { 0, 27, 48, 48, 48, 49, 45, 48, 49, 45, 48, 49, 32, 48, 48, 58, 48, 50, 58, 48, 51, 46, 52, 53, 54, 55, 56, 57, 48 };
            DateTime value = new DateTime(1234567890);
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value, true);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestDateTimeFalse()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[22] { 0, 20, 48, 48, 48, 49, 45, 48, 49, 45, 48, 49, 32, 48, 48, 58, 48, 50, 58, 48, 51, 90 };
            DateTime value = new DateTime(1234567890);
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value, false);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestLong()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[9] { 0, 21, 129, 233, 125, 244, 16, 34, 17 };
            long value = 1234567890123456789;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestUInt()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[5] { 0, 210, 2, 150, 73 };
            uint value = 1234567890;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestUShort()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[3] { 0, 210, 4 };
            ushort value = 1234;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestInt()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[5] { 0, 210, 2, 150, 73 };
            int value = 1234567890;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestChar()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] { 0, 84 };
            char value = 'T';
            Encoding encoding = Encoding.UTF8;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value, encoding);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestString()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[22] { 0, 84, 104, 105, 115, 32, 105, 115, 32, 97, 32, 116, 101, 115, 116, 32, 115, 116, 114, 105, 110, 103 };
            string value = "This is a test string";
            Encoding encoding = Encoding.UTF8;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value, encoding);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestDouble()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[9] { 0, 0, 0, 0, 192, 128, 101, 210, 65 };
            double value = 1234567890.1234567890F;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestDecimal()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[17] { 0, 210, 10, 31, 235, 140, 169, 84, 171, 0, 0, 0, 0, 0, 0, 10, 0 };
            Decimal value = new Decimal();
            value = 1234567890.1234567890M;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestULong()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[9] { 0, 210, 10, 31, 235, 140, 169, 84, 171 };
            ulong value = 12345678901234567890;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestFloat()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[5] { 0, 6, 44, 147, 78 };
            float value = 1234567890.1234567890F;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestTimeSpan()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[18] { 0, 16, 48, 48, 58, 48, 50, 58, 48, 51, 46, 52, 53, 54, 55, 56, 57, 48 };
            TimeSpan value = new TimeSpan(1234567890);
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestGuid()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[17] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            Guid value = new Guid();
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestSBtye()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] { 0, 123 };
            sbyte value = 123;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestShort()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[3] { 0, 57, 48 };
            short value = 12345;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestByte()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] { 0, 123 };
            byte value = 123;
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestByteArray()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] { 0, 123 };
            byte[] value = new byte[1] { 123 };
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
            arrayExpected = new byte[1031];
            value = new byte[1030];
            byte j = 0;
            arrayExpected[0] = 0;
            for (int i = 1; i < 1031; i++)
            {
                value[i - 1] = j;
                arrayExpected[i] = j;
                if (j != 0xFF)
                    j++;
                else
                    j = 0;
            }
            usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestDateTimeOffsetFalse()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[22] { 0, 20, 50, 48, 49, 49, 45, 48, 56, 45, 50, 49, 32, 49, 52, 58, 51, 55, 58, 48, 48, 90 };
            TimeSpan offset = new TimeSpan(1, 0, 0);
            DateTimeOffset value = new DateTimeOffset(2011, 08, 21, 15, 37, 00, offset);
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value, false);
            Assert.AreEqual(arrayExpected.Length, usedBuffer, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTestDateTimeOffsetTrue()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[38] { 0, 34, 50, 48, 49, 49, 45, 48, 56, 45, 50, 49, 32, 49, 53, 58, 51, 55, 58, 48, 48, 46, 48, 48, 48, 48, 48, 48, 48, 32, 43, 48, 49, 58, 48, 48, 0, 0 };
            TimeSpan offset = new TimeSpan(1, 0, 0);
            DateTimeOffset value = new DateTimeOffset(2011, 08, 21, 15, 37, 00, offset);
            int usedBuffer = 1;
            Utility.Append(ref array, ref usedBuffer, value, true);
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i], String.Format("Column {0} in array is wrong", i));
            }
        }


        /// <summary>
        ///A test for SafeNullHandlingPropName
        ///</summary>
        [TestMethod()]
        public void SafeNullHandlingPropNameTest()
        {
            string actual;
            actual = Utility.SafeNullHandlingPropName;
            Assert.AreEqual("SafeNullHandling", actual);
        }

        /// <summary>
        ///A test for HandleMillisecondPropName
        ///</summary>
        [TestMethod()]
        public void HandleMillisecondPropNameTest()
        {
            string actual;
            actual = Utility.HandleMillisecondPropName;
            Assert.AreEqual("IncludeMillsecond", actual);
        }

        /// <summary>
        ///A test for OutputColumnOutputTypePropName
        ///</summary>
        [TestMethod()]
        public void OutputColumnOutputTypePropNameTest()
        {
            string actual;
            actual = Utility.OutputColumnOutputTypePropName;
            Assert.AreEqual("HashOutputType", actual);
        }



        /// <summary>
        /// A test to check if we can set output column data types.
        /// </summary>
        [TestMethod()]
        public void SetOutputColumnDataTypeTest()
        {
            IDTSOutput100 output = new OutputTestImpl();
            IDTSOutputColumn100 outputColumn;
            outputColumn = output.OutputColumnCollection.New();

            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.None, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(16, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MD5, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(16, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA1, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(20, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.RipeMD160, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(20, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA256, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(32, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA384, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(48, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA512, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(64, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(4, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(4, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a32, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(4, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a64, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(8, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MurmurHash3a, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);  // MurmurHash3a using 128bit hash result.  (See this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/MurmurHash/MurmurHash3.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(16, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.xxHash, MultipleHash.OutputTypeEnumerator.Binary, outputColumn);  // xxHash using 64bit hash result. (see this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/xxHash/xxHash.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES, outputColumn.DataType, "DT_BYTES wasn't set as data type");
            Assert.AreEqual(8, outputColumn.Length, "Length wasn't set correctly");

            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.None, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(34, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MD5, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(34, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA1, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(42, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.RipeMD160, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(42, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA256, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(66, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA384, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(98, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA512, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(130, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(6, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(6, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a32, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(6, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a64, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(10, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MurmurHash3a, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);  // MurmurHash3a using 128bit hash result.  (See this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/MurmurHash/MurmurHash3.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(34, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.xxHash, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);  // xxHash using 64bit hash result. (see this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/xxHash/xxHash.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(10, outputColumn.Length, "Length wasn't set correctly");

            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.None, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(24, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MD5, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(24, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA1, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(28, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.RipeMD160, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(28, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA256, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(44, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA384, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(64, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.SHA512, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(88, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(8, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(8, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a32, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(8, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a64, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(12, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MurmurHash3a, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);  // MurmurHash3a using 128bit hash result.  (See this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/MurmurHash/MurmurHash3.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(24, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.xxHash, MultipleHash.OutputTypeEnumerator.Base64String, outputColumn);  // xxHash using 64bit hash result. (see this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/xxHash/xxHash.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(12, outputColumn.Length, "Length wasn't set correctly");
        }


        /// <summary>
        ///A test for CalculateHash
        ///StringData,MoreStringData,2012-01-04,18,19.05
        ///String tableCreate = "CREATE TABLE [TestRecords] ([StringData] varchar(255), [MoreString] varchar(255), [DateColumn] DATETIME, [IntegerColumn] bigint, [NumericColumn] numeric(15,2), [MD5BinaryOutput] varbinary(16), [MD5HexOutput] varchar(34), [MD5BaseOutput] varchar(24))";
        ///</summary>
        [TestMethod()]
        public void CalculateHashTest()
        {
            Microsoft.SqlServer.Dts.Runtime.Package package = new Microsoft.SqlServer.Dts.Runtime.Package();
            Executable exec = package.Executables.Add("STOCK:PipelineTask");
            Microsoft.SqlServer.Dts.Runtime.TaskHost thMainPipe = exec as Microsoft.SqlServer.Dts.Runtime.TaskHost;
            MainPipe dataFlowTask = thMainPipe.InnerObject as MainPipe;
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
            
            var app = new Microsoft.SqlServer.Dts.Runtime.Application();

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



            //[MD5BinaryOutput] varbinary(16), [MD5HexOutput] varchar(34), [MD5BaseOutput] varchar(24))";
            IDTSComponentMetaData100 multipleHash = dataFlowTask.ComponentMetaDataCollection.New();
            multipleHash.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
            CManagedComponentWrapper multipleHashInstance = multipleHash.Instantiate();

            multipleHashInstance.ProvideComponentProperties();
            multipleHash.Name = "Multiple Hash Test";
            multipleHashInstance.ReinitializeMetaData();

            // Create the path from source to destination.
            CreatePath(dataFlowTask, flatFileSource.OutputCollection[0], multipleHash, multipleHashInstance);

            // Select the input columns.
            IDTSInput100 multipleHashInput = multipleHash.InputCollection[0];
            IDTSVirtualInput100 multipleHashvInput = multipleHashInput.GetVirtualInput();
            foreach (IDTSVirtualInputColumn100 vColumn in multipleHashvInput.VirtualInputColumnCollection)
            {
                multipleHashInstance.SetUsageType(multipleHashInput.ID, multipleHashvInput, vColumn.LineageID, DTSUsageType.UT_READONLY);
            }

            // Add the output columns
            // Generate the Lineage String
            String lineageString = String.Empty;
            foreach(IDTSInputColumn100 inputColumn in multipleHashInput.InputColumnCollection)
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
            CreateSQLCEComponent(package, dataFlowTask, "TestRecords", out sqlCECM, out sqlCETarget, out sqlCEInstance);
            CreatePath(dataFlowTask, multipleHash.OutputCollection[0], sqlCETarget, sqlCEInstance);

            app.SaveToXml(@"d:\test\test.dtsx", package, null);

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
            SqlCeConnection connection = new SqlCeConnection(connectionString());
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
                        Assert.AreEqual("NullRow", sqlData.GetString(1), "StringData <> NullRow");
                        Assert.AreEqual("Joe Smith", sqlData.GetString(2), "AccountName <> Joe Smith");
                        break;
                    case 2:
                        Assert.AreEqual(12346, sqlData.GetInt32(1), "AccountCode <> 12346");
                        Assert.AreEqual("James Smith", sqlData.GetString(2), "AccountName <> James Smith");
                        break;
                    case 3:
                        Assert.AreEqual(12356, sqlData.GetInt32(1), "Account Code <> 12356");
                        Assert.AreEqual("Mike Smith", sqlData.GetString(2), "AccountName <> Mike Smith");
                        break;
                    case 4:
                        Assert.AreEqual(12856, sqlData.GetInt32(1), "Account Code <> 12856");
                        Assert.AreEqual("John Smith", sqlData.GetString(2), "AccountName <> John Smith");
                        break;
                    default:
                        Assert.Fail(string.Format("Account has to many records AccountCode {0}, AccountName {1}", sqlData.GetInt32(1), sqlData.GetString(2)));
                        break;
                }
            }
            Assert.AreEqual(4, rowCount, "Rows in Account");


            Assert.Inconclusive("Test Not Complete!");
        }


        private void CreatePath(MainPipe dataFlowTask, IDTSOutput100 fromOutput, IDTSComponentMetaData100 toComponent, CManagedComponentWrapper toInstance)
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

        private void CreateSQLCEComponent(Microsoft.SqlServer.Dts.Runtime.Package package, MainPipe dataFlowTask, String tableName, out ConnectionManager sqlCECM, out IDTSComponentMetaData100 sqlCETarget, out CManagedComponentWrapper sqlCEInstance)
        {
            // Add SQL CE Connection
            sqlCECM = package.Connections.Add("SQLMOBILE");
            sqlCECM.ConnectionString = connectionString();
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


        #region Error Delegate

        private List<String> errorMessages;

        void PostError(string errorMessage)
        {
            errorMessages.Add(errorMessage);
        }
        #endregion

        /*
         * The following test is commented out as it can't work due to SSIS design.
         * Use the ExecuteTests.cmd file to test the Hash Calculation, using SSIS packages.
         * 

                /// <summary>
                ///A test for CalculateHash
                ///</summary>
                [TestMethod()]
                public void CalculateHashTest()
                {
                    Assert.Inconclusive("Unable to test this as we can't initialise a PipelineBuffer...");
                    MockRepository repository = new MockRepository();
            

                    OutputColumn columnToProcess = new OutputColumn();
                    IDTSBufferManager100 bufferManager = new BufferManagerTestImpl();
                    IDTSOutput100 output = new OutputTestImpl();
                    IDTSInput100 input = new InputTestImpl();
                    IDTSOutputColumn100 outputColumn;
                    IDTSCustomProperty100 customProperty;
                    int outputColumnIndex = 0;
                    outputColumn = output.OutputColumnCollection.New();
                    customProperty = outputColumn.CustomPropertyCollection.New();
                    customProperty.Name = Utility.HashTypePropName;
                    customProperty.Value = MultipleHash.HashTypeEnumerator.MD5;
                    customProperty = outputColumn.CustomPropertyCollection.New();
                    customProperty.Name = Utility.InputColumnLineagePropName;
                    customProperty.Value = "#1,#2,#3,#4,#5,#6";

                    columnToProcess.AddColumnInformation(bufferManager, output, input, outputColumnIndex);

                    BUFFER_WIRE_PACKET bufferWirePacket = new BUFFER_WIRE_PACKET();

                    IntPtr pColInfo = Marshal.AllocHGlobal(500);
                    Marshal.StructureToPtr(bufferWirePacket, pColInfo, true);

                    bufferWirePacket.pColInfo = pColInfo;
                    bufferWirePacket.dwColCount = 1;
                    bufferWirePacket.dwRowCount = 1;
                    bufferWirePacket.ppvRowStarts = pColInfo;

                    IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(bufferWirePacket));
                    Marshal.StructureToPtr(bufferWirePacket, ptr, true);

            

                    PipelineBuffer buffer = repository.StrictMock<PipelineBuffer>(ptr, PipelineBufferMode.Input);
                    buffer.CurrentRow = 0;

                    Expect.Call(buffer.IsNull(0)).Return(false);
                    repository.ReplayAll();

                    Utility.CalculateHash(columnToProcess, buffer, false);

                    repository.VerifyAll();
         */
        /*
        Assert.Inconclusive("Unable to test this as we can't initialise a PipelineBuffer...");
        OutputColumn columnToProcess = new OutputColumn(); // TODO: Initialize to an appropriate value

        ManagedComponentHost mch = new ManagedComponentHost();

        DTSBufferManagerClass bufferManagerClass = new DTSBufferManagerClass();
        MainPipeClass mainPipeClass = new MainPipeClass();
        DTP_BUFFCOL[] bufferColumns = new DTP_BUFFCOL[1];
        PipelineBuffer buffer;
        IDTSBuffer100 iBuffer;

        //= new DTP_BUFFCOL();
        int bufferID;

        bufferColumns[0].lCodePage = 1205;
        bufferColumns[0].lLengthOffset = 0;
        bufferColumns[0].lMaxLength = 128;
        bufferColumns[0].lOffset = 0;
        bufferColumns[0].DataType = Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR;
        bufferID = bufferManagerClass.RegisterBufferType(1, ref bufferColumns[0], 100, (uint)Microsoft.SqlServer.Dts.Pipeline.Wrapper.DTSBufferFlags.BUFF_INIT);

        IDTSComponentMetaData100 metaData = mainPipeClass.ComponentMetaDataCollection.New();
        metaData.Description = "This is a test metaData";
        metaData.Name = "Input Buffer";
        //metaData.ComponentClassID = typeof(Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSBuffer100).AssemblyQualifiedName;
        metaData.ComponentClassID = typeof(Martin.SQLServer.Dts.MultipleHash).AssemblyQualifiedName;
        metaData.ComponentClassID = "{874F7595-FB5F-40FF-96AF-FBFF8250E3EF}";

        iBuffer = bufferManagerClass.CreateBuffer(bufferID, metaData);


        BUFFER_WIRE_PACKET bufferWirePacket = new BUFFER_WIRE_PACKET();

        IntPtr pColInfo = Marshal.AllocHGlobal(bufferColumns.Length * Marshal.SizeOf(bufferColumns[0]));
        Marshal.StructureToPtr(bufferColumns[0], pColInfo, true);

        bufferWirePacket.pColInfo = pColInfo;
        bufferWirePacket.dwColCount = 1;
        bufferWirePacket.dwRowCount = 0;
        bufferWirePacket.ppvRowStarts = pColInfo;

        IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(bufferWirePacket));
        Marshal.StructureToPtr(bufferWirePacket, ptr, true);
        buffer = MockRepository.GenerateStub<PipelineBuffer>(ptr, PipelineBufferMode.Input);
        //buffer.AddRow();
            
        Utility.CalculateHash(columnToProcess, buffer, false);
         * 
    }

         */

        ///// <summary>
        /////A test for Utility Constructor
        /////</summary>
        //[TestMethod()]
        //public void UtilityConstructorTest()
        //{
        //    Utility_Accessor target = new Utility_Accessor();
        //    Assert.IsNotNull(target);
        //}

    }
}
