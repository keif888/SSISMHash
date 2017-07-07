using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace Martin.SQLServer.Dts
{
    // This is failing the unit tests
//    public class Murmur3a : HashAlgorithm
//    {
//        #region Globals
//        private const UInt32 Seed = 0;
//        private const UInt64 c1_128 = 0x87c37b91114253d5;
//        private const UInt64 c2_128 = 0x4cf5ad432745937f;
//        private byte[] tmpHash;
//        #endregion

//        #region Creators
//        public Murmur3a()
//        {
//            Initialize();
//        }
//        #endregion

//        #region Privates
//        #endregion

//        #region Protecteds
//        new public byte[] ComputeHash(byte[] buffer)
//        {
//            return ComputeHash(buffer, 0, buffer.Length);
//        }

//        new public byte[] ComputeHash(byte[] buffer, int ibStart, int cbSize)
//        {
//            HashCore(buffer, ibStart, cbSize);
//            return HashFinal();
//        }

//        protected override void HashCore(byte[] array, int ibStart, int cbSize)
//        {
//            UInt64 h1 = (UInt64)Seed;
//            UInt64 h2 = (UInt64)Seed;

//            int dataCount = 0;
//            int position = ibStart;
//            int length = cbSize;
//            int remaining = cbSize - ibStart;


//            var remainderLength = cbSize % 16;

//            if (length - remainderLength > 0)
//            {
//                ProcessGroup(ref h1, ref h2, array, position, length - remainderLength);
//                dataCount += 16;
//            }
//            if (remainderLength > 0)
//            {
//                ProcessRemainder(ref h1, ref h2, array, length - remainderLength, remainderLength);
//                dataCount += 16;
//            }

//            h1 ^= (UInt64)dataCount;
//            h2 ^= (UInt64)dataCount;

//            h1 += h2;
//            h2 += h1;

//            Mix(ref h1);
//            Mix(ref h2);

//            h1 += h2;
//            h2 += h1;


//            var hashBytes = new byte[16];

//            BitConverter.GetBytes(h1).CopyTo(hashBytes, 0);

//            BitConverter.GetBytes(h2).CopyTo(hashBytes, 8);

//            tmpHash = hashBytes;
//        }

//        protected override byte[] HashFinal()
//        {
//            return tmpHash;
//        }

//#if NET45
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//#endif
//        private static void ProcessGroup(ref UInt64 h1, ref UInt64 h2, byte[] dataGroup, int position, int length)
//        {
//            for (var x = position; x < position + length; x += 16)
//            {
//                UInt64 k1 = BitConverter.ToUInt64(dataGroup, 0);
//                UInt64 k2 = BitConverter.ToUInt64(dataGroup, 8);

//                k1 *= c1_128;
//                k1 = RotateLeft(k1, 31);
//                k1 *= c2_128;
//                h1 ^= k1;

//                h1 = RotateLeft(h1, 27);
//                h1 += h2;
//                h1 = (h1 * 5) + 0x52dce729;

//                k2 *= c2_128;
//                k2 = RotateLeft(k2, 33);
//                k2 *= c1_128;
//                h2 ^= k2;

//                h2 = RotateLeft(h2, 31);
//                h2 += h1;
//                h2 = (h2 * 5) + 0x38495ab5;
//            }
//        }


//#if NET45
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//#endif
//        private static void ProcessRemainder(ref UInt64 h1, ref UInt64 h2, byte[] remainder, int position, int length)
//        {
//            UInt64 k1 = 0;
//            UInt64 k2 = 0;

//            switch (length)
//            {
//                case 15: k2 ^= (UInt64)remainder[position + 14] << 48; goto case 14;
//                case 14: k2 ^= (UInt64)remainder[position + 13] << 40; goto case 13;
//                case 13: k2 ^= (UInt64)remainder[position + 12] << 32; goto case 12;
//                case 12: k2 ^= (UInt64)remainder[position + 11] << 24; goto case 11;
//                case 11: k2 ^= (UInt64)remainder[position + 10] << 16; goto case 10;
//                case 10: k2 ^= (UInt64)remainder[position + 9] << 8; goto case 9;
//                case 9:
//                    k2 ^= ((UInt64)remainder[position + 8]) << 0;
//                    k2 *= c2_128;
//                    k2 = RotateLeft(k2, 33);
//                    k2 *= c1_128; h2 ^= k2;

//                    goto case 8;

//                case 8:
//                    k1 ^= BitConverter.ToUInt64(remainder, position);
//                    break;

//                case 7: k1 ^= (UInt64)remainder[position + 6] << 48; goto case 6;
//                case 6: k1 ^= (UInt64)remainder[position + 5] << 40; goto case 5;
//                case 5: k1 ^= (UInt64)remainder[position + 4] << 32; goto case 4;
//                case 4: k1 ^= (UInt64)remainder[position + 3] << 24; goto case 3;
//                case 3: k1 ^= (UInt64)remainder[position + 2] << 16; goto case 2;
//                case 2: k1 ^= (UInt64)remainder[position + 1] << 8; goto case 1;
//                case 1:
//                    k1 ^= (UInt64)remainder[position] << 0;
//                    break;
//            }

//            k1 *= c1_128;
//            k1 = RotateLeft(k1, 31);
//            k1 *= c2_128;
//            h1 ^= k1;
//        }

//#if NET45
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//#endif
//        public static UInt64 RotateLeft(UInt64 operand, int shiftCount)
//        {
//            shiftCount &= 0x3f;

//            return
//                (operand << shiftCount) |
//                (operand >> (64 - shiftCount));
//        }



//#if NET45
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//#endif
//        private static void Mix(ref UInt32 h)
//        {
//            h ^= h >> 16;
//            h *= 0x85ebca6b;
//            h ^= h >> 13;
//            h *= 0xc2b2ae35;
//            h ^= h >> 16;
//        }

//#if NET45
//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//#endif
//        private static void Mix(ref UInt64 k)
//        {
//            k ^= k >> 33;
//            k *= 0xff51afd7ed558ccd;
//            k ^= k >> 33;
//            k *= 0xc4ceb9fe1a85ec53;
//            k ^= k >> 33;
//        }

//        #endregion

//        #region Publics
//        public override void Initialize()
//        {
//            HashSizeValue = 128;
//        }

//        new public static Murmur3a Create()
//        {
//            return new Murmur3a();
//        }
//        #endregion
//    }

    public class Murmur3a : HashAlgorithm
    {
        // 128 bit output, 64 bit platform version

        public static ulong READ_SIZE = 16;
        private static ulong C1 = 0x87c37b91114253d5L;
        private static ulong C2 = 0x4cf5ad432745937fL;

        private ulong length;
        private uint seed; // if want to start with a seed, create a constructor
        ulong h1;
        ulong h2;

        private void MixBody(ulong k1, ulong k2)
        {
            h1 ^= MixKey1(k1);

            h1 = h1.RotateLeft(27);
            h1 += h2;
            h1 = h1 * 5 + 0x52dce729;

            h2 ^= MixKey2(k2);

            h2 = h2.RotateLeft(31);
            h2 += h1;
            h2 = h2 * 5 + 0x38495ab5;
        }

        private static ulong MixKey1(ulong k1)
        {
            k1 *= C1;
            k1 = k1.RotateLeft(31);
            k1 *= C2;
            return k1;
        }

        private static ulong MixKey2(ulong k2)
        {
            k2 *= C2;
            k2 = k2.RotateLeft(33);
            k2 *= C1;
            return k2;
        }

        private static ulong MixFinal(ulong k)
        {
            // avalanche bits

            k ^= k >> 33;
            k *= 0xff51afd7ed558ccdL;
            k ^= k >> 33;
            k *= 0xc4ceb9fe1a85ec53L;
            k ^= k >> 33;
            return k;
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

        protected override void HashCore(byte[] buffer, int ibStart, int cbSize)
        {
            h1 = seed;
            this.length = 0L;

            int pos = ibStart;
            ulong remaining = (ulong)(cbSize - ibStart);

            // read 128 bits, 16 bytes, 2 longs in eacy cycle
            while (remaining >= READ_SIZE)
            {
                ulong k1 = buffer.GetUInt64(pos);
                pos += 8;

                ulong k2 = buffer.GetUInt64(pos);
                pos += 8;

                length += READ_SIZE;
                remaining -= READ_SIZE;

                MixBody(k1, k2);
            }

            // if the input MOD 16 != 0
            if (remaining > 0)
                ProcessBytesRemaining(buffer, remaining, pos);
        }

        private void ProcessBytesRemaining(byte[] bb, ulong remaining, int pos)
        {
            ulong k1 = 0;
            ulong k2 = 0;
            length += remaining;

            // little endian (x86) processing
            switch (remaining)
            {
                case 15:
                    k2 ^= (ulong)bb[pos + 14] << 48; // fall through
                    goto case 14;
                case 14:
                    k2 ^= (ulong)bb[pos + 13] << 40; // fall through
                    goto case 13;
                case 13:
                    k2 ^= (ulong)bb[pos + 12] << 32; // fall through
                    goto case 12;
                case 12:
                    k2 ^= (ulong)bb[pos + 11] << 24; // fall through
                    goto case 11;
                case 11:
                    k2 ^= (ulong)bb[pos + 10] << 16; // fall through
                    goto case 10;
                case 10:
                    k2 ^= (ulong)bb[pos + 9] << 8; // fall through
                    goto case 9;
                case 9:
                    k2 ^= (ulong)bb[pos + 8]; // fall through
                    goto case 8;
                case 8:
                    k1 ^= bb.GetUInt64(pos);
                    break;
                case 7:
                    k1 ^= (ulong)bb[pos + 6] << 48; // fall through
                    goto case 6;
                case 6:
                    k1 ^= (ulong)bb[pos + 5] << 40; // fall through
                    goto case 5;
                case 5:
                    k1 ^= (ulong)bb[pos + 4] << 32; // fall through
                    goto case 4;
                case 4:
                    k1 ^= (ulong)bb[pos + 3] << 24; // fall through
                    goto case 3;
                case 3:
                    k1 ^= (ulong)bb[pos + 2] << 16; // fall through
                    goto case 2;
                case 2:
                    k1 ^= (ulong)bb[pos + 1] << 8; // fall through
                    goto case 1;
                case 1:
                    k1 ^= (ulong)bb[pos]; // fall through
                    break;
                default:
                    throw new Exception("Something went wrong with remaining bytes calculation.");
            }

            h1 ^= MixKey1(k1);
            h2 ^= MixKey2(k2);
        }

        protected override byte[] HashFinal()
        {
            h1 ^= length;
            h2 ^= length;

            h1 += h2;
            h2 += h1;

            h1 = Murmur3a.MixFinal(h1);
            h2 = Murmur3a.MixFinal(h2);

            h1 += h2;
            h2 += h1;

            var hash = new byte[Murmur3a.READ_SIZE];

            Array.Copy(BitConverter.GetBytes(h1), 0, hash, 0, 8);
            Array.Copy(BitConverter.GetBytes(h2), 0, hash, 8, 8);

            return hash;
        }

        public override void Initialize()
        {
            this.seed = 0;
        }

        new public static Murmur3a Create()
        {
            return new Murmur3a();
        }
    }
}
