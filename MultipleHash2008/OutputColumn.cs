// Multiple Hash SSIS Data Flow Transformation Component
//
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

#region Usings
using System;
using System.Collections;
using System.Security.Cryptography;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
#endregion

namespace Martin.SQLServer.Dts
{

#region OutputColumn
	/// <summary>
    /// Provides the output columns for PreExecute and ProcessInput...
    /// </summary>
    class OutputColumn
    {
    #region Members
		private ArrayList inputColumnIDs;
        private MultipleHash.HashTypeEnum outputHashType;
        private int outputColumnID;
        private MD5 hashMD5;
        private RIPEMD160 hashRipeMD160;
        private SHA1 hashSHA1;
        private SHA256 hashSHA256;
        private SHA384 hashSHA384;
        private SHA512 hashSHA512; 
	#endregion

    #region Creator
        /// <summary>
        /// Public Creator
        /// </summary>
        public OutputColumn()
        {
            inputColumnIDs = new ArrayList();
            outputHashType = MultipleHash.HashTypeEnum.None;
            outputColumnID = 0;
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
                return (int)inputColumnIDs[index];
            }
            set
            {
                inputColumnIDs[index] = value;
            }
        }
 
	#endregion

    #region Add
		/// <summary>
        /// Adds a new column index into the array
        /// </summary>
        /// <param name="inputColumnID">The input column id to add</param>
        /// <returns>The index that the value was added at</returns>
        public int Add(int inputColumnID)
        {
            return inputColumnIDs.Add(inputColumnID);
        } 
	#endregion

    #region Count
		/// <summary>
        /// Returns the number of items in the array.
        /// </summary>
        public int Count
        {
            get
            {
                return inputColumnIDs.Count;
            }
        } 
	#endregion

    #region HashType
		/// <summary>
        /// Returns the type of Hash that is to be done for this output column.
        /// Set by AddColumnInformation.
        /// </summary>

        public MultipleHash.HashTypeEnum HashType
        {
            get
            {
                return outputHashType;
            }
        } 
	#endregion

    #region HashObject
		/// <summary>
        /// Returns the Hash Object to enable the creation of hash's.
        /// </summary>
        public HashAlgorithm HashObject
        {
            get
            {
                switch (outputHashType)
                {
                    case MultipleHash.HashTypeEnum.None:
                        return null;
                    case MultipleHash.HashTypeEnum.MD5:
                        return hashMD5;
                    case MultipleHash.HashTypeEnum.RipeMD160:
                        return hashRipeMD160;
                    case MultipleHash.HashTypeEnum.SHA1:
                        return hashSHA1;
                    case MultipleHash.HashTypeEnum.SHA256:
                        return hashSHA256;
                    case MultipleHash.HashTypeEnum.SHA384:
                        return hashSHA384;
                    case MultipleHash.HashTypeEnum.SHA512:
                        return hashSHA512;
                    default:
                        return null;
                }
            }
        } 
	#endregion

    #region OutputColumnID
		/// <summary>
        /// Returns the ID for the OutputColumn that this is associated with.
        /// Set by AddColumnInformation.
        /// </summary>
        public int OutputColumnID
        {
            get
            {
                return outputColumnID;
            }
        } 
	#endregion

    #region AddColumnInformation
		/// <summary>
        /// Populates the OutputColumn class with all the required information to generate Hash's.
        /// </summary>
        /// <param name="bufferManager"></param>
        /// <param name="output">The output to find the column in</param>
        /// <param name="input">The input where all the data comes from</param>
        /// <param name="outputColumnIndex">The Column Index of the output column.</param>
        public void AddColumnInformation(IDTSBufferManager100 bufferManager, IDTSOutput100 output, IDTSInput100 input, int outputColumnIndex)
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

            IDTSOutputColumn100 outputColumn = output.OutputColumnCollection[outputColumnIndex];
            string[] inputLineageIDList;
            if (outputColumn.CustomPropertyCollection[0].Name == "HashType")
            {
                inputLineageIDList = outputColumn.CustomPropertyCollection[1].Value.ToString().Split(',');
                outputHashType = (MultipleHash.HashTypeEnum)outputColumn.CustomPropertyCollection[0].Value;
            }
            else
            {
                inputLineageIDList = outputColumn.CustomPropertyCollection[0].Value.ToString().Split(',');
                outputHashType = (MultipleHash.HashTypeEnum)outputColumn.CustomPropertyCollection[1].Value;
            }
            switch (outputHashType)
            {
                case MultipleHash.HashTypeEnum.None:
                    break;
                case MultipleHash.HashTypeEnum.MD5:
                    hashMD5 = MD5.Create();
                    break;
                case MultipleHash.HashTypeEnum.RipeMD160:
                    hashRipeMD160 = RIPEMD160.Create();
                    break;
                case MultipleHash.HashTypeEnum.SHA1:
                    hashSHA1 = SHA1.Create();
                    break;
                case MultipleHash.HashTypeEnum.SHA256:
                    hashSHA256 = SHA256.Create();
                    break;
                case MultipleHash.HashTypeEnum.SHA384:
                    hashSHA384 = SHA384.Create();
                    break;
                case MultipleHash.HashTypeEnum.SHA512:
                    hashSHA512 = SHA512.Create();
                    break;
                default:
                    break;
            }

            // Store all the input column ID's for the column's to be Hashed
            int inputColumnLineageID;
            foreach (string inputLineageID in inputLineageIDList)
            {
                inputColumnLineageID = bufferManager.FindColumnByLineageID(input.Buffer, System.Convert.ToInt32(inputLineageID));
                inputColumnIDs.Add(inputColumnLineageID);
            }

            // Store the Column ID for the Output Column
            outputColumnID = bufferManager.FindColumnByLineageID(input.Buffer,outputColumn.LineageID);
        } 
	#endregion
    }
 
#endregion
}
