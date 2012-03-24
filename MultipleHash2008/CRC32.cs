using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.IO;

namespace Martin.SQLServer.Dts
{
    /// <summary>
    /// Computes a CRC32 Hash from the input data.
    /// </summary>
    public class CRC32 : CRCSliceBy8
    {
        protected override uint[] Polynomial32definition
        {
            get { return new UInt32[] { 26, 23, 22, 16, 12, 11, 10, 8, 7, 5, 4, 2, 1, 0 }; }
        }

        public CRC32()
            : base()
        {
        }

        new public static CRC32 Create()
        {
            return new CRC32();
        }

        protected override byte[] HashFinal()
        {
            byte[] resultHash = new byte[4];

            UInt64 resultCRC = ~crc32;

            resultHash[0] = (byte)((resultCRC >> 0) & 0xff);
            resultHash[1] = (byte)((resultCRC >> 8) & 0xff);
            resultHash[2] = (byte)((resultCRC >> 16) & 0xff);
            resultHash[3] = (byte)((resultCRC >> 24) & 0xff);

            return resultHash;
        }
    }
}
