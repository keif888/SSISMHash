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
using System.ComponentModel;
using System.Windows.Forms; 
#endregion

namespace Martin.SQLServer.Dts
{
    public partial class MultipleHashForm : Form
    {

        #region Exposed events

        /// There are 7 required events:
        /// GetInputColumns - Retrieves all the input columns from the SSIS data flow object
        /// SetInputColumn - Passes alterations to input columns back to the SSIS data flow object
        /// DeleteInputColumn - Removes an input column from the SSIS data flow object
        /// GetOutputColumns - Retreives all the output columns from the SSIS data flow object
        /// AddOutputColumn - Adds a new output column to the SSIS data flow object
        /// AlterOutputColumn - Passes alterations to an output column back to the SSIS data flow object
        /// DeleteOutputColumn - Removes an output column from the SSIS data flow object


        internal event GetInputColumnsEventHandler GetInputColumns;
        internal event ChangeInputColumnEventHandler SetInputColumn;
        internal event ChangeInputColumnEventHandler DeleteInputColumn;
        internal event GetOutputColumnsEventHandler GetOutputColumns;
        internal event AddOutputColumnEventHandler AddOutputColumn;
        internal event AlterOutputColumnEventHandler AlterOutputColumn;
        internal event DeleteOutputColumnEventHandler DeleteOutputColumn;
        internal event ErrorEventHandler CallErrorHandler;

        #endregion

        // This flag is set to true while loading the state from the component, to disable
        // the grid events during that time.
        // it is also used when updating the grids, to prevent other grids firing events.

        bool isLoading;

        public MultipleHashForm()
        {
            InitializeComponent();
        }

        #region Helper functions

        /// <summary>
        /// Hooking up a data flow element to a grid cell.
        /// </summary>
        /// <param name="cell">Where to put the DataFlowElement</param>
        /// <param name="dataFlowElement"></param>
        private static void SetGridCellData(DataGridViewCell cell, DataFlowElement dataFlowElement)
        {
                cell.Value = dataFlowElement.ToString();
                cell.Tag = dataFlowElement.Tag;
                cell.ToolTipText = dataFlowElement.ToolTip;
        }

        /// <summary>
        /// Hooking up an Output Column Element to a grid cell.
        /// Used on the dgvOutputColumns
        /// </summary>
        /// <param name="cell">Where to put the OutputColumnElement</param>
        /// <param name="outputColumnElement"></param>
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
                            this.dgvOutputColumns.Rows[i].Cells[this.dgvOutputColumnsHashType.Index].Value = GetHashName(outputColumnRow.Hash);
                        }
                    }
                    this.dgvOutputColumns.ResumeLayout();
                    RefreshdgvHashColumns();
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
        /// <param name="args"></param>
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
        /// <param name="args"></param>
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
        /// Refreshes the contents of the dgvHashColumns grid.
        /// </summary>
        private void RefreshdgvHashColumns()
        {
            bool blnCurrentisLoading = this.isLoading;
            try
            {
                // Load the data into the dgvOutputColumns grid.
                this.isLoading = true;
                dgvHashColumns.SuspendLayout();
                dgvHashColumns.Rows.Clear();
                // Make sure that something is selected in the Output Columns. 
                // Default to the first row if nothing is already selected.
                if (dgvOutputColumns.SelectedRows.Count == 0)
                {
                    dgvOutputColumns.Rows[0].Selected = true;
                }
                // If we have a Tag (for when there are no output columns yet.
                if (dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag != null)
                {
                    // If we have input columns...
                    if (((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns.Length > 0)
                    {
                        // Add all the required output rows from the Tag, which holds an OutputColumnElement
                        dgvHashColumns.Rows.Add(((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns.Length);
                        // Now push all the rows into the grid
                        for (int i = 0; i < ((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns.Length; i++)
                        {
                            OutputColumnElement outputColumnRow = ((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag);

                            // Filling the cells.
                            SetGridCellData(this.dgvHashColumns.Rows[i].Cells[this.dgvHashColumnsColumnName.Index], outputColumnRow.InputColumns[i].InputColumn);
                            this.dgvHashColumns.Rows[i].Cells[this.dgvHashColumnsSelected.Index].Value = outputColumnRow.InputColumns[i].Selected;
                            this.dgvHashColumns.Rows[i].Cells[this.dgvHashColumnsSortPosition.Index].Value = outputColumnRow.InputColumns[i].SortPosition.ToString("D6");
                        }
                    }
                }
                // Sort by the sort column, and then resume layout...
                dgvHashColumns.Sort(dgvHashColumnsSortPosition, ListSortDirection.Ascending);
                dgvHashColumns.ResumeLayout();
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

        #region Overridden methods

        /// <summary>
        /// This get's fired as the OnLoad event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.isLoading = true;
            try
            {
                // Loading available and previously selected columns.
                this.LoadAvailableColumns();
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

        #region Event Handlers

        #region tcTabs Handlers

        /// <summary>
        /// Used to handle which tab we are on...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tcTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                if (e.TabPage == tbOutput)
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
        /// Commiting change in the checkbox columns so it will trigger CellValueChanged immediately.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAvailableColumns_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                // If this is a check box, then commit now.
                if (this.dgvAvailableColumns.CurrentCell != null && this.dgvAvailableColumns.CurrentCell is DataGridViewCheckBoxCell)
                {
                    this.dgvAvailableColumns.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }


        /// <summary>
        /// Fired when a check box is selected on the Input tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAvailableColumns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ignoring this event while loading columns.
            if (this.isLoading)
            {
                return;
            }

            try
            {
                // If the changed column is the Check Box column...
                if (e.ColumnIndex == this.gridColumnCheckbox.Index && e.RowIndex >= 0)
                {
                    // Get the check box
                    DataGridViewCheckBoxCell checkBoxCell = this.dgvAvailableColumns.CurrentCell as DataGridViewCheckBoxCell;
                    // Get the Cell which contains the columns name
                    DataGridViewCell columnCell = this.dgvAvailableColumns.Rows[e.RowIndex].Cells[this.gridColumnAvailableColumns.Index];

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
            }
            catch (Exception ex)
            {
                IAsyncResult res = this.CallErrorHandler.BeginInvoke(this, ex, null, null);
                this.CallErrorHandler.EndInvoke(res);
            }
        }


        /// <summary>
        /// Fired when the selection changes.
        /// Call the function to refresh the hash columns...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                if (dgvOutputColumns.Rows.Count > 0)
                {
                    RefreshdgvHashColumns();
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



        #endregion

        #region Change Order Events
        /// <summary>
        /// Change the order of the items in the Has Columns grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            int index;
            string value;
            try
            {
                if (dgvHashColumns.SelectedRows.Count > 0)
                {
                    if (dgvHashColumns.SelectedRows[0].Index > 0 && (bool)dgvHashColumns.SelectedRows[0].Cells[this.dgvHashColumnsSelected.Index].Value)
                    {
                        index = dgvHashColumns.SelectedRows[0].Index;
                        // Push the change back to the OutputColumns Grid.
                        InputColumnElement[] inputColumns = ((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns;
                        for (int i = 0; i < inputColumns.Length; i++)
                        {
                            if (inputColumns[i].InputColumn.Tag == dgvHashColumns.Rows[index].Cells[dgvHashColumnsColumnName.Index].Tag)
                                inputColumns[i].SortPosition--;
                            if (inputColumns[i].InputColumn.Tag == dgvHashColumns.Rows[index - 1].Cells[dgvHashColumnsColumnName.Index].Tag)
                                inputColumns[i].SortPosition++;
                        }
                        // Update the data grid
                        value = dgvHashColumns.Rows[index - 1].Cells[this.dgvHashColumnsSortPosition.Index].Value.ToString();
                        dgvHashColumns.Rows[index - 1].Cells[this.dgvHashColumnsSortPosition.Index].Value = dgvHashColumns.Rows[index].Cells[this.dgvHashColumnsSortPosition.Index].Value;
                        dgvHashColumns.Rows[index].Cells[this.dgvHashColumnsSortPosition.Index].Value = value;
                        dgvHashColumns.Sort(dgvHashColumnsSortPosition, ListSortDirection.Ascending);

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDown_Click(object sender, EventArgs e)
        {
            int index;
            string value;
            try
            {
                if (dgvHashColumns.SelectedRows.Count > 0)
                {
                    if (dgvHashColumns.SelectedRows[0].Index < dgvHashColumns.Rows.Count-1 && (bool)dgvHashColumns.SelectedRows[0].Cells[this.dgvHashColumnsSelected.Index].Value)
                    {
                        if ((bool)dgvHashColumns.Rows[dgvHashColumns.SelectedRows[0].Index + 1].Cells[this.dgvHashColumnsSelected.Index].Value)
                        {
                            index = dgvHashColumns.SelectedRows[0].Index;
                            // Push the change back to the OutputColumns Grid.
                            InputColumnElement[] inputColumns = ((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns;
                            for (int i = 0; i < inputColumns.Length; i++)
                            {
                                if (inputColumns[i].InputColumn.Tag == dgvHashColumns.Rows[index].Cells[dgvHashColumnsColumnName.Index].Tag)
                                    inputColumns[i].SortPosition++;
                                if (inputColumns[i].InputColumn.Tag == dgvHashColumns.Rows[index + 1].Cells[dgvHashColumnsColumnName.Index].Tag)
                                    inputColumns[i].SortPosition--;
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

        #region dgvHashColumns Handlers
        
        /// <summary>
        /// This is fired when someone changes a tick on the Hash Columns.
        /// Resets the sort order, with new columns at the end of the already selected columns.
        /// Removal of a column, will decrement all columns after that ones sort order...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHashColumns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Ignoring this event while loading columns.
            if (this.isLoading)
            {
                return;
            }

            try
            {
                if (e.ColumnIndex == dgvHashColumnsSelected.Index)
                {
                    // prevent subsequent fires from doing anything whilst we work here.
                    this.isLoading = true;
                    // Save the top of the grid that is displayed to reset later.
                    int currentRow = dgvHashColumns.CurrentCell.RowIndex;

                    int index = e.RowIndex;
                    // Push the change back to the OutputColumns Tag.
                    InputColumnElement[] inputColumns = ((OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag).InputColumns;
                    for (int i = 0; i < inputColumns.Length; i++)
                    {
                        if (inputColumns[i].InputColumn.Tag == dgvHashColumns.Rows[index].Cells[dgvHashColumnsColumnName.Index].Tag)
                        {
                            inputColumns[i].Selected = (bool)dgvHashColumns.Rows[index].Cells[dgvHashColumnsSelected.Index].Value;
                            int sortPosition = 0;
                            if (inputColumns[i].Selected)
                            {
                                for (int j = 0; j < inputColumns.Length; j++)
                                {
                                    if (inputColumns[j].SortPosition < 999999 && inputColumns[j].SortPosition > sortPosition)
                                        sortPosition = inputColumns[j].SortPosition;
                                }
                                inputColumns[i].SortPosition = sortPosition + 1;
                            }
                            else
                            {
                                sortPosition = inputColumns[i].SortPosition;
                                for (int j = 0; j < inputColumns.Length; j++)
                                {
                                    if (inputColumns[j].SortPosition > sortPosition && inputColumns[j].SortPosition < 999999)
                                        inputColumns[j].SortPosition--;
                                }
                                inputColumns[i].SortPosition = 999999;
                            }
                            break;
                        }
                    }

                    RefreshdgvHashColumns();

                    AlterOutputColumnArgs args = new AlterOutputColumnArgs();
                    args.OutputColumnDetail = (OutputColumnElement)dgvOutputColumns.SelectedRows[0].Cells[this.dgvOutputColumnsColumnName.Index].Tag;
                    // Call the AddOutputColumn event...
                    IAsyncResult res = this.AlterOutputColumn.BeginInvoke(this, args, null, null);
                    this.AlterOutputColumn.EndInvoke(res);
                    dgvHashColumns.ClearSelection();
                    dgvHashColumns.CurrentCell = dgvHashColumns.Rows[currentRow].Cells[0];
                    dgvHashColumns.FirstDisplayedCell = dgvHashColumns.CurrentCell;
                }
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

        // Commit the change immediately to improve UI interaction.
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
        /// Fired when someone has changed a cell in the Output grid
        /// Handles the defaulting of cells if no data has been provided
        /// If there is no Tag, then this will create a new Output Column in the SSIS Component
        /// Otherwise, it will change the name or data type as required.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                            outputColumnRow.Hash = GetHashEnum(comboCell.Value.ToString());
                        else
                        {
                            outputColumnRow.Hash = MultipleHash.HashTypeEnum.None;
                            comboCell.Value = GetHashName(MultipleHash.HashTypeEnum.None);
                        }
                        // Filling the cells.
                        AlterOutputColumnArgs AOCargs = new AlterOutputColumnArgs();
                        AOCargs.OutputColumnDetail = outputColumnRow;
                        // Call the AlterOutputColumn event...
                        res = this.AlterOutputColumn.BeginInvoke(this, AOCargs, null, null);
                        this.AlterOutputColumn.EndInvoke(res);
                        // Push the altered column data to the grid...
                        SetGridCellData(this.dgvOutputColumns.Rows[textBoxCell.RowIndex].Cells[this.dgvOutputColumnsColumnName.Index], AOCargs.OutputColumnDetail);
                        RefreshdgvHashColumns();
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
                            outputColumnRow.Hash = GetHashEnum(comboCell.Value.ToString());
                        }
                        AlterOutputColumnArgs args = new AlterOutputColumnArgs();
                        args.OutputColumnDetail = outputColumnRow;
                        // Call the AlterOutputColumn event...
                        IAsyncResult res = this.AlterOutputColumn.BeginInvoke(this, args, null, null);
                        this.AlterOutputColumn.EndInvoke(res);
                        SetGridCellData(this.dgvOutputColumns.Rows[textBoxCell.RowIndex].Cells[this.dgvOutputColumnsColumnName.Index], args.OutputColumnDetail);
                        //textBoxCell.Tag = args.OutputColumnDetail;
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void llCodeplex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://ssismhash.codeplex.com/");
        }

        #endregion

        #region Hash Value Helper Functions

        /// <summary>
        /// Returns the string value from the hashValue enum.
        /// </summary>
        /// <param name="hashValue">The HashTypeEnum value to return a string for</param>
        /// <returns>The string value for the HashTypeEnum</returns>
        private string GetHashName(MultipleHash.HashTypeEnum hashValue)
        {
            switch (hashValue)
            {
                case MultipleHash.HashTypeEnum.MD5:
                    return "MD5";
                case MultipleHash.HashTypeEnum.RipeMD160:
                    return "RipeMD160";
                case MultipleHash.HashTypeEnum.SHA1:
                    return "SHA1";
                case MultipleHash.HashTypeEnum.SHA256:
                    return "SHA256";
                case MultipleHash.HashTypeEnum.SHA384:
                    return "SHA384";
                case MultipleHash.HashTypeEnum.SHA512:
                    return "SHA512";
                case MultipleHash.HashTypeEnum.None:
                default:
                    return "None";
            }
        }


        /// <summary>
        /// Returns the HashTypeEnum value for the passed in string hashValue
        /// </summary>
        /// <param name="hashValue">The string value for the HashTypeEnum</param>
        /// <returns>The HashTypeEnum value for the passed in string.</returns>
        private MultipleHash.HashTypeEnum GetHashEnum(string hashValue)
        {
            switch (hashValue)
            {

                case "MD5":
                    return MultipleHash.HashTypeEnum.MD5;
                case "RipeMD160":
                    return MultipleHash.HashTypeEnum.RipeMD160;
                case "SHA1":
                    return MultipleHash.HashTypeEnum.SHA1;
                case "SHA256":
                    return MultipleHash.HashTypeEnum.SHA256;
                case "SHA384":
                    return MultipleHash.HashTypeEnum.SHA384;
                case "SHA512":
                    return MultipleHash.HashTypeEnum.SHA512;
                case "None":
                default:
                    return MultipleHash.HashTypeEnum.None;
            }
        } 
        #endregion


    }

    # region Helper structures and delegates

    internal struct InputColumnElement
    {
        public bool Selected;
        public Int32 LineageID;
        public Int32 SortPosition;
        public DataFlowElement InputColumn;
    }

    internal struct SelectedInputColumns
    {
        public DataFlowElement InputColumn;
    }

    internal class InputColumnsArgs
    {
        public InputColumnElement[] InputColumns;
    }

    internal delegate void GetInputColumnsEventHandler(object sender, InputColumnsArgs args);

    internal struct OutputColumnElement
    {
        public MultipleHash.HashTypeEnum Hash;
        public DataFlowElement OutputColumn;
        public InputColumnElement[] InputColumns;
    }

    internal class OutputColumnsArgs
    {
        public OutputColumnElement[] OutputColumns;
    }

    internal delegate void GetOutputColumnsEventHandler(object sender, OutputColumnsArgs args);

    internal class SetInputColumnArgs
    {
        public DataFlowElement VirtualColumn;
        public SelectedInputColumns GeneratedColumns;
        public bool CancelAction;
    }

    internal delegate void ChangeInputColumnEventHandler(object sender, SetInputColumnArgs args);

    internal class AddOutputColumnNameArgs
    {
        public OutputColumnElement OutputColumnDetail;
        public bool CancelAction;
    }

    internal delegate void AddOutputColumnEventHandler(object sender, AddOutputColumnNameArgs args);

    internal class AlterOutputColumnArgs
    {
        public OutputColumnElement OutputColumnDetail;
        public bool CancelAction;
    }

    internal delegate void AlterOutputColumnEventHandler (object sender, AlterOutputColumnArgs args);

    internal class DeleteOutputColumnArgs
    {
        public OutputColumnElement OutputColumnDetail;
        public bool CancelAction;
    }

    internal delegate void DeleteOutputColumnEventHandler (object sender, DeleteOutputColumnArgs args);

    internal delegate void ErrorEventHandler (object sender, Exception ex);

    #endregion
}
