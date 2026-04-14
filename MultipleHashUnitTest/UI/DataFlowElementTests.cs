using Microsoft.VisualStudio.TestTools.UnitTesting;
using Martin.SQLServer.Dts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class DataFlowElementTests
    {
        [TestMethod()]
        public void DataFlowElementTest()
        {
            DataFlowElement dataFlowElement;
            dataFlowElement = new DataFlowElement();
            Assert.IsNotNull((object)dataFlowElement);
            Assert.IsNull(dataFlowElement.Name);
            Assert.IsNull(dataFlowElement.Tag);
        }

        [TestMethod()]
        public void DataFlowElementTest1()
        {
            DataFlowElement dataFlowElement;
            dataFlowElement = new DataFlowElement("name");
            Assert.IsNotNull((object)dataFlowElement);
            Assert.AreEqual<string>("name", dataFlowElement.Name);
            Assert.IsNull(dataFlowElement.Tag);
        }

        [TestMethod()]
        public void DataFlowElementTest2()
        {
            DataFlowElement dataFlowElement;
            dataFlowElement = new DataFlowElement("name", "tag");
            Assert.IsNotNull((object)dataFlowElement);
            Assert.AreEqual<string>("name", dataFlowElement.Name);
            Assert.AreEqual("tag", dataFlowElement.Tag);
        }

        [TestMethod()]
        public void CloneTest()
        {
            DataFlowElement dataFlowElement;
            DataFlowElement dataFlowElement1;
            dataFlowElement = new DataFlowElement("name", "tag");
            dataFlowElement1 = dataFlowElement.Clone();
            Assert.IsNotNull((object)dataFlowElement1);
            Assert.AreEqual<string>("name", dataFlowElement1.Name);
            Assert.AreEqual("tag",dataFlowElement1.Tag);
            Assert.AreEqual<string>("", dataFlowElement1.ToolTip);
            Assert.IsNotNull((object)dataFlowElement);
            Assert.AreEqual<string>("name", dataFlowElement.Name);
            Assert.AreEqual("tag", dataFlowElement.Tag);
            Assert.AreEqual<string>("", dataFlowElement.ToolTip);
        }

        [TestMethod()]
        public void ToStringTest()
        {
            DataFlowElement dataFlowElement;
            dataFlowElement = new DataFlowElement("name", "tag");
            Assert.AreEqual<string>("name", dataFlowElement.ToString());
        }

        [TestMethod()]
        public void GetHashCodeTest()
        {
            DataFlowElement dataFlowElement;
            dataFlowElement = new DataFlowElement("name", "tag");
            Assert.AreEqual("name".GetHashCode(), dataFlowElement.GetHashCode());
        }
    }
}