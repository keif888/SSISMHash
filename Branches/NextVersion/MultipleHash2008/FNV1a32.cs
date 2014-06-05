using System;
using System.Security.Cryptography;

namespace Martin.SQLServer.Dts
{
    public class FNV1a32 : HashAlgorithm
    {

        #region Globals
        private const UInt32 FNVPrime = 0x01000193;
        private const UInt32 FNVOffsetBasis = 0x811C9DC5;
        private UInt32 FNVHash;
        #endregion

        #region Creators
        public FNV1a32()
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
            byte[] resultHash = new byte[4];

            resultHash[0] = (byte)((FNVHash >> 0) & 0xff);
            resultHash[1] = (byte)((FNVHash >> 8) & 0xff);
            resultHash[2] = (byte)((FNVHash >> 16) & 0xff);
            resultHash[3] = (byte)((FNVHash >> 24) & 0xff);

            return resultHash;
        }
        #endregion

        #region Publics
        public override void Initialize()
        {
            HashSizeValue = 32;
            FNVHash = FNVOffsetBasis;
        }

        new public static FNV1a32 Create()
        {
            return new FNV1a32();
        }
        #endregion

    }
}
