using System;

#if SQL2005

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute
    {
        public ExtensionAttribute() { }
    }
}

#endif

namespace Martin.SQLServer.Dts
{
    public static class IntHelpers
    {
        public static ulong RotateLeft(this ulong original, int bits)
        {
            return (original << bits) | (original >> (64 - bits));
        }

        public static ulong RotateRight(this ulong original, int bits)
        {
            return (original >> bits) | (original << (64 - bits));
        }

        // Avoid the unsafe code as I'm not sure that's good in a SQL SSIS component..
        //unsafe public static ulong GetUInt64(this byte[] bb, int pos)
        //{
        //    // we only read aligned longs, so a simple casting is enough
        //    fixed (byte* pbyte = &bb[pos])
        //    {
        //        return *((ulong*)pbyte);
        //    }
        //}
        public static ulong GetUInt64(this byte[] bb, int pos)
        {
            return BitConverter.ToUInt64(bb, pos);
        }
    }
}
