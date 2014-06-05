// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="MultipleHashUI.cs" company="NA">
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
    using System.Diagnostics;
    using System.Windows.Forms;
    using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
#if SQL2014
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput100;
    using IDTSCustomProperty = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSCustomProperty100;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn100;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn100;
    using IDTSInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumnCollection100;
    using IDTSVirtualInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumnCollection100;
    using IDTSOutputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumnCollection100;
#endif
#if SQL2012
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput100;
    using IDTSCustomProperty = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSCustomProperty100;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn100;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn100;
    using IDTSInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumnCollection100;
    using IDTSVirtualInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumnCollection100;
    using IDTSOutputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumnCollection100;
#endif
#if SQL2008
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput100;
    using IDTSCustomProperty = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSCustomProperty100;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn100;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput100;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn100;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn100;
    using IDTSInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumnCollection100;
    using IDTSVirtualInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumnCollection100;
    using IDTSOutputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumnCollection100;
#endif
#if SQL2005
    using IDTSOutput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutput90;
    using IDTSCustomProperty = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSCustomProperty90;
    using IDTSOutputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumn90;
    using IDTSInput = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInput90;
    using IDTSInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumn90;
    using IDTSVirtualInputColumn = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumn90;
    using IDTSInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSInputColumnCollection90;
    using IDTSVirtualInputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSVirtualInputColumnCollection90;
    using IDTSOutputColumnCollection = Microsoft.SqlServer.Dts.Pipeline.Wrapper.IDTSOutputColumnCollection90;
    using Microsoft.SqlServer.Dts.Runtime.Wrapper;
#endif
    #endregion

    /// <summary>
    /// This is the class which provides the interface to the data flow from the UI.
    /// </summary>
    public class MultipleHashUI : DataFlowComponentUI
    {
        #region Virtual methods

        /// <summary>
        /// Implementation of the method resposible for displaying the form.
        /// This one is abstract in the base class.
        /// </summary>
        /// <param name="parentControl">The owner window</param>
        /// <returns>true when the form is shown ok</returns>
        protected override bool EditImpl(IWin32Window parentControl)
        {
            using (MultipleHashForm form = new MultipleHashForm())
            {
                this.HookupEvents(form);

                if (form.ShowDialog(parentControl) == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        #endregion

        /// <summary>
        /// Communication with UI form goes through these events. The UI will raise events when some data/action
        /// is needed and this class is responsible to answer those requests. This way we create separation between UI 
        /// specific logic and interactions with SSIS data flow object model.
        /// There are 7 required events:
        /// GetInputColumns - Retrieves all the input columns from the SSIS data flow object
        /// SetInputColumn - Passes alterations to input columns back to the SSIS data flow object
        /// DeleteInputColumn - Removes an input column from the SSIS data flow object
        /// GetOutputColumns - Retreives all the output columns from the SSIS data flow object
        /// AddOutputColumn - Adds a new output column to the SSIS data flow object
        /// AlterOutputColumn - Passes alterations to an output column back to the SSIS data flow object
        /// DeleteOutputColumn - Removes an output column from the SSIS data flow object
        /// </summary>
        /// <param name="form">The form that called this</param>
        private void HookupEvents(MultipleHashForm form)
        {
            form.GetInputColumns += new GetInputColumnsEventHandler(this.form_GetInputColumns);
            form.SetInputColumn += new ChangeInputColumnEventHandler(this.form_SetInputColumn);
            form.DeleteInputColumn += new ChangeInputColumnEventHandler(this.form_DeleteInputColumn);
            form.GetOutputColumns += new GetOutputColumnsEventHandler(this.form_GetOutputColumns);
            form.AddOutputColumn += new AddOutputColumnEventHandler(this.form_AddOutputColumn);
            form.AlterOutputColumn += new AlterOutputColumnEventHandler(this.form_AlterOutputColumn);
            form.DeleteOutputColumn += new DeleteOutputColumnEventHandler(this.form_DeleteOutputColumn);
            form.CallErrorHandler += new ErrorEventHandler(this.form_CallErrorHandler);
            form.GetThreadingDetail += new GetThreadingDetailEventHandler(this.form_GetThreadingDetail);
            form.SetThreadingDetail += new SetThreadingDetailEventHandler(this.form_SetThreadingDetail);
            form.GetSafeNullHandlingDetail += new GetSafeNullDetailsEventHandler(this.form_GetSafeNullDetail);
            form.SetSafeNullHandlingDetail += new SetSafeNullDetailsEventHandler(this.form_SetSafeNullDetail);
            form.GetMillisecondHandlingDetail += new GetMillisecondHandlingDetailEventHandler (this.form_GetMillisecondDetail);
            form.SetMillisecondHandlingDetail += new SetMillisecondHandlingDetailEventHandler(this.form_SetMillisecondDetail);
        }

        #region Event handlers

        #region Input Event Handlers
        /// <summary>
        /// retrieves the list of available input columns into AvailableColumnsArgs
        /// </summary>
        /// <param name="sender">Who caused this to be fired</param>
        /// <param name="args">The list of available input columns</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_GetInputColumns(object sender, InputColumnsArgs args)
        {
            Debug.Assert(this.VirtualInput != null, "Virtual input is not valid.");

            this.ClearErrors();

            try
            {
                // Grab the virtual input collection, which has all the columns that are available to be selected
                IDTSVirtualInputColumnCollection virtualInputColumnCollection = this.VirtualInput.VirtualInputColumnCollection;
                int virtualInputColumnsCount = virtualInputColumnCollection.Count;

                // Allocate the array of columns
                args.InputColumns = new InputColumnElement[virtualInputColumnsCount];

                // populate the array with all the available input columns.
                for (int i = 0; i < virtualInputColumnsCount; i++)
                {
                    IDTSVirtualInputColumn virtualInputColumn = virtualInputColumnCollection[i];
                    args.InputColumns[i].Selected = virtualInputColumn.UsageType != DTSUsageType.UT_IGNORED;
                    args.InputColumns[i].InputColumn = new DataFlowElement(virtualInputColumn.Name, virtualInputColumn);
                    args.InputColumns[i].LineageID = virtualInputColumn.LineageID;
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }

        /// <summary>
        /// Pushes the selected input column back to the SSIS component.  (SetUsageType).
        /// </summary>
        /// <param name="sender">Who caused this to be fired</param>
        /// <param name="args">The arguments to pass</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_SetInputColumn(object sender, SetInputColumnArgs args)
        {
            Debug.Assert(args.VirtualColumn != null, "Invalid arguments passed from the UI");

            this.ClearErrors();
            try
            {
                // Grab the used input collection.  This component can only have one...
                IDTSInput input = this.ComponentMetadata.InputCollection[0];

                // Get the virtual column from the args...
                IDTSVirtualInputColumn virtualInputColumn = args.VirtualColumn.Tag as IDTSVirtualInputColumn;
                if (virtualInputColumn == null)
                {
                    throw new ApplicationException(Properties.Resources.UIisInconsistentState);
                }
                // Get the lineageId, so we can use it to enable this column as an input column...
                int lineageId = virtualInputColumn.LineageID;

                IDTSInputColumn inputColumn = this.DesigntimeComponent.SetUsageType(input.ID, this.VirtualInput, lineageId, DTSUsageType.UT_READONLY);

#if SQL2005
                // No support for DT_DBTIME image columns.
                if (inputColumn.DataType == DataType.DT_DBTIME)
                {
                    throw new Exception(String.Format("Column {0} has issue {1}", inputColumn.Name, Properties.Resources.DBTimeDataTypeNotSupported));
                }

                // No support for DT_DBDATE image columns.
                if (inputColumn.DataType == DataType.DT_DBDATE)
                {
                    throw new Exception(String.Format("Column {0} has issue {1}", inputColumn.Name, Properties.Resources.DBDateDataTypeNotSupported));
                }

                if (inputColumn.DataType == DataType.DT_FILETIME)
                {
                    throw new Exception(String.Format("Column {0} has issue {1}", inputColumn.Name, Properties.Resources.DBFileTimeDataTypeNotSupported));
                }
#endif
                // return the new column back to the GUI to stick into a Tag...
                args.GeneratedColumns.InputColumn = new DataFlowElement(inputColumn.Name, inputColumn);
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
                args.CancelAction = true;
            }
        }

        /// <summary>
        /// Removes a column from the SSIS Component via SetUsageType.
        /// </summary>
        /// <param name="sender">Who caused this to be fired</param>
        /// <param name="args">The arguments to be passed</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_DeleteInputColumn(object sender, SetInputColumnArgs args)
        {
            Debug.Assert(args.VirtualColumn != null, "Invalid arguments passed from the UI");

            this.ClearErrors();
            try
            {
                // Grab the used input columns from the Component.
                IDTSInput input = this.ComponentMetadata.InputCollection[0];

                // Get the Virtual column from the args.
                IDTSVirtualInputColumn virtualInputColumn = args.VirtualColumn.Tag as IDTSVirtualInputColumn;
                if (virtualInputColumn == null)
                {
                    throw new ApplicationException(Properties.Resources.UIisInconsistentState);
                }

                // Get the lineageId, so we can use it to disable this column as an input column...
                int lineageId = virtualInputColumn.LineageID;

                this.DesigntimeComponent.SetUsageType(input.ID, this.VirtualInput, lineageId, DTSUsageType.UT_IGNORED);
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
                args.CancelAction = true;
            }
        }
        #endregion

        #region Output Event Handlers

        /// <summary>
        /// Gets all the output columns from the data flow object.  Also extracts the Custom Properties,
        /// and populates the DataFlowElements with the required information.
        /// </summary>
        /// <param name="sender">Who caused this to be fired</param>
        /// <param name="args">The OutputColumnsArgs class that will contain all the data about the output's</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_GetOutputColumns(object sender, OutputColumnsArgs args)
        {
            this.ClearErrors();

            try
            {
                // Get the output columns from the component, and store the number
                IDTSOutputColumnCollection outputColumns = this.ComponentMetadata.OutputCollection[0].OutputColumnCollection;
                int outputColumnsCount = outputColumns.Count;

                // Get the input columns that have been ticked, and store the number
                IDTSInputColumnCollection inputColumns = this.ComponentMetadata.InputCollection[0].InputColumnCollection;
                int inputColumnsCount = inputColumns.Count;

                // Assign the array to hold the output columns.
                args.OutputColumns = new OutputColumnElement[outputColumnsCount];

                // Loop through the output's, and get the Hash and selected input columns
                for (int i = 0; i < outputColumnsCount; i++)
                {
                    string[] inputLineageIDs;
                    string inputLineageList;
                    IDTSOutputColumn outputColumn = outputColumns[i];
                    if (outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName)
                    {
                        args.OutputColumns[i].Hash = (MultipleHash.HashTypeEnumerator)outputColumn.CustomPropertyCollection[0].Value;
                        inputLineageList = outputColumn.CustomPropertyCollection[1].Value.ToString();
                        inputLineageIDs = inputLineageList.Split(',');
                    }
                    else
                    {
                        args.OutputColumns[i].Hash = (MultipleHash.HashTypeEnumerator)outputColumn.CustomPropertyCollection[1].Value;
                        inputLineageList = outputColumn.CustomPropertyCollection[0].Value.ToString();
                        inputLineageIDs = inputLineageList.Split(',');
                    }

                    // Assign the array to hold the input columns
                    args.OutputColumns[i].InputColumns = new InputColumnElement[inputColumnsCount];

                    // Get all the input columns, and then flag the selected ones, and assign
                    // the sort order...
                    int j = 0;
                    foreach (IDTSInputColumn inputColumn in inputColumns)
                    {
                        args.OutputColumns[i].InputColumns[j] = new InputColumnElement();
                        args.OutputColumns[i].InputColumns[j].InputColumn = new DataFlowElement(inputColumn.Name, inputColumn);
                        args.OutputColumns[i].InputColumns[j].LineageID = inputColumn.LineageID;

                        // Default to unselected and sort to bottom of grid
                        args.OutputColumns[i].InputColumns[j].Selected = false;
                        args.OutputColumns[i].InputColumns[j].SortPosition = 999999;
                        for (int k = 0; k < inputLineageIDs.Length; k++)
                        {
                            if ("#" + inputColumn.LineageID.ToString() == inputLineageIDs[k])
                            {
                                // change to selected (because it's in the list)
                                // and assign the correct sort order.
                                args.OutputColumns[i].InputColumns[j].Selected = true;
                                args.OutputColumns[i].InputColumns[j].SortPosition = k;
                                inputLineageIDs[k] = "found";
                                break;
                            }
                        }

                        j++;
                    }

                    // Bug Fix: 4238 - Index and Counter error...
                    if (inputLineageList.Length > 0)
                    {
                        List<String> inputList = new List<string>(inputLineageList.Split(','));
                        // go through the list and remove any that don't exist.
                        for (int k = 0; k < inputLineageIDs.Length; k++)
                        {
                            if (inputLineageIDs[k] != "found")
                            {
                                if (inputList.Contains(inputLineageIDs[k]))
                                {
                                    inputList.Remove(inputLineageIDs[k]);
                                }
                            }
                        }
                        inputLineageList = String.Join(",", inputList.ToArray());
                    }

                    // Push the changed list back to the SSIS Component...
                    if (outputColumn.CustomPropertyCollection[0].Name == Utility.HashTypePropName)
                    {
                        outputColumn.CustomPropertyCollection[1].Value = inputLineageList;
                    }
                    else
                    {
                        outputColumn.CustomPropertyCollection[0].Value = inputLineageList;
                    }

                    args.OutputColumns[i].OutputColumn = new DataFlowElement(outputColumn.Name, outputColumn);
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }

        /// <summary>
        /// Adds an Output Column to the SSIS Component, and send's it back to the UI.
        /// </summary>
        /// <param name="sender">Who caused this to fire</param>
        /// <param name="args">The arguments that are needed</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_AddOutputColumn(object sender, AddOutputColumnNameArgs args)
        {
            Debug.Assert(args.OutputColumnDetail.OutputColumn != null, "Invalid arguments passed from the UI");

            this.ClearErrors();
            try
            {
                // Grab the output collection, and the number of output columns
                IDTSOutput output = this.ComponentMetadata.OutputCollection[0];
                int locationID = output.OutputColumnCollection.Count;

                // Create a new output column at the end of the set of output columns.
                IDTSOutputColumn outputColumn = this.DesigntimeComponent.InsertOutputColumnAt(output.ID, locationID, args.OutputColumnDetail.OutputColumn.Name, string.Empty);
                if (outputColumn == null)
                {
                    throw new ApplicationException(Properties.Resources.UIisInconsistentState);
                }

                // assign the output column details into the args.
                args.OutputColumnDetail.OutputColumn = new DataFlowElement(outputColumn.Name.ToString(), outputColumn);
                int j = 0;

                // assign the array to hold all the input columns
                args.OutputColumnDetail.InputColumns = new InputColumnElement[this.ComponentMetadata.InputCollection[0].InputColumnCollection.Count];

                // fill the array with unselected input columns.
                foreach (IDTSInputColumn inputColumn in this.ComponentMetadata.InputCollection[0].InputColumnCollection)
                {
                    args.OutputColumnDetail.InputColumns[j] = new InputColumnElement();
                    args.OutputColumnDetail.InputColumns[j].InputColumn = new DataFlowElement(inputColumn.Name, inputColumn);
                    args.OutputColumnDetail.InputColumns[j].LineageID = inputColumn.LineageID;
                    args.OutputColumnDetail.InputColumns[j].Selected = false;
                    args.OutputColumnDetail.InputColumns[j].SortPosition = 999999;
                    j++;
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
                args.CancelAction = true;
            }
        }

        /// <summary>
        /// Alters an existing Output Column in the SSIS Component to match that in the UI.
        /// </summary>
        /// <param name="sender">Where this got fired from</param>
        /// <param name="args">The arguments to pass in</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_AlterOutputColumn(object sender, AlterOutputColumnArgs args)
        {
            Debug.Assert(args.OutputColumnDetail.OutputColumn != null, "Invalid arguments passed from the UI");

            this.ClearErrors();
            try
            {
                // Grab the output collection, and the output column to change
                ////IDTSOutput output = this.ComponentMetadata.OutputCollection[0];
                IDTSOutputColumn outputColumn = args.OutputColumnDetail.OutputColumn.Tag as IDTSOutputColumn;
                if (outputColumn == null)
                {
                    throw new ApplicationException(Properties.Resources.UIisInconsistentState);
                }

                // Assign the column name from the args
                outputColumn.Name = args.OutputColumnDetail.OutputColumn.Name;

                // If the hash value has changed, assign the new value, and correct the output column data type.
                if ((MultipleHash.HashTypeEnumerator)outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value != args.OutputColumnDetail.Hash)
                {
                    Utility.SetOutputColumnDataType(args.OutputColumnDetail.Hash, outputColumn);
                    outputColumn.CustomPropertyCollection[Utility.HashTypePropName].Value = args.OutputColumnDetail.Hash;
                }

                // define a sorted list to hold the input columns
                SortedList<int, int> inputLineageIDs = new SortedList<int, int>(); ////(args.OutputColumnDetail.InputColumns.Length);
                string inputList = string.Empty;

                // Fill the sorted list with the input columns that have been selected.
                // Use the sort order as the key
                for (int i = 0; i < args.OutputColumnDetail.InputColumns.Length; i++)
                {
                    if (args.OutputColumnDetail.InputColumns[i].Selected)
                    {
                        inputLineageIDs.Add(args.OutputColumnDetail.InputColumns[i].SortPosition, args.OutputColumnDetail.InputColumns[i].LineageID);
                    }
                }

                // Populate the inputList to have the lineageID's from the input columns in the sorted order.
                foreach (KeyValuePair<int, int> kvp in inputLineageIDs)
                {
                    if (String.IsNullOrEmpty(inputList))
                    {
                        inputList = "#" + kvp.Value.ToString();
                    }
                    else
                    {
                        inputList += ",#" + kvp.Value.ToString();
                    }
                }

                // Update the Custom Property with the updated selected columns.
                outputColumn.CustomPropertyCollection[Utility.InputColumnLineagePropName].Value = inputList;

                // Ensure that the args passed back to the UI are up to date...
                args.OutputColumnDetail.OutputColumn = new DataFlowElement(outputColumn.Name, outputColumn);
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
                args.CancelAction = true;
            }
        }

        /// <summary>
        /// Fired when a column is selected for deletion
        /// </summary>
        /// <param name="sender">where the delete came from</param>
        /// <param name="args">the arguments passed</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_DeleteOutputColumn(object sender, DeleteOutputColumnArgs args)
        {
            Debug.Assert(args.OutputColumnDetail.OutputColumn != null, "Invalid arguments passed from the UI");

            this.ClearErrors();
            try
            {
                // Grab the output collection
                IDTSOutput output = this.ComponentMetadata.OutputCollection[0];

                // Remove the column from the output collect.
                output.OutputColumnCollection.RemoveObjectByID(((IDTSOutputColumn)args.OutputColumnDetail.OutputColumn.Tag).ID);
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
                args.CancelAction = true;
            }
        }

        #endregion

        /// <summary>
        /// Causes the form to call the error handler
        /// </summary>
        /// <param name="sender">Where the exception came from</param>
        /// <param name="ex">the exception</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void form_CallErrorHandler(object sender, Exception ex)
        {
            this.ReportErrors(ex);
        }

        #region Threading Detail Handlers
        /// <summary>
        /// Sends the Threading Detail back to the Component.
        /// </summary>
        /// <param name="sender">where the request came from</param>
        /// <param name="args">the arguments passed</param>
        void form_SetThreadingDetail(object sender, ThreadingArgs args)
        {
            Debug.Assert(args != null, "Invalid arguments passed from the UI");
            this.ClearErrors();
            try
            {
                foreach (IDTSCustomProperty customProperty in this.ComponentMetadata.CustomPropertyCollection)
                {
                    if (customProperty.Name == Utility.MultipleThreadPropName)
                    {
                        customProperty.Value = args.threadDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }

        /// <summary>
        /// Gets the Threading Detail from the Component.
        /// </summary>
        /// <param name="sender">where the request came from</param>
        /// <param name="args">the arguments passed</param>
        void form_GetThreadingDetail(object sender, ThreadingArgs args)
        {
            try
            {
                foreach (IDTSCustomProperty customProperty in this.ComponentMetadata.CustomPropertyCollection)
                {
                    if (customProperty.Name == Utility.MultipleThreadPropName)
                    {
                        args.threadDetail = (MultipleHash.MultipleThread)customProperty.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }
        #endregion

        #region Safe Null Handlers
        /// <summary>
        /// Sets the Safe Null details to the Component
        /// </summary>
        /// <param name="sender">where the request came from</param>
        /// <param name="args">the arguments passed</param>
        void form_SetSafeNullDetail(object sender, SafeNullArgs args)
        {
            Debug.Assert(args != null, "Invalid arguments passed from the UI");
            this.ClearErrors();
            try
            {
                foreach (IDTSCustomProperty customProperty in this.ComponentMetadata.CustomPropertyCollection)
                {
                    if (customProperty.Name == Utility.SafeNullHandlingPropName)
                    {
                        customProperty.Value = args.safeNullHandlingDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }

        /// <summary>
        /// Gets the Safe Null details from the Component
        /// </summary>
        /// <param name="sender">where the request came from</param>
        /// <param name="args">the arguments passed</param>
        void form_GetSafeNullDetail(object sender, SafeNullArgs args)
        {
            try
            {
                foreach (IDTSCustomProperty customProperty in this.ComponentMetadata.CustomPropertyCollection)
                {
                    if (customProperty.Name == Utility.SafeNullHandlingPropName)
                    {
                        args.safeNullHandlingDetail = (MultipleHash.SafeNullHandling)customProperty.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }
        #endregion

        #region Millisecond Handlers
        /// <summary>
        /// Sets the Millisecond details to the Component
        /// </summary>
        /// <param name="sender">where the request came from</param>
        /// <param name="args">the arguments passed</param>
        void form_SetMillisecondDetail(object sender, HashMillisecondArgs args)
        {
            Debug.Assert(args != null, "Invalid arguments passed from the UI");
            this.ClearErrors();
            try
            {
                foreach (IDTSCustomProperty customProperty in this.ComponentMetadata.CustomPropertyCollection)
                {
                    if (customProperty.Name == Utility.HandleMillisecondPropName)
                    {
                        customProperty.Value = args.millisecondHandlingDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }

        /// <summary>
        /// Gets the Millisecond details from the Component
        /// </summary>
        /// <param name="sender">where the request came from</param>
        /// <param name="args">the arguments passed</param>
        void form_GetMillisecondDetail(object sender, HashMillisecondArgs args)
        {
            try
            {
                foreach (IDTSCustomProperty customProperty in this.ComponentMetadata.CustomPropertyCollection)
                {
                    if (customProperty.Name == Utility.HandleMillisecondPropName)
                    {
                        args.millisecondHandlingDetail = (MultipleHash.MillisecondHandling)customProperty.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReportErrors(ex);
            }
        }
        #endregion

        #endregion
    }
}
