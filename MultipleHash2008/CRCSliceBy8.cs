using System;
using System.Security.Cryptography;

namespace Martin.SQLServer.Dts
{
    public abstract class CRCSliceBy8 : HashAlgorithm
    {
        #region Globals

        protected UInt32[] Table32_32;
        protected UInt32[] Table32_40;
        protected UInt32[] Table32_48;
        protected UInt32[] Table32_56;
        protected UInt32[] Table32_64;
        protected UInt32[] Table32_72;
        protected UInt32[] Table32_80;
        protected UInt32[] Table32_88;

        #endregion

        #region Creators

        protected CRCSliceBy8()
            : base()
        {
            MakeTables32();
            Initialize();
        }

        #endregion

        #region Privates

        protected UInt32 crc32;

        private void MakeTables32()
        {
            UInt32 poly32Reversed = MakeReversePolynomial(Polynomial32definition);
            Table32_32 = MakeTable(poly32Reversed, 0);
            Table32_40 = MakeTable(poly32Reversed, 8);
            Table32_48 = MakeTable(poly32Reversed, 16);
            Table32_56 = MakeTable(poly32Reversed, 24);
            Table32_64 = MakeTable(poly32Reversed, 32);
            Table32_72 = MakeTable(poly32Reversed, 40);
            Table32_80 = MakeTable(poly32Reversed, 48);
            Table32_88 = MakeTable(poly32Reversed, 56);
        }

        private UInt32[] MakeTable(UInt32 polynomial, int offset)
        {
            UInt32[] table = new UInt32[256];

            UInt64 c, x, rem;
            for (int i = table.GetLowerBound(0); i <= table.GetUpperBound(0); i++)
            {
                c = ((UInt64)i) << offset;
                rem = 0;

                for (int part = 8; --part >= 0; )
                {
                    x = c >> 56;
                    c <<= 8;

                    rem ^= x;
                    for (int j = 8; --j >= 0; )
                        rem = (rem & (0x01u)) != 0 ? polynomial ^ (rem >> 1) : (rem >> 1);

                }

                table[i] = (UInt32)rem;
            }
            return table;
        }

        private static UInt32 MakeReversePolynomial(UInt32[] Polynomial32definition)
        {
            UInt32 polynomial = 0;
            for (int i = 0; i <= Polynomial32definition.GetUpperBound(0); i++)
                polynomial |= 1u << (int)(31u - Polynomial32definition[i]);
            return polynomial;
        }


        #endregion



        protected abstract UInt32[] Polynomial32definition { get; }


        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            int headCycles = cbSize >> 3;
            int tailCycles = cbSize & 0x07;

            int offset = ibStart;
            // head
            for (int i = headCycles; --i >= 0; offset += 8)
            {
                crc32 ^= BitConverter.ToUInt32(array, offset);

                crc32 = Table32_88[crc32 & 0xFF]
                        ^ Table32_80[(crc32 >> 8) & 0xFF]
                        ^ Table32_72[(crc32 >> 16) & 0xFF]
                        ^ Table32_64[(crc32 >> 24) & 0xFF]
                        ^ Table32_56[array[offset + 4]]
                        ^ Table32_48[array[offset + 5]]
                        ^ Table32_40[array[offset + 6]]
                        ^ Table32_32[array[offset + 7]];
            }

            // tail
            if (tailCycles != 0)
                for (int i = tailCycles; --i >= 0; )
                    crc32 = Table32_32[(crc32 ^ (UInt32)array[offset++]) & 0xFF] ^ (crc32 >> 8);
        }

        /// <summary>
        /// This is the CRC32 not CRC32C HashFinal.  Be Warned!
        /// </summary>
        /// <returns></returns>
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

        public override void Initialize()
        {
            crc32 = UInt32.MaxValue;
            HashSizeValue = 32;
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
    }
}
