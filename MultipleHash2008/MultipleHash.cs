// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="MultipleHash.cs" company="NA">
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
    using System.Text;
    using System.Threading;
    using Microsoft.SqlServer.Dts.Pipeline;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
    using Microsoft.SqlServer.Dts.Runtime.Wrapper;
    #endregion

    /// <summary>
    /// Enables the creation of Hash's on selected input columns.
    /// The user can select the Hash type (MD5 etc.), and the columns that the hash is to be applied to.
    /// This uses the following Custom values:
    /// HashType - the type of hash.
    /// InputColumnLineageIDs - the comma separated list of LineageID's to calculate the Hash on.
    /// </summary>
    [DtsPipelineComponent(
        DisplayName = "Multiple Hash",
        Description = "Creates Multiple Hash's from selected input columns.",
        IconResource = "Martin.SQLServer.Dts.key.ico",
        UITypeName = "Martin.SQLServer.Dts.MultipleHashUI, MultipleHash2008, Version=1.0.0.0, Culture=neutral, PublicKeyToken=51c551904274ab44",
        ComponentType = ComponentType.Transform,
        CurrentVersion = 2)]
    public class MultipleHash : PipelineComponent
    {
        #region Members

        /// <summary>
        /// Used when an Event needs to be canceled.
        /// </summary>
        private bool cancelEvent;

        /// <summary>
        /// An array of output columns
        /// </summary>
        private OutputColumn[] outputColumnsArray;

        /// <summary>
        /// The number of Output Columns used in the component
        /// </summary>
        private int numOfOutputColumns;

        /// <summary>
        /// Stores the number of rows that have been processed.
        /// </summary>
        private long numOfRowsProcessed;

        /// <summary>
        /// The number of threads that are allowed to be created
        /// </summary>
        private int numOfThreads;

        /// <summary>
        /// An array of Events that are used to signal that all the
        /// child threads have finished.
        /// </summary>
        private ManualResetEvent[] threadResets;

        /// <summary>
        ///  Stores the allowed types of Hash's.
        ///  If you add to this, make sure you update the SetOutputColumnProperty, PreExecute, and ProcessInput methods.
        /// </summary>
        public enum HashTypeEnumerator
        {
            /// <summary>
            /// No choice has been made
            /// </summary>
            None,

            /// <summary>
            /// Creates an MD5 Hash
            /// </summary>
            MD5,

            /// <summary>
            /// Creates an RipeMD160 Hash
            /// </summary>
            RipeMD160,

            /// <summary>
            /// Creates a SHA1 Hash
            /// </summary>
            SHA1,

            /// <summary>
            /// Creates a SHA256 Hash
            /// </summary>
            SHA256,

            /// <summary>
            /// Creates a SHA384 Hash
            /// </summary>
            SHA384,

            /// <summary>
            /// Creates a SHA512 Hash
            /// </summary>
            SHA512
        }

        /// <summary>
        /// Defines the set of valid Thread counts
        /// </summary>
        public enum MultipleThread
        {
            /// <summary>
            /// Don't use Multiple Threads
            /// </summary>
            None, 

            /// <summary>
            /// Check the number of CPU's and if greater than 2, enable threading
            /// </summary>
            Auto,

            /// <summary>
            /// Use threading no matter what.
            /// </summary>
            On
        }

        #endregion

        #region Design Time

        #region PerformUpgrade
        /// <summary>
        /// Upgrades the component if required.
        /// Adds the Multiple Thread Poroperty if we were Version 1
        /// </summary>
        /// <param name="pipelineVersion">Not used at the moment</param>
        public override void PerformUpgrade(int pipelineVersion)
        {
            // Obtain the current component version from the attribute.
            DtsPipelineComponentAttribute componentAttribute = (DtsPipelineComponentAttribute)Attribute.GetCustomAttribute(this.GetType(), typeof(DtsPipelineComponentAttribute), false);
            int currentVersion = componentAttribute.CurrentVersion;

            // If the component version saved in the package is less than
            //  the current version, perform the upgrade.
            if (ComponentMetaData.Version < currentVersion)
            {
                if (ComponentMetaData.Version == 1)
                {
                    // Add Properties for Version 2...
                    this.AddMultipleThreadProperty(this.ComponentMetaData);
                }

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
            // Support resettability.
            // The base class calls RemoveAllInputsOutputsAndCustomProperties to reset the
            // component. Used here to highlight the functionality of the base class.
            this.RemoveAllInputsOutputsAndCustomProperties();

            // Let the base component add the input and output.
            base.ProvideComponentProperties();

            // Add the Multiple Thread property (as None)...
            this.AddMultipleThreadProperty(this.ComponentMetaData);

            // Name the input and output, and make the output asynchronous.
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
        /// <returns>The status of the validation</returns>
        public override DTSValidationStatus Validate()
        {
            int testMultiThreadThere = 0;

            // Check that the MultiThread property is there.
            foreach (IDTSCustomProperty100 customProperty in ComponentMetaData.CustomPropertyCollection)
            {
                if (customProperty.Name == Utility.MultipleThreadPropName)
                {
                    testMultiThreadThere++;
                    try
                    {
                        MultipleThread testThread = (MultipleThread)customProperty.Value;
                    }
                    catch (Exception)
                    {
                        this.InternalFireError(Properties.Resources.PropertyMultiThreadInvalid);
                        return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                    }
                }
            }

            if (testMultiThreadThere == 0)
            {
                this.InternalFireError(Properties.Resources.MultiThreadPropertyMissing);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (testMultiThreadThere > 1)
            {
                this.InternalFireError(Properties.Resources.ToManyMultiThreadProperties);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (ComponentMetaData.OutputCollection.Count != 1)
            {
                this.InternalFireError(Properties.Resources.Exactly1Output);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            if (ComponentMetaData.InputCollection.Count != 1)
            {
                this.InternalFireError(Properties.Resources.Exactly1Input);
                return DTSValidationStatus.VS_ISCORRUPT;
            }

            IDTSInput100 input = ComponentMetaData.InputCollection[0];
            ////foreach (IDTSInputColumn100 inputColumn in input.InputColumnCollection)
            ////{
                ////// No support for DT_DBTIME image columns.
                ////if (inputColumn.DataType == DataType.DT_DBTIME)
                ////{
                ////    throw new Exception(Properties.Resources.DBTimeDataTypeNotSupported);
                ////}

                ////if (inputColumn.DataType == DataType.DT_FILETIME)
                ////{
                ////    throw new Exception(Properties.Resources.DBFileTimeDataTypeNotSupported1);
                ////}
            ////}

            IDTSOutput100 output = ComponentMetaData.OutputCollection[0];

            foreach (IDTSOutputColumn100 outputColumn in output.OutputColumnCollection)
            {
                if (outputColumn.DataType != DataType.DT_BYTES)
                {
                    this.InternalFireError(Properties.Resources.OutputDatatypeInvalid);
                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                }
                else
                {
                    // Check that the custom properties are correct.
                    switch (outputColumn.CustomPropertyCollection.Count)
                    {
                        case 1:
                            if ((outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name == Utility.InputColumnLineagePropName))
                            {
                                this.InternalFireError(Properties.Resources.PropertyHashTypeMissing.Replace("%s", outputColumn.Name));
                                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                            }
                            else
                            {
                                if ((outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName))
                                {
                                    this.InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing.Replace("%s", outputColumn.Name));
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                                else
                                {
                                    this.InternalFireError(Properties.Resources.PropertyRemoved);
                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                            }

                        case 2:
                            // Validate that the correct Customer Properties are there, and have valid values.
                            if ((outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName)
                             && (outputColumn.CustomPropertyCollection[1].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[1].Name != Utility.InputColumnLineagePropName))
                            {
                                this.InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing.Replace("%s", outputColumn.Name));
                                this.InternalFireError(Properties.Resources.PropertyHashTypeMissing.Replace("%s", outputColumn.Name));
                                return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                            }

                            // Check first property
                            if (outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName)
                            {
                                if (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName)
                                {
                                    if (outputColumn.CustomPropertyCollection[1].Name == Utility.HashTypePropName)
                                    {
                                        this.InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing.Replace("%s", outputColumn.Name));
                                    }
                                    else
                                    {
                                        this.InternalFireError(Properties.Resources.PropertyHashTypeMissing.Replace("%s", outputColumn.Name));
                                    }

                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                                else
                                {
                                    if (!this.ValidateColumnList(outputColumn.CustomPropertyCollection[0].Value.ToString(), input.InputColumnCollection))
                                    {
                                        this.InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsInvalid.Replace("%s", outputColumn.Name));
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
                                        this.InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsMissing.Replace("%s", outputColumn.Name));
                                    }
                                    else
                                    {
                                        this.InternalFireError(Properties.Resources.PropertyHashTypeMissing.Replace("%s", outputColumn.Name));
                                    }

                                    return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                                }
                                else
                                {
                                    if (!this.ValidateColumnList(outputColumn.CustomPropertyCollection[1].Value.ToString(), input.InputColumnCollection))
                                    {
                                        this.InternalFireError(Properties.Resources.PropertyInputColumnLineageIDsInvalid.Replace("%s", outputColumn.Name));
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
                            this.InternalFireError(Properties.Resources.PropertyRemoved);
                            return DTSValidationStatus.VS_NEEDSNEWMETADATA;
                    }
                }
            }

            // Finally, call the base class, which validates that the LineageID of each column in the input collection
            // matches the LineageID of a column in the VirtualInputColumnCollection. 
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

            IDTSVirtualInputColumn100 virtualCol = virtualInput.VirtualInputColumnCollection.GetVirtualInputColumnByLineageID(lineageID);
            IDTSInputColumn100 col = null;

            // No support for DT_DBTIME image columns.
            ////if (vCol.DataType == DataType.DT_DBTIME)
            ////{
            ////    throw new Exception(Properties.Resources.DBTimeDataTypeNotSupported);
            ////}

            // No support for DT_FILETIME image columns.
            ////if (vCol.DataType == DataType.DT_FILETIME)
            ////{
            ////    throw new Exception(Properties.Resources.DBFileTimeDataTypeNotSupported1);
            ////}

            // If the usageType is UT_IGNORED, then the column is being removed.
            // So remove it from the outputs also.
            if (usageType == DTSUsageType.UT_IGNORED)
            {
                // If the usageType is UT_IGNORED, the base class removes the column
                // and the returned column is null. 
                col = base.SetUsageType(inputID, virtualInput, lineageID, usageType);
            }
            else if (usageType == DTSUsageType.UT_READWRITE)
            {
                throw new Exception(Properties.Resources.ReadWriteNotSupported);
            }
            else
            {
                // Let the base class add the input column.
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
            int multiThreadCount = 0;

            // Make sure that ONLY the right property is here.
            foreach (IDTSCustomProperty100 customProperty in ComponentMetaData.CustomPropertyCollection)
            {
                if (customProperty.Name == Utility.MultipleThreadPropName)
                {
                    multiThreadCount++;
                    if (multiThreadCount > 1)
                    {
                        ComponentMetaData.CustomPropertyCollection.RemoveObjectByID(customProperty.ID);
                    }
                    else
                    {
                        // Attempt to get the value, and fire if it's not valid.
                        try
                        {
                            MultipleThread testThread = (MultipleThread)customProperty.Value;
                        }
                        catch (Exception)
                        {
                            ComponentMetaData.CustomPropertyCollection.RemoveObjectByID(customProperty.ID);
                            this.AddMultipleThreadProperty(ComponentMetaData);
                        }
                    }
                }
            }

            // Go through all the output columns now.
            foreach (IDTSOutputColumn100 outputColumn in output.OutputColumnCollection)
            {
                if (outputColumn.DataType != DataType.DT_BYTES)
                {
                    Utility.SetOutputColumnDataType(HashTypeEnumerator.None, outputColumn);
                }
                else
                {
                    // Check that the custom properties are correct.
                    switch (outputColumn.CustomPropertyCollection.Count)
                    {
                        case 1:
                            // Check which one is missing, and add.  If neither are there, then remove what is, and add the correct ones.
                            if ((outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName) && (outputColumn.CustomPropertyCollection[0].Name == Utility.InputColumnLineagePropName))
                            {
                                this.AddHashTypeProperty(outputColumn);
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
                                this.AddHashTypeProperty(outputColumn);
                                this.AddInputLineageIDsProperty(outputColumn);
                            }

                            // Check first property
                            if (outputColumn.CustomPropertyCollection[0].Name != Utility.HashTypePropName)
                            {
                                if (outputColumn.CustomPropertyCollection[0].Name != Utility.InputColumnLineagePropName)
                                {
                                    int colID = outputColumn.CustomPropertyCollection[0].ID;
                                    if (outputColumn.CustomPropertyCollection[1].Name == Utility.HashTypePropName)
                                    {
                                        this.AddInputLineageIDsProperty(outputColumn);
                                    }
                                    else
                                    {
                                        this.AddHashTypeProperty(outputColumn);
                                    }

                                    outputColumn.CustomPropertyCollection.RemoveObjectByID(colID);
                                }
                                else
                                {
                                    if (!this.ValidateColumnList(outputColumn.CustomPropertyCollection[0].Value.ToString(), input.InputColumnCollection))
                                    {
                                        outputColumn.CustomPropertyCollection[0].Value = this.FixColumnList(outputColumn.CustomPropertyCollection[0].Value.ToString(), input.InputColumnCollection);
                                    }
                                }
                            }
                            else
                            {
                                if (!ValidateDataType(outputColumn, 0))
                                {
                                    Utility.SetOutputColumnDataType((HashTypeEnumerator)outputColumn.CustomPropertyCollection[0].Value, outputColumn);
                                }
                            }

                            // Check second property
                            if (outputColumn.CustomPropertyCollection[1].Name != Utility.HashTypePropName)
                            {
                                if (outputColumn.CustomPropertyCollection[1].Name != Utility.InputColumnLineagePropName)
                                {
                                    int colID = outputColumn.CustomPropertyCollection[1].ID;
                                    if (outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName)
                                    {
                                        this.AddInputLineageIDsProperty(outputColumn);
                                    }
                                    else
                                    {
                                        this.AddHashTypeProperty(outputColumn);
                                    }

                                    outputColumn.CustomPropertyCollection.RemoveObjectByID(colID);
                                }
                                else
                                {
                                    if (!this.ValidateColumnList(outputColumn.CustomPropertyCollection[1].Value.ToString(), input.InputColumnCollection))
                                    {
                                        outputColumn.CustomPropertyCollection[1].Value = this.FixColumnList(outputColumn.CustomPropertyCollection[1].Value.ToString(), input.InputColumnCollection);
                                    }
                                }
                            }
                            else
                            {
                                if (!ValidateDataType(outputColumn, 1))
                                {
                                    Utility.SetOutputColumnDataType((HashTypeEnumerator)outputColumn.CustomPropertyCollection[1].Value, outputColumn);
                                }
                            }

                            break;
                        default:
                            outputColumn.CustomPropertyCollection.RemoveAll();
                            this.AddHashTypeProperty(outputColumn);
                            this.AddInputLineageIDsProperty(outputColumn);
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
        /// <param name="inputID">The id for the input.  Passed by SSIS.</param>
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
        /// <param name="insertPlacement">Where the column is to be placed</param>
        /// <param name="inputID">The input to place</param>
        /// <returns>An error as this is disabled</returns>
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
        /// <param name="insertPlacement">The location where the output is to be placed</param>
        /// <param name="outputID">The ID of the output to place</param>
        /// <returns>the column that has been added</returns>
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
        /// <param name="outputID">The output id from SSIS</param>
        /// <param name="outputColumnIndex">The index where the column is to ba added</param>
        /// <param name="name">The name of the column</param>
        /// <param name="description">The decription of the column</param>
        /// <returns>The column that has been created</returns>
        public override IDTSOutputColumn100 InsertOutputColumnAt(int outputID, int outputColumnIndex, string name, string description)
        {
            IDTSOutput100 output = ComponentMetaData.OutputCollection.FindObjectByID(outputID);
            IDTSOutputColumn100 outputColumn = output.OutputColumnCollection.NewAt(outputColumnIndex);

            // Add the HashType property.
            this.AddHashTypeProperty(outputColumn);

            // Add the InputColumnLineageIDs property.
            this.AddInputLineageIDsProperty(outputColumn);

            // Set the data type based on the MD5 default.
            Utility.SetOutputColumnDataType(HashTypeEnumerator.MD5, outputColumn);

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
        /// <param name="outputID">The output id from SSIS</param>
        /// <param name="outputColumnID">The output column id from SSIS</param>
        /// <param name="propertyName">The name of the property to set</param>
        /// <param name="propertyValue">The value to set the property to</param>
        /// <returns>The custom property which has been configured</returns>
        public override IDTSCustomProperty100 SetOutputColumnProperty(int outputID, int outputColumnID, string propertyName, object propertyValue)
        {
            if (propertyName == Utility.HashTypePropName)
            {
                // Update the output type to match the HashType...
                IDTSOutput100 output = ComponentMetaData.OutputCollection[0];
                IDTSOutputColumn100 outputColumn = output.OutputColumnCollection.FindObjectByID(outputColumnID);
                Utility.SetOutputColumnDataType((HashTypeEnumerator)propertyValue, outputColumn);
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
            MultipleThread testThread = MultipleThread.None;
            bool fireAgain = true;
            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Pre-Execute phase is beginning.", string.Empty, 0, ref fireAgain);
            this.numOfRowsProcessed = 0;

            foreach (IDTSCustomProperty100 customProperty in ComponentMetaData.CustomPropertyCollection)
            {
                if (customProperty.Name == Utility.MultipleThreadPropName)
                {
                    testThread = (MultipleThread)customProperty.Value;
                }
            }

            switch (testThread)
            {
                case MultipleThread.None:
                    this.numOfThreads = 1;
                    break;
                case MultipleThread.Auto:
                    this.numOfThreads = Math.Max(1, Utility.GetNumberOfProcessorCores() - 1);
                    break;
                case MultipleThread.On:
                    this.numOfThreads = 2;
                    break;
                default:
                    break;
            }

            this.numOfOutputColumns = ComponentMetaData.OutputCollection[0].OutputColumnCollection.Count;
            this.outputColumnsArray = new OutputColumn[this.numOfOutputColumns];
            for (int i = 0; i < this.numOfOutputColumns; i++)
            {
                this.outputColumnsArray[i] = new OutputColumn();
                this.outputColumnsArray[i].AddColumnInformation(BufferManager, ComponentMetaData.OutputCollection[0], ComponentMetaData.InputCollection[0], i);
                if (this.outputColumnsArray[i].HashObject == null)
                {
                    this.ComponentMetaData.FireWarning(0, this.ComponentMetaData.Name, "Inside PreExecute: HashObject has not been set for " + ComponentMetaData.OutputCollection[0].OutputColumnCollection[i].Name, string.Empty, 0);
                }
            }

            if (this.numOfThreads > 1)
            {
                // Setup Thread Requirements
                this.threadResets = new ManualResetEvent[this.numOfOutputColumns];
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
            PassThreadState passThreadState;
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
#if DEBUG
            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput", string.Empty, 0, ref FireAgain);
#endif
            // Have we received the last buffer from the upstream component?
            if (!buffer.EndOfRowset)
            {
                // No. Can we advance the buffer to the next row?
                while (buffer.NextRow())
                {
                    this.numOfRowsProcessed++;
#if DEBUG
                    this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: while (buffer.NextRow())", string.Empty, 0, ref FireAgain); 
#endif

                    if (this.numOfThreads > 1)
                    {
#if DEBUG
                        this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Running MultiThreaded", string.Empty, 0, ref FireAgain);
#endif
                        for (int i = 0; i < this.numOfOutputColumns; i++)
                        {
                            this.threadResets[i] = new ManualResetEvent(false);
                            passThreadState = new PassThreadState(this.outputColumnsArray[i], buffer, this.ComponentMetaData, this.threadResets[i]);
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessOutputColumn.CalculateHash), passThreadState);
                        }

                        WaitHandle.WaitAll(this.threadResets);
                    }
                    else
                    {
#if DEBUG
                        this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Running SingleThreaded", string.Empty, 0, ref FireAgain);
#endif
                        // Step through each output column
                        for (int i = 0; i < this.numOfOutputColumns; i++)
                        {
                            byte[] inputByteBuffer = new byte[0];

                            // Step through each input column for that output column
                            for (int j = 0; j < this.outputColumnsArray[i].Count; j++)
                            {
                                // Skip NULL values, as they "don't" exist...
                                if (!buffer.IsNull(this.outputColumnsArray[i][j]))
                                {
#if DEBUG
                                    this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: DataType is " + buffer.GetColumnInfo(this.outputColumnsArray[i][j]).DataType.ToString(), string.Empty, 0, ref FireAgain);
#endif
                                    switch (buffer.GetColumnInfo(this.outputColumnsArray[i][j]).DataType)
                                    {
                                        case DataType.DT_BOOL:
                                            Utility.Append(ref inputByteBuffer, buffer.GetBoolean(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_IMAGE:
                                            blobLength = buffer.GetBlobLength(this.outputColumnsArray[i][j]);
                                            Utility.Append(ref inputByteBuffer, buffer.GetBlobData(this.outputColumnsArray[i][j], 0, (int)blobLength));
                                            break;
                                        case DataType.DT_BYTES:
                                            Utility.Append(ref inputByteBuffer, buffer.GetBytes(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_CY:
                                        case DataType.DT_DECIMAL:
                                        case DataType.DT_NUMERIC:
                                            Utility.Append(ref inputByteBuffer, buffer.GetDecimal(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_DATE:
                                        case DataType.DT_DBDATE:
                                        case DataType.DT_DBTIMESTAMP:
                                        case DataType.DT_DBTIMESTAMP2:
                                        case DataType.DT_DBTIMESTAMPOFFSET:
                                        case DataType.DT_FILETIME:
                                            Utility.Append(ref inputByteBuffer, buffer.GetDateTime(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_DBTIME:
                                        case DataType.DT_DBTIME2:
                                            Utility.Append(ref inputByteBuffer, buffer.GetTime(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_GUID:
                                            Utility.Append(ref inputByteBuffer, buffer.GetGuid(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_I1:
                                            Utility.Append(ref inputByteBuffer, buffer.GetSByte(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_I2:
                                            Utility.Append(ref inputByteBuffer, buffer.GetInt16(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_I4:
                                            Utility.Append(ref inputByteBuffer, buffer.GetInt32(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_I8:
                                            Utility.Append(ref inputByteBuffer, buffer.GetInt64(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_NTEXT:
                                        case DataType.DT_STR:
                                        case DataType.DT_TEXT:
                                        case DataType.DT_WSTR:
                                            Utility.Append(ref inputByteBuffer, buffer.GetString(this.outputColumnsArray[i][j]), Encoding.UTF8);
                                            break;
                                        case DataType.DT_R4:
                                            Utility.Append(ref inputByteBuffer, buffer.GetSingle(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_R8:
                                            Utility.Append(ref inputByteBuffer, buffer.GetDouble(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_UI1:
                                            Utility.Append(ref inputByteBuffer, buffer.GetByte(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_UI2:
                                            Utility.Append(ref inputByteBuffer, buffer.GetUInt16(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_UI4:
                                            Utility.Append(ref inputByteBuffer, buffer.GetUInt32(this.outputColumnsArray[i][j]));
                                            break;
                                        case DataType.DT_UI8:
                                            Utility.Append(ref inputByteBuffer, buffer.GetUInt64(this.outputColumnsArray[i][j]));
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
                                    this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Null Value Encountered", string.Empty, 0, ref FireAgain);
                                }
#endif
                            }

                            // Ok, we have all the data in a Byte Buffer
                            // So now generate the Hash
#if DEBUG
                            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Generate Hash from " + inputByteBuffer.ToString(), string.Empty, 0, ref FireAgain);
#endif
                            byte[] hash;
                            switch (this.outputColumnsArray[i].HashType)
                            {
                                case HashTypeEnumerator.None:
                                    hash = new byte[1];
                                    break;
                                case HashTypeEnumerator.MD5:
                                case HashTypeEnumerator.RipeMD160:
                                case HashTypeEnumerator.SHA1:
                                case HashTypeEnumerator.SHA256:
                                case HashTypeEnumerator.SHA384:
                                case HashTypeEnumerator.SHA512:
                                    hash = this.outputColumnsArray[i].HashObject.ComputeHash(inputByteBuffer);
                                    break;
                                default:
                                    hash = new byte[1];
                                    break;
                            }
#if DEBUG
                            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Inside ProcessInput: Assign hash to Output", string.Empty, 0, ref FireAgain);
#endif
                            buffer.SetBytes(this.outputColumnsArray[i].OutputColumnId, hash);
                        }
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
            bool fireAgain = true;
            this.ComponentMetaData.FireInformation(0, this.ComponentMetaData.Name, "Post-Execute phase is beginning, after processing " + this.numOfRowsProcessed.ToString() + " rows.", string.Empty, 0, ref fireAgain);
            base.PostExecute();
        }

        #endregion

        #endregion

        #region Helpers

        #region ValidateColumnList
        /// <summary>
        /// Validates that all the input columns are available in the passed in string.
        /// </summary>
        /// <param name="inputLineageIDs">The list of Lineage ID's that are to be hashed</param>
        /// <param name="inputColumns">The collection of input columns.</param>
        /// <returns>True when validation ok</returns>
        private bool ValidateColumnList(string inputLineageIDs, IDTSInputColumnCollection100 inputColumns)
        {
            string[] inputLineageArray;
            bool inputsOk = true;

            // Bug Fix: 4238 - Index and Counter error...
            // Remove "Validation error. Data Flow Task: Multiple Hash: Error Input string was not in a correct format. thrown when checking LineageID"
            if (inputLineageIDs.Trim().Length > 0)
            {
                inputLineageArray = inputLineageIDs.Split(',');
                foreach (string lineageID in inputLineageArray)
                {
                    try
                    {
                        if (inputColumns.GetInputColumnByLineageID(System.Convert.ToInt32(lineageID)) == null)
                        {
                            inputsOk = false;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.InternalFireError(Properties.Resources.ColumnLineageIDInvalid.Replace("%s", ex.Message));
                        inputsOk = false;
                        break;
                    }
                }
            }
            else
            {
                inputsOk = false;
            }

            return inputsOk;
        } 
        #endregion

        #region FixColumnList
        /// <summary>
        /// Corrects any errors in the Input Lineage List, by removing the invalid ones.
        /// </summary>
        /// <param name="inputLineageIDs">The list of lineage id's.</param>
        /// <param name="inputColumns">The columns in the input.</param>
        /// <returns>A valid list of LineageID's</returns>
        private string FixColumnList(string inputLineageIDs, IDTSInputColumnCollection100 inputColumns)
        {
            string[] inputLineageArray;
            string inputValidatedList = string.Empty;

            inputLineageArray = inputLineageIDs.Split(',');
            foreach (string lineageID in inputLineageArray)
            {
                try
                {
                    if (inputColumns.GetInputColumnByLineageID(System.Convert.ToInt32(lineageID)) != null)
                    {
                        if (inputValidatedList == string.Empty)
                        {
                            inputValidatedList = lineageID;
                        }
                        else
                        {
                            inputValidatedList += "," + lineageID;
                        }
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
        /// <returns>returns true when validated ok</returns>
        private bool ValidateDataType(IDTSOutputColumn100 outputColumn, int customPropertyIndex)
        {
            switch ((HashTypeEnumerator)outputColumn.CustomPropertyCollection[customPropertyIndex].Value)
            {
                case HashTypeEnumerator.None:
                    break;
                case HashTypeEnumerator.MD5:
                    if (outputColumn.Length != 16)
                    {
                        return false;
                    }

                    break;
                case HashTypeEnumerator.RipeMD160:
                case HashTypeEnumerator.SHA1:
                    if (outputColumn.Length != 20)
                    {
                        return false;
                    }

                    break;
                case HashTypeEnumerator.SHA256:
                    if (outputColumn.Length != 32)
                    {
                        return false;
                    }

                    break;
                case HashTypeEnumerator.SHA384:
                    if (outputColumn.Length != 48)
                    {
                        return false;
                    }

                    break;
                case HashTypeEnumerator.SHA512:
                    if (outputColumn.Length != 64)
                    {
                        return false;
                    }

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
            hashProperty.TypeConverter = typeof(HashTypeEnumerator).AssemblyQualifiedName;
            hashProperty.Value = HashTypeEnumerator.None;
        } 
        #endregion

        #region AddMultipleThreadProperty

        /// <summary>
        /// Creates a new custom property collection to hold the Multiple Thread property
        /// </summary>
        /// <param name="metaData">The component MetaData to add the new property to</param>
        private void AddMultipleThreadProperty(IDTSComponentMetaData100 metaData)
        {
            // Add the Multi Thread Property
            IDTSCustomProperty100 multiThread = metaData.CustomPropertyCollection.New();
            multiThread.Description = "Select the number of threads to use";
            multiThread.Name = Utility.MultipleThreadPropName;
            multiThread.ContainsID = false;
            multiThread.EncryptionRequired = false;
            multiThread.ExpressionType = DTSCustomPropertyExpressionType.CPET_NONE;
            multiThread.TypeConverter = typeof(MultipleThread).AssemblyQualifiedName;
            multiThread.Value = MultipleThread.None;
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
            inputColumnLineageIDs.Value = string.Empty;
        } 
        #endregion

        #region InternalFireError
        /// <summary>
        /// Sticks an Error message out to the Error Log.
        /// </summary>
        /// <param name="message">The error message to fire</param>
        private void InternalFireError(string message)
        {
            ComponentMetaData.FireError(0, ComponentMetaData.Name, message, string.Empty, 0, out this.cancelEvent);
        } 
        #endregion

        #endregion
    }
}
