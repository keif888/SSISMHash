using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Martin.SQLServer.Dts;
using System.Text;

namespace MultipleHash2012Test
{
    [TestClass]
    public class xxHashTests
    {
        [TestMethod]
        public void InitialiseTest()
        {
            xxHash actual = new Martin.SQLServer.Dts.xxHash();
            actual.Initialize();
        }

        [TestMethod]
        public void xxHashTestComputeHashBase()
        {
            xxHash target = new Martin.SQLServer.Dts.xxHash();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("ThisIsATestValueForxxHash64");
            byte[] expected = GetStringToBytes("f8d1c99eda0d8577");  // https://asecuritysite.com/encryption/xxHash  BUT, reverse the hash bytes.
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void xxHashTestComputeHashOffsetZero()
        {
            xxHash target = new Martin.SQLServer.Dts.xxHash();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("ThisIsATestValueForxxHash64");
            byte[] expected = GetStringToBytes("f8d1c99eda0d8577");  // https://asecuritysite.com/encryption/xxHash  BUT, reverse the hash bytes.
            byte[] actual;
            actual = target.ComputeHash(buffer,0, buffer.Length);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void xxHashTestComputeHashOffsetTen()
        {
            xxHash target = new Martin.SQLServer.Dts.xxHash();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("0123456789ThisIsATestValueForxxHash64");
            byte[] expected = GetStringToBytes("f8d1c99eda0d8577");  // https://asecuritysite.com/encryption/xxHash  BUT, reverse the hash bytes.
            byte[] actual;
            actual = target.ComputeHash(buffer, 10, buffer.Length - 10);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        #region Byte Stuff

        public byte[] GetStringToBytes(string value)
        {
            SoapHexBinary shb = SoapHexBinary.Parse(value);
            return shb.Value;
        }

        public void testByteValues(byte[] expected, byte[] actual, String message)
        {
            Assert.AreEqual(expected.Length, actual.Length, String.Format("{0} Length", message));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i], String.Format("{0} Array Item {1}", message, i));
            }
        }

        #endregion
    }
}
