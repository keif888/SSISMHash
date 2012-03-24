using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Martin.SQLServer.Dts
{
    public class CRC32C : CRCSliceBy8
    {
        protected override uint[] Polynomial32definition
        {
            get { return new UInt32[] { 28, 27, 26, 25, 23, 22, 20, 19, 18, 14, 13, 11, 10, 9, 8, 6, 0 }; }
        }

        public CRC32C() : base()
        {
        }

        new public static CRC32C Create()
        {
            return new CRC32C();
        }

        protected override byte[] HashFinal()
        {
            byte[] resultHash = new byte[4];

            resultHash[0] = (byte)((crc32 >> 0) & 0xff);
            resultHash[1] = (byte)((crc32 >> 8) & 0xff);
            resultHash[2] = (byte)((crc32 >> 16) & 0xff);
            resultHash[3] = (byte)((crc32 >> 24) & 0xff);

            return resultHash;
        }
    }
}
