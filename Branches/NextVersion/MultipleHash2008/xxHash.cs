using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Martin.SQLServer.Dts
{
    class xxHash : HashAlgorithm
    {
        #region Globals
        private const UInt32 Seed = 0;
        private const UInt64 c1_128 = 0x87c37b91114253d5;
        private const UInt64 c2_128 = 0x4cf5ad432745937f;
        private byte[] tmpHash;
        #endregion

        #region Creators
        public xxHash()
        {
            Initialize();
        }
        #endregion

        #region Privates
        #endregion

        #region Protecteds
        new public byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, 0, buffer.Length);
        }

        new public byte[] ComputeHash(byte[] buffer, int ibStart, int cbSize)
        {
            HashCore(buffer, ibStart, cbSize);
            return HashFinal();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            throw new Exception("Not Implemented!");
            // Hash to go here.
        }

        protected override byte[] HashFinal()
        {
            return new byte[16];
        }
        #endregion

        #region Publics
        public override void Initialize()
        {
            HashSizeValue = 128;
        }

        new public static xxHash Create()
        {
            return new xxHash();
        }
        #endregion
    }
}
