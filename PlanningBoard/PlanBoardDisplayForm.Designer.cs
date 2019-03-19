namespace PlanningBoard
{
    partial class PlanBoardDisplayForm
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
            this.components = new System.ComponentModel.Container();
            this.planBoardDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.Search = new System.Windows.Forms.Button();
            this.fromDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.MachineComboBox = new System.Windows.Forms.ComboBox();
            this.MStatuscomboBox = new System.Windows.Forms.ComboBox();
            this.GenerateExcelBtn = new System.Windows.Forms.Button();
            this.BtnAddPlan = new System.Windows.Forms.Button();
            this.BtnGeneratePlan = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.pBContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.forwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeWorkDateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateActualQtyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Revert = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.planBoardDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pBContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // planBoardDataGridView
            // 
            this.planBoardDataGridView.AllowUserToAddRows = false;
            this.planBoardDataGridView.AllowUserToDeleteRows = false;
            this.planBoardDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlDarkDark;
            this.planBoardDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.planBoardDataGridView.ColumnHeadersVisible = false;
            this.planBoardDataGridView.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.planBoardDataGridView.Location = new System.Drawing.Point(12, 85);
            this.planBoardDataGridView.Name = "planBoardDataGridView";
            this.planBoardDataGridView.ReadOnly = true;
            this.planBoardDataGridView.RowHeadersVisible = false;
            this.planBoardDataGridView.RowHeadersWidth = 4;
            this.planBoardDataGridView.Size = new System.Drawing.Size(1272, 504);
            this.planBoardDataGridView.TabIndex = 0;
            this.planBoardDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.planBoardDataGridView_CellClick);
            this.planBoardDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.planBoardDataGridView_CellDoubleClick);
            this.planBoardDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.planBoardDataGridView_CellFormatting);
            this.planBoardDataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.planBoardDataGridView_CellMouseDown);
            this.planBoardDataGridView.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.planBoardDataGridView_CellMouseUp);
            this.planBoardDataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.planBoardDataGridView_CellPainting);
            this.planBoardDataGridView.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.planBoardDataGridView_ColumnAdded);
            this.planBoardDataGridView.CurrentCellChanged += new System.EventHandler(this.planBoardDataGridView_CurrentCellChanged);
            this.planBoardDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.planBoardDataGridView_DataBindingComplete);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.SlateGray;
            this.groupBox1.Controls.Add(this.Revert);
            this.groupBox1.Controls.Add(this.toDateTimePicker);
            this.groupBox1.Controls.Add(this.Search);
            this.groupBox1.Controls.Add(this.fromDateTimePicker);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.MachineComboBox);
            this.groupBox1.Controls.Add(this.MStatuscomboBox);
            this.groupBox1.Controls.Add(this.GenerateExcelBtn);
            this.groupBox1.Controls.Add(this.BtnAddPlan);
            this.groupBox1.Controls.Add(this.BtnGeneratePlan);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Tai Le", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1272, 67);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AAAAAAAA";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // toDateTimePicker
            // 
            this.toDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.toDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDateTimePicker.Location = new System.Drawing.Point(259, 25);
            this.toDateTimePicker.Name = "toDateTimePicker";
            this.toDateTimePicker.Size = new System.Drawing.Size(127, 27);
            this.toDateTimePicker.TabIndex = 14;
            // 
            // Search
            // 
            this.Search.BackColor = System.Drawing.Color.RosyBrown;
            this.Search.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Search.Location = new System.Drawing.Point(1086, 23);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(87, 31);
            this.Search.TabIndex = 39;
            this.Search.Text = "Search";
            this.Search.UseVisualStyleBackColor = false;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // fromDateTimePicker
            // 
            this.fromDateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.fromDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDateTimePicker.Location = new System.Drawing.Point(81, 25);
            this.fromDateTimePicker.Name = "fromDateTimePicker";
            this.fromDateTimePicker.Size = new System.Drawing.Size(127, 27);
            this.fromDateTimePicker.TabIndex = 12;
            this.fromDateTimePicker.Value = new System.DateTime(2019, 2, 5, 11, 53, 30, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(608, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 18);
            this.label2.TabIndex = 38;
            this.label2.Text = "Machine No";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(402, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 18);
            this.label1.TabIndex = 37;
            this.label1.Text = "Machine Status";
            // 
            // MachineComboBox
            // 
            this.MachineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MachineComboBox.FormattingEnabled = true;
            this.MachineComboBox.Location = new System.Drawing.Point(702, 25);
            this.MachineComboBox.Name = "MachineComboBox";
            this.MachineComboBox.Size = new System.Drawing.Size(84, 27);
            this.MachineComboBox.TabIndex = 36;
            // 
            // MStatuscomboBox
            // 
            this.MStatuscomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MStatuscomboBox.FormattingEnabled = true;
            this.MStatuscomboBox.Location = new System.Drawing.Point(518, 25);
            this.MStatuscomboBox.Name = "MStatuscomboBox";
            this.MStatuscomboBox.Size = new System.Drawing.Size(84, 27);
            this.MStatuscomboBox.TabIndex = 35;
            this.MStatuscomboBox.SelectedIndexChanged += new System.EventHandler(this.MStatuscomboBox_SelectedIndexChanged);
            // 
            // GenerateExcelBtn
            // 
            this.GenerateExcelBtn.BackColor = System.Drawing.Color.RosyBrown;
            this.GenerateExcelBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateExcelBtn.Location = new System.Drawing.Point(993, 23);
            this.GenerateExcelBtn.Name = "GenerateExcelBtn";
            this.GenerateExcelBtn.Size = new System.Drawing.Size(87, 31);
            this.GenerateExcelBtn.TabIndex = 18;
            this.GenerateExcelBtn.Text = "Export Excel";
            this.GenerateExcelBtn.UseVisualStyleBackColor = false;
            this.GenerateExcelBtn.Click += new System.EventHandler(this.GenerateExcelBtn_Click);
            // 
            // BtnAddPlan
            // 
            this.BtnAddPlan.BackColor = System.Drawing.Color.RosyBrown;
            this.BtnAddPlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAddPlan.Location = new System.Drawing.Point(900, 23);
            this.BtnAddPlan.Name = "BtnAddPlan";
            this.BtnAddPlan.Size = new System.Drawing.Size(87, 31);
            this.BtnAddPlan.TabIndex = 16;
            this.BtnAddPlan.Text = "Add Plan";
            this.BtnAddPlan.UseVisualStyleBackColor = false;
            this.BtnAddPlan.Click += new System.EventHandler(this.BtnAddPlan_Click);
            // 
            // BtnGeneratePlan
            // 
            this.BtnGeneratePlan.BackColor = System.Drawing.Color.RosyBrown;
            this.BtnGeneratePlan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGeneratePlan.Location = new System.Drawing.Point(807, 23);
            this.BtnGeneratePlan.Name = "BtnGeneratePlan";
            this.BtnGeneratePlan.Size = new System.Drawing.Size(87, 31);
            this.BtnGeneratePlan.TabIndex = 15;
            this.BtnGeneratePlan.Text = "Generate";
            this.BtnGeneratePlan.UseVisualStyleBackColor = false;
            this.BtnGeneratePlan.Click += new System.EventHandler(this.BtnGeneratePlan_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(229, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(26, 18);
            this.label14.TabIndex = 13;
            this.label14.Text = "To";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(33, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 18);
            this.label15.TabIndex = 11;
            this.label15.Text = "From";
            // 
            // pBContextMenuStrip
            // 
            this.pBContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forwardToolStripMenuItem,
            this.backwardToolStripMenuItem,
            this.changeWorkDateToolStripMenuItem,
            this.updateActualQtyToolStripMenuItem});
            this.pBContextMenuStrip.Name = "pBContextMenuStrip";
            this.pBContextMenuStrip.Size = new System.Drawing.Size(174, 92);
            // 
            // forwardToolStripMenuItem
            // 
            this.forwardToolStripMenuItem.Name = "forwardToolStripMenuItem";
            this.forwardToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.forwardToolStripMenuItem.Text = "Forward Plan";
            this.forwardToolStripMenuItem.Click += new System.EventHandler(this.forwardToolStripMenuItem_Click);
            // 
            // backwardToolStripMenuItem
            // 
            this.backwardToolStripMenuItem.Name = "backwardToolStripMenuItem";
            this.backwardToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.backwardToolStripMenuItem.Text = "Backward Plan";
            this.backwardToolStripMenuItem.Click += new System.EventHandler(this.backwardToolStripMenuItem_Click);
            // 
            // changeWorkDateToolStripMenuItem
            // 
            this.changeWorkDateToolStripMenuItem.Name = "changeWorkDateToolStripMenuItem";
            this.changeWorkDateToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.changeWorkDateToolStripMenuItem.Text = "Change Work Date";
            this.changeWorkDateToolStripMenuItem.Click += new System.EventHandler(this.changeWorkDateToolStripMenuItem_Click);
            // 
            // updateActualQtyToolStripMenuItem
            // 
            this.updateActualQtyToolStripMenuItem.Name = "updateActualQtyToolStripMenuItem";
            this.updateActualQtyToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.updateActualQtyToolStripMenuItem.Text = "Update Actual Qty";
            this.updateActualQtyToolStripMenuItem.Click += new System.EventHandler(this.updateActualQtyToolStripMenuItem_Click);
            // 
            // Revert
            // 
            this.Revert.BackColor = System.Drawing.Color.RosyBrown;
            this.Revert.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Revert.Location = new System.Drawing.Point(1179, 23);
            this.Revert.Name = "Revert";
            this.Revert.Size = new System.Drawing.Size(87, 31);
            this.Revert.TabIndex = 40;
            this.Revert.Text = "Revert";
            this.Revert.UseVisualStyleBackColor = false;
            // 
            // PlanBoardDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.ClientSize = new System.Drawing.Size(1296, 599);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.planBoardDataGridView);
            this.Name = "PlanBoardDisplayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PlanBoardDisplayForm";
            this.Load += new System.EventHandler(this.PlanBoardDisplayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.planBoardDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pBContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView planBoardDataGridView;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker toDateTimePicker;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.DateTimePicker fromDateTimePicker;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button BtnAddPlan;
        private System.Windows.Forms.Button BtnGeneratePlan;
        private System.Windows.Forms.ContextMenuStrip pBContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem forwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backwardToolStripMenuItem;
        private System.Windows.Forms.Button GenerateExcelBtn;
        private System.Windows.Forms.ToolStripMenuItem changeWorkDateToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox MachineComboBox;
        private System.Windows.Forms.ComboBox MStatuscomboBox;
        private System.Windows.Forms.ToolStripMenuItem updateActualQtyToolStripMenuItem;
        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.Button Revert;

    }
}