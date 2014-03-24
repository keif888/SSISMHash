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
    public class FNV1a64Tests
    {
        /// <summary>
        ///A test for FNV1a64 Constructor
        ///</summary>
        [TestMethod()]
        public void FNV1a64ConstructorTest()
        {
            FNV1a64 target = new FNV1a64();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for ComputeHash
        ///</summary>
        [TestMethod()]
        public void ComputeHashTest()
        {
            FNV1a64 target = new FNV1a64();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("curds and whey\n");
            byte[] expected = { 0xec, 0x85, 0x13, 0xfe, 0xcc, 0x44, 0x0b, 0x1a }; // 0x1a0b44ccfe1385ecULL  
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
        public void ComputeHashTestBufferIndexAndLength()
        {
            FNV1a64 target = new FNV1a64(); // TODO: Initialize to an appropriate value
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("chongo was here!\n");
            int ibStart = 0;
            int cbSize = buffer.Length;
            byte[] expected = { 0x15, 0xf9, 0xf5, 0xef, 0x40, 0x09, 0x81, 0x46 }; // 0x46810940eff5f915ULL  
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
            FNV1a64 expected = new FNV1a64(); // TODO: Initialize to an appropriate value
            FNV1a64 actual;
            actual = FNV1a64.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        /// <summary>
        ///A test for Initialize
        ///</summary>
        [TestMethod()]
        public void InitializeTest()
        {
            FNV1a64 target = new FNV1a64();
            target.Initialize();
            Assert.AreEqual(64, target.HashSize);
        }
    }
}
