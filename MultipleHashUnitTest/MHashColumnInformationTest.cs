using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Martin.SQLServer.Dts;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass]
    public class MHashColumnInformationTest
    {
        [TestMethod]
        public void TestConstructorAndGettors()
        {
            MHashColumnInformation columnInformation;
            columnInformation = new MHashColumnInformation(0, DataType.DT_EMPTY);
            Assert.IsNotNull((object)columnInformation);
            Assert.AreEqual<int>(0, columnInformation.ColumnId);
            Assert.AreEqual<DataType>(DataType.DT_EMPTY, columnInformation.ColumnDataType);
        }

    }
}
