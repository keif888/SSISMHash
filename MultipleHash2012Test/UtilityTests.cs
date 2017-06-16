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
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class UtilityTests
    {

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
            Assert.AreEqual(10, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.CRC32C, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(10, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a32, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(10, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.FNV1a64, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(18, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.MurmurHash3a, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);  // MurmurHash3a using 128bit hash result.  (See this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/MurmurHash/MurmurHash3.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(34, outputColumn.Length, "Length wasn't set correctly");
            Utility.SetOutputColumnDataType(MultipleHash.HashTypeEnumerator.xxHash, MultipleHash.OutputTypeEnumerator.HexString, outputColumn);  // xxHash using 64bit hash result. (see this for code https://github.com/brandondahler/Data.HashFunction/blob/master/src/xxHash/xxHash.cs)
            Assert.AreEqual(Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_STR, outputColumn.DataType, "DT_STR wasn't set as data type");
            Assert.AreEqual(18, outputColumn.Length, "Length wasn't set correctly");

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
    }
}
