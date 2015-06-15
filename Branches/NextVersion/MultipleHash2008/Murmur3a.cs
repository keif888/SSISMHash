using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace Martin.SQLServer.Dts
{
    class Murmur3a : HashAlgorithm
    {
        #region Globals
        private const UInt32 Seed = 0;
        private const UInt64 c1_128 = 0x87c37b91114253d5;
        private const UInt64 c2_128 = 0x4cf5ad432745937f;
        private byte[] tmpHash;
        #endregion

        #region Creators
        public Murmur3a()
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
            UInt64 h1 = (UInt64)Seed;
            UInt64 h2 = (UInt64)Seed;

            int dataCount = 0;


            data.ForEachGroup(16,
                (dataGroup, position, length) =>
                {
                    ProcessGroup(ref h1, ref h2, dataGroup, position, length);

                    dataCount += length;
                },
                (remainder, position, length) =>
                {
                    ProcessRemainder(ref h1, ref h2, remainder, position, length);

                    dataCount += length;
                });


            h1 ^= (UInt64)dataCount;
            h2 ^= (UInt64)dataCount;

            h1 += h2;
            h2 += h1;

            Mix(ref h1);
            Mix(ref h2);

            h1 += h2;
            h2 += h1;


            var hashBytes = new byte[16];

            BitConverter.GetBytes(h1).CopyTo(hashBytes, 0);

            BitConverter.GetBytes(h2).CopyTo(hashBytes, 8);

            tmpHash = hashBytes;
        }

        protected override byte[] HashFinal()
        {
            return tmpHash;
        }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void ProcessGroup(ref UInt64 h1, ref UInt64 h2, byte[] dataGroup, int position, int length)
        {
            for (var x = position; x < position + length; x += 16)
            {
                UInt64 k1 = BitConverter.ToUInt64(dataGroup, 0);
                UInt64 k2 = BitConverter.ToUInt64(dataGroup, 8);

                k1 *= c1_128;
                k1 = RotateLeft(k1, 31);
                k1 *= c2_128;
                h1 ^= k1;

                h1 = RotateLeft(h1, 27);
                h1 += h2;
                h1 = (h1 * 5) + 0x52dce729;

                k2 *= c2_128;
                k2 = RotateLeft(k2, 33);
                k2 *= c1_128;
                h2 ^= k2;

                h2 = RotateLeft(h2, 31);
                h2 += h1;
                h2 = (h2 * 5) + 0x38495ab5;
            }
        }


#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void ProcessRemainder(ref UInt64 h1, ref UInt64 h2, byte[] remainder, int position, int length)
        {
            UInt64 k1 = 0;
            UInt64 k2 = 0;

            switch (length)
            {
                case 15: k2 ^= (UInt64)remainder[position + 14] << 48; goto case 14;
                case 14: k2 ^= (UInt64)remainder[position + 13] << 40; goto case 13;
                case 13: k2 ^= (UInt64)remainder[position + 12] << 32; goto case 12;
                case 12: k2 ^= (UInt64)remainder[position + 11] << 24; goto case 11;
                case 11: k2 ^= (UInt64)remainder[position + 10] << 16; goto case 10;
                case 10: k2 ^= (UInt64)remainder[position + 9] << 8; goto case 9;
                case 9:
                    k2 ^= ((UInt64)remainder[position + 8]) << 0;
                    k2 *= c2_128;
                    k2 = RotateLeft(k2, 33);
                    k2 *= c1_128; h2 ^= k2;

                    goto case 8;

                case 8:
                    k1 ^= BitConverter.ToUInt64(remainder, position);
                    break;

                case 7: k1 ^= (UInt64)remainder[position + 6] << 48; goto case 6;
                case 6: k1 ^= (UInt64)remainder[position + 5] << 40; goto case 5;
                case 5: k1 ^= (UInt64)remainder[position + 4] << 32; goto case 4;
                case 4: k1 ^= (UInt64)remainder[position + 3] << 24; goto case 3;
                case 3: k1 ^= (UInt64)remainder[position + 2] << 16; goto case 2;
                case 2: k1 ^= (UInt64)remainder[position + 1] << 8; goto case 1;
                case 1:
                    k1 ^= (UInt64)remainder[position] << 0;
                    break;
            }

            k1 *= c1_128;
            k1 = RotateLeft(k1, 31);
            k1 *= c2_128;
            h1 ^= k1;
        }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static UInt64 RotateLeft(this UInt64 operand, int shiftCount)
        {
            shiftCount &= 0x3f;

            return
                (operand << shiftCount) |
                (operand >> (64 - shiftCount));
        }



#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void Mix(ref UInt32 h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;
        }

#if NET45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        private static void Mix(ref UInt64 k)
        {
            k ^= k >> 33;
            k *= 0xff51afd7ed558ccd;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53;
            k ^= k >> 33;
        }

        #endregion

        #region Publics
        public override void Initialize()
        {
            HashSizeValue = 128;
        }

        new public static Murmur3a Create()
        {
            return new Murmur3a();
        }
        #endregion
    }
}
