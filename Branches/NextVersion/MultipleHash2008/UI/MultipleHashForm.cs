// Multiple Hash SSIS Data Flow Transformation Component
//
// <copyright file="MultipleHashForm.cs" company="NA">
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
    using System.ComponentModel;
    using System.Windows.Forms;

    #endregion

    #region Helper structures and delegates

    internal delegate void DeleteOutputColumnEventHandler(object sender, DeleteOutputColumnArgs args);

    internal delegate void ErrorEventHandler(object sender, Exception ex);

    internal delegate void AlterOutputColumnEventHandler(object sender, AlterOutputColumnArgs args);

    internal delegate void AddOutputColumnEventHandler(object sender, AddOutputColumnNameArgs args);

    internal delegate void ChangeInputColumnEventHandler(object sender, SetInputColumnArgs args);

    internal delegate void GetOutputColumnsEventHandler(object sender, OutputColumnsArgs args);

    internal delegate void GetInputColumnsEventHandler(object sender, InputColumnsArgs args);

    /// <summary>
    /// Retrieves the threading details
    /// </summary>
    /// <param name="sender">Who called me?</param>
    /// <param name="args">The threading details</param>
    internal delegate void GetThreadingDetailEventHandler(object sender, ThreadingArgs args);

    /// <summary>
    /// Updates the threading details
    /// </summary>
    /// <param name="sender">Who called me?</param>
    /// <param name="args">The threading details</param>
    internal delegate void SetThreadingDetailEventHandler(object sender, ThreadingArgs args);

    /// <summary>
    /// Retrieves the safe null details
    /// </summary>
    /// <param name="sender">Who called me?</param>
    /// <param name="args">the safe null details</param>
    internal delegate void GetSafeNullDetailsEventHandler(object sender, SafeNullArgs args);

    /// <summary>
    /// Updates the safe null details
    /// </summary>
    /// <param name="sender">Who called me?</param>
    /// <param name="args">the safe null details</param>
    internal delegate void SetSafeNullDetailsEventHandler(object sender, SafeNullArgs args);

    /// <summary>
    /// Retrieves the Millisecond details
    /// </summary>
    /// <param name="sender">Who Called Me?</param>
    /// <param name="args">the millisecond details</param>
    internal delegate void GetMillisecondHandlingDetailEventHandler (object sender, HashMillisecondArgs args);
    
    /// <summary>
    /// Updates the Millisecond details
    /// </summary>
    /// <param name="sender">Who Called Me?</param>
    /// <param name="args">the millisecond detail</param>
    internal delegate void SetMillisecondHandlingDetailEventHandler(object sender, HashMillisecondArgs args);


    internal struct InputColumnElement
    {
        public bool Selected;
        public int LineageID;
        public int SortPosition;
        public DataFlowElement InputColumn;
    }

    internal struct SelectedInputColumns
    {
        public DataFlowElement InputColumn;
    }

    internal struct OutputColumnElement
    {
        public MultipleHash.HashTypeEnumerator Hash;
        public MultipleHash.OutputTypeEnumerator dataType;
        public DataFlowElement OutputColumn;
        public InputColumnElement[] InputColumns;
    }
    #endregion

    /// <summary>
    /// This is the form for the UI of this SSIS component
    /// </summary>
    public partial class MultipleHashForm : Form
    {
        /// <summary>
        /// This flag is set to true while loading the state from the component, to disable
        /// the grid events during that time.
        /// it is also used when updating the grids, to prevent other grids firing events.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// Initializes a new instance of the MultipleHashForm class.
        /// </summary>
        public MultipleHashForm()
        {
            // Force W2K3R2 x64 environments to work as the events are firing when the grids are being created.
            // This doesn't seem to happen on any other environment.
            this.isLoading = true;
            this.InitializeComponent();
            this.isLoading = false;
        }

        #region Exposed events

        // There are 9 required events:
        // GetInputColumns - Retrieves all the input columns from the SSIS data flow object
        // SetInputColumn - Passes alterations to input columns back to the SSIS data flow object
        // DeleteInputColumn - Removes an input column from the SSIS data flow object
        // GetOutputColumns - Retreives all the output columns from the SSIS data flow object
        // AddOutputColumn - Adds a new output column to the SSIS data flow object
        // AlterOutputColumn - Passes alterations to an output column back to the SSIS data flow object
        // DeleteOutputColumn - Removes an output column from the SSIS data flow object
        // GetThreadingDetail - Returns the threading details
        // SetThreadingDetail - Sets the threading details

        /// <summary>
        /// Fires when the Input Columns are required
        /// </summary>
        internal event GetInputColumnsEventHandler GetInputColumns;

        /// <summary>
        /// Fires when an Input Columns need to be set
        /// </summary>
        internal event ChangeInputColumnEventHandler SetInputColumn;

        /// <summary>
        /// Fires when the Input Columns need to be deleted
        /// </summary>
        internal event ChangeInputColumnEventHandler DeleteInputColumn;

        /// <summary>
        /// Fires when the Output Columns are need
        /// </summary>
        internal event GetOutputColumnsEventHandler GetOutputColumns;

        /// <summary>
        /// Fires when a new Output column is required
        /// </summary>
        internal event AddOutputColumnEventHandler AddOutputColumn;

        /// <summary>
        /// Fires when the Output Columns need to be changed
        /// </summary>
        internal event AlterOutputColumnEventHandler AlterOutputColumn;

        /// <summary>
        /// Fires when the Output Columns needs to be deleted
        /// </summary>
        internal event DeleteOutputColumnEventHandler DeleteOutputColumn;

        /// <summary>
        /// Fires when the threading details are to be returned
        /// </summary>
        internal event GetThreadingDetailEventHandler GetThreadingDetail;

        /// <summary>
        /// Fires when the threading details are to be updated
        /// </summary>
        internal event SetThreadingDetailEventHandler SetThreadingDetail;

        /// <summary>
        /// Fires when the safe null handling details are to be returned.
        /// </summary>
        internal event GetSafeNullDetailsEventHandler GetSafeNullHandlingDetail;

        /// <summary>
        /// Fires when the safe null handling details are to be updated.
        /// </summary>
        internal event SetSafeNullDetailsEventHandler SetSafeNullHandlingDetail;

        /// <summary>
        /// Fires when the Error Handler is needed
        /// </summary>
        internal event ErrorEventHandler CallErrorHandler;

        /// <summary>
        /// Fires when the Millisecond handling details are to be returned.
        /// </summary>
        internal event GetMillisecondHandlingDetailEventHandler GetMillisecondHandlingDetail;

        /// <summary>
        /// Fires when the Millisecond handling details are to be updated.
        /// </summary>
        internal event SetMillisecondHandlingDetailEventHandler SetMillisecondHandlingDetail;


        #endregion

        #region Overridden methods

        /// <summary>
        /// This get's fired as the OnLoad event.
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.isLoading = true;
            try
            {
                // Loading available and previously selected columns.
                this.LoadAvailableColumns();
                ThreadingArgs args = new ThreadingArgs();
                SafeNullArgs safeNullArgs = new SafeNullArgs();
                HashMillisecondArgs millisecondArgs = new HashMillisecondArgs();

                args.threadDetail = MultipleHash.MultipleThread.None;
                safeNullArgs.safeNullHandlingDetail = MultipleHash.SafeNullHandling.True;
                millisecondArgs.millisecondHandlingDetail = MultipleHash.MillisecondHandling.True;

                // Call the GetThreading event...
                IAsyncResult res = this.GetThreadingDetail.BeginInvoke(this, args, null, null);
                this.GetThreadingDetail.EndInvoke(res);
                cbThreading.Text = MultipleHashForm.GetThreadingName(args.threadDetail);
                    
                // Call the GetSafeNull event...
                res = this.GetSafeNullHandlingDetail.BeginInvoke(this, safeNullArgs, null, null);
                this.GetSafeNullHandlingDetail.EndInvoke(res);
                cbSafeNullHandling.Checked = MultipleHashForm.GetSafeNullValue(safeNullArgs.safeNullHandlingDetail);

                // Call the Millisecond event
                res = this.GetMillisecondHandlingDetail.BeginInvoke(this, millisecondArgs, null, null);
                this.GetMillisecondHandlingDetail.EndInvoke(res);
                cbMilliseconds.Checked = MultipleHashForm.GetMillisecondValue(millisecondArgs.millisecondHandlingDetail);
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                this.isLoading = false;
            }
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Hooking up a data flow element to a grid cell.
        /// </summary>
        /// <param name="cell">Where to put the DataFlowElement</param>
        /// <param name="dataFlowElement">What to put in the cell</param>
        private static void SetGridCellData(DataGridViewCell cell, DataFlowElement dataFlowElement)
        {
            cell.Value = dataFlowElement.ToString();
            cell.Tag = dataFlowElement.Tag;
            cell.ToolTipText = dataFlowElement.ToolTip;
        }

        /// <summary>
        /// Hooking up the IndexID to a grid cell
        /// </summary>
        /// <param name="cell">Where to stuff the data</param>
        /// <param name="value">The actual value to display</param>
        /// <param name="lineageID">The lineageID of the input column</param>
        /// <param name="toolTip">The text to display on a tool tip.</param>
        private static void SetGridCellData(DataGridViewCell cell, String value, int lineageID, String toolTip)
        {
            cell.Value = value;
            cell.Tag = lineageID;
            cell.ToolTipText = toolTip;
        }

        /// <summary>
        /// Hooking up an Output Column Element to a grid cell.
        /// Used on the dgvOutputColumns
        /// </summary>
        /// <param name="cell">Where to put the OutputColumnElement</param>
        /// <param name="outputColumnElement">What to put in the cell</param>
        private static void SetGridCellData(DataGridViewCell cell, OutputColumnElement outputColumnElement)
        {
            cell.Value = outputColumnElement.OutputColumn.ToString();
            cell.Tag = outputColumnElement;
            cell.ToolTipText = outputColumnElement.OutputColumn.ToolTip;
        }

        /// <summary>
        /// Loading available columns to the selection grid.
        /// </summary>
        private void LoadAvailableColumns()
        {
            bool blnCurrentisLoading = this.isLoading;
            try
            {
                // If we have defined the Event
                if (this.GetInputColumns != null)
                {
                    // Get all the input columns via Invoke
                    this.isLoading = true;
                    InputColumnsArgs args = new InputColumnsArgs();
                    IAsyncResult res = this.GetInputColumns.BeginInvoke(this, args, null, null);
                    this.GetInputColumns.EndInvoke(res);

                    // If there were columns returned
                    this.dgvAvailableColumns.SuspendLayout();
                    this.dgvAvailableColumns.Rows.Clear();
                    if (args.InputColumns != null && args.InputColumns.Length > 0)
                    {
                        // Push them all onto the dgvAvailableColumns Grid
                        this.dgvAvailableColumns.Rows.Add(args.InputColumns.Length);
                        for (int i = 0; i < args.InputColumns.Length; i++)
                        {
                            InputColumnElement availableColumnRow = args.InputColumns[i];

                            // Filling the cells.
                            this.dgvAvailableColumns.Rows[i].Cells[this.gridColumnCheckbox.Index].Value = availableColumnRow.Selected;
                            SetGridCellData(this.dgvAvailableColumns.Rows[i].Cells[this.gridColumnAvailableColumns.Index], availableColumnRow.InputColumn);
                        }
                    }

                    this.dgvAvailableColumns.ResumeLayout();
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                this.isLoading = blnCurrentisLoading;
            }
        }

        /// <summary>
        /// Load the output columns that have been defined to the output grid.
        /// </summary>
        private void LoadOutputColumns()
        {
            bool blnCurrentisLoading = this.isLoading;
            try
            {
                // If we have defined the event
                if (this.GetOutputColumns != null)
                {
                    // Get all the output columns via Invoke
                    this.isLoading = true;
                    OutputColumnsArgs args = new OutputColumnsArgs();
                    IAsyncResult res = this.GetOutputColumns.BeginInvoke(this, args, null, null);
                    this.GetOutputColumns.EndInvoke(res);

                    // Push eveything onto the Output Cgrid.
                    this.dgvOutputColumns.SuspendLayout();
                    this.dgvOutputColumns.Rows.Clear();
                    if (args.OutputColumns != null && args.OutputColumns.Length > 0)
                    {
                        this.dgvOutputColumns.Rows.Add(args.OutputColumns.Length);
                        for (int i = 0; i < args.OutputColumns.Length; i++)
                        {
                            OutputColumnElement outputColumnRow = args.OutputColumns[i];

                            // Filling the cells.
                            SetGridCellData(this.dgvOutputColumns.Rows[i].Cells[this.dgvOutputColumnsColumnName.Index], outputColumnRow);
                            this.dgvOutputColumns.Rows[i].Cells[this.dgvOutputColumnsHashType.Index].Value = MultipleHashForm.GetHashName(outputColumnRow.Hash);
                            this.dgvOutputColumns.Rows[i].Cells[this.dgvOutputColumnsDataType.Index].Value = MultipleHashForm.GetDataTypeName(outputColumnRow.dataType);
                        }
                    }

                    this.dgvOutputColumns.ResumeLayout();
                    this.RefreshdgvInputColumns();
                    this.RefreshdgvHashColumns();
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                this.isLoading = blnCurrentisLoading;
            }
        }

        /// <summary>
        /// Adding newly selected columns to the Data Flow.
        /// </summary>
        /// <param name="args">The arguments to pass to SetInputColumn</param>
        private void SetColumns(SetInputColumnArgs args)
        {
            try
            {
                if (this.SetInputColumn != null)
                {
                    IAsyncResult res = this.SetInputColumn.BeginInvoke(this, args, null, null);
                    this.SetInputColumn.EndInvoke(res);
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        /// <summary>
        /// Deleting unselected columns from the Data Flow and the bottom grid.
        /// </summary>
        /// <param name="args">The arguments to pass to DeleteInputColumn</param>
        private void DeleteColumns(SetInputColumnArgs args)
        {
            try
            {
                if (this.DeleteInputColumn != null)
                {
                    IAsyncResult res = this.DeleteInputColumn.BeginInvoke(this, args, null, null);
                    this.DeleteInputColumn.EndInvoke(res);

                    // If call to the OM succeeded, update the bottom grid.
                    if (!args.CancelAction)
                    {
                        // We should remove the columns from the Output's if it is used.
                        // Actually this should be done by the base SSIS component.
                    }
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        /// <summary>
        /// Refreshes the list of columns that can be selected.
        /// </summary>
        private void RefreshdgvInputColumns()
        {
            bool blnCurrentisLoading = this.isLoading;
            try
            {
                // Load the data into the dgvHashColumns grid.
                this.isLoading = true;
                this.dgvInputColumns.SuspendLayout();
                this.dgvInputColumns.Rows.Clear();

                // Make sure that something is selected in the Output Columns. 
                // Default to the first row if nothing is already selected.
                if (this.dgvOutputColumns.SelectedRows.Count == 0)
                {
                    this.dgvOutputColumns.Rows[0].Selected = true;
                }

                // If we have a Tag (for when there are no output columns yet.
                if (this.dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag != null)
                {
                    // If we have input columns...
                    if (((OutputColumnElement)this.dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns.Length > 0)
                    {
                        // Add all the required output rows from the Tag, which holds an OutputColumnElement
                        foreach (InputColumnElement inputColumnDetails in ((OutputColumnElement)this.dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns)
                        {
                            int rowIndex = this.dgvInputColumns.Rows.Add();
                            SetGridCellData(this.dgvInputColumns.Rows[rowIndex].Cells[this.dgvInputColumnsColumnName.Index], inputColumnDetails.InputColumn.Name, inputColumnDetails.LineageID, inputColumnDetails.InputColumn.ToolTip);
                            this.dgvInputColumns.Rows[rowIndex].Cells[this.dgvInputColumnsSelected.Index].Value = inputColumnDetails.Selected;
                        }
                    }
                }
                this.dgvInputColumns.ResumeLayout();
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                this.isLoading = blnCurrentisLoading;
            }
        }

        /// <summary>
        /// Refreshes the contents of the dgvHashColumns grid.
        /// </summary>
        private void RefreshdgvHashColumns()
        {
            bool blnCurrentisLoading = this.isLoading;
            try
            {
                // Load the data into the dgvHashColumns grid.
                this.isLoading = true;
                this.dgvHashColumns.SuspendLayout();
                this.dgvHashColumns.Rows.Clear();

                // Make sure that something is selected in the Output Columns. 
                // Default to the first row if nothing is already selected.
                if (this.dgvOutputColumns.SelectedRows.Count == 0)
                {
                    this.dgvOutputColumns.Rows[0].Selected = true;
                }

                // If we have a Tag (for when there are no output columns yet.
                if (this.dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag != null)
                {
                    // If we have input columns...
                    if (((OutputColumnElement)this.dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns.Length > 0)
                    {
                        // Add all the required output rows from the Tag, which holds an OutputColumnElement
                        foreach (InputColumnElement inputColumnDetails in ((OutputColumnElement)this.dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns)
                        {
                            if (inputColumnDetails.Selected)
                            {
                                int rowIndex = this.dgvHashColumns.Rows.Add();
                                SetGridCellData(this.dgvHashColumns.Rows[rowIndex].Cells[this.dgvHashColumnsColumnName.Index], inputColumnDetails.InputColumn.Name, inputColumnDetails.LineageID, inputColumnDetails.InputColumn.ToolTip);
                                this.dgvHashColumns.Rows[rowIndex].Cells[this.dgvHashColumnsSortPosition.Index].Value = inputColumnDetails.SortPosition.ToString("D6");
                            }
                        }
                    }
                }

                // Sort by the sort column, and then resume layout...
                this.dgvHashColumns.Sort(dgvHashColumnsSortPosition, ListSortDirection.Ascending);
                this.dgvHashColumns.ResumeLayout();
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                this.isLoading = blnCurrentisLoading;
            }
        }

        #endregion

        #region Event Handlers

        #region tcTabs Handlers

        /// <summary>
        /// Used to handle which tab we are on...
        /// </summary>
        /// <param name="sender">Where the message came from</param>
        /// <param name="e">The arguments from the caller</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void tcTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                if (e.TabPage == this.tbOutput)
                {
                    this.isLoading = true;
                    this.LoadOutputColumns();
                    this.isLoading = false;
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        #endregion

        #region dgvAvailableColumns Handlers

        /// <summary>
        /// Check/Uncheck all tick boxes that have been selected
        /// </summary>
        /// <param name="sender">Where the message came from</param>
        /// <param name="e">The arguments from the caller</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void dgvAvailableColumns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    Boolean clickedCellValue = !(Boolean)dgvAvailableColumns.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;

                    for (int i = 0; i < dgvAvailableColumns.RowCount; i++)
                    {
                        foreach (DataGridViewCell dgvCell in dgvAvailableColumns.Rows[i].Cells)
                        {
                            if ((dgvCell.ColumnIndex == 0) && dgvCell.Selected)
                            {
                                dgvCell.Value = clickedCellValue;
                                try
                                {
                                    // Get the check box
                                    DataGridViewCheckBoxCell checkBoxCell = dgvCell as DataGridViewCheckBoxCell;

                                    // Get the Cell which contains the columns name
                                    DataGridViewCell columnCell = this.dgvAvailableColumns.Rows[i].Cells[this.gridColumnAvailableColumns.Index];

                                    SetInputColumnArgs args = new SetInputColumnArgs();
                                    args.VirtualColumn = new DataFlowElement(columnCell.Value.ToString(), columnCell.Tag);

                                    if ((bool)checkBoxCell.Value)
                                    {
                                        // Set used columns if the checkbox is selected.
                                        this.SetColumns(args);
                                    }
                                    else
                                    {
                                        // Delete columns if the checkbox is unselected.
                                        this.DeleteColumns(args);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                                    this.CallErrorHandler.EndInvoke(res);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }
        #endregion

        #region Change Order Events
        /// <summary>
        /// Change the order of the items in the Has Columns grid
        /// </summary>
        /// <param name="sender">Who called me?</param>
        /// <param name="e">The event arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void btnUp_Click(object sender, EventArgs e)
        {
            int index;
            string value;
            try
            {
                if (dgvHashColumns.SelectedRows.Count > 0)
                {
                    if (dgvHashColumns.SelectedRows[0].Index > 0) // && (bool)dgvHashColumns.SelectedRows[0].Cells[this.dgvHashColumnsSelected.Index].Value)
                    {
                        index = this.dgvHashColumns.SelectedRows[0].Index;

                        // Push the change back to the OutputColumns Grid.
                        InputColumnElement[] inputColumns = ((OutputColumnElement)this.dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns;
                        for (int i = 0; i < inputColumns.Length; i++)
                        {
                            if (inputColumns[i].LineageID == (int)this.dgvHashColumns.Rows[index].Cells[dgvHashColumnsColumnName.Index].Tag)
                            {
                                inputColumns[i].SortPosition--;
                            }

                            if (inputColumns[i].LineageID == (int)this.dgvHashColumns.Rows[index - 1].Cells[dgvHashColumnsColumnName.Index].Tag)
                            {
                                inputColumns[i].SortPosition++;
                            }
                        }

                        // Update the data grid
                        value = this.dgvHashColumns.Rows[index - 1].Cells[this.dgvHashColumnsSortPosition.Index].Value.ToString();
                        this.dgvHashColumns.Rows[index - 1].Cells[this.dgvHashColumnsSortPosition.Index].Value = dgvHashColumns.Rows[index].Cells[this.dgvHashColumnsSortPosition.Index].Value;
                        this.dgvHashColumns.Rows[index].Cells[this.dgvHashColumnsSortPosition.Index].Value = value;
                        this.dgvHashColumns.Sort(dgvHashColumnsSortPosition, ListSortDirection.Ascending);

                        AlterOutputColumnArgs args = new AlterOutputColumnArgs();
                        args.OutputColumnDetail = (OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag;

                        // Call the AddOutputColumn event...
                        IAsyncResult res = this.AlterOutputColumn.BeginInvoke(this, args, null, null);
                        this.AlterOutputColumn.EndInvoke(res);
                    }
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        /// <summary>
        /// Change the order of the items in the Has Columns grid
        /// </summary>
        /// <param name="sender">Who called me?</param>
        /// <param name="e">The event arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void btnDown_Click(object sender, EventArgs e)
        {
            int index;
            string value;
            try
            {
                if (dgvHashColumns.SelectedRows.Count > 0)
                {
                    if (dgvHashColumns.SelectedRows[0].Index < dgvHashColumns.Rows.Count - 1) // && (bool)dgvHashColumns.SelectedRows[0].Cells[this.dgvHashColumnsSelected.Index].Value)
                    {
                        //if ((bool)dgvHashColumns.Rows[dgvHashColumns.SelectedRows[0].Index + 1].Cells[this.dgvHashColumnsSelected.Index].Value)
                        //{
                            index = dgvHashColumns.SelectedRows[0].Index;

                            // Push the change back to the OutputColumns Grid.
                            InputColumnElement[] inputColumns = ((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns;
                            for (int i = 0; i < inputColumns.Length; i++)
                            {
                                if (inputColumns[i].LineageID == (int)dgvHashColumns.Rows[index].Cells[dgvHashColumnsColumnName.Index].Tag)
                                {
                                    inputColumns[i].SortPosition++;
                                }

                                if (inputColumns[i].LineageID == (int)dgvHashColumns.Rows[index + 1].Cells[dgvHashColumnsColumnName.Index].Tag)
                                {
                                    inputColumns[i].SortPosition--;
                                }
                            }

                            // Update the data grid
                            value = dgvHashColumns.Rows[index + 1].Cells[this.dgvHashColumnsSortPosition.Index].Value.ToString();
                            dgvHashColumns.Rows[index + 1].Cells[this.dgvHashColumnsSortPosition.Index].Value = dgvHashColumns.Rows[index].Cells[this.dgvHashColumnsSortPosition.Index].Value;
                            dgvHashColumns.Rows[index].Cells[this.dgvHashColumnsSortPosition.Index].Value = value;
                            dgvHashColumns.Sort(dgvHashColumnsSortPosition, ListSortDirection.Ascending);

                            AlterOutputColumnArgs args = new AlterOutputColumnArgs();
                            args.OutputColumnDetail = (OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag;

                            // Call the AddOutputColumn event...
                            IAsyncResult res = this.AlterOutputColumn.BeginInvoke(this, args, null, null);
                            this.AlterOutputColumn.EndInvoke(res);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }
        #endregion

        #region dgvInputColumns Handlers

        /// <summary>
        /// Check/Uncheck all tick boxes that have been selected
        /// </summary>
        /// <param name="sender">Where the message came from</param>
        /// <param name="e">The arguments from the caller</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void dgvInputColumns_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int sortPosition = 0;
            try
            {
                if (e.ColumnIndex == 0)
                {
                    Boolean clickedCellValue = !(Boolean)dgvInputColumns.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;

                    // Set IsLoading to force order of execution and addition of columns to other lists.
                    //this.isLoading = true;

                    // Grab the tag that holds the list of selected columns.
                    InputColumnElement[] inputColumns = ((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns;

                    for (int i = 0; i < dgvInputColumns.RowCount; i++)
                    {
                        foreach (DataGridViewCell dgvCell in dgvInputColumns.Rows[i].Cells)
                        {
                            if ((dgvCell.ColumnIndex == 0) && dgvCell.Selected)
                            {
                                if ((bool)dgvCell.Value == clickedCellValue)  // Bug Fix: If multi selected, and this is already set to the target value, then skip.
                                {
                                    break;
                                }
                                dgvCell.Value = clickedCellValue;
                                if (e.ColumnIndex == dgvInputColumnsSelected.Index)
                                {
                                    // Iterate through that list, to apply the change
                                    for (int j = 0; j < inputColumns.Length; j++)
                                    {
                                        // Find the column that has been changed
                                        if (inputColumns[j].LineageID == (int)dgvInputColumns.Rows[i].Cells[dgvInputColumnsColumnName.Index].Tag)
                                        {
                                            sortPosition = inputColumns[j].SortPosition;
                                            if (inputColumns[j].Selected)
                                            {
                                                // I have unselected this one!
                                                for (int k = 0; k < inputColumns.Length; k++)
                                                {
                                                    if (inputColumns[k].SortPosition < 999999 && inputColumns[k].SortPosition > sortPosition)
                                                    {
                                                        inputColumns[k].SortPosition--;
                                                    }
                                                }

                                                inputColumns[j].SortPosition = 999999;
                                            }
                                            else
                                            {
                                                sortPosition = -1;
                                                Boolean noneFound = true;
                                                for (int k = 0; k < inputColumns.Length; k++)
                                                {
                                                    if (inputColumns[k].SortPosition > sortPosition && inputColumns[k].SortPosition < 999999)
                                                    {
                                                        sortPosition = inputColumns[k].SortPosition;
                                                        noneFound = false;
                                                    }
                                                }
                                                inputColumns[j].SortPosition = (noneFound ? 0 : sortPosition + 1);
                                            }
                                            inputColumns[j].Selected = (bool)dgvInputColumns.Rows[i].Cells[dgvInputColumnsSelected.Index].Value;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    this.RefreshdgvHashColumns();

                    AlterOutputColumnArgs args = new AlterOutputColumnArgs();
                    args.OutputColumnDetail = (OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag;

                    // Call the AddOutputColumn event...
                    IAsyncResult res = this.AlterOutputColumn.BeginInvoke(this, args, null, null);
                    this.AlterOutputColumn.EndInvoke(res);

                    //this.isLoading = false;
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                //this.isLoading = false;
            }
        }

        #endregion

        #region dgvHashColumns Handlers

        /// <summary>
        /// Commit the change immediately to improve UI interaction. 
        /// </summary>
        /// <param name="sender">Who called me?</param>
        /// <param name="e">The event arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void dgvHashColumns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvHashColumns.CurrentCell != null && this.dgvHashColumns.CurrentCell is DataGridViewCheckBoxCell)
                {
                    this.dgvHashColumns.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        #endregion

        #region dgvOutputColumns Handlers
        /// <summary>
        /// Fired when the selection changes.
        /// Call the function to refresh the hash columns...
        /// </summary>
        /// <param name="sender">Where the message came from</param>
        /// <param name="e">The arguments from the caller</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void dgvOutputColumns_SelectionChanged(object sender, EventArgs e)
        {
            // Ignoring this event while loading columns.
            if (this.isLoading)
            {
                return;
            }

            try
            {
                // Set isLoading to ensure other events don't fire and cause issues.
                this.isLoading = true;

                // If we have an Output Column...
                if (this.dgvOutputColumns.Rows.Count > 0)
                {
                    this.RefreshdgvInputColumns();
                    this.RefreshdgvHashColumns();
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                // Clear isLoading...
                this.isLoading = false;
            }
        }
        
        /// <summary>
        /// Fired when someone has changed a cell in the Output grid
        /// Handles the defaulting of cells if no data has been provided
        /// If there is no Tag, then this will create a new Output Column in the SSIS Component
        /// Otherwise, it will change the name or data type as required.
        /// </summary>
        /// <param name="sender">Who called me?</param>
        /// <param name="e">The event arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void dgvOutputColumns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ignoring this event while loading columns.
            if (this.isLoading)
            {
                return;
            }

            try
            {
                this.isLoading = true;

                if (e.RowIndex >= 0)
                {
                    // Get the text box and Hash Type
                    DataGridViewTextBoxCell textBoxCell = this.dgvOutputColumns.Rows[e.RowIndex].Cells[this.dgvOutputColumnsColumnName.Index] as DataGridViewTextBoxCell;
                    DataGridViewComboBoxCell comboCell = this.dgvOutputColumns.Rows[e.RowIndex].Cells[this.dgvOutputColumnsHashType.Index] as DataGridViewComboBoxCell;
                    DataGridViewComboBoxCell dataTypeCell = this.dgvOutputColumns.Rows[e.RowIndex].Cells[this.dgvOutputColumnsDataType.Index] as DataGridViewComboBoxCell;

                    // Default the text box data, if there isn't anything.
                    if (textBoxCell.Value == null)
                    {
                        textBoxCell.Value = "Output Column " + e.RowIndex.ToString();
                    }

                    // If there isn't an Output Column associated with this, then default it.
                    if (textBoxCell.Tag == null)
                    {
                        // Build the argument to the event
                        AddOutputColumnNameArgs args = new AddOutputColumnNameArgs();
                        args.OutputColumnDetail.OutputColumn = new DataFlowElement(textBoxCell.Value.ToString());

                        // Call the AddOutputColumn event...
                        IAsyncResult res = this.AddOutputColumn.BeginInvoke(this, args, null, null);
                        this.AddOutputColumn.EndInvoke(res);

                        // At this point we have created the output column in the Data Flow, but it isn't hooked to the Tag...
                        OutputColumnElement outputColumnRow = new OutputColumnElement();
                        outputColumnRow = args.OutputColumnDetail;
                        if (comboCell.Value != null)
                        {
                            outputColumnRow.Hash = MultipleHashForm.GetHashEnum(comboCell.Value.ToString());
                        }
                        else
                        {
                            outputColumnRow.Hash = MultipleHash.HashTypeEnumerator.None;
                            comboCell.Value = MultipleHashForm.GetHashName(MultipleHash.HashTypeEnumerator.None);
                        }

                        if (dataTypeCell.Value != null)
                        {
                            outputColumnRow.dataType = MultipleHashForm.GetDataTypeEnum(dataTypeCell.Value.ToString());
                        }
                        else
                        {
                            outputColumnRow.dataType = MultipleHash.OutputTypeEnumerator.Binary;
                            dataTypeCell.Value = MultipleHashForm.GetDataTypeName(MultipleHash.OutputTypeEnumerator.Binary);
                        }

                        // Filling the cells.
                        AlterOutputColumnArgs aocArgs = new AlterOutputColumnArgs();
                        aocArgs.OutputColumnDetail = outputColumnRow;

                        // Call the AlterOutputColumn event...
                        res = this.AlterOutputColumn.BeginInvoke(this, aocArgs, null, null);
                        this.AlterOutputColumn.EndInvoke(res);

                        // Push the altered column data to the grid...
                        SetGridCellData(this.dgvOutputColumns.Rows[textBoxCell.RowIndex].Cells[this.dgvOutputColumnsColumnName.Index], aocArgs.OutputColumnDetail);
                        this.RefreshdgvInputColumns();
                        this.RefreshdgvHashColumns();
                    }
                    else
                    {
                        OutputColumnElement outputColumnRow = (OutputColumnElement)textBoxCell.Tag;

                        // If the changed column is the Column Name column...
                        if (e.ColumnIndex == this.dgvOutputColumnsColumnName.Index)
                        {
                            // Push the change into the Tag...
                            outputColumnRow.OutputColumn.Name = textBoxCell.Value.ToString();
                        }

                        // If the changed column is the Hash column...
                        if (e.ColumnIndex == this.dgvOutputColumnsHashType.Index)
                        {
                            // Push the change into the Tag...
                            outputColumnRow.Hash = MultipleHashForm.GetHashEnum(comboCell.Value.ToString());
                        }

                        // If the changed column is the DataType column...
                        if (e.ColumnIndex == this.dgvOutputColumnsDataType.Index)
                        {
                            outputColumnRow.dataType = MultipleHashForm.GetDataTypeEnum(dataTypeCell.Value.ToString());
                        }

                        AlterOutputColumnArgs args = new AlterOutputColumnArgs();
                        args.OutputColumnDetail = outputColumnRow;

                        // Call the AlterOutputColumn event...
                        IAsyncResult res = this.AlterOutputColumn.BeginInvoke(this, args, null, null);
                        this.AlterOutputColumn.EndInvoke(res);
                        SetGridCellData(this.dgvOutputColumns.Rows[textBoxCell.RowIndex].Cells[this.dgvOutputColumnsColumnName.Index], args.OutputColumnDetail);
                        ////textBoxCell.Tag = args.OutputColumnDetail;
                    }
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
            finally
            {
                // Clear isLoading...
                this.isLoading = false;
            }
        }

        /// <summary>
        /// If an output column is deleted, then remove it from the SSIS Component.
        /// </summary>
        /// <param name="sender">Who called me?</param>
        /// <param name="e">The event arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void dgvOutputColumns_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                if (e.Row.Cells[this.dgvOutputColumnsColumnName.Index].Tag != null)
                {
                    DeleteOutputColumnArgs args = new DeleteOutputColumnArgs();
                    args.OutputColumnDetail = (OutputColumnElement)e.Row.Cells[this.dgvOutputColumnsColumnName.Index].Tag;

                    // Call the DeleteOutputColumn event...
                    IAsyncResult res = this.DeleteOutputColumn.BeginInvoke(this, args, null, null);
                    this.DeleteOutputColumn.EndInvoke(res);
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        #endregion

        /// <summary>
        /// Calls the home page
        /// </summary>
        /// <param name="sender">Who called me?</param>
        /// <param name="e">The event arguments</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "form Generated Code")]
        private void llCodeplex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ssismhash.codeplex.com/");
        }

        /// <summary>
        /// Fires when the threading is changed.  This pushes the value back to the component
        /// </summary>
        /// <param name="sender">Who sent the message</param>
        /// <param name="e">The arguments from the sender</param>
        private void cbThreading_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(cbThreading.Text))
                {
                    ThreadingArgs args = new ThreadingArgs();
                    args.threadDetail = MultipleHashForm.GetThreadingEnum(cbThreading.Text);

                    // Call the SetThreading event...
                    IAsyncResult res = this.SetThreadingDetail.BeginInvoke(this, args, null, null);
                    this.SetThreadingDetail.EndInvoke(res);
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        private void cbSafeNullHandling_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                SafeNullArgs args = new SafeNullArgs();
                args.safeNullHandlingDetail = MultipleHashForm.GetSafeNullEnum(cbSafeNullHandling.Checked);

                // Call the SetThreading event...
                IAsyncResult res = this.SetSafeNullHandlingDetail.BeginInvoke(this, args, null, null);
                this.SetSafeNullHandlingDetail.EndInvoke(res);
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        private void cbMilliseconds_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                HashMillisecondArgs args = new HashMillisecondArgs();
                args.millisecondHandlingDetail = MultipleHashForm.GetMillisecondEnum(cbMilliseconds.Checked);

                // Call the SetMilliseconds event...
                IAsyncResult res = this.SetMillisecondHandlingDetail.BeginInvoke(this, args, null, null);
                this.SetMillisecondHandlingDetail.EndInvoke(res);
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }

        #endregion

        #region Hash Value Helper Functions

        /// <summary>
        /// Returns the string value from the hashValue enum.
        /// </summary>
        /// <param name="hashValue">The HashTypeEnum value to return a string for</param>
        /// <returns>The string value for the HashTypeEnum</returns>
        static private string GetHashName(MultipleHash.HashTypeEnumerator hashValue)
        {
            switch (hashValue)
            {
                case MultipleHash.HashTypeEnumerator.MD5:
                    return "MD5";
                case MultipleHash.HashTypeEnumerator.RipeMD160:
                    return "RipeMD160";
                case MultipleHash.HashTypeEnumerator.SHA1:
                    return "SHA1";
                case MultipleHash.HashTypeEnumerator.SHA256:
                    return "SHA256";
                case MultipleHash.HashTypeEnumerator.SHA384:
                    return "SHA384";
                case MultipleHash.HashTypeEnumerator.SHA512:
                    return "SHA512";
                case MultipleHash.HashTypeEnumerator.CRC32:
                    return "CRC32";
                case MultipleHash.HashTypeEnumerator.CRC32C:
                    return "CRC32C";
                case MultipleHash.HashTypeEnumerator.FNV1a32:
                    return "FNV1a32";
                case MultipleHash.HashTypeEnumerator.FNV1a64:
                    return "FNV1a64";
                case MultipleHash.HashTypeEnumerator.MurmurHash3a:
                    return "MurmurHash3a";
                case MultipleHash.HashTypeEnumerator.xxHash:
                    return "xxHash";
                case MultipleHash.HashTypeEnumerator.None:
                default:
                    return "None";
            }
        }

        /// <summary>
        /// Returns the HashTypeEnum value for the passed in string hashValue
        /// </summary>
        /// <param name="hashValue">The string value for the HashTypeEnum</param>
        /// <returns>The HashTypeEnum value for the passed in string.</returns>
        static private MultipleHash.HashTypeEnumerator GetHashEnum(string hashValue)
        {
            switch (hashValue)
            {
                case "MD5":
                    return MultipleHash.HashTypeEnumerator.MD5;
                case "RipeMD160":
                    return MultipleHash.HashTypeEnumerator.RipeMD160;
                case "SHA1":
                    return MultipleHash.HashTypeEnumerator.SHA1;
                case "SHA256":
                    return MultipleHash.HashTypeEnumerator.SHA256;
                case "SHA384":
                    return MultipleHash.HashTypeEnumerator.SHA384;
                case "SHA512":
                    return MultipleHash.HashTypeEnumerator.SHA512;
                case "CRC32":
                    return MultipleHash.HashTypeEnumerator.CRC32;
                case "CRC32C":
                    return MultipleHash.HashTypeEnumerator.CRC32C;
                case "FNV1a32":
                    return MultipleHash.HashTypeEnumerator.FNV1a32;
                case "FNV1a64":
                    return MultipleHash.HashTypeEnumerator.FNV1a64;
                case "MurmurHash3a":
                    return MultipleHash.HashTypeEnumerator.MurmurHash3a;
                case "xxHash":
                    return MultipleHash.HashTypeEnumerator.xxHash;
                case "None":
                default:
                    return MultipleHash.HashTypeEnumerator.None;
            }
        }
        #endregion

        #region dataType Value Helper Functions

        /// <summary>
        /// Returns the string value from the outputValue enum.
        /// </summary>
        /// <param name="outputValue">The OutputTypeEnumerator value to return a string for</param>
        /// <returns>The string value for the OutputTypeEnumerator</returns>
        static private string GetDataTypeName(MultipleHash.OutputTypeEnumerator outputValue)
        {
            switch(outputValue)
            {
                case MultipleHash.OutputTypeEnumerator.Base64String:
                    return "Base64String";
                case MultipleHash.OutputTypeEnumerator.HexString:
                    return "HexString";
                case MultipleHash.OutputTypeEnumerator.Binary:
                default:
                    return "Binary";
            }
        }

        /// <summary>
        /// Returns the OutputTypeEnumerator value for the passed in string outputValue
        /// </summary>
        /// <param name="hashValue">The string value for the OutputTypeEnumerator</param>
        /// <returns>The OutputTypeEnumerator value for the passed in string.</returns>
        static private MultipleHash.OutputTypeEnumerator GetDataTypeEnum(string outputValue)
        {
            switch (outputValue)
            {
                case "Base64String":
                    return MultipleHash.OutputTypeEnumerator.Base64String;
                case "HexString":
                    return MultipleHash.OutputTypeEnumerator.HexString;
                case "Binary":
                default:
                    return MultipleHash.OutputTypeEnumerator.Binary;
            }
        }
        #endregion

        #region Threading Value Helper Functions
        /// <summary>
        /// Returns the string value from the MultipleThread enum.
        /// </summary>
        /// <param name="threadingValue">The MultipleThread value to return a string for</param>
        /// <returns>The string value for the MultipleThread</returns>
        static private string GetThreadingName(MultipleHash.MultipleThread threadingValue)
        {
            switch (threadingValue)
            {
                case MultipleHash.MultipleThread.Auto:
                    return "Auto";
                case MultipleHash.MultipleThread.On:
                    return "On";
                case MultipleHash.MultipleThread.None:
                default:
                    return "None";
            }
        }

        /// <summary>
        /// Returns the MultipleThread value for the passed in string threadingValue
        /// </summary>
        /// <param name="hashValue">The string value for the MultipleThread</param>
        /// <returns>The MultipleThread value for the passed in string.</returns>
        static private MultipleHash.MultipleThread GetThreadingEnum(string threadingValue)
        {
            switch (threadingValue)
            {
                case "Auto":
                    return MultipleHash.MultipleThread.Auto;
                case "On":
                    return MultipleHash.MultipleThread.On;
                case "None":
                default:
                    return MultipleHash.MultipleThread.None;

            }
        }
        #endregion

        #region Safe Null Handling Helper Functions

        /// <summary>
        /// Returns a bool to indicate the value for the Safe Null Handling parameter
        /// </summary>
        /// <param name="safeNullValue">The enum value for safe null handling</param>
        /// <returns>True or False based on the Safe Null Handling</returns>
        static private bool GetSafeNullValue(MultipleHash.SafeNullHandling safeNullValue)
        {
            switch (safeNullValue)
            {
                case MultipleHash.SafeNullHandling.False:
                    return false;
                case MultipleHash.SafeNullHandling.True:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="safeNullValue"></param>
        /// <returns></returns>
        static private MultipleHash.SafeNullHandling GetSafeNullEnum(bool safeNullValue)
        {
            switch (safeNullValue)
            {
                case true:
                    return MultipleHash.SafeNullHandling.True;
                default:
                    return MultipleHash.SafeNullHandling.False;
            }
        }

        #endregion

        #region Output Type Enum Helper Functions

        /// <summary>
        /// Returns the string value from the OutputTypeEnumerator enum.
        /// </summary>
        /// <param name="outputTypeValue">The OutputTypeEnumerator value to return a string for</param>
        /// <returns>The string value for the OutputTypeEnumerator</returns>
        static private string GetOutputTypeName(MultipleHash.OutputTypeEnumerator outputTypeValue)
        {
            switch(outputTypeValue)
            {
                case MultipleHash.OutputTypeEnumerator.Base64String:
                    return "Base64";
                case MultipleHash.OutputTypeEnumerator.HexString:
                    return "HexString";
                case MultipleHash.OutputTypeEnumerator.Binary:
                default:
                    return "Binary";
            }
        }

        /// <summary>
        /// Returns the OutputTypeEnumerator value for the passed in string hashValue
        /// </summary>
        /// <param name="outputTypeValue">The string value for the OutputTypeEnumerator</param>
        /// <returns>The OutputTypeEnumerator value for the passed in string.</returns>
        static private MultipleHash.OutputTypeEnumerator GetOutputTypeEnum(string outputTypeValue)
        {
            switch (outputTypeValue)
            {
                case "Base64":
                    return MultipleHash.OutputTypeEnumerator.Base64String;
                case "HexString":
                    return MultipleHash.OutputTypeEnumerator.HexString;
                case "Binary":
                default:
                    return MultipleHash.OutputTypeEnumerator.Binary;
            }
        }
        #endregion

        #region GetSafeMillisecondEnum
        /// <summary>
        /// Returns a bool to indicate the value for the Millisecond Handling parameter
        /// </summary>
        /// <param name="millisecondValue">The enum value for Millisecond handling</param>
        /// <returns>True or False based on the Millisecond Handling</returns>
        static private bool GetMillisecondValue(MultipleHash.MillisecondHandling millisecondValue)
        {
            switch (millisecondValue)
            {
                case MultipleHash.MillisecondHandling.False:
                    return false;
                case MultipleHash.MillisecondHandling.True:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="millisecondValue"></param>
        /// <returns></returns>
        static private MultipleHash.MillisecondHandling GetMillisecondEnum(bool millisecondValue)
        {
            switch (millisecondValue)
            {
                case true:
                    return MultipleHash.MillisecondHandling.True;
                default:
                    return MultipleHash.MillisecondHandling.False;
            }
        }
        #endregion

        private void tpAbout_Click(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// Class to pass the input column arguments
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Fields are used for data transfer only")]
    internal class InputColumnsArgs
    {
        /// <summary>
        /// The set of input columns
        /// </summary>
        public InputColumnElement[] InputColumns;
    }

    /// <summary>
    /// Class to pass the output column arguments
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Fields are used for data transfer only")]
    internal class OutputColumnsArgs
    {
        /// <summary>
        /// The set of output columns
        /// </summary>
        public OutputColumnElement[] OutputColumns;
    }

    /// <summary>
    /// Class to pass the input column arguments
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Fields are used for data transfer only")]
    internal class SetInputColumnArgs
    {
        /// <summary>
        /// The set of virtual columns
        /// </summary>
        public DataFlowElement VirtualColumn;

        /// <summary>
        /// The set of selected input columns
        /// </summary>
        public SelectedInputColumns GeneratedColumns;

        /// <summary>
        /// Should we cancel the action?
        /// </summary>
        public bool CancelAction;
    }

    /// <summary>
    /// Class to pass the Add arguments
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Fields are used for data transfer only")]
    internal class AddOutputColumnNameArgs
    {
        /// <summary>
        /// The detail for the Output to add
        /// </summary>
        public OutputColumnElement OutputColumnDetail;

        /// <summary>
        /// Should we cancel the action?
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public bool CancelAction;
    }

    /// <summary>
    /// Class to pass the Alter arguments
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Fields are used for data transfer only")]
    internal class AlterOutputColumnArgs
    {
        /// <summary>
        /// The detail for the column to alter
        /// </summary>
        public OutputColumnElement OutputColumnDetail;

        /// <summary>
        /// Should we cancel the action?
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public bool CancelAction;
    }

    /// <summary>
    /// Class to pass the Delete Arguments
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Fields are used for data transfer only")]
    internal class DeleteOutputColumnArgs
    {
        /// <summary>
        /// The detail for the column to delete
        /// </summary>
        public OutputColumnElement OutputColumnDetail;

        /// <summary>
        /// Should we cancel the action?
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        public bool CancelAction;
    }
    
    /// <summary>
    /// Class to pass the Thread Details
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Fields are used for data transfer only")]
    internal class ThreadingArgs
    {
        /// <summary>
        /// The threading value to pass
        /// </summary>
        public MultipleHash.MultipleThread threadDetail;
    }

    /// <summary>
    /// Class to pass the Safe Null Handling details.
    /// </summary>
    internal class SafeNullArgs
    {
        /// <summary>
        /// The safe Null Handling value to pass
        /// </summary>
        public MultipleHash.SafeNullHandling safeNullHandlingDetail;
    }

    /// <summary>
    /// Class to pass the Millseconds Boolean around
    /// </summary>
    internal class HashMillisecondArgs
    {
        /// <summary>
        /// The Milliseconds to pass around.
        /// </summary>
        public MultipleHash.MillisecondHandling millisecondHandlingDetail;
    }
}
