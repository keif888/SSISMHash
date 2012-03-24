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
    public class CRC32 : HashAlgorithm
    {
        #region Globals
        private static UInt32 CRC32Polinomial = 0xEDB88320;  // see http://en.wikipedia.org/wiki/Cyclic_redundancy_check for details
        private static UInt32 allFs = 0xffffffff;
        #endregion

        #region Variables
        private static CRC32 defaultCRC32;
        private static Hashtable hashTableCache;
        private UInt32[] crc32Table;
        private UInt32 crc;
        #endregion

        #region Constructors
        public CRC32()
            : this(CRC32Polinomial)
        { }

        public CRC32(UInt32 polynomial)
        {
            HashSizeValue = 32;
            crc32Table = (UInt32[])hashTableCache[polynomial];
            if (crc32Table == null)
            {
                crc32Table = CRC32.buildCRC32Table(polynomial);
                hashTableCache.Add(polynomial, crc32Table);
            }
            Initialize();
        }

        /// <summary>
        /// Static Contstructor of the CRC32 Class
        /// defines the new HashTable.
        /// </summary>
        static CRC32()
        {
            hashTableCache = Hashtable.Synchronized(new Hashtable());
            defaultCRC32 = new CRC32();
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            crc = allFs;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (int i = ibStart; i < cbSize; i++)
            {
                UInt64 pointer = (crc & 0xff) ^ array[i];
                crc >>= 8;
                crc ^= crc32Table[pointer];
            }
        }

        protected override byte[] HashFinal()
        {
            byte[] resultHash = new byte[4];
            UInt64 resultCRC = crc ^ allFs;

            resultHash[0] = (byte)((resultCRC >> 0) & 0xff);
            resultHash[1] = (byte)((resultCRC >> 8) & 0xff);
            resultHash[2] = (byte)((resultCRC >> 16) & 0xff);
            resultHash[3] = (byte)((resultCRC >> 24) & 0xff);

            return resultHash;
        }

        new public byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, 0, buffer.Length);
        }

        new public byte[] ComputeHash(byte[] buffer, int ibStart, int cbSize)
        {
            HashCore(buffer, ibStart, cbSize);
            return HashFinal();
        }

        new public byte[] ComputeHash(Stream inputStream)
        {
            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = inputStream.Read(buffer, 0, 4096)) > 0)
            {
                HashCore(buffer, 0, bytesRead);
            }
            return HashFinal();
        }

        public static CRC32 Create()
        {
            return new CRC32();
        }

        private static uint[] buildCRC32Table(UInt32 polynomial)
        {
            UInt32 crc;
            UInt32[] table = new UInt32[256];

            // 256 values representing ASCII character codes. 
            for (int i = 0; i < 256; i++)
            {
                crc = (UInt32)i;
                for (int j = 8; j > 0; j--)
                {
                    if ((crc & 1) == 1)
                        crc = (crc >> 1) ^ polynomial;
                    else
                        crc >>= 1;
                }
                table[i] = crc;
            }

            return table;
        }
        
        #endregion
    }
}
