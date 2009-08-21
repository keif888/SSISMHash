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
using System.Text;
using Microsoft.SqlServer.Dts.Pipeline;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
#endregion

namespace Martin.SQLServer.Dts
{
    /// <summary>
    /// Enables the creation of Hash's on selected input columns.
    /// The user can select the Hash type (MD5 etc.), and the columns that the hash is to be applied to.
    /// </summary>

    /// This uses the following Custom values:
    /// HashType - the type of hash.
    /// InputColumnLineageIDs - the comma separated list of LineageID's to calculate the Hash on.
    /// 

    [DtsPipelineComponent(
        DisplayName = "Multiple Hash",
        Description = "Creates Multiple Hash's from selected input columns.",
        IconResource = "Martin.SQLServer.Dts.key.ico",
        UITypeName = "Martin.SQLServer.Dts.MultipleHashUI, MultipleHash2008, Version=1.0.0.0, Culture=neutral, PublicKeyToken=51c551904274ab44",
        ComponentType = ComponentType.Transform,
        CurrentVersion = 1)]
    public class MultipleHash : PipelineComponent
    {
        #region Members

        /// <summary>
        ///  Stores the allowed types of Hash's.
        ///  If you add to this, make sure you update the SetOutputColumnProperty, PreExecute, and ProcessInput methods.
        /// </summary>
        public enum HashTypeEnum
        {
            None, MD5, RipeMD160, SHA1, SHA256, SHA384, SHA512
        }

        private bool cancelEvent;
        private OutputColumn[] outputColumnsArray;
        private int numOfOutputColumns;
        private Int64 numOfRowsProcessed;

        #endregion

        #region Design Time

        #region PerformUpgrade
        /// <summary>
        /// Upgrades the component if required.
        /// Doesn't do anything at the moment as this is still version 1...
        /// </summary>
        /// <param name="pipelineVersion"></param>
        public override void PerformUpgrade(int pipelineVersion)
        {
            // Obtain the current component version from the attribute.
            DtsPipelineComponentAttribute componentAttribute = (DtsPipelineComponentAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(DtsPipelineComponentAttribute), false);
            int currentVersion = componentAttribute.CurrentVersion;

            // If the component version saved in the package is less than
            //  the current version, perform the upgrade.
            if (ComponentMetaData.Version < currentVersion)
            {
                // Update the saved component version metadata to the current version.
                ComponentMetaData.Version = currentVersion;
            }
        }
        #endregion

        #region ProvideComponentProperties
        /// <summary>
        /// Called when the component is initally added to a data flow task. 
        /// Create and configure the input and outputs of the component.
        /// </summary>
        public override void ProvideComponentProperties()
        {
            /// Support resettability.
            /// The base class calls RemoveAllInputsOutputsAndCustomProperties to reset the
            /// component. Used here to highlight the functionality of the base class.
            base.RemoveAllInputsOutputsAndCustomProperties();

            //    Let the base component add the input and output.
            base.ProvideComponentProperties();

            //    Name the input and output, and make the output asynchronous.
            ComponentMetaData.InputCollection[0].Name = "Input";

            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
            output.Name = "HashedOutput";
            output.Description = "Hashed rows are directed to this output.";
            output.SynchronousInputID = ComponentMetaData.InputCollection[0].ID;
        }
        #endregion

        #region Validate
        /// <summary>
        /// Called repeatedly when the component is edited in the designer, and once at the beginning of execution.
        /// Verifies the following:
        /// 1. Check that there is only one output
        /// 2. Check that there is only one input
        /// 3. Check that the output columns are DT_BYTES...
        /// 4. Check that the required CustomProperties exist (and are valid)
        /// 5. The base class validation succeeds.
        /// </summary>
        /// <returns></returns>
        public override DTSValidationStatus Validate()
        {
            if (ComponentMetaData.OutputCollection.Count != 1)
            {
                InternalFireError(Properties.Resources.Exactly1Output);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (ComponentMetaData.InputCollection.Count != 1)
            {
                InternalFireError(Properties.Resources.Exactly1Input);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            IDTSInput100 input = ComponentMetaData.InputCollection[0];
            //foreach (IDTSInputColumn100 inputColumn in input.InputColumnCollection)
            //{
                ///// No support for DT_DBTIME image columns.
                //if (inputColumn.DataType == DataType.DT_DBTIME)
                //{
                //    throw new Exception(Properties.Resources.DBTimeDataTypeNotSupported);
                //}

                //if (inputColumn.DataType == DataType.DT_FILETIME)
                //{
                //    throw new Exception(Properties.Resources.DBFileTimeDataTypeNotSupported1);
                //}
                
            //}

            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

            foreach (IDTSOutputColumn100 outputColumn in output.OutputColumnCollection)
            {
                if (outputColumn.DataType != DataType.DT_BYTES)
                {
                    InternalFireError(Properties.Resources.OutputDatatypeInvalid);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }
                else
                {
                    /// Check that the custom properties are correct.
                    switch (outputColumn.CustomPropertyCollection.Count)
                    {
                        case 1:
                            if ((outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name == Utility.InputColumnLineagePropName))
                            {
                                InternalFireError(Properties.Resources.PropertyHashTypeMissing);
                                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                            }
                            else
                            {
                                if ((outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName))
                                {
                                    InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing);
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                                else
                                {
                                    InternalFireError(Properties.Resources.PropertyRemoved);
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                            }
                        case 2:
                            // Validate that the correct Customer Properties are there, and have valid values.
                            if ((outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName)
                             && (outputColumn.CustomPropertyCollection[1].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[1].Name != Utility.InputColumnLineagePropName))
                            {
                                InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing);
                                InternalFireError(Properties.Resources.PropertyHashTypeMissing);
                                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                            }
                            // Check first property
                            if (outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName)
                            {
                                if (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName)
                                {
                                    if (outputColumn.CustomPropertyCollection[1].Name == Utility.HashTypePropName)
                                    {
                                        InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing);
                                    }
                                    else
                                    {
                                        InternalFireError(Properties.Resources.PropertyHashTypeMissing);
                                    }
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                                else
                                {
                                    if (!ValidateColumnList(outputColumn.CustomPropertyCollection[0].Value.ToString(), input.InputColumnCollection))
                                    {
                                        InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing);
                                        return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                    }
                                }
                            }
                            else
                            {
                                if (!ValidateDataType(outputColumn, 0))
                                {
                                    InternalFireError(Properties.Resources.OutputDatatypeInvalid);
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                            }
                            // Check second property
                            if (outputColumn.CustomPropertyCollection[1].Name != Utility.HashTypePropName)
                            {
                                if (outputColumn.CustomPropertyCollection[1].Name != Utility.InputColumnLineagePropName)
                                {
                                    if (outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName)
                                    {
                                        InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing);
                                    }
                                    else
                                    {
                                        InternalFireError(Properties.Resources.PropertyHashTypeMissing);
                                    }
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                                else
                                {
                                    if (!ValidateColumnList(outputColumn.CustomPropertyCollection[1].Value.ToString(), input.InputColumnCollection))
                                    {
                                        InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing);
                                        return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                    }
                                }
                            }
                            else
                            {
                                if (!ValidateDataType(outputColumn, 1))
                                {
                                    InternalFireError(Properties.Resources.OutputDatatypeInvalid);
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                            }
                            break;
                        default:
                            InternalFireError(Properties.Resources.PropertyRemoved);
                            return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                    }
                }
            }

            /// Finally, call the base class, which validates that the LineageID of each column in the input collection
            /// matches the LineageID of a column in the VirtualInputColumnCollection. 
            return base.Validate();
        }
        #endregion

        #region SetUsageType
        /// <summary>
        /// The SetUsageType method is called when a column from the virtual input collection is selected for use by the component.
        /// The virtual input collection contains the collection of columns that the upstream component is providing to this component.
        /// If the column DataType is either DT_FILETIME or DT_DBTIME it is rejected, since these datatypes are not supported by the pipeline buffer natively.
        /// Based on the usageType parameter the column is either added, removed, or rejected by the component. 
        /// UsageType.UT_IGNORED - The column was previously added to the input collection. The base class removes the column from the input.
        /// the matching output columns are removed.
        /// UsageType.UT_READONLY - The column is added to the input collection of the component.
        /// UsageType.UT_READWRITE - Since this component does not modify columns this usagetype is rejected.
        /// </summary>
        /// <param name="inputID">The ID of the input the column is mapped to.</param>
        /// <param name="virtualInput">The IDTSVirtualInput100 for the input.</param>
        /// <param name="lineageID">The lineageID of the virtual input column.</param>
        /// <param name="usageType">The usagetype of the column.</param>
        /// <returns>The newly created input column.</returns>
        public override IDTSInputColumn100 SetUsageType(int inputID, IDTSVirtualInput100 virtualInput, int lineageID, DTSUsageType usageType)
        {
            if (virtualInput == null)
            {
                throw new ArgumentNullException("virtualInput");
            }

            IDTSVirtualInputColumn100 vCol = virtualInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(lineageID);
            IDTSInputColumn100 col = null;

            /// No support for DT_DBTIME image columns.
            //if (vCol.DataType == DataType.DT_DBTIME)
            //{
            //    throw new Exception(Properties.Resources.DBTimeDataTypeNotSupported);
            //}

            /// No support for DT_FILETIME image columns.
            //if (vCol.DataType == DataType.DT_FILETIME)
            //{
            //    throw new Exception(Properties.Resources.DBFileTimeDataTypeNotSupported1);
            //}

            ///    If the usageType is UT_IGNORED, then the column is being removed.
            /// So remove it from the outputs also.
            if (usageType == DTSUsageType.UT_IGNORED)
            {
                ///    If the usageType is UT_IGNORED, the base class removes the column
                /// and the returned column is null. 
                col = base.SetUsageType(inputID, virtualInput, lineageID, usageType);
            }
            else if (usageType == DTSUsageType.UT_READWRITE)
            {
                throw new Exception(Properties.Resources.ReadWriteNotSupported);
            }
            else
            {
                ///    Let the base class add the input column.
                col = base.SetUsageType(inputID, virtualInput, lineageID, usageType);
            }

            return col;
        }
        #endregion

        #region ReinitializeMetaData
        /// <summary>
        /// Called when VS_NEEDSNEWMETADATA is returned from Validate. 
        /// Reset all of the output columns.
        /// </summary>
        public override void ReinitializeMetaData()
        {
            IDTSInput100 input = ComponentMetaData.InputCollection[0];
            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

            foreach (IDTSOutputColumn100 outputColumn in output.OutputColumnCollection)
            {
                if (outputColumn.DataType != DataType.DT_BYTES)
                {
                    Utility.SetOutputColumnDataType(HashTypeEnum.None, outputColumn);
                }
                else
                {
                    /// Check that the custom properties are correct.
                    switch (outputColumn.CustomPropertyCollection.Count)
                    {
                        case 1:
                            // Check which one is missing, and add.  If neither are there, then remove what is, and add the correct ones.
                            if ((outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name == Utility.InputColumnLineagePropName))
                            {
                                AddHashTypeProperty(outputColumn);
                            }
                            else
                            {
                                if ((outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName))
                                {
                                    AddInputLineageIDsProperty(outputColumn);
                                }
                                else
                                {
                                    outputColumn.CustomPropertyCollection.RemoveAll();
                                    AddHashTypeProperty(outputColumn);
                                    AddInputLineageIDsProperty(outputColumn);
                                }
                            }
                            break;
                        case 2:
                            // Validate that the correct Customer Properties are there, and have valid values.
                            if ((outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName)
                             && (outputColumn.CustomPropertyCollection[1].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[1].Name != Utility.InputColumnLineagePropName))
                            {
                                outputColumn.CustomPropertyCollection.RemoveAll();
                                AddHashTypeProperty(outputColumn);
                                AddInputLineageIDsProperty(outputColumn);
                            }

                            // Check first property
                            if (outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName)
                            {
                                if (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName)
                                {
                                    int ColID = outputColumn.CustomPropertyCollection[0].ID;
                                    if (outputColumn.CustomPropertyCollection[1].Name == Utility.HashTypePropName)
                                        AddInputLineageIDsProperty(outputColumn);
                                    else
                                        AddHashTypeProperty(outputColumn);
                                    outputColumn.CustomPropertyCollection.RemoveObjectByID(ColID);
                                }
                                else
                                {
                                    if (!ValidateColumnList(outputColumn.CustomPropertyCollection[0].Value.ToString(), input.InputColumnCollection))
                                    {
                                        outputColumn.CustomPropertyCollection[0].Value = FixColumnList(outputColumn.CustomPropertyCollection[0].Value.ToString(), input.InputColumnCollection);
                                    }
                                }
                            }
                            else
                            {
                                if (!ValidateDataType(outputColumn, 0))
                                {
                                    Utility.SetOutputColumnDataType((HashTypeEnum)outputColumn.CustomPropertyCollection[0].Value, outputColumn);
                                }
                            }
                            // Check second property
                            if (outputColumn.CustomPropertyCollection[1].Name != Utility.HashTypePropName)
                            {
                                if (outputColumn.CustomPropertyCollection[1].Name != Utility.InputColumnLineagePropName)
                                {
                                    int ColID = outputColumn.CustomPropertyCollection[1].ID;
                                    if (outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName)
                                        AddInputLineageIDsProperty(outputColumn);
                                    else
                                        AddHashTypeProperty(outputColumn);
                                    outputColumn.CustomPropertyCollection.RemoveObjectByID(ColID);
                                }
                                else
                                {
                                    if (!ValidateColumnList(outputColumn.CustomPropertyCollection[1].Value.ToString(), input.InputColumnCollection))
                                    {
                                        outputColumn.CustomPropertyCollection[1].Value = FixColumnList(outputColumn.CustomPropertyCollection[1].Value.ToString(), input.InputColumnCollection);
                                    }
                                }
                            }
                            else
                            {
                                if (!ValidateDataType(outputColumn, 1))
                                {
                                    Utility.SetOutputColumnDataType((HashTypeEnum)outputColumn.CustomPropertyCollection[1].Value, outputColumn);
                                }
                            }

                            break;
                        default:
                            outputColumn.CustomPropertyCollection.RemoveAll();
                            AddHashTypeProperty(outputColumn);
                            AddInputLineageIDsProperty(outputColumn);
                            break;
                    }
                }
            }
        }
        #endregion

        #region OnInputPathDetached
        /// <summary>
        /// Called when the path connecting the component to an upstream component is deleted. 
        /// Clear the columns from the output collections.
        /// </summary>
        /// <param name="inputID"></param>
        public override void OnInputPathDetached(int inputID)
        {
            base.OnInputPathDetached(inputID);

            ComponentMetaData.OutputCollection[0].OutputColumnCollection.RemoveAll();
        }
        #endregion

        #region InsertInput
        /// <summary>
        /// Called to add an input to the ComponentMetaData. This component doesn't allow adding inputs,
        /// so an exception is thrown.
        /// </summary>
        /// <param name="insertPlacement"></param>
        /// <param name="inputID"></param>
        /// <returns></returns>
        public override IDTSInput100 InsertInput(DTSInsertPlacement insertPlacement, int inputID)
        {
            throw new Exception(Properties.Resources.Component
                + ComponentMetaData.Name
                + Properties.Resources.NotAllowAddingInputs);
        }
        #endregion

        #region InsertOutput
        /// <summary>
        /// Called to add an output to the ComponentMetaData. This component doesn't allow adding outputs,
        /// so an exception is thrown.
        /// </summary>
        /// <param name="insertPlacement"></param>
        /// <param name="outputID"></param>
        /// <returns></returns>
        public override IDTSOutput100 InsertOutput(DTSInsertPlacement insertPlacement, int outputID)
        {
            throw new Exception(Properties.Resources.Component
                + ComponentMetaData.Name
                + Properties.Resources.NotAllowAddingOutputs);
        }
        #endregion

        #region InsertOutputColumnAt
        /// <summary>
        /// Called to add an output to the ComponentMetaData.
        /// </summary>
        /// <param name="outputID"></param>
        /// <param name="outputColumnIndex"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public override IDTSOutputColumn100 InsertOutputColumnAt(int outputID, int outputColumnIndex, string name, string description)
        {
            IDTSOutput100 output = ComponentMetaData.OutputCollection.FindObjectByID(outputID);
            IDTSOutputColumn100 outputColumn = output.OutputColumnCollection.NewAt(outputColumnIndex);

            // Add the HashType property.
            AddHashTypeProperty(outputColumn);

            // Add the InputColumnLineageIDs property.
            AddInputLineageIDsProperty(outputColumn);

            // Set the data type based on the MD5 default.
            Utility.SetOutputColumnDataType(HashTypeEnum.MD5, outputColumn);

            outputColumn.Name = name;
            outputColumn.Description = description;
            return outputColumn;
        }
        #endregion

        #region SetOutputColumnProperty
        /// <summary>
        /// Called when the custom property on an output column is set.
        /// If the HashType property is changed, then update the output column type details to match the Hash...
        /// Allow changes to Name, Description, and InputColumnLineageIDs
        /// Prevent changes to any other property.
        /// </summary>
        /// <param name="outputID"></param>
        /// <param name="outputColumnID"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <returns></returns>
        public override IDTSCustomProperty100 SetOutputColumnProperty(int outputID, int outputColumnID, string propertyName, object propertyValue)
        {
            if (propertyName == Utility.HashTypePropName)
            {
                // Update the output type to match the HashType...
                IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
                IDTSOutputColumn100 outputColumn = output.OutputColumnCollection.FindObjectByID(outputColumnID);
                Utility.SetOutputColumnDataType((HashTypeEnum)propertyValue, outputColumn);
                return base.SetOutputColumnProperty(outputID, outputColumnID, propertyName, propertyValue);
            }
            if (propertyName == Utility.InputColumnLineagePropName || propertyName == "Description" || propertyName == "Name")
            {
                    return base.SetOutputColumnProperty(outputID, outputColumnID, propertyName, propertyValue);
            }
            throw new Exception(Properties.Resources.OutputPropertyCannotBeChanged);
        }
        #endregion

        #region SetOutputProperty
        /// <summary>
        /// Called when a custom property of an output object is set.
        /// </summary>
        /// <param name="outputID">The ID of the output.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="propertyValue">The value to assign to the property.</param>
        /// <returns>The custom property.</returns>
        public override IDTSCustomProperty100 SetOutputProperty(int outputID, string propertyName, object propertyValue)
        {
            return base.SetOutputProperty(outputID, propertyName, propertyValue);
        }
        #endregion

        #endregion

        #region Runtime Methods

        #region PreExecute
        /// <summary>
        /// Called prior to PrimeOutput and ProcessInput. 
        /// Populate the outputColumnsArray with the data from each OutputColumn...
        /// </summary>
        public override void PreExecute()
        {
            bool FireAgain = true;
            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Pre-Execute phase is beginning.", "", 0, ref FireAgain);
            numOfRowsProcessed = 0;

            numOfOutputColumns = ComponentMetaData.OutputCollection[0].OutputColumnCollection.Count;
            outputColumnsArray = new OutputColumn[numOfOutputColumns];

            for (int i = 0; i < numOfOutputColumns; i++)
            {
                outputColumnsArray[i] = new OutputColumn();
                outputColumnsArray[i].AddColumnInformation(BufferManager, ComponentMetaData.OutputCollection[0], ComponentMetaData.InputCollection[0], i);
                if (outputColumnsArray[i].HashObject == null)
                {
                    this.ComponentMetaData.FireWarning(0, this.ComponentMetaData.Name, "Inside PreExecute: HashObject has not been set for " + ComponentMetaData.OutputCollection[0].OutputColumnCollection[i].Name, "", 0);
                }
            }
        }
        #endregion

        #region ProcessInput
        /// <summary>
        /// This method is called repeatedly during package execution. It is called each time the data flow task
        /// has a full buffer provided by an upstream component. 
        /// </summary>
        /// <param name="inputID">The ID of the input object of the component.</param>
        /// <param name="buffer">The input buffer.</param>
        public override void ProcessInput(int inputID, PipelineBuffer buffer)
        {
#if DEBUG
            bool FireAgain = true;
#endif
            uint blobLength = 0;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
#if DEBUG
            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput", "", 0, ref FireAgain);
#endif
            //    Have we received the last buffer from the upstream component?
            if (!buffer.EndOfRowset)
            {
                // No. Can we advance the buffer to the next row?
                while (buffer.NextRow())
                {
                    numOfRowsProcessed++;
#if DEBUG
                    this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: while (buffer.NextRow())", "", 0, ref FireAgain); 
#endif
                    // Step through each output column
                    for (int i = 0; i < numOfOutputColumns; i++)
                    {
                        byte[] inputByteBuffer = new byte[0];
                        // Step through each input column for that output column
                        for (int j = 0; j < outputColumnsArray[i].Count; j++)
                        {
                            /// Skip NULL values, as they "don't" exist...
                            if (!buffer.IsNull(outputColumnsArray[i][j]))
                            {
#if DEBUG
                                this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: DataType is " + buffer.GetColumnInfo(outputColumnsArray[i][j]).DataType.ToString(), "", 0, ref FireAgain);
#endif
                                switch (buffer.GetColumnInfo(outputColumnsArray[i][j]).DataType)
                                {
                                    case DataType.DT_BOOL:
                                        Utility.Append(ref inputByteBuffer, buffer.GetBoolean(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_IMAGE:
                                        blobLength = buffer.GetBlobLength(outputColumnsArray[i][j]);
                                        Utility.Append(ref inputByteBuffer, buffer.GetBlobData(outputColumnsArray[i][j],0,(int)blobLength));
                                        break;
                                    case DataType.DT_BYTES:
                                        Utility.Append(ref inputByteBuffer,  buffer.GetBytes(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_CY:
                                    case DataType.DT_DECIMAL:
                                    case DataType.DT_NUMERIC:
                                        Utility.Append(ref inputByteBuffer, buffer.GetDecimal(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_DATE:
                                    case DataType.DT_DBDATE:
                                    case DataType.DT_DBTIMESTAMP:
                                    case DataType.DT_DBTIMESTAMP2:
                                    case DataType.DT_DBTIMESTAMPOFFSET:
                                    case DataType.DT_FILETIME:
                                        Utility.Append(ref inputByteBuffer, buffer.GetDateTime(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_DBTIME:
                                    case DataType.DT_DBTIME2:
                                        Utility.Append(ref inputByteBuffer, buffer.GetTime(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_GUID:
                                        Utility.Append(ref inputByteBuffer, buffer.GetGuid(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_I1:
                                        Utility.Append(ref inputByteBuffer, buffer.GetSByte(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_I2:
                                        Utility.Append(ref inputByteBuffer, buffer.GetInt16(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_I4:
                                        Utility.Append(ref inputByteBuffer, buffer.GetInt32(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_I8:
                                        Utility.Append(ref inputByteBuffer, buffer.GetInt64(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_NTEXT:
                                    case DataType.DT_STR:
                                    case DataType.DT_TEXT:
                                    case DataType.DT_WSTR:
                                        Utility.Append(ref inputByteBuffer, buffer.GetString(outputColumnsArray[i][j]), Encoding.UTF8);
                                        break;
                                    case DataType.DT_R4:
                                        Utility.Append(ref inputByteBuffer, buffer.GetSingle(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_R8:
                                        Utility.Append(ref inputByteBuffer, buffer.GetDouble(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_UI1:
                                        Utility.Append(ref inputByteBuffer, buffer.GetByte(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_UI2:
                                        Utility.Append(ref inputByteBuffer, buffer.GetUInt16(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_UI4:
                                        Utility.Append(ref inputByteBuffer, buffer.GetUInt32(outputColumnsArray[i][j]));
                                        break;
                                    case DataType.DT_UI8:
                                        Utility.Append(ref inputByteBuffer, buffer.GetUInt64(outputColumnsArray[i][j]));
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
                                this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Null Value Encountered", "", 0, ref FireAgain);
                            }
                            #endif
                        }
                        // Ok, we have all the data in a Byte Buffer
                        // So now generate the Hash
                        #if DEBUG
                        this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Generate Hash from " + inputByteBuffer.ToString(), "", 0, ref FireAgain); 
                        #endif
                        byte[] hash;
                        switch (outputColumnsArray[i].HashType)
                        {
                            case HashTypeEnum.None:
                                hash = new byte[1];
                                break;
                            case HashTypeEnum.MD5:
                            case HashTypeEnum.RipeMD160:
                            case HashTypeEnum.SHA1:
                            case HashTypeEnum.SHA256:
                            case HashTypeEnum.SHA384:
                            case HashTypeEnum.SHA512:
                                hash = outputColumnsArray[i].HashObject.ComputeHash(inputByteBuffer);
                                break;
                            default:
                                hash = new byte[1];
                                break;
                        }
#if DEBUG
                        this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Assign hash to Output", "", 0, ref FireAgain); 
#endif
                        buffer.SetBytes(outputColumnsArray[i].OutputColumnID, hash);
                    }
                }
            }
        }
        #endregion

        #region PostExecute

        /// <summary>
        /// Posts an Information message that all is done, and calls base...
        /// </summary>
        public override void PostExecute()
        {
            bool FireAgain = true;
            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Post-Execute phase is beginning, after processing " + numOfRowsProcessed.ToString() + " rows.", "", 0, ref FireAgain);
            base.PostExecute();
        }

        #endregion

        #endregion

        #region Helpers

        #region ValidateColumnList
        /// <summary>
        /// Validates that all the input columns are available in the passed in string.
        /// </summary>
        /// <param name="InputLineageIDs">The list of Lineage ID's that are to be hashed</param>
        /// <param name="inputColumns">The collection of input columns.</param>
        /// <returns></returns>
        private bool ValidateColumnList(string InputLineageIDs, IDTSInputColumnCollection100 inputColumns)
        {
            string[] inputLineageArray;
            bool inputsOk = true;

            inputLineageArray = InputLineageIDs.Split(',');
            foreach (string LineageID in inputLineageArray)
            {

                try
                {
                    if (inputColumns.GetInputColumnByLineageID(System.Convert.ToInt32(LineageID)) == null)
                    {
                        inputsOk = false;
                        break;
                    }

                }
                catch (Exception ex)
                {
                    InternalFireError(Properties.Resources.ColumnLineageIDInvalid.Replace("%s", ex.Message));
                    inputsOk = false;
                    break;
                }
            }
            return inputsOk;
        } 
        #endregion

        #region FixColumnList
        /// <summary>
        /// Corrects any errors in the Input Lineage List, by removing the invalid ones.
        /// </summary>
        /// <param name="InputLineageIDs">The list of lineage id's.</param>
        /// <param name="inputColumns">The columns in the input.</param>
        /// <returns>A valid list of LineageID's</returns>
        private string FixColumnList(string InputLineageIDs, IDTSInputColumnCollection100 inputColumns)
        {
            string[] inputLineageArray;
            string inputValidatedList = "";

            inputLineageArray = InputLineageIDs.Split(',');
            foreach (string LineageID in inputLineageArray)
            {
                try
                {
                    if (inputColumns.GetInputColumnByLineageID(System.Convert.ToInt32(LineageID)) != null)
                    {
                        if (inputValidatedList == "")
                            inputValidatedList = LineageID;
                        else
                            inputValidatedList += "," + LineageID;
                    }
                }
                catch (Exception)
                {
                    // Don't do anything about it, as this is how we find the bad columns.
                }
            }
            return inputValidatedList;
        } 
        #endregion

        #region ValidateDataType
        /// <summary>
        /// Validates that the length of the output column is correct.
        /// </summary>
        /// <param name="outputColumn">The output column to validate.</param>
        /// <param name="customPropertyIndex">The output column's Custom Property to use for the HashTypeEnum to validate.</param>
        /// <returns>bool</returns>
        private bool ValidateDataType(IDTSOutputColumn100 outputColumn, int customPropertyIndex)
        {
            switch ((HashTypeEnum)outputColumn.CustomPropertyCollection[customPropertyIndex].Value)
            {
                case HashTypeEnum.None:
                    break;
                case HashTypeEnum.MD5:
                    if (outputColumn.Length != 16)
                        return false;
                    break;
                case HashTypeEnum.RipeMD160:
                case HashTypeEnum.SHA1:
                    if (outputColumn.Length != 20)
                        return false;
                    break;
                case HashTypeEnum.SHA256:
                    if (outputColumn.Length != 32)
                        return false;
                    break;
                case HashTypeEnum.SHA384:
                    if (outputColumn.Length != 48)
                        return false;
                    break;
                case HashTypeEnum.SHA512:
                    if (outputColumn.Length != 64)
                        return false;
                    break;
                default:
                    return false;
            }
            return true;
        } 
        #endregion

        #region AddHashTypeProperty
        /// <summary>
        /// Adds the Custom Property for the HashType.
        /// </summary>
        /// <param name="outputColumn">The column to add the HashType property to.</param>

        private void AddHashTypeProperty(IDTSOutputColumn100 outputColumn)
        {
            // Add the HashType property.
            IDTSCustomProperty100 hashProperty = outputColumn.CustomPropertyCollection.New();
            hashProperty.Name = Utility.HashTypePropName;
            hashProperty.Description = "Select the Hash Type that will be used for this output column.";
            hashProperty.ContainsID = false;
            hashProperty.EncryptionRequired = false;
            hashProperty.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            hashProperty.TypeConverter = typeof(HashTypeEnum).AssemblyQualifiedName;
            hashProperty.Value = HashTypeEnum.None;
        } 
        #endregion

        #region AddInputLineageIDsProperty
        /// <summary>
        /// Adds the InputColumnLineageIDs custom property.
        /// </summary>
        /// <param name="outputColumn">The column to add the InputColumnLineageIDs property to.</param>
        private void AddInputLineageIDsProperty(IDTSOutputColumn100 outputColumn)
        {
            // Add the InputColumnLineageIDs property.
            IDTSCustomProperty100 inputColumnLineageIDs = outputColumn.CustomPropertyCollection.New();
            inputColumnLineageIDs.Name = Utility.InputColumnLineagePropName;
            inputColumnLineageIDs.Description = "Enter the Lineage ID's that will be used to calculate the hash for this output column.";
            inputColumnLineageIDs.ContainsID = false;
            inputColumnLineageIDs.EncryptionRequired = false;
            inputColumnLineageIDs.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            inputColumnLineageIDs.Value = "";
        } 
        #endregion

        #region InternalFireError
        /// <summary>
        /// Sticks an Error message out to the Error Log.
        /// </summary>
        /// <param name="Message"></param>
        private void InternalFireError(string Message)
        {
            ComponentMetaData.FireError(0, ComponentMetaData.Name, Message, "", 0, out cancelEvent);
        } 
        #endregion

        #endregion

    }
}
