using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class IntHelpersTests
    {
        [TestMethod()]
        public void RotateLeftTest()
        {
            UInt64 actual = 3421843292831082394UL;
            UInt64 expected = 17856004537878215074UL;
            Assert.AreEqual(expected, actual.RotateLeft(4));
        }

        [TestMethod()]
        public void RotateRightTest()
        {
            UInt64 actual = 3421843292831082394UL;
            UInt64 expected = 11743080251870412409UL;
            Assert.AreEqual(expected, actual.RotateRight(4));
        }

        [TestMethod()]
        public void GetUInt64Test()
        {
            UInt64 expected = 11743080251870412409UL;
            byte[] data = BitConverter.GetBytes(expected);
            Assert.AreEqual(expected, data.GetUInt64(0));
        }

    }
}
