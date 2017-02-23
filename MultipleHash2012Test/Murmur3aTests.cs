using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martin.SQLServer.Dts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Remoting.Metadata.W3cXsd2001;


namespace Martin.SQLServer.Dts.Tests
{
    [TestClass()]
    public class Murmur3aTests
    {
        [TestMethod()]
        public void Murmur3aTest()
        {
            Murmur3a target = new Murmur3a();
            byte[] buffer = ASCIIEncoding.ASCII.GetBytes("ThisIsATestValueForMurmurHash3");
            byte[] expected = GetStringToBytes("bbcac44bfcae82a84a46adad35cecf90");  // http://murmurhash.shorelabs.com/  but you need to use the x64/128 bit implementation, AND do a BigEndian on it.
            byte[] actual;
            actual = target.ComputeHash(buffer);
            Assert.AreEqual(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod()]
        public void InitializeTest()
        {
            Murmur3a target = new Murmur3a();
            target.Initialize();  // Rely on an exception to indicate test failure.
            //Assert.AreEqual(0, target.seed);  // seed is private :-(
        }

        [TestMethod()]
        public void CreateTest()
        {
            Murmur3a expected = new Murmur3a(); // TODO: Initialize to an appropriate value
            Murmur3a actual;
            actual = Murmur3a.Create();
            Assert.AreEqual(expected.ToString(), actual.ToString());
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
