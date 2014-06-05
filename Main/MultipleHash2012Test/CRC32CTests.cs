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
    public class CRC32CTests
    {
        /// <summary>
        ///A test for CRC32C Constructor
        ///</summary>
        [TestMethod()]
        public void CRC32CConstructorTest()
        {
            CRC32C target = new CRC32C();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestBuffer()
        {
            CRC32C target = new CRC32C();
            byte[] buffer = {0x01, 0xC0, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x01, 0xFE, 0x60, 0xAC,
                            0x00, 0x00, 0x00, 0x08,
                            0x00, 0x00, 0x00, 0x04,
                            0x00, 0x00, 0x00, 0x09,
                            0x25, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00};
            byte[] expected = { 0xeb, 0x75, 0x4f, 0x66 };
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestBufferOffsetLength()
        {
            CRC32C target = new CRC32C();
            byte[] buffer = {0x01, 0xC0, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x01, 0xFE, 0x60, 0xAC,
                            0x00, 0x00, 0x00, 0x08,
                            0x00, 0x00, 0x00, 0x04,
                            0x00, 0x00, 0x00, 0x09,
                            0x25, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00,
                            0x00, 0x00, 0x00, 0x00};
            int ibStart = 0;
            int cbSize = buffer.Length;
            byte[] expected = { 0xeb, 0x75, 0x4f, 0x66 };
            byte[] actual;
            actual = target.ComputeHash(buffer, ibStart, cbSize);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CRC32C expected = new CRC32C();
            CRC32C actual;
            actual = CRC32C.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }
    }
}
