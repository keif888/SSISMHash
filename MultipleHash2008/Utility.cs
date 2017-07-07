// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="Utility.cs" company="NA">
//     Copyright (c) Keith Martin. All rights reserved.
// </copyright>
// Created by Keith Martin
//
// This software is licensed under the Microsoft Reciprocal License (Ms-RL)
/*
 * This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
 *
 *1. Definitions
 *
 *The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
 *
 *A "contribution" is the original software, or any additions or changes to the software.
 *
 *A "contributor" is any person that distributes its contribution under this license.
 *
 *"Licensed patents" are a contributor's patent claims that read directly on its contribution.
 *
 *2. Grant of Rights
 *
 *(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
 *
 *(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
 *
 *3. Conditions and Limitations
 *
 *(A) Reciprocal Grants- For any file you distribute that contains code from the software (in source code or binary format), you must provide recipients the source code to that file along with a copy of this license, which license will govern that file. You may license other files that are entirely your own work and do not contain code from the software under any terms you choose.
 *
 *(B) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
 *
 *(C) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
 *
 *(D) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
 *
 *(E) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
 *
 *(F) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement. 
 * 
 */

namespace Martin.SQLServer.Dts
{
    #region Usings
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.SqlServer.Dts.Runtime.Wrapper;
    using Microsoft.SqlServer.Dts.Pipeline;
#if SQL2016
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
#endif
#if SQL2014
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
#endif
#if SQL2012
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
#endif
#if SQL2008
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
#endif
    #endregion
    /// <summary>
    /// The purpose of the Utility class is to provide a single location for routines that are
    /// shared between the Runtime and Design time and UI parts of this component.
    /// The Utility Class provides data type conversion to Byte[]
    /// and the function to control the data type of the output columns.
    /// </summary>
    /*

     * Part's of this class were taken from the following project:

    // Kimball Method Slowly Changing Dimension SSIS Dataflow Transform Component
    //
    // Created by Todd McDermid
    //
    // This component is intended as a replacement to the built-in "SCD Wizard" pseudo-component
    // included in SQL Server Integration Services.
    // Despite the name of this component, no affiliation exists between the author and
    // Ralph Kimball or the Kimball Group.  This component is NOT endorsed or supported by them.
    // It merely follows the Kimball Method of data warehousing, and encapsulates it into one component.
    //
    // Writing SSIS custom components is not easy - and that is a SHAME.  The "samples" included in SSIS 
    // assisted somewhat, but without the information provided by multiple blog entries, and particularly 
    // Alberto Ferrari's TableDifference component (http://sqlbi.eu), it would have been impossible.  Thanks
    // also to blog and forum posts by Jamie Thomson, John Welch, Kirk Haselden, Phil Brammer, and Darren Green.
    //
    // The source code and component are hosted on CodePlex in the "kimballscd" project (www.codeplex.com/kimballscd).  
    // Source and binaries are protected by the Microsoft Reciprocal License - which basically means you can do what
    // you like with the source and binaries as long as you let everyone know your work builds on top of open source,
    // and include a complete copy of this and your source in anything you distribute.  Oh yeah, and you can't sue
    // me if the code doesn't work right.  :)

     * That code has been modified to include other data types.
     * 
     * The relevant code is in the Types to Byte Arrays and Byte Array Appending regions.
     * 
    */

    public sealed class Utility
    {
        #region Property Name Constants
        //// These are the constants for the Custom Properties used in this component.

        /// <summary>
        /// Stores the hash property name
        /// </summary>
        private const string ConstHashTypePropName = "HashType";

        /// <summary>
        /// Stores the input column's lineage id
        /// </summary>
        private const string ConstInputColumnLineagePropName = "InputColumnLineageIDs";

        /// <summary>
        /// Stores the thread name
        /// </summary>
        private const string ConstMultipleThreadPropName = "MultipleThreads";

        /// <summary>
        /// This is the name of the SSIS Property that holds the Safe Null Handling details.
        /// </summary>
        private const string ConstSafeNullHandlingPropName = "SafeNullHandling";

        /// <summary>
        /// This is the name of the SSIS Propery that holds the Millisecond handling details.
        /// </summary>
        private const string ConstMillsecondPropName = "IncludeMillsecond";

        /// <summary>
        /// This is the name of the SSIS property that holds the output type details.
        /// </summary>
        private const string ConstOutputTypePropName = "HashOutputType";
        #endregion

        /// <summary>
        /// Prevents a default instance of the Utility class from being created.
        /// </summary>
        //private Utility()
        //{
        //}

        /// <summary>
        /// Gets the hash property name
        /// </summary>
        public static string HashTypePropName 
        { 
            get 
            { 
                return ConstHashTypePropName; 
            } 
        }

        /// <summary>
        /// Gets the lineage property name
        /// </summary>
        public static string InputColumnLineagePropName 
        { 
            get 
            { 
                return ConstInputColumnLineagePropName; 
            } 
        }

        /// <summary>
        /// Gets the output columns output type property name
        /// </summary>
        public static string OutputColumnOutputTypePropName
        {
            get
            {
                return ConstOutputTypePropName;
            }
        }

        /// <summary>
        /// Gets the name of the thread
        /// </summary>
        public static string MultipleThreadPropName 
        { 
            get 
            { 
                return ConstMultipleThreadPropName; 
            } 
        }

        /// <summary>
        /// Gets the name of the Safe Null Handling Property.
        /// </summary>
        public static string SafeNullHandlingPropName
        {
            get
            {
                return ConstSafeNullHandlingPropName;
            }
        }


        public static string HandleMillisecondPropName
        {
            get
            {
                return ConstMillsecondPropName;
            }
        }

        #region Types to Byte Arrays
        /// <summary>
        /// Converts from bool to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(bool value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from decimal to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(decimal value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from DateTimeOffset to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        private static byte[] ToArray(DateTimeOffset value, Boolean millisecondHandling)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    if (millisecondHandling)
                    {
                        writer.Write(value.ToString("yyyy-MM-dd HH:mm:ss.fffffff zzz")); // Issue 10534 Fix (Change from u to yyyy-MM-dd HH:mm:ss.fffffff zzz).
                    }
                    else
                    {
                        writer.Write(value.ToString("u"));
                    }
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from DateTime to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(DateTime value, Boolean millisecondHandling)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    if (millisecondHandling)  // Keep the "old" format prior to 10534 if no milliseconds.  (Date types).
                    {
                        //writer.Write(String.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}.{6:D7}", value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second, value.Millisecond));  // This isn't faster :-(
                        // The following call is the slowest in the hashing function.  But the one above is slower.
                        writer.Write(value.ToString("yyyy-MM-dd HH:mm:ss.fffffff"));   // Issue 10534 Fix (Change from u to yyyy-MM-dd HH:mm:ss.fffffff).
                    }
                    else
                    {
                        writer.Write(value.ToString("u"));
                    }
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from TimeSpan to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(TimeSpan value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value.ToString());
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from byte to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(byte value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from Guid to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(Guid value)
        {
            return value.ToByteArray();
        }

        /// <summary>
        /// Converts from int16 to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(short value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from Int32 to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(int value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from Int64 to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(long value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from Single to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(float value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from Double to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(double value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from UInt16 to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(ushort value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from UInt32 to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(uint value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from UInt64 to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(ulong value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        /// <summary>
        /// Converts from sbyte to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(sbyte value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value);
                    return stream.ToArray();
                }
            }
        }

        #endregion

        #region Byte Array Appending

        /// <summary>
        /// Append bool To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, bool value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append DateTimeOffset To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, DateTimeOffset value, Boolean millisecondHandling)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value, millisecondHandling));
        }

        /// <summary>
        /// Append DateTime To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, DateTime value, Boolean millisecondHandling)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value, millisecondHandling));
        }

        /// <summary>
        /// Append Time To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, TimeSpan value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Guid To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, Guid value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append UInt64 To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, ulong value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Single To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, float value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Byte To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, byte value)
        {
            if (bufferUsed + 1 >= array.Length)
            {
                System.Array.Resize<byte>(ref array, array.Length + 1000);
            }

            array[bufferUsed++] = value;
        }

        /// <summary>
        /// Append Bytes To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, byte[] value)
        {
            int valueLength = value.Length;
            int arrayLength = array.Length;

            if (bufferUsed + valueLength >= arrayLength)
            {
                if (valueLength > 1000)
                {
                    System.Array.Resize<byte>(ref array, arrayLength + valueLength + 1000); 
                }
                else
                {
                    System.Array.Resize<byte>(ref array, arrayLength + 1000);
                }
            }

            System.Array.Copy(value, 0, array, bufferUsed, valueLength);
            bufferUsed += valueLength;
        }

        /// <summary>
        /// Append SByte Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, sbyte value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Short Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, short value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append UShort Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, ushort value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Integer Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, int value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Long Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, long value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append UInt Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, uint value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Double Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, double value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Decimal Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, decimal value)
        {
            Utility.Append(ref array, ref bufferUsed, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Char Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        /// <param name="encoding">The encoding of the data</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, char value, Encoding encoding)
        {
            Utility.Append(ref array, ref bufferUsed, encoding.GetBytes(new char[] { value }));
        }

        /// <summary>
        /// Append String Bytes From Encoding To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        /// <param name="encoding">Encoding To Use</param>
        public static void Append(ref byte[] array, ref Int32 bufferUsed, string value, System.Text.Encoding encoding)
        {
            Utility.Append(ref array, ref bufferUsed, encoding.GetBytes(value));
        }
        #endregion

        #region SetOutputColumnDataType
        /// <summary>
        /// Configures the output column to the correct data type and length.
        /// </summary>
        /// <param name="propertyValue">The type of output that is to be produced</param>
        /// <param name="outputColumn">The column to configure</param>
        public static void SetOutputColumnDataType(MultipleHash.HashTypeEnumerator propertyValue, MultipleHash.OutputTypeEnumerator dataTypeValue, IDTSOutputColumn outputColumn)
        {
            switch (dataTypeValue)
            {
                case MultipleHash.OutputTypeEnumerator.HexString:
                    switch (propertyValue)
                    {
                        case MultipleHash.HashTypeEnumerator.None:
                        case MultipleHash.HashTypeEnumerator.MD5:
                        case MultipleHash.HashTypeEnumerator.MurmurHash3a:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 34, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.RipeMD160:
                        case MultipleHash.HashTypeEnumerator.SHA1:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 42, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA256:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 66, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA384:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 98, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA512:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 130, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.CRC32:
                        case MultipleHash.HashTypeEnumerator.CRC32C:
                        case MultipleHash.HashTypeEnumerator.FNV1a32:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 10, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.FNV1a64:
                        case MultipleHash.HashTypeEnumerator.xxHash:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 18, 0, 0, 1252);
                            break;
                    }
                    break;
                case MultipleHash.OutputTypeEnumerator.Base64String:
                    switch (propertyValue)
                    {
                        case MultipleHash.HashTypeEnumerator.None:
                        case MultipleHash.HashTypeEnumerator.MD5:
                        case MultipleHash.HashTypeEnumerator.MurmurHash3a:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 24, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.RipeMD160:
                        case MultipleHash.HashTypeEnumerator.SHA1:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 28, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA256:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 44, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA384:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 64, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA512:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 88, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.CRC32:
                        case MultipleHash.HashTypeEnumerator.CRC32C:
                        case MultipleHash.HashTypeEnumerator.FNV1a32:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 8, 0, 0, 1252);
                            break;
                        case MultipleHash.HashTypeEnumerator.FNV1a64:
                        case MultipleHash.HashTypeEnumerator.xxHash:
                            outputColumn.SetDataTypeProperties(DataType.DT_STR, 12, 0, 0, 1252);
                            break;
                    }
                    break;
                case MultipleHash.OutputTypeEnumerator.Binary:
                default:
                    switch (propertyValue)
                    {
                        case MultipleHash.HashTypeEnumerator.None:
                        case MultipleHash.HashTypeEnumerator.MD5:
                        case MultipleHash.HashTypeEnumerator.MurmurHash3a:
                            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 16, 0, 0, 0);
                            break;
                        case MultipleHash.HashTypeEnumerator.RipeMD160:
                        case MultipleHash.HashTypeEnumerator.SHA1:
                            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 20, 0, 0, 0);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA256:
                            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 32, 0, 0, 0);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA384:
                            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 48, 0, 0, 0);
                            break;
                        case MultipleHash.HashTypeEnumerator.SHA512:
                            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 64, 0, 0, 0);
                            break;
                        case MultipleHash.HashTypeEnumerator.CRC32:
                        case MultipleHash.HashTypeEnumerator.CRC32C:
                        case MultipleHash.HashTypeEnumerator.FNV1a32:
                            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 4, 0, 0, 0);
                            break;
                        case MultipleHash.HashTypeEnumerator.FNV1a64:
                        case MultipleHash.HashTypeEnumerator.xxHash:
                            outputColumn.SetDataTypeProperties(DataType.DT_BYTES, 8, 0, 0, 0);
                            break;
                    }
                    break;
            }
        }

        #endregion

        #region System Information
        /// <summary>
        /// Function to get the number of processor cores
        /// </summary>
        /// <returns>The number of cores</returns>
        public static int GetNumberOfProcessorCores()
        {
            try
            {
                Int64 processorMask = System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity.ToInt64();
                int numProcessors = (int)Math.Log(processorMask, 2) + 1;
                return Math.Max(1, numProcessors);
            }
            catch
            {
                return 1;
            }
        }
        #endregion

        #region CalculateHash
        /// <summary>
        /// This creates the hash value from a thread
        /// </summary>
        /// <param name="state">this is the thread state object that is passed</param>
        public static void CalculateHash(OutputColumn columnToProcess, PipelineBuffer buffer, bool safeNullHandling, bool millisecondHandling)
        {
            byte[] inputByteBuffer = new byte[1000];
            Int32 bufferUsed = 0;
            string nullHandling = String.Empty;
            uint blobLength = 0;
            Int32 columnToProcessID = 0;
            DataType columnDataType = DataType.DT_NULL;

            // Step through each input column for that output column
            for (int j = 0; j < columnToProcess.Count; j++)
            {
                columnToProcessID = columnToProcess[j].ColumnId;  // Only call this once, as it appears to be "slow".
                columnDataType = columnToProcess[j].ColumnDataType;
                // Skip NULL values, as they "don't" exist...
                if (!buffer.IsNull(columnToProcessID))
                {
                    nullHandling += "N";
                    switch (columnDataType) //buffer.GetColumnInfo(columnToProcessID).DataType)
                    {
                        case DataType.DT_BOOL:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetBoolean(columnToProcessID));
                            break;
                        case DataType.DT_IMAGE:
                            blobLength = buffer.GetBlobLength(columnToProcessID);
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetBlobData(columnToProcessID, 0, (int)blobLength));
                            nullHandling += blobLength.ToString();
                            break;
                        case DataType.DT_BYTES:
                            byte[] bytesFromBuffer = buffer.GetBytes(columnToProcessID);
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, bytesFromBuffer);
                            nullHandling += bytesFromBuffer.GetLength(0).ToString();
                            break;
                        case DataType.DT_CY:
                        case DataType.DT_DECIMAL:
                        case DataType.DT_NUMERIC:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetDecimal(columnToProcessID));
                            break;
                        case DataType.DT_DBTIMESTAMPOFFSET:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetDateTimeOffset(columnToProcessID), millisecondHandling);
                            break;
                        case DataType.DT_DBDATE:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetDate(columnToProcessID), millisecondHandling);
                            break;
                        case DataType.DT_DATE:
                        case DataType.DT_DBTIMESTAMP:
                        case DataType.DT_DBTIMESTAMP2:
                        case DataType.DT_FILETIME:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetDateTime(columnToProcessID), millisecondHandling);
                            break;
                        case DataType.DT_DBTIME:
                        case DataType.DT_DBTIME2:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetTime(columnToProcessID));
                            break;
                        case DataType.DT_GUID:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetGuid(columnToProcessID));
                            break;
                        case DataType.DT_I1:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetSByte(columnToProcessID));
                            break;
                        case DataType.DT_I2:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetInt16(columnToProcessID));
                            break;
                        case DataType.DT_I4:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetInt32(columnToProcessID));
                            break;
                        case DataType.DT_I8:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetInt64(columnToProcessID));
                            break;
                        case DataType.DT_NTEXT:
                        case DataType.DT_STR:
                        case DataType.DT_TEXT:
                        case DataType.DT_WSTR:
                            String stringFromBuffer = buffer.GetString(columnToProcessID);
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, stringFromBuffer, Encoding.UTF8);
                            nullHandling += stringFromBuffer.Length.ToString();
                            break;
                        case DataType.DT_R4:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetSingle(columnToProcessID));
                            break;
                        case DataType.DT_R8:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetDouble(columnToProcessID));
                            break;
                        case DataType.DT_UI1:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetByte(columnToProcessID));
                            break;
                        case DataType.DT_UI2:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetUInt16(columnToProcessID));
                            break;
                        case DataType.DT_UI4:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetUInt32(columnToProcessID));
                            break;
                        case DataType.DT_UI8:
                            Utility.Append(ref inputByteBuffer, ref bufferUsed, buffer.GetUInt64(columnToProcessID));
                            break;
                        case DataType.DT_EMPTY:
                        case DataType.DT_NULL:
                        default:
                            break;
                    }
                }
                else
                {
                    nullHandling += "Y";
                }
            }

            if (safeNullHandling)
            {
                Utility.Append(ref inputByteBuffer, ref bufferUsed, nullHandling, Encoding.UTF8);
            }

            // Ok, we have all the data in a Byte Buffer
            // So now generate the Hash
            byte[] hash;
            switch (columnToProcess.HashType)
            {
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.None:
                    hash = new byte[1];
                    break;
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.MD5:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.RipeMD160:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.SHA1:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.SHA256:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.SHA384:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.SHA512:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.CRC32:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.CRC32C:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.FNV1a32:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.FNV1a64:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.MurmurHash3a:
                case Martin.SQLServer.Dts.MultipleHash.HashTypeEnumerator.xxHash:
                    hash = columnToProcess.HashObject.ComputeHash(inputByteBuffer, 0, bufferUsed);
                    break;
                default:
                    hash = new byte[1];
                    break;
            }
            switch(columnToProcess.OutputHashDataType)
            {
                case MultipleHash.OutputTypeEnumerator.Binary:
                    buffer.SetBytes(columnToProcess.OutputColumnId, hash);
                    break;
                case MultipleHash.OutputTypeEnumerator.Base64String:
                    buffer.SetString(columnToProcess.OutputColumnId, System.Convert.ToBase64String(hash, 0, hash.Length));
                    break;
                case MultipleHash.OutputTypeEnumerator.HexString:
                    buffer.SetString(columnToProcess.OutputColumnId, String.Format("0x{0}", ByteArrayToHexViaLookup32(hash)));
                    break;
            }
            
        }

        #endregion

        #region Hex String

        /// <summary>
        /// This is some stuff that I found via Google, which is supposed to be much faster than the SoapHexBinary implementation.  And speed is king here.
        /// http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
        /// http://stackoverflow.com/a/24343727/48700
        /// </summary>

        private static readonly uint[] _lookup32 = CreateLookup32();

        private static uint[] CreateLookup32()
        {
            var result = new uint[256];
            for (int i = 0; i < 256; i++)
            {
                string s = i.ToString("X2");
                result[i] = ((uint)s[0]) + ((uint)s[1] << 16);
            }
            return result;
        }

        private static string ByteArrayToHexViaLookup32(byte[] bytes)
        {
            var lookup32 = _lookup32;
            var result = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = lookup32[bytes[i]];
                result[2 * i] = (char)val;
                result[2 * i + 1] = (char)(val >> 16);
            }
            return new string(result);
        }

        #endregion
    }
}
