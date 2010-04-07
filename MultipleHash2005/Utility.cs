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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime.Wrapper;
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
     * The relevant code is in the Types to Byte Arrays and Byte Arrat Appending regions.
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
        #endregion

        /// <summary>
        /// Prevents a default instance of the Utility class from being created.
        /// </summary>
        private Utility()
        {
        }

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
        /// Gets the name of the thread
        /// </summary>
        public static string MultipleThreadPropName 
        { 
            get 
            { 
                return ConstMultipleThreadPropName; 
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
////                    stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
                }
            }
        }

        /// <summary>
        /// Converts from DateTime to a byte array.
        /// </summary>
        /// <param name="value">input value to convert to byte array</param>
        /// <returns>byte array</returns>
        public static byte[] ToArray(DateTime value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(value.ToString("u"));
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
                    byte[] bytes = stream.ToArray();
                    stream.Close();
                    ////stream.Dispose();
                    return bytes;
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
        public static void Append(ref byte[] array, bool value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append DateTime To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, DateTime value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Time To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, TimeSpan value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Guid To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, Guid value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append UInt64 To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ulong value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Single To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, float value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Byte To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, byte value)
        {
            System.Array.Resize<byte>(ref array, array.Length + 1);
            array[array.Length - 1] = value;
        }

        /// <summary>
        /// Append Bytes To End Of Byte Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, byte[] value)
        {
            System.Array.Resize<byte>(ref array, array.Length + value.Length);
            System.Array.Copy(value, 0, array, array.Length - value.Length, value.Length);
        }

        /// <summary>
        /// Append SByte Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, sbyte value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Short Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, short value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append UShort Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, ushort value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Integer Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, int value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Long Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, long value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append UInt Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, uint value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Double Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, double value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Decimal Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        public static void Append(ref byte[] array, decimal value)
        {
            Utility.Append(ref array, Utility.ToArray(value));
        }

        /// <summary>
        /// Append Char Value Bytes To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        /// <param name="encoding">The encoding of the data</param>
        public static void Append(ref byte[] array, char value, Encoding encoding)
        {
            Utility.Append(ref array, encoding.GetBytes(new char[] { value }));
        }

        /// <summary>
        /// Append String Bytes From Encoding To Array
        /// </summary>
        /// <param name="array">Original Value</param>
        /// <param name="value">Value To Append</param>
        /// <param name="encoding">Encoding To Use</param>
        public static void Append(ref byte[] array, string value, System.Text.Encoding encoding)
        {
            Utility.Append(ref array, encoding.GetBytes(value));
        }
        #endregion

        #region SetOutputColumnDataType
        /// <summary>
        /// Configures the output column to the correct data type and length.
        /// </summary>
        /// <param name="propertyValue">The type of output that is to be produced</param>
        /// <param name="outputColumn">The column to configure</param>
        public static void SetOutputColumnDataType(MultipleHash.HashTypeEnumerator propertyValue, IDTSOutputColumn90 outputColumn)
        {
            switch (propertyValue)
            {
                case MultipleHash.HashTypeEnumerator.None:
                case MultipleHash.HashTypeEnumerator.MD5:
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
                default:
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
            int processorMask = System.Diagnostics.Process.GetCurrentProcess().ProcessorAffinity.ToInt32();
            int numProcessors = (int)Math.Log(processorMask, 2) + 1;
            return Math.Max(1, numProcessors);
        }
        #endregion
    }
}
