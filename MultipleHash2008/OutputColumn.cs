// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="OutputColumn.cs" company="NA">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Security.Cryptography;

#if SQL2014
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput100;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSBufferManager = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSBufferManager100;
#endif
#if SQL2012
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput100;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSBufferManager = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSBufferManager100;
#endif
#if SQL2008
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput100;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSBufferManager = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSBufferManager100;
#endif
#if SQL2005
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput90;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn90;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput90;
    using IDTSBufferManager = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSBufferManager90;
#endif
    #endregion

    #region OutputColumn
    /// <summary>
    /// Provides the output columns for PreExecute and ProcessInput...
    /// </summary>
    public class OutputColumn
    {
        #region Members
        /// <summary>
        /// Stores the list of input columns from SSIS.
        /// </summary>
        private List<int> inputColumnIDs;  // Change from ArrayList to Generic List in the hope that it performs better.  (It should as there is no need to box/unbox)...

        /// <summary>
        /// Stores which hash value to generate
        /// </summary>
        private MultipleHash.HashTypeEnumerator outputHashType;

        /// <summary>
        /// Stores the column id for the SSIS component
        /// </summary>
        private int outputColumnID;

        /// <summary>
        /// Stores the generated MD5 hash
        /// </summary>
        private MD5 hashMD5;

        /// <summary>
        /// Stores the generated RIPEMD160 hash
        /// </summary>
        private RIPEMD160 hashRipeMD160;

        /// <summary>
        /// Stores the generated SHA1 hash
        /// </summary>
        private SHA1 hashSHA1;

        /// <summary>
        /// Stores the generated SHA256 hash
        /// </summary>
        private SHA256 hashSHA256;

        /// <summary>
        /// Stores the generated SHA384 hash
        /// </summary>
        private SHA384 hashSHA384;

        /// <summary>
        /// Stores the generated SHA512 hash
        /// </summary>
        private SHA512 hashSHA512;

        /// <summary>
        /// Stores the generated CRC32 hash
        /// </summary>
        private CRC32 hashCRC32;

        /// <summary>
        /// Stores the generated CRC32C hash
        /// </summary>
        private CRC32C hashCRC32C;

        /// <summary>
        /// Stores the generated FNV1a 32 bit hash
        /// </summary>
        private FNV1a32 hashFNV1a32;

        /// <summary>
        /// Stores the generated FNV1a 64 bit hash
        /// </summary>
        private FNV1a64 hashFNV1a64;

        #endregion

        #region Creator
        /// <summary>
        /// Initializes a new instance of the OutputColumn class
        /// </summary>
        public OutputColumn()
        {
            this.inputColumnIDs = new List<int>();
            this.outputHashType = MultipleHash.HashTypeEnumerator.None;
            this.outputColumnID = 0;
        } 
        #endregion

    #region Count
        /// <summary>
        /// Gets the number of items in the array.
        /// </summary>
        public int Count
        {
            get
            {
                return this.inputColumnIDs.Count;
            }
        } 
    #endregion

    #region HashType
        /// <summary>
        /// Gets the type of Hash that is to be done for this output column.
        /// Set by AddColumnInformation.
        /// </summary>
        public MultipleHash.HashTypeEnumerator HashType
        {
            get
            {
                return this.outputHashType;
            }
        } 
    #endregion

    #region HashObject
        /// <summary>
        /// Gets the Hash Object to enable the creation of hash's.
        /// </summary>
        public HashAlgorithm HashObject
        {
            get
            {
                switch (this.outputHashType)
                {
                    case MultipleHash.HashTypeEnumerator.None:
                        return null;
                    case MultipleHash.HashTypeEnumerator.MD5:
                        return this.hashMD5;
                    case MultipleHash.HashTypeEnumerator.RipeMD160:
                        return this.hashRipeMD160;
                    case MultipleHash.HashTypeEnumerator.SHA1:
                        return this.hashSHA1;
                    case MultipleHash.HashTypeEnumerator.SHA256:
                        return this.hashSHA256;
                    case MultipleHash.HashTypeEnumerator.SHA384:
                        return this.hashSHA384;
                    case MultipleHash.HashTypeEnumerator.SHA512:
                        return this.hashSHA512;
                    case MultipleHash.HashTypeEnumerator.CRC32:
                        return this.hashCRC32;
                    case MultipleHash.HashTypeEnumerator.CRC32C:
                        return this.hashCRC32C;
                    case MultipleHash.HashTypeEnumerator.FNV1a32:
                        return this.hashFNV1a32;
                    case MultipleHash.HashTypeEnumerator.FNV1a64:
                        return this.hashFNV1a64;
                    default:
                        return null;
                }
            }
        } 
    #endregion

    #region OutputColumnId
        /// <summary>
        /// Gets the ID for the OutputColumn that this is associated with.
        /// Set by AddColumnInformation.
        /// </summary>
        public int OutputColumnId
        {
            get
            {
                return this.outputColumnID;
            }
        } 
    #endregion

    #region BaseIterator
    /// <summary>
    /// Gets or Sets an array item with an integer Column Index.
    /// </summary>
    /// <param name="index">Index within the array to return</param>
    /// <returns>Requested input column index</returns>
    public int this[int index]
    {
        get
        {
            return this.inputColumnIDs[index];
        }

        set
        {
            this.inputColumnIDs[index] = value;
        }
    }

    #endregion

    #region Add
    /// <summary>
    /// Adds a new column index into the array
    /// </summary>
    /// <param name="inputColumnId">The input column id to add</param>
    /// <returns>The index that the value was added at</returns>
    public int Add(int inputColumnId)
    {
        this.inputColumnIDs.Add(inputColumnId);
        return this.inputColumnIDs.LastIndexOf(inputColumnId);  // Return the position in the list.  Get the last time it's in the list, just in case of duplicates.
    }
    #endregion

    #region AddColumnInformation
        /// <summary>
        /// Populates the OutputColumn class with all the required information to generate Hash's.
        /// </summary>
        /// <param name="bufferManager">The buffermanager that is affected by this</param>
        /// <param name="output">The output to find the column in</param>
        /// <param name="input">The input where all the data comes from</param>
        /// <param name="outputColumnIndex">The Column Index of the output column.</param>
        public void AddColumnInformation(IDTSBufferManager bufferManager, IDTSOutput output, IDTSInput input, int outputColumnIndex)
        {
            if (bufferManager == null)
            {
                throw new ArgumentNullException("bufferManager");
            }

            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            IDTSOutputColumn outputColumn = output.OutputColumnCollection[outputColumnIndex];
            string[] inputLineageIDList;
            if (outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName)
            {
                inputLineageIDList = outputColumn.CustomPropertyCollection[1].Value.ToString().Split(',');
                this.outputHashType = (MultipleHash.HashTypeEnumerator)outputColumn.CustomPropertyCollection[0].Value;
            }
            else
            {
                inputLineageIDList = outputColumn.CustomPropertyCollection[0].Value.ToString().Split(',');
                this.outputHashType = (MultipleHash.HashTypeEnumerator)outputColumn.CustomPropertyCollection[1].Value;
            }

            switch (this.outputHashType)
            {
                case MultipleHash.HashTypeEnumerator.None:
                    break;
                case MultipleHash.HashTypeEnumerator.MD5:
                    this.hashMD5 = MD5.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.RipeMD160:
                    this.hashRipeMD160 = RIPEMD160.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.SHA1:
                    this.hashSHA1 = SHA1.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.SHA256:
                    this.hashSHA256 = SHA256.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.SHA384:
                    this.hashSHA384 = SHA384.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.SHA512:
                    this.hashSHA512 = SHA512.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.CRC32:
                    this.hashCRC32 = CRC32.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.CRC32C:
                    this.hashCRC32C = CRC32C.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.FNV1a32:
                    this.hashFNV1a32 = FNV1a32.Create();
                    break;
                case MultipleHash.HashTypeEnumerator.FNV1a64:
                    this.hashFNV1a64 = FNV1a64.Create();
                    break;
                default:
                    break;
            }

            // Store all the input column ID's for the column's to be Hashed
            int inputColumnLineageID;
            foreach (string inputLineageID in inputLineageIDList)
            {
                inputColumnLineageID = bufferManager.FindColumnByLineageID(input.Buffer, System.Convert.ToInt32(inputLineageID.Substring(1)));  // Strip the # from the ID
                this.inputColumnIDs.Add(inputColumnLineageID);
            }

            // Store the Column ID for the Output Column
            this.outputColumnID = bufferManager.FindColumnByLineageID(input.Buffer, outputColumn.LineageID);
        } 
    #endregion
    }
 
#endregion
}
