using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using MultipleHash2012Test;
using MultipleHash2012Test.SSISImplementations;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class OutputColumnTests
    {
        /// <summary>
        ///A test for OutputColumnId
        ///</summary>
        [TestMethod()]
        public void OutputColumnIdTest()
        {
            OutputColumn target = new OutputColumn();
            int actual;
            actual = target.OutputColumnId;
            Assert.AreEqual(0, actual);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest()
        {
            OutputColumn target = new OutputColumn();
            int index = 0;
            MHashColumnInformation expected = new MHashColumnInformation(25, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BOOL);
            MHashColumnInformation actual;
            target.Add(new MHashColumnInformation(25, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BOOL));
            target[index] = new MHashColumnInformation(25, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_BOOL);
            actual = target[index];
            Assert.AreEqual(expected.ColumnDataType, actual.ColumnDataType);
            Assert.AreEqual(expected.ColumnId, actual.ColumnId);
        }

        /// <summary>
        ///A test for HashType
        ///</summary>
        [TestMethod()]
        public void HashTypeTest()
        {
            OutputColumn target = new OutputColumn();
            MultipleHash.HashTypeEnumerator actual;
            actual = target.HashType;
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.None, actual);
        }

        /// <summary>
        ///A test for HashObject
        ///</summary>
        [TestMethod()]
        public void HashObjectTest()
        {
            OutputColumn target = new OutputColumn();
            HashAlgorithm actual;
            actual = target.HashObject;
            Assert.IsNull(actual);
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        [TestMethod()]
        public void CountTest()
        {
            OutputColumn target = new OutputColumn();
            int actual;
            actual = target.Count;
            Assert.AreEqual(0, actual);
            target.Add(new MHashColumnInformation(12, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I2));
            actual = target.Count;
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        ///A test for AddColumnInformation
        ///</summary>
        [TestMethod()]
        public void AddColumnInformationTest()
        {
            OutputColumn target = new OutputColumn();
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
            customProperty = outputColumn.CustomPropertyCollection.New();
            customProperty.Name = Utility.OutputColumnOutputTypePropName;
            customProperty.Value = MultipleHash.OutputTypeEnumerator.Binary;

            target.AddColumnInformation(bufferManager, output, input, outputColumnIndex);
            Assert.AreEqual(6, target.Count, "The number of items in the list");
            Assert.AreEqual(1, target[0].ColumnId, "The first input");
            Assert.AreEqual(2, target[1].ColumnId, "The second input");
            Assert.AreEqual(3, target[2].ColumnId, "The third input");
            Assert.AreEqual(4, target[3].ColumnId, "The forth input");
            Assert.AreEqual(5, target[4].ColumnId, "The fifth input");
            Assert.AreEqual(6, target[5].ColumnId, "The sixth input");
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.MD5, target.HashType, "Hash");
            Assert.AreEqual(MD5.Create().ToString(), target.HashObject.ToString(), "Hash Object");
        }

        /// <summary>
        ///A test for AddColumnInformation
        ///</summary>
        [TestMethod()]
        public void AddColumnInformationTestErrors()
        {
            OutputColumn target = new OutputColumn();
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

            try
            {
                target.AddColumnInformation(null, output, input, outputColumnIndex);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value cannot be null.\r\nParameter name: bufferManager", ex.Message);
            }

            try
            {
                target.AddColumnInformation(bufferManager, null, input, outputColumnIndex);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value cannot be null.\r\nParameter name: output", ex.Message);
            }
            try
            {
                target.AddColumnInformation(bufferManager, output, null, outputColumnIndex);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Value cannot be null.\r\nParameter name: input", ex.Message);
            }
        }

        /// <summary>
        ///A test for AddColumnInformation
        ///</summary>
        [TestMethod()]
        public void AddColumnInformationTestReverse()
        {
            OutputColumn target = new OutputColumn();
            IDTSBufferManager100 bufferManager = new BufferManagerTestImpl();
            IDTSOutput100 output = new OutputTestImpl();
            IDTSInput100 input = new InputTestImpl();
            IDTSOutputColumn100 outputColumn;
            IDTSCustomProperty100 customProperty;
            int outputColumnIndex = 0;
            outputColumn = output.OutputColumnCollection.New();
            customProperty = outputColumn.CustomPropertyCollection.New();
            customProperty.Name = Utility.InputColumnLineagePropName;
            customProperty.Value = "#1,#2,#3,#4,#5,#6";
            customProperty = outputColumn.CustomPropertyCollection.New();
            customProperty.Name = Utility.HashTypePropName;
            customProperty.Value = MultipleHash.HashTypeEnumerator.RipeMD160;
            customProperty = outputColumn.CustomPropertyCollection.New();
            customProperty.Name = Utility.OutputColumnOutputTypePropName;
            customProperty.Value = MultipleHash.OutputTypeEnumerator.Binary;

            target.AddColumnInformation(bufferManager, output, input, outputColumnIndex);
            Assert.AreEqual(6, target.Count, "The number of items in the list");
            Assert.AreEqual(1, target[0].ColumnId, "The first input");
            Assert.AreEqual(2, target[1].ColumnId, "The second input");
            Assert.AreEqual(3, target[2].ColumnId, "The third input");
            Assert.AreEqual(4, target[3].ColumnId, "The forth input");
            Assert.AreEqual(5, target[4].ColumnId, "The fifth input");
            Assert.AreEqual(6, target[5].ColumnId, "The sixth input");
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.RipeMD160, target.HashType, "Hash");
            Assert.AreEqual(RIPEMD160.Create().ToString(), target.HashObject.ToString(), "Hash Object");
        }


        /// <summary>
        ///A test for AddColumnInformation
        ///</summary>
        [TestMethod()]
        public void AddColumnInformationTestHashTypes()
        {
            OutputColumn target = new OutputColumn();
            IDTSBufferManager100 bufferManager = new BufferManagerTestImpl();
            IDTSOutput100 output = new OutputTestImpl();
            IDTSInput100 input = new InputTestImpl();
            IDTSOutputColumn100 outputColumn;
            IDTSCustomProperty100 customProperty;
            int outputColumnIndex = 0;
            outputColumn = output.OutputColumnCollection.New();
            customProperty = outputColumn.CustomPropertyCollection.New();
            customProperty.Name = Utility.InputColumnLineagePropName;
            customProperty.Value = "#1,#2,#3,#4,#5,#6";
            customProperty = outputColumn.CustomPropertyCollection.New();
            customProperty.Name = Utility.OutputColumnOutputTypePropName;
            customProperty.Value = MultipleHash.OutputTypeEnumerator.Binary;
            customProperty = outputColumn.CustomPropertyCollection.New();
            customProperty.Name = Utility.HashTypePropName;
            customProperty.Value = MultipleHash.HashTypeEnumerator.SHA1;

            target.AddColumnInformation(bufferManager, output, input, outputColumnIndex);
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.SHA1, target.HashType, "Hash");
            Assert.AreEqual(SHA1.Create().ToString(), target.HashObject.ToString(), "Hash Object");
            customProperty.Value = MultipleHash.HashTypeEnumerator.SHA256;
            target.AddColumnInformation(bufferManager, output, input, outputColumnIndex);
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.SHA256, target.HashType, "Hash");
            Assert.AreEqual(SHA256.Create().ToString(), target.HashObject.ToString(), "Hash Object");
            customProperty.Value = MultipleHash.HashTypeEnumerator.SHA384;
            target.AddColumnInformation(bufferManager, output, input, outputColumnIndex);
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.SHA384, target.HashType, "Hash");
            Assert.AreEqual(SHA384.Create().ToString(), target.HashObject.ToString(), "Hash Object");
            customProperty.Value = MultipleHash.HashTypeEnumerator.SHA512;
            target.AddColumnInformation(bufferManager, output, input, outputColumnIndex);
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.SHA512, target.HashType, "Hash");
            Assert.AreEqual(SHA512.Create().ToString(), target.HashObject.ToString(), "Hash Object");
            customProperty.Value = MultipleHash.HashTypeEnumerator.None;
            target.AddColumnInformation(bufferManager, output, input, outputColumnIndex);
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.None, target.HashType, "Hash");
            Assert.AreEqual(null, target.HashObject, "Hash Object");
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            OutputColumn target = new OutputColumn();
            MHashColumnInformation expected = new MHashColumnInformation(0, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I8);
            int actual;
            actual = target.Add(new MHashColumnInformation(0, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I8));
            Assert.AreEqual(0, actual);
            actual = target.Add(new MHashColumnInformation(0, Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType.DT_I4));
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        ///A test for OutputColumn Constructor
        ///</summary>
        [TestMethod()]
        public void OutputColumnConstructorTest()
        {
            OutputColumn target = new OutputColumn();
            Assert.AreEqual(MultipleHash.HashTypeEnumerator.None, target.HashType, "Hash");
            Assert.AreEqual(0, target.OutputColumnId, "Target Column ID");
            Assert.AreEqual(0, target.Count, "Count of inputs");
        }
    }
}
