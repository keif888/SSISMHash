// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="ProcessOutputColumn.cs" company="NA">
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
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.SqlServer.Dts.Pipeline;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime.Wrapper;
    #endregion
    /// <summary>
    /// Class to process the data to an Output Column
    /// </summary>
    public sealed class ProcessOutputColumn
    {
        /// <summary>
        /// Prevents a default instance of the ProcessOutputColumn class from being created.
        /// </summary>
        private ProcessOutputColumn() 
        { 
        }

        /// <summary>
        /// This creates the hash value from a thread
        /// </summary>
        /// <param name="state">this is the thread state object that is passed</param>
        public static void CalculateHash(object state)
        {
#if DEBUG
            bool fireAgain = true;
#endif
            PassThreadState passThreadState = (PassThreadState)state;
            byte[] inputByteBuffer = new byte[0];
            uint blobLength = 0;

            // Step through each input column for that output column
            for (int j = 0; j < passThreadState.ColumnToProcess.Count; j++)
            {
                // Skip NULL values, as they "don't" exist...
                if (!passThreadState.Buffer.IsNull(passThreadState.ColumnToProcess[j]))
                {
#if DEBUG
                    passThreadState.MetaData.FireInformation(0, passThreadState.MetaData.Name, "Inside ProcessInput: DataType is " + passThreadState.Buffer.GetColumnInfo(passThreadState.ColumnToProcess[j]).DataType.ToString(), string.Empty, 0, ref fireAgain);
#endif
                    switch (passThreadState.Buffer.GetColumnInfo(passThreadState.ColumnToProcess[j]).DataType)
                    {
                        case DataType.DT_BOOL:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetBoolean(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_IMAGE:
                            blobLength = passThreadState.Buffer.GetBlobLength(passThreadState.ColumnToProcess[j]);
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetBlobData(passThreadState.ColumnToProcess[j], 0, (int)blobLength));
                            break;
                        case DataType.DT_BYTES:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetBytes(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_CY:
                        case DataType.DT_DECIMAL:
                        case DataType.DT_NUMERIC:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetDecimal(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_DATE:
                        case DataType.DT_DBDATE:
                        case DataType.DT_DBTIMESTAMP:
////                        case DataType.DT_DBTIMESTAMP2:
////                        case DataType.DT_DBTIMESTAMPOFFSET:
                        case DataType.DT_FILETIME:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetDateTime(passThreadState.ColumnToProcess[j]));
                            break;
////                        case DataType.DT_DBTIME:
////                        case DataType.DT_DBTIME2:
////                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetTime(passThreadState.ColumnToProcess[j]));
////                            break;
                        case DataType.DT_GUID:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetGuid(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_I1:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetSByte(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_I2:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetInt16(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_I4:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetInt32(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_I8:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetInt64(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_NTEXT:
                        case DataType.DT_STR:
                        case DataType.DT_TEXT:
                        case DataType.DT_WSTR:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetString(passThreadState.ColumnToProcess[j]), Encoding.UTF8);
                            break;
                        case DataType.DT_R4:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetSingle(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_R8:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetDouble(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_UI1:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetByte(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_UI2:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetUInt16(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_UI4:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetUInt32(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_UI8:
                            Utility.Append(ref inputByteBuffer, passThreadState.Buffer.GetUInt64(passThreadState.ColumnToProcess[j]));
                            break;
                        case DataType.DT_EMPTY:
                        case DataType.DT_NULL:
                        default:
                            break;
                    }
                }
#if DEBUG
                else
                {
                    passThreadState.MetaData.FireInformation(0, passThreadState.MetaData.Name, "Inside ProcessInput: Null Value Encountered", string.Empty, 0, ref fireAgain);
                }
#endif
            }

            // Ok, we have all the data in a Byte Buffer
            // So now generate the Hash
#if DEBUG
            passThreadState.MetaData.FireInformation(0, passThreadState.MetaData.Name, "Inside ProcessInput: Generate Hash from " + inputByteBuffer.ToString(), string.Empty, 0, ref fireAgain);
#endif
            byte[] hash;
            switch (passThreadState.ColumnToProcess.HashType)
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
                    hash = passThreadState.ColumnToProcess.HashObject.ComputeHash(inputByteBuffer);
                    break;
                default:
                    hash = new byte[1];
                    break;
            }
#if DEBUG
            passThreadState.MetaData.FireInformation(0, passThreadState.MetaData.Name, "Inside ProcessInput: Assign hash to Output", string.Empty, 0, ref fireAgain);
#endif
            passThreadState.Buffer.SetBytes(passThreadState.ColumnToProcess.OutputColumnId, hash);
            passThreadState.ThreadReset.Set();
        }
    }
}
