﻿// <auto-generated />
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "codeplex"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "MultipleHashForm"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "Untick"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ssismhash")]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultipleHashForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tcTabs = new System.Windows.Forms.TabControl();
            this.tbInput = new System.Windows.Forms.TabPage();
            this.dgvAvailableColumns = new System.Windows.Forms.DataGridView();
            this.gridColumnCheckbox = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gridColumnAvailableColumns = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbMilliseconds = new System.Windows.Forms.CheckBox();
            this.cbSafeNullHandling = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbThreading = new System.Windows.Forms.ComboBox();
            this.tbOutput = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvOutputColumns = new System.Windows.Forms.DataGridView();
            this.dgvOutputColumnsColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOutputColumnsHashType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.outputHashContainer = new System.Windows.Forms.SplitContainer();
            this.dgvInputColumns = new System.Windows.Forms.DataGridView();
            this.dgvInputColumnsSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvInputColumnsColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHashColumns = new System.Windows.Forms.DataGridView();
            this.dgvHashColumnsColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvHashColumnsSortPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.llCodeplex = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tcTabs.SuspendLayout();
            this.tbInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableColumns)).BeginInit();
            this.panel2.SuspendLayout();
            this.tbOutput.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputColumns)).BeginInit();
            this.outputHashContainer.Panel1.SuspendLayout();
            this.outputHashContainer.Panel2.SuspendLayout();
            this.outputHashContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashColumns)).BeginInit();
            this.panel3.SuspendLayout();
            this.tpAbout.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(767, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOK.Location = new System.Drawing.Point(692, 0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 29);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // tcTabs
            // 
            this.tcTabs.Controls.Add(this.tbInput);
            this.tcTabs.Controls.Add(this.tbOutput);
            this.tcTabs.Controls.Add(this.tpAbout);
            this.tcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcTabs.Location = new System.Drawing.Point(0, 0);
            this.tcTabs.Name = "tcTabs";
            this.tcTabs.SelectedIndex = 0;
            this.tcTabs.Size = new System.Drawing.Size(842, 461);
            this.tcTabs.TabIndex = 0;
            this.tcTabs.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tcTabs_Selecting);
            // 
            // tbInput
            // 
            this.tbInput.Controls.Add(this.dgvAvailableColumns);
            this.tbInput.Controls.Add(this.panel2);
            this.tbInput.Location = new System.Drawing.Point(4, 22);
            this.tbInput.Name = "tbInput";
            this.tbInput.Padding = new System.Windows.Forms.Padding(3);
            this.tbInput.Size = new System.Drawing.Size(834, 435);
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
            this.dgvAvailableColumns.Location = new System.Drawing.Point(3, 40);
            this.dgvAvailableColumns.Name = "dgvAvailableColumns";
            this.dgvAvailableColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAvailableColumns.Size = new System.Drawing.Size(828, 392);
            this.dgvAvailableColumns.TabIndex = 0;
            this.dgvAvailableColumns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAvailableColumns_CellContentClick);
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
            // panel2
            // 
            this.panel2.Controls.Add(this.cbMilliseconds);
            this.panel2.Controls.Add(this.cbSafeNullHandling);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.cbThreading);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(828, 37);
            this.panel2.TabIndex = 5;
            // 
            // cbMilliseconds
            // 
            this.cbMilliseconds.AutoSize = true;
            this.cbMilliseconds.Location = new System.Drawing.Point(425, 8);
            this.cbMilliseconds.Name = "cbMilliseconds";
            this.cbMilliseconds.Size = new System.Drawing.Size(89, 17);
            this.cbMilliseconds.TabIndex = 5;
            this.cbMilliseconds.Text = "Milliseconds?";
            this.toolTip1.SetToolTip(this.cbMilliseconds, "Enables Milliseconds for Hash Calculation on Time types.\r\nOff for Backwards Compa" +
        "tability to 1.5.1 or earlier");
            this.cbMilliseconds.UseVisualStyleBackColor = true;
            this.cbMilliseconds.CheckedChanged += new System.EventHandler(this.cbMilliseconds_CheckedChanged);
            // 
            // cbSafeNullHandling
            // 
            this.cbSafeNullHandling.AutoSize = true;
            this.cbSafeNullHandling.Location = new System.Drawing.Point(5, 8);
            this.cbSafeNullHandling.Name = "cbSafeNullHandling";
            this.cbSafeNullHandling.Size = new System.Drawing.Size(114, 17);
            this.cbSafeNullHandling.TabIndex = 4;
            this.cbSafeNullHandling.Text = "Safe Null Handling";
            this.toolTip1.SetToolTip(this.cbSafeNullHandling, "Turn off to disable Null and Empty string detection.\r\nNot Recommended...");
            this.cbSafeNullHandling.UseVisualStyleBackColor = true;
            this.cbSafeNullHandling.CheckedChanged += new System.EventHandler(this.cbSafeNullHandling_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enable Multiple Threading";
            // 
            // cbThreading
            // 
            this.cbThreading.FormattingEnabled = true;
            this.cbThreading.Items.AddRange(new object[] {
            "None",
            "Auto",
            "On"});
            this.cbThreading.Location = new System.Drawing.Point(261, 6);
            this.cbThreading.MaxDropDownItems = 3;
            this.cbThreading.Name = "cbThreading";
            this.cbThreading.Size = new System.Drawing.Size(158, 21);
            this.cbThreading.TabIndex = 1;
            this.cbThreading.Text = "None";
            this.toolTip1.SetToolTip(this.cbThreading, resources.GetString("cbThreading.ToolTip"));
            this.cbThreading.TextChanged += new System.EventHandler(this.cbThreading_TextChanged);
            // 
            // tbOutput
            // 
            this.tbOutput.Controls.Add(this.splitContainer1);
            this.tbOutput.Location = new System.Drawing.Point(4, 22);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tbOutput.Size = new System.Drawing.Size(834, 435);
            this.tbOutput.TabIndex = 1;
            this.tbOutput.Text = "Output Columns";
            this.tbOutput.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvOutputColumns);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.outputHashContainer);
            this.splitContainer1.Size = new System.Drawing.Size(828, 429);
            this.splitContainer1.SplitterDistance = 245;
            this.splitContainer1.TabIndex = 11;
            // 
            // dgvOutputColumns
            // 
            this.dgvOutputColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutputColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvOutputColumnsColumnName,
            this.dgvOutputColumnsHashType});
            this.dgvOutputColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOutputColumns.Location = new System.Drawing.Point(0, 0);
            this.dgvOutputColumns.MultiSelect = false;
            this.dgvOutputColumns.Name = "dgvOutputColumns";
            this.dgvOutputColumns.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dgvOutputColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOutputColumns.Size = new System.Drawing.Size(245, 429);
            this.dgvOutputColumns.TabIndex = 7;
            this.dgvOutputColumns.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOutputColumns_CellValueChanged);
            this.dgvOutputColumns.SelectionChanged += new System.EventHandler(this.dgvOutputColumns_SelectionChanged);
            this.dgvOutputColumns.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvOutputColumns_UserDeletingRow);
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
            "SHA512",
            "CRC32",
            "CRC32C",
            "FNV1a32",
            "FNV1a64"});
            this.dgvOutputColumnsHashType.MinimumWidth = 30;
            this.dgvOutputColumnsHashType.Name = "dgvOutputColumnsHashType";
            this.dgvOutputColumnsHashType.ToolTipText = "Select the hash value to be applied to this output column";
            // 
            // outputHashContainer
            // 
            this.outputHashContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputHashContainer.Location = new System.Drawing.Point(0, 0);
            this.outputHashContainer.Name = "outputHashContainer";
            // 
            // outputHashContainer.Panel1
            // 
            this.outputHashContainer.Panel1.Controls.Add(this.dgvInputColumns);
            // 
            // outputHashContainer.Panel2
            // 
            this.outputHashContainer.Panel2.Controls.Add(this.dgvHashColumns);
            this.outputHashContainer.Panel2.Controls.Add(this.panel3);
            this.outputHashContainer.Size = new System.Drawing.Size(579, 429);
            this.outputHashContainer.SplitterDistance = 247;
            this.outputHashContainer.TabIndex = 12;
            // 
            // dgvInputColumns
            // 
            this.dgvInputColumns.AllowUserToAddRows = false;
            this.dgvInputColumns.AllowUserToDeleteRows = false;
            this.dgvInputColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInputColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvInputColumnsSelected,
            this.dgvInputColumnsColumnName});
            this.dgvInputColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInputColumns.Location = new System.Drawing.Point(0, 0);
            this.dgvInputColumns.Name = "dgvInputColumns";
            this.dgvInputColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInputColumns.Size = new System.Drawing.Size(247, 429);
            this.dgvInputColumns.TabIndex = 0;
            this.dgvInputColumns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInputColumns_CellContentClick);
            // 
            // dgvInputColumnsSelected
            // 
            this.dgvInputColumnsSelected.HeaderText = "";
            this.dgvInputColumnsSelected.MinimumWidth = 25;
            this.dgvInputColumnsSelected.Name = "dgvInputColumnsSelected";
            this.dgvInputColumnsSelected.ToolTipText = "Untick to exclude this column from the hash.";
            this.dgvInputColumnsSelected.Width = 25;
            // 
            // dgvInputColumnsColumnName
            // 
            this.dgvInputColumnsColumnName.HeaderText = "Column";
            this.dgvInputColumnsColumnName.MinimumWidth = 50;
            this.dgvInputColumnsColumnName.Name = "dgvInputColumnsColumnName";
            this.dgvInputColumnsColumnName.Width = 175;
            // 
            // dgvHashColumns
            // 
            this.dgvHashColumns.AllowUserToAddRows = false;
            this.dgvHashColumns.AllowUserToDeleteRows = false;
            this.dgvHashColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHashColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvHashColumnsColumnName,
            this.dgvHashColumnsSortPosition});
            this.dgvHashColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHashColumns.Location = new System.Drawing.Point(0, 0);
            this.dgvHashColumns.Name = "dgvHashColumns";
            this.dgvHashColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHashColumns.Size = new System.Drawing.Size(328, 399);
            this.dgvHashColumns.TabIndex = 8;
            this.dgvHashColumns.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvHashColumns_CurrentCellDirtyStateChanged);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.btnUp);
            this.panel3.Controls.Add(this.btnDown);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 399);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(328, 30);
            this.panel3.TabIndex = 11;
            // 
            // btnUp
            // 
            this.btnUp.Location = new System.Drawing.Point(3, 3);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(75, 23);
            this.btnUp.TabIndex = 9;
            this.btnUp.Text = "Move Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Location = new System.Drawing.Point(84, 3);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 10;
            this.btnDown.Text = "Move Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // tpAbout
            // 
            this.tpAbout.Controls.Add(this.llCodeplex);
            this.tpAbout.Controls.Add(this.label1);
            this.tpAbout.Controls.Add(this.label8);
            this.tpAbout.Location = new System.Drawing.Point(4, 22);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tpAbout.Size = new System.Drawing.Size(834, 435);
            this.tpAbout.TabIndex = 2;
            this.tpAbout.Text = "About";
            this.tpAbout.UseVisualStyleBackColor = true;
            this.tpAbout.Click += new System.EventHandler(this.tpAbout_Click);
            // 
            // llCodeplex
            // 
            this.llCodeplex.AutoSize = true;
            this.llCodeplex.Location = new System.Drawing.Point(6, 47);
            this.llCodeplex.Name = "llCodeplex";
            this.llCodeplex.Size = new System.Drawing.Size(160, 13);
            this.llCodeplex.TabIndex = 8;
            this.llCodeplex.TabStop = true;
            this.llCodeplex.Text = "http://ssismhash.codeplex.com/";
            this.llCodeplex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llCodeplex_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 221);
            this.label1.TabIndex = 7;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(422, 26);
            this.label8.TabIndex = 6;
            this.label8.Text = "This component was last updated by Keith Martin in March 2016.  This is version 1" +
    ".6.5.4\r\nTo download the latest version or get help go to:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 461);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(842, 29);
            this.panel1.TabIndex = 6;
            // 
            // MultipleHashForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(842, 490);
            this.Controls.Add(this.tcTabs);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "MultipleHashForm";
            this.Text = "MultipleHashForm";
            this.tcTabs.ResumeLayout(false);
            this.tbInput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAvailableColumns)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tbOutput.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutputColumns)).EndInit();
            this.outputHashContainer.Panel1.ResumeLayout(false);
            this.outputHashContainer.Panel2.ResumeLayout(false);
            this.outputHashContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHashColumns)).EndInit();
            this.panel3.ResumeLayout(false);
            this.tpAbout.ResumeLayout(false);
            this.tpAbout.PerformLayout();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridView dgvOutputColumns;
        private System.Windows.Forms.DataGridView dgvHashColumns;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.TabPage tpAbout;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel llCodeplex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbThreading;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbSafeNullHandling;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer outputHashContainer;
        private System.Windows.Forms.DataGridView dgvInputColumns;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvInputColumnsSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvInputColumnsColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvHashColumnsColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvHashColumnsSortPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvOutputColumnsColumnName;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgvOutputColumnsHashType;
        private System.Windows.Forms.CheckBox cbMilliseconds;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}