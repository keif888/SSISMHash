using System;
using System.Security.Cryptography;

namespace Martin.SQLServer.Dts
{
    public class FNV1a64 : HashAlgorithm
    {

        #region Globals
        private const UInt64 FNVPrime = 0x0100000001B3;
        private const UInt64 FNVOffsetBasis = 0xcbf29ce484222325; // 0x14650FB0739D0383;  Interesting.  Using the OffsetBasis from http://www.isthe.com/chongo/tech/comp/fnv/index.html, fails the tests on the same site.
        private UInt64 FNVHash;
        #endregion

        #region Creators
        public FNV1a64()
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
            for (int i = ibStart; i< cbSize; i++)
            {
                FNVHash = (array[i] ^ FNVHash) * FNVPrime;
            }
        }

        protected override byte[] HashFinal()
        {
            byte[] resultHash = new byte[8];

            resultHash[0] = (byte)((FNVHash >> 0) & 0xff);
            resultHash[1] = (byte)((FNVHash >> 8) & 0xff);
            resultHash[2] = (byte)((FNVHash >> 16) & 0xff);
            resultHash[3] = (byte)((FNVHash >> 24) & 0xff);
            resultHash[4] = (byte)((FNVHash >> 32) & 0xff);
            resultHash[5] = (byte)((FNVHash >> 40) & 0xff);
            resultHash[6] = (byte)((FNVHash >> 48) & 0xff);
            resultHash[7] = (byte)((FNVHash >> 56) & 0xff);

            return resultHash;
        }
        #endregion

        #region Publics
        public override void Initialize()
        {
            HashSizeValue = 64;
            FNVHash = FNVOffsetBasis;
        }

        new public static FNV1a64 Create()
        {
            return new FNV1a64();
        }
        #endregion    
    }
}
