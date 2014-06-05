using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Martin.SQLServer.Dts;
using System.Text;


namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class UnitTestCRC32
    {
        /// <summary>
        ///A test for CRC32 Constructor
        ///</summary>
        [TestMethod()]
        public void CRC32ConstructorTest()
        {
            CRC32 target = new CRC32();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        [TestMethod()]
        public void CreateTest()
        {
            CRC32 expected = new CRC32();
            CRC32 actual;
            actual = CRC32.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTestJustBuffer()
        {
            CRC32 target = new CRC32();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("abcdefghijklmnopqrstuvwxyz");
            byte[] expected = { 0xbd, 0x50, 0x27, 0x4c };
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}
