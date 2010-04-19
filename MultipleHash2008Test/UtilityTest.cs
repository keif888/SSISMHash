using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Pipeline;

namespace MultipleHash2008Test
{
    
    
    /// <summary>
    ///This is a test class for UtilityTest and is intended
    ///to contain all UtilityTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UtilityTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


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
        public void ToArrayTest14()
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
        public void ToArrayTest13()
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
        public void ToArrayTest12()
        {
            short value = 12345;
            byte[] expected = new byte[2] {57, 48};
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
        public void ToArrayTest11()
        {
            sbyte value = 123;
            byte[] expected = new byte[1] {123};
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
        public void ToArrayTest10()
        {
            uint value = 1234567890;
            byte[] expected = new byte[4] { 210, 2, 150, 73};
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
        public void ToArrayTest9()
        {
            ulong value = 12345678901234567890;
            byte[] expected = new byte[8] {210, 10, 31, 235, 140, 169, 84, 171};
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
        public void ToArrayTest8()
        {
            double value = 1234567890.1234567890F;
            byte[] expected = new byte[8] {0, 0, 0, 192, 128, 101, 210, 65};
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
        public void ToArrayTest7()
        {
            ushort value = 1234;
            byte[] expected = new byte[2]{210, 4};
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
        public void ToArrayTest6()
        {
            Decimal value = new Decimal();
            value = 1234567890.1234567890M;
            byte[] expected = new byte[16] {210, 10, 31, 235, 140, 169, 84, 171, 0, 0, 0, 0, 0, 0, 10, 0};
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
        public void ToArrayTest5()
        {
            bool value = true;
            byte[] expected = new byte[1] {1};
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
        public void ToArrayTest4()
        {
            int value = 1234567890;
            byte[] expected = new byte[4] {210, 2, 150, 73};
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
        public void ToArrayTest3()
        {
            DateTime value = new DateTime(1234567890);
            byte[] expected = new byte[21] { 20, 48, 48, 48, 49, 45, 48, 49, 45, 48, 49, 32, 48, 48, 58, 48, 50, 58, 48, 51, 90 };
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
        public void ToArrayTest2()
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
        public void ToArrayTest1()
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
        public void ToArrayTest()
        {
            TimeSpan value = new TimeSpan(1234567890);
            byte[] expected = new byte[17] {16, 48, 48, 58, 48, 50, 58, 48, 51, 46, 52, 53, 54, 55, 56, 57, 48};
            byte[] actual;
            actual = Utility.ToArray(value);
            Assert.AreEqual(expected.Length, actual.Length, "Values are not same length");
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        /// <summary>
        ///A test for SetOutputColumnDataType
        ///</summary>
        [TestMethod()]
        public void SetOutputColumnDataTypeTest()
        {
            MultipleHash.HashTypeEnumerator propertyValue = new MultipleHash.HashTypeEnumerator();
            propertyValue = MultipleHash.HashTypeEnumerator.MD5;
            IDTSOutputColumn100 outputColumn = new OutputColumnTestImpl();
            Utility.SetOutputColumnDataType(propertyValue, outputColumn);
            Assert.AreEqual(outputColumn.CodePage, 0);
            Assert.AreEqual(outputColumn.DataType, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BYTES);
            Assert.AreEqual(outputColumn.Length, 16);
            Assert.AreEqual(outputColumn.Precision, 0);
            Assert.AreEqual(outputColumn.Scale, 0);
        }

        /// <summary>
        ///A test for GetNumberOfProcessorCores
        ///</summary>
        [TestMethod()]
        public void GetNumberOfProcessorCoresTest()
        {
            int expected = 1; // This has to be changed for every machine the test is run on!
            int actual;
            actual = Utility.GetNumberOfProcessorCores();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest17()
        {
            byte[] array = new byte[1] {0};
            byte[] arrayExpected = new byte[2] {0,1};
            bool value = true;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest16()
        {
            byte[] array = new byte[1] {0};
            byte[] arrayExpected = new byte[22] {0, 20, 48, 48, 48, 49, 45, 48, 49, 45, 48, 49, 32, 48, 48, 58, 48, 50, 58, 48, 51, 90 };
            DateTime value = new DateTime(1234567890);
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest15()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[9] { 0, 21, 129, 233, 125, 244, 16, 34, 17 };
            long value = 1234567890123456789;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest14()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[5] {0, 210, 2, 150, 73 };
            uint value = 1234567890;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest13()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[3] {0, 210, 4 };
            ushort value = 1234;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest12()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[5] {0, 210, 2, 150, 73 };
            int value = 1234567890;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest11()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] { 0, 84 };
            char value = 'T';
            Encoding encoding = Encoding.UTF8;
            Utility.Append(ref array, value, encoding);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest10()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[22] { 0, 84, 104, 105, 115, 32, 105, 115, 32, 97, 32, 116, 101, 115, 116, 32, 115, 116, 114, 105, 110, 103 };
            string value = "This is a test string";
            Encoding encoding = Encoding.UTF8;
            Utility.Append(ref array, value, encoding);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest9()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[9] {0, 0, 0, 0, 192, 128, 101, 210, 65 };
            double value = 1234567890.1234567890F;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest8()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[17] {0, 210, 10, 31, 235, 140, 169, 84, 171, 0, 0, 0, 0, 0, 0, 10, 0 };
            Decimal value = new Decimal();
            value = 1234567890.1234567890M;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest7()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[9] {0, 210, 10, 31, 235, 140, 169, 84, 171 }; 
            ulong value = 12345678901234567890;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest6()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[5] {0, 6, 44, 147, 78 }; 
            float value = 1234567890.1234567890F;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest5()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[18] {0, 16, 48, 48, 58, 48, 50, 58, 48, 51, 46, 52, 53, 54, 55, 56, 57, 48 };
            TimeSpan value = new TimeSpan(1234567890);
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest4()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[17] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
            Guid value = new Guid();
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest3()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] {0, 123 }; 
            sbyte value = 123;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest2()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[3] {0, 57, 48 };
            short value = 12345;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest1()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] { 0, 123 };
            byte value = 123;
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for Append
        ///</summary>
        [TestMethod()]
        public void AppendTest()
        {
            byte[] array = new byte[1] { 0 };
            byte[] arrayExpected = new byte[2] {0, 123 };
            byte[] value = new byte[1] { 123 };
            Utility.Append(ref array, value);
            Assert.AreEqual(arrayExpected.Length, array.Length, "Values are not same length");
            for (int i = 0; i < arrayExpected.Length; i++)
            {
                Assert.AreEqual(arrayExpected[i], array[i]);
            }
        }

        /// <summary>
        ///A test for CalculateHash
        ///</summary>
        [TestMethod()]
        public void CalculateHashTest()
        {
            Assert.Inconclusive("Unable to test this as we can't initialise a PipelineBuffer...");
            OutputColumn columnToProcess = new OutputColumn(); // TODO: Initialize to an appropriate value
            PipelineBuffer buffer = null; // TODO: Initialize to an appropriate value
            Utility.CalculateHash(columnToProcess, buffer);
        }
    }
}
