namespace Martin.SQLServer.Dts
{
    partial class MultipleHashForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleHashForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tbInput = new System.Windows.Forms.TabPage();
            this.dgvAvailableColumns = new System.Windows.Forms.DataGridView();
            this.gridColumnCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gridColumnAvailableColumns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbOutput = new System.Windows.Forms.TabPage();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.dgvHashColumns = new System.Windows.Forms.DataGridView();
            this.dgvHashColumnsSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvHashColumnsColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHashColumnsSortPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOutputColumns = new System.Windows.Forms.DataGridView();
            this.dgvOutputColumnsColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOutputColumnsHashType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.tcTabs.SuspendLayout();
            this.tbInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableColumns)).BeginInit();
            this.tbOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputColumns)).BeginInit();
            this.tpAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(523, 355);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(442, 355);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // tcTabs
            // 
            this.tcTabs.Controls.Add(this.tbInput);
            this.tcTabs.Controls.Add(this.tbOutput);
            this.tcTabs.Controls.Add(this.tpAbout);
            this.tcTabs.Location = new System.Drawing.Point(13, 12);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new System.Drawing.Size(585, 337);
            this.tcTabs.TabIndex = 0;
            this.tcTabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tcTabs_Selecting);
            // 
            // tbInput
            // 
            this.tbInput.Controls.Add(this.dgvAvailableColumns);
            this.tbInput.Location = new System.Drawing.Point(4, 22);
            this.tbInput.Name = "tbInput";
            this.tbInput.Padding = new System.Windows.Forms.Padding(3);
            this.tbInput.Size = new System.Drawing.Size(577, 311);
            this.tbInput.TabIndex = 0;
            this.tbInput.Text = "Input Columns";
            this.tbInput.UseVisualStyleBackColor = true;
            // 
            // dgvAvailableColumns
            // 
            this.dgvAvailableColumns.AllowUserToAddRows = false;
            this.dgvAvailableColumns.AllowUserToDeleteRows = false;
            this.dgvAvailableColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAvailableColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridColumnCheckbox,
            this.gridColumnAvailableColumns});
            this.dgvAvailableColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAvailableColumns.Location = new System.Drawing.Point(3, 3);
            this.dgvAvailableColumns.Name = "dgvAvailableColumns";
            this.dgvAvailableColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvailableColumns.Size = new System.Drawing.Size(571, 305);
            this.dgvAvailableColumns.TabIndex = 0;
            this.dgvAvailableColumns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAvailableColumns_CellValueChanged);
            this.dgvAvailableColumns.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvAvailableColumns_CurrentCellDirtyStateChanged);
            // 
            // gridColumnCheckbox
            // 
            this.gridColumnCheckbox.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.gridColumnCheckbox.HeaderText = "";
            this.gridColumnCheckbox.MinimumWidth = 20;
            this.gridColumnCheckbox.Name = "gridColumnCheckbox";
            this.gridColumnCheckbox.Width = 50;
            // 
            // gridColumnAvailableColumns
            // 
            this.gridColumnAvailableColumns.HeaderText = "Available Columns";
            this.gridColumnAvailableColumns.Name = "gridColumnAvailableColumns";
            this.gridColumnAvailableColumns.ReadOnly = true;
            this.gridColumnAvailableColumns.Width = 450;
            // 
            // tbOutput
            // 
            this.tbOutput.Controls.Add(this.btnDown);
            this.tbOutput.Controls.Add(this.btnUp);
            this.tbOutput.Controls.Add(this.dgvHashColumns);
            this.tbOutput.Controls.Add(this.dgvOutputColumns);
            this.tbOutput.Controls.Add(this.label3);
            this.tbOutput.Location = new System.Drawing.Point(4, 22);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tbOutput.Size = new System.Drawing.Size(577, 311);
            this.tbOutput.TabIndex = 1;
            this.tbOutput.Text = "Output Columns";
            this.tbOutput.UseVisualStyleBackColor = true;
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(495, 280);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 10;
            this.btnDown.Text = "Move Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(310, 281);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 9;
            this.btnUp.Text = "Move Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // dgvHashColumns
            // 
            this.dgvHashColumns.AllowUserToAddRows = false;
            this.dgvHashColumns.AllowUserToDeleteRows = false;
            this.dgvHashColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHashColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvHashColumnsSelected,
            this.dgvHashColumnsColumnName,
            this.dgvHashColumnsSortPosition});
            this.dgvHashColumns.Location = new System.Drawing.Point(309, 24);
            this.dgvHashColumns.Name = "dgvHashColumns";
            this.dgvHashColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHashColumns.Size = new System.Drawing.Size(262, 250);
            this.dgvHashColumns.TabIndex = 8;
            this.dgvHashColumns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHashColumns_CellValueChanged);
            this.dgvHashColumns.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvHashColumns_CurrentCellDirtyStateChanged);
            // 
            // dgvHashColumnsSelected
            // 
            this.dgvHashColumnsSelected.HeaderText = "";
            this.dgvHashColumnsSelected.Name = "dgvHashColumnsSelected";
            this.dgvHashColumnsSelected.ToolTipText = "Untick to exclude this column from the hash.";
            this.dgvHashColumnsSelected.Width = 25;
            // 
            // dgvHashColumnsColumnName
            // 
            this.dgvHashColumnsColumnName.HeaderText = "Input Column";
            this.dgvHashColumnsColumnName.MinimumWidth = 50;
            this.dgvHashColumnsColumnName.Name = "dgvHashColumnsColumnName";
            this.dgvHashColumnsColumnName.ReadOnly = true;
            this.dgvHashColumnsColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvHashColumnsColumnName.Width = 175;
            // 
            // dgvHashColumnsSortPosition
            // 
            this.dgvHashColumnsSortPosition.HeaderText = "Sort Position";
            this.dgvHashColumnsSortPosition.Name = "dgvHashColumnsSortPosition";
            this.dgvHashColumnsSortPosition.ReadOnly = true;
            // 
            // dgvOutputColumns
            // 
            this.dgvOutputColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutputColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvOutputColumnsColumnName,
            this.dgvOutputColumnsHashType});
            this.dgvOutputColumns.Location = new System.Drawing.Point(12, 24);
            this.dgvOutputColumns.MultiSelect = false;
            this.dgvOutputColumns.Name = "dgvOutputColumns";
            this.dgvOutputColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOutputColumns.Size = new System.Drawing.Size(291, 281);
            this.dgvOutputColumns.TabIndex = 7;
            this.dgvOutputColumns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOutputColumns_CellValueChanged);
            this.dgvOutputColumns.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvOutputColumns_UserDeletingRow);
            this.dgvOutputColumns.SelectionChanged += new System.EventHandler(this.dgvOutputColumns_SelectionChanged);
            // 
            // dgvOutputColumnsColumnName
            // 
            this.dgvOutputColumnsColumnName.HeaderText = "Column Name";
            this.dgvOutputColumnsColumnName.MinimumWidth = 30;
            this.dgvOutputColumnsColumnName.Name = "dgvOutputColumnsColumnName";
            this.dgvOutputColumnsColumnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dgvOutputColumnsColumnName.ToolTipText = "Enter the name for this output column";
            // 
            // dgvOutputColumnsHashType
            // 
            this.dgvOutputColumnsHashType.HeaderText = "Hash";
            this.dgvOutputColumnsHashType.Items.AddRange(new object[] {
            "None",
            "MD5",
            "RipeMD160",
            "SHA1",
            "SHA256",
            "SHA384",
            "SHA512"});
            this.dgvOutputColumnsHashType.MinimumWidth = 30;
            this.dgvOutputColumnsHashType.Name = "dgvOutputColumnsHashType";
            this.dgvOutputColumnsHashType.ToolTipText = "Select the hash value to be applied to this output column";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Output Columns";
            // 
            // tpAbout
            // 
            this.tpAbout.Controls.Add(this.label8);
            this.tpAbout.Location = new System.Drawing.Point(4, 22);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tpAbout.Size = new System.Drawing.Size(577, 311);
            this.tpAbout.TabIndex = 2;
            this.tpAbout.Text = "About";
            this.tpAbout.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(546, 247);
            this.label8.TabIndex = 6;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // MultipleHashForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(610, 390);
            this.Controls.Add(this.tcTabs);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MultipleHashForm";
            this.Text = "MultipleHashForm";
            this.tcTabs.ResumeLayout(false);
            this.tbInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableColumns)).EndInit();
            this.tbOutput.ResumeLayout(false);
            this.tbOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputColumns)).EndInit();
            this.tpAbout.ResumeLayout(false);
            this.tpAbout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabControl tcTabs;
        private System.Windows.Forms.TabPage tbInput;
        private System.Windows.Forms.DataGridView dgvAvailableColumns;
        private System.Windows.Forms.DataGridViewCheckBoxColumn gridColumnCheckbox;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColumnAvailableColumns;
        private System.Windows.Forms.TabPage tbOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvOutputColumns;
        private System.Windows.Forms.DataGridView dgvHashColumns;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOutputColumnsColumnName;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvOutputColumnsHashType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvHashColumnsSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvHashColumnsColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvHashColumnsSortPosition;
        private System.Windows.Forms.TabPage tpAbout;
        private System.Windows.Forms.Label label8;
    }
}