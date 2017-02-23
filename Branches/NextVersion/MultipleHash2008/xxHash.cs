using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

/*
 * 
xxHash Library
Copyright (c) 2012-2014, Yann Collet
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this
  list of conditions and the following disclaimer in the documentation and/or
  other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
*  You can contact the author at :
*  - xxHash homepage: http://www.xxhash.com
*  - xxHash source repository : https://github.com/Cyan4973/xxHash     * 

*/

namespace Martin.SQLServer.Dts
{
    public class xxHash : HashAlgorithm
    {
        #region Globals
        /*======   xxh64   ======*/

        private const UInt64 PRIME64_1 = 11400714785074694791UL;
        private const UInt64 PRIME64_2 = 14029467366897019727UL;
        private const UInt64 PRIME64_3 = 1609587929392839161UL;
        private const UInt64 PRIME64_4 = 9650029242287828579UL;
        private const UInt64 PRIME64_5 = 2870177450012600261UL;

        private uint seed; // if want to start with a seed, create a constructor
        private UInt64 tmpHash;
        #endregion

        #region Creators
        public xxHash()
        {
            Initialize();
        }
        #endregion

        #region Privates

        //private static UInt64 XXH_read64(const void* memPtr) 
        //{ 
        //   return *(const UInt64*) memPtr; 
        //}

        private static UInt64 XXH_rotl64(UInt64 acc, Int32 v)
        {
            return ((acc << v) | (acc >> (64 - v)));
        }

        //private static UInt64 XXH_swap64(UInt64 x)
        //{
        //    return ((x << 56) & 0xff00000000000000UL) |
        //           ((x << 40) & 0x00ff000000000000UL) |
        //           ((x << 24) & 0x0000ff0000000000UL) |
        //           ((x << 8) & 0x000000ff00000000UL) |
        //          ((x >> 8) & 0x00000000ff000000UL) |
        //         ((x >> 24) & 0x0000000000ff0000UL) |
        //         ((x >> 40) & 0x000000000000ff00UL) |
        //         ((x >> 56) & 0x00000000000000ffUL);
        //}

        private static ulong XXH64_round(UInt64 acc, UInt64 input)
        {
            acc += input * PRIME64_2;
            acc = XXH_rotl64(acc, 31);
            acc *= PRIME64_1;
            return acc;
        }

        private static ulong XXH64_mergeRound(UInt64 acc, UInt64 val)
        {
            val = XXH64_round(0, val);
            acc ^= val;
            acc = acc * PRIME64_1 + PRIME64_4;
            return acc;
        }

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
            InputByteStruct p = new InputByteStruct(array, ibStart);
            Int64 bEnd = p.Location + cbSize;

            if (cbSize >= 32)
            {
                Int64 limit = bEnd - 32;
                UInt64 v1 = seed + PRIME64_1 + PRIME64_2;
                UInt64 v2 = seed + PRIME64_2;
                UInt64 v3 = seed + 0;
                UInt64 v4 = seed - PRIME64_1;

                do
                {
                    v1 = XXH64_round(v1, p.Get64BitsAndIncrement());
                    v2 = XXH64_round(v2, p.Get64BitsAndIncrement());
                    v3 = XXH64_round(v3, p.Get64BitsAndIncrement());
                    v4 = XXH64_round(v4, p.Get64BitsAndIncrement());
                } while (p.Location <= limit);

                tmpHash = XXH_rotl64(v1, 1) + XXH_rotl64(v2, 7) + XXH_rotl64(v3, 12) + XXH_rotl64(v4, 18);
                tmpHash = XXH64_mergeRound(tmpHash, v1);
                tmpHash = XXH64_mergeRound(tmpHash, v2);
                tmpHash = XXH64_mergeRound(tmpHash, v3);
                tmpHash = XXH64_mergeRound(tmpHash, v4);

            }
            else
            {
                tmpHash = seed + PRIME64_5;
            }

            tmpHash += (UInt64)cbSize;

            while (p.Location + 8 <= bEnd)
            {
                UInt64 k1 = XXH64_round(0, p.Get64BitsAndIncrement());
                tmpHash ^= k1;
                tmpHash = XXH_rotl64(tmpHash, 27) * PRIME64_1 + PRIME64_4;
            }

            if (p.Location + 4 <= bEnd)
            {
                tmpHash ^= (UInt64)(p.Get32BitsAndIncrement()) * PRIME64_1;
                tmpHash = XXH_rotl64(tmpHash, 23) * PRIME64_2 + PRIME64_3;
            }

            while (p.Location < bEnd)
            {
                tmpHash ^= p.GetByteAndIncrement() * PRIME64_5;
                tmpHash = XXH_rotl64(tmpHash, 11) * PRIME64_1;
            }

            tmpHash ^= tmpHash >> 33;
            tmpHash *= PRIME64_2;
            tmpHash ^= tmpHash >> 29;
            tmpHash *= PRIME64_3;
            tmpHash ^= tmpHash >> 32;
        }

        protected override byte[] HashFinal()
        {
            var hash = new byte[8];
            Array.Copy(BitConverter.GetBytes(tmpHash), 0, hash, 0, 8);
            return hash;
        }
        #endregion

        #region Publics
        public override void Initialize()
        {
            HashSizeValue = 64;
            seed = 0;
        }

        new public static xxHash Create()
        {
            return new xxHash();
        }
        #endregion
    }

    internal class InputByteStruct
    {
        private byte[] array;
        private Int32 ibStart;

        public InputByteStruct(byte[] array)
        {
            this.array = array;
            this.ibStart = 0;
        }

        public InputByteStruct(byte[] array, Int32 ibStart) : this(array)
        {
            this.ibStart = ibStart;
        }

        public Int32 Location { get { return this.ibStart; } set { this.ibStart = Location; } }

        internal ulong Get32BitsAndIncrement()
        {
            ulong result = BitConverter.ToUInt32(this.array, this.ibStart);
            this.ibStart += 4;
            return result;
        }

        public ulong Get64BitsAndIncrement()
        {
            ulong result = BitConverter.ToUInt64(this.array, this.ibStart);
            this.ibStart += 8;
            return result;
        }

        public byte GetByteAndIncrement()
        {
            return this.array[this.ibStart++];
        }

    }
}
